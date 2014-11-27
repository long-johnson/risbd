CREATE OR REPLACE FUNCTION initial.select_all_countries_A() 
RETURNS TABLE(id integer, name text) AS $$
BEGIN
    set search_path to initial;
    RETURN QUERY
        SELECT *
	FROM countries
	order by name;
END;
$$ LANGUAGE plpgsql;

select *
from initial.select_all_countries_A();