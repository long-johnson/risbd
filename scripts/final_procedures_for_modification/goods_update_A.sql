drop function if exists initial.update_goods_A(integer, integer, integer, text, numeric, text);
CREATE OR REPLACE FUNCTION initial.update_goods_A(_id integer, _category_id integer, _company_id integer, _model text, _price numeric(12,2), _description text) 
RETURNS setof void as $$
BEGIN
	set search_path to initial;
	
	-- update table A
	update goods
	set category_id = _category_id, company_id = _company_id, 
		model = _model, price = _price, description = _description
	where goods.id = _id;
	
	RETURN;
END;
$$ LANGUAGE plpgsql;

select * from goods
order by id desc
limit 100;
