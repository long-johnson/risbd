drop function if exists initial.delete_clients_A(integer);
CREATE OR REPLACE FUNCTION initial.delete_clients_A(_id integer) 
RETURNS setof void as $$
BEGIN
	set search_path to initial;
	
	-- delete from table A
	delete from clients
	where clients.id = _id;
	
	RETURN;
END;
$$ LANGUAGE plpgsql;

select * from initial.select_all_customers_A() limit 20;