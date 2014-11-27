drop function if exists initial.insert_clients_A(integer, text,  text,  text,  date,  text,  text,  text, text,  text,  date,  text);
CREATE OR REPLACE FUNCTION initial.insert_clients_A(_id integer, _surname text, _name text, _patronymic text, _birthdate date, _phone text, _email text, _address text, 
													_passport_series text, _passport_number text, _issue_date date, _issue_department text) 
RETURNS setof void as $$
BEGIN
	set search_path to initial;
	
	-- insert into table A
	-- and sum_sale_amount to zero!
	insert into clients(id, surname, name, patronymic, birthdate, phone, email, address, passport_series, passport_number, issue_date, issue_department, sum_sale_amount)
	values(_id, _surname, _name, _patronymic, _birthdate, _phone, _email, _address, _passport_series, _passport_number, _issue_date, _issue_department, 0);
	
	RETURN;
END;
$$ LANGUAGE plpgsql;

select * from initial.select_all_customers_A() limit 20;