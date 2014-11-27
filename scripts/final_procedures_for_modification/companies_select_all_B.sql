CREATE OR REPLACE FUNCTION initial.select_all_companies_B() 
RETURNS TABLE(id integer, name text, country_id integer, head_full_name text, phone text, address text, bank_details text) AS $$
BEGIN
    set search_path to initial;
    RETURN QUERY
        SELECT *
	FROM companies
	order by name;
END;
$$ LANGUAGE plpgsql;

select *
from initial.select_all_companies_B();