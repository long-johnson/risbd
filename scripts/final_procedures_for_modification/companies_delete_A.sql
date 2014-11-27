drop function if exists initial.delete_companies_A(integer);
CREATE OR REPLACE FUNCTION initial.delete_companies_A(_id integer) 
RETURNS setof void as $$
BEGIN
	set search_path to initial;
	
	-- delete from table A
	delete from companies
	where companies.id = _id;
	
	RETURN;
END;
$$ LANGUAGE plpgsql;

select *
from initial.select_all_companies_A();
