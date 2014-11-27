drop function if exists initial.insert_companies_A(integer, text, integer, text, text, text, text);
CREATE OR REPLACE FUNCTION initial.insert_companies_A(_id integer, _name text, _country_id integer, _head_full_name text, _phone text, _address text, _bank_details text) 
RETURNS setof void as $$
BEGIN
	set search_path to initial;
	
	-- insert into table A
	insert into companies(id, name, country_id, head_full_name, phone, address, bank_details)
		values(_id, _name, _country_id, _head_full_name, _phone, _address, _bank_details);
	
	RETURN;
END;
$$ LANGUAGE plpgsql;

select *
from initial.select_all_companies_A();
