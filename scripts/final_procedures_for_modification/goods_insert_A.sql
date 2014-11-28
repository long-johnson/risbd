drop function if exists initial.insert_goods_A(integer, integer, integer, text, numeric, text);
CREATE OR REPLACE FUNCTION initial.insert_goods_A(_id integer, _category_id integer, _company_id integer, _model text, _price numeric(12,2), _description text) 
RETURNS setof void as $$
BEGIN
	set search_path to initial;
	
	-- insert into table A
	insert into goods(id, category_id, company_id, model, price, description)
		values(_id, _category_id, _company_id, _model, _price, _description);
	
	RETURN;
END;
$$ LANGUAGE plpgsql;

select * from goods
order by id desc
limit 100;
