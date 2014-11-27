drop function if exists initial.update_clients_B(integer, text,  text,  text,  date,  text,  text,  text, text,  text,  date,  text);
CREATE OR REPLACE FUNCTION initial.update_clients_B(_id integer, _surname text, _name text, _patronymic text, _birthdate date, _phone text, _email text, _address text, 
													_passport_series text, _passport_number text, _issue_date date, _issue_department text) 
RETURNS setof void as $$
BEGIN
	set search_path to initial;
	
	-- update table B
	update clients
	set surname = _surname, name = _name, patronymic = _patronymic,  birthdate = _birthdate
	where clients.id = _id;
	
	-- update table A
	perform * from public.dblink('host=students.ami.nstu.ru dbname=risbd4 user=risbd4 password=ris14bd4',
	'
		select * from 
		initial.update_clients_A(' || _id || ' , ''' || _surname || ''' , ''' || _name || ''' , ''' || _patronymic || ''', '''
		|| _birthdate  || ''' , ''' ||  _phone  || ''' , ''' ||  _email  || ''' , ''' ||  _address  || ''' , ''' ||  _passport_series  ||
		''' , ''' ||  _passport_number  || ''' , ''' ||  _issue_date  || ''' , ''' ||  _issue_department  || '''
		);
	') as T(info text);
	
	RETURN;
END;
$$ LANGUAGE plpgsql;

select * from initial.update_clients_B(50002,'I','ve','got','01-01-1994', 'this', 'summertime', 'summertime', 'sadness', 'vbn', '02-02-1995', 'mmm');
select * from initial.clients order by surname, name limit 20;