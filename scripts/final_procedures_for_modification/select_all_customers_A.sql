drop function if exists initial.select_all_customers_A();
CREATE OR REPLACE FUNCTION initial.select_all_customers_A()
RETURNS TABLE(id int, surname text, name text, patronymic text, birthdate date, phone text,
			email text, address text, series text, num_pass text, issue_date date, issued_by text, sum_sale_amount integer) as $$
BEGIN
	set search_path to initial;
	
	RETURN QUERY
		SELECT *
		FROM clients
		order by clients.surname, clients.name, clients.patronymic;
END;
$$ LANGUAGE plpgsql;

select * from initial.select_all_customers_A();