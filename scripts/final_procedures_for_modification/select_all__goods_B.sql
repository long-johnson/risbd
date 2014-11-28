drop function if exists initial.select_all_goods_B();
CREATE OR REPLACE FUNCTION initial.select_all_goods_B()
RETURNS TABLE(id int, category_title text, company_name text, model text, price numeric(12,2), description text) as $$
BEGIN
	set search_path to initial;
	
	RETURN QUERY
		SELECT goods.id, categories.title, companies.name, goods.model, goods.price, goods.description
		FROM goods
		join categories on categories.id = goods.category_id
		join companies on companies.id = goods.company_id
		order by goods.model, companies.name
		limit 200;
END;
$$ LANGUAGE plpgsql;

select * from initial.select_all_goods_B();
select * from goods limit 1000;