drop function if exists initial.update_companies_A(integer, text, integer, text, text, text, text);
CREATE OR REPLACE FUNCTION initial.update_companies_A(_id integer, _name text, _country_id integer, _head_full_name text, _phone text, _address text, _bank_details text) 
RETURNS setof void as $$
DECLARE _count_countries integer;
BEGIN
	set search_path to initial;
	
	-- update table A
	update companies
	set name = _name, country_id = _country_id, head_full_name = _head_full_name,
		phone = _phone,  address = _address, bank_details = _bank_details
	where companies.id = _id;
	
	RETURN;
END;
$$ LANGUAGE plpgsql;

select *
from initial.select_all_companies_A();
