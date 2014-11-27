CREATE OR REPLACE FUNCTION initial.select_all_companies_A() 
RETURNS TABLE(id integer, name text, country_name text, head_full_name text, phone text, address text, bank_details text) AS $$
BEGIN
    set search_path to initial;
    RETURN QUERY
        SELECT companies.id, companies.name, countries.name, companies.head_full_name, companies.phone, companies.address, companies.bank_details
	FROM companies
	join countries on countries.id = companies.country_id
	order by companies.name;
END;
$$ LANGUAGE plpgsql;

select *
from initial.select_all_companies_A();
