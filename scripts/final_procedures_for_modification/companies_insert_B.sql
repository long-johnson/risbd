﻿drop function if exists initial.insert_companies_B(text, integer, text, text, text, text);
CREATE OR REPLACE FUNCTION initial.insert_companies_B(_name text, _country_id integer, _head_full_name text, _phone text, _address text, _bank_details text) 
RETURNS setof void as $$
DECLARE new_id integer;
DECLARE _count_countries integer;
BEGIN
	set search_path to initial;
	
	-- check for existence of foreign key _country_id in Countries (on server A)
	select check_for_key.count_countries into _count_countries
	from
		public.dblink('host=students.ami.nstu.ru dbname=risbd4 user=risbd4 password=ris14bd4',
		'
			set search_path to initial;
			SELECT count(*)
			FROM countries
			where countries.id = ' || _country_id || ';
		') as check_for_key(count_countries integer);
	if _count_countries = 0
	then
		raise exception 'Foreign key violation (country_id)';
		return;
	end if;
	
	-- get maximal id
	SELECT companies.id into new_id 
	FROM companies
	ORDER BY id DESC 
	LIMIT 1;
	-- new_id is max_id + 1
	new_id := new_id + 1;
	
	-- insert into table B
	insert into companies(id, name, country_id, head_full_name, phone, address, bank_details)
		values(new_id, _name, _country_id, _head_full_name, _phone, _address, _bank_details);
	
	-- insert into table A
	perform * from public.dblink('host=students.ami.nstu.ru dbname=risbd4 user=risbd4 password=ris14bd4',
	'
		select * from 
		initial.insert_companies_A(' || new_id || ' ,''' || _name || ''', ' || _country_id || ' ,''' || _head_full_name || ''', ''' || _phone  || ''', ''' ||  _address  || ''', ''' ||  _bank_details || ''');
	') as T(info text);
	
	RETURN;
END;
$$ LANGUAGE plpgsql;

select * from insert_companies_B('1AA', 5, 'Joseph', '8-913', 'Пушкина', '228'); 

select * from companies order by name;