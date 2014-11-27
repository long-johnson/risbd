drop function if exists initial.update_clients_A(integer, text,  text,  text,  date,  text,  text,  text, text,  text,  date,  text);
CREATE OR REPLACE FUNCTION initial.update_clients_A(_id integer, _surname text, _name text, _patronymic text, _birthdate date, _phone text, _email text, _address text, 
													_passport_series text, _passport_number text, _issue_date date, _issue_department text) 
RETURNS setof void as $$
BEGIN
	set search_path to initial;
	
	-- update table A
	update clients
	set surname = _surname, name = _name, patronymic = _patronymic,  birthdate = _birthdate, phone = _phone, 
		email = _email,  address = _address,  passport_series = _passport_series, passport_number = _passport_number, 
		issue_date = _issue_date, issue_department = _issue_department
	where clients.id = _id;
	RETURN;
END;
$$ LANGUAGE plpgsql;

select * from initial.select_all_customers_A() limit 20;