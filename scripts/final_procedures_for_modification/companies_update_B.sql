drop function if exists initial.update_companies_B(integer, text, integer, text, text, text, text);
CREATE OR REPLACE FUNCTION initial.update_companies_B(_id integer, _name text, _country_id integer, _head_full_name text, _phone text, _address text, _bank_details text) 
RETURNS setof void as $$
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
	
	-- update table B
	update companies
	set name = _name, country_id = _country_id, head_full_name = _head_full_name,
		phone = _phone,  address = _address, bank_details = _bank_details
	where companies.id = _id;
	
	-- update table A
	perform * from public.dblink('host=students.ami.nstu.ru dbname=risbd4 user=risbd4 password=ris14bd4',
	'
		select * from 
		initial.update_companies_A(' || _id || ' ,''' || _name || ''', ' || _country_id || ' ,''' || _head_full_name || ''', ''' || _phone  || ''', ''' ||  _address  || ''', ''' ||  _bank_details || ''');
	') as T(info text);
	
	RETURN;
END;
$$ LANGUAGE plpgsql;

select * from update_companies_B(599, '1AA', 5, 'Joseph', '8-913', 'SERBIA STRONG', '228'); 

select * from companies order by name;