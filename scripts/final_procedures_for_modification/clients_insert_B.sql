drop function if exists initial.insert_clients_B( text,  text,  text,  date,  text,  text,  text, text,  text,  date,  text);
CREATE OR REPLACE FUNCTION initial.insert_clients_B(_surname text, _name text, _patronymic text, _birthdate date, _phone text, _email text, _address text, 
													_passport_series text, _passport_number text, _issue_date date, _issue_department text) 
RETURNS setof void as $$
DECLARE new_id integer;
BEGIN
	set search_path to initial;
	
	-- get maximal id
	SELECT clients.id into new_id 
	FROM clients
	ORDER BY id DESC 
	LIMIT 1;
	-- new_id is max_id + 1
	new_id := new_id + 1;
	
	-- insert into table B
	insert into clients(id, surname, name, patronymic, birthdate)
		values(new_id, _surname, _name, _patronymic, _birthdate);
	
	-- insert into table A
	perform * from public.dblink('host=students.ami.nstu.ru dbname=risbd4 user=risbd4 password=ris14bd4',
	'
		select * from 
		initial.insert_clients_A(' || new_id || ' , ''' || _surname || ''' , ''' || _name || ''' , ''' || _patronymic || ''', '''
		|| _birthdate  || ''' , ''' ||  _phone  || ''' , ''' ||  _email  || ''' , ''' ||  _address  || ''' , ''' ||  _passport_series  ||
		''' , ''' ||  _passport_number  || ''' , ''' ||  _issue_date  || ''' , ''' ||  _issue_department  || '''
		);
	') as T(info text);
	RETURN;
END;
$$ LANGUAGE plpgsql;

select * from initial.insert_clients_B('qwe','rty','uio','01-01-1994', 'asd', 'fgh', 'jkl', 'zxc', 'vbn', '02-02-1995', 'mmm');
select * from initial.clients order by surname, name limit 20;