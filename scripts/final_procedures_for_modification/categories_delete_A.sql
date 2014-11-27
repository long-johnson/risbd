drop function if exists initial.delete_categories_A(integer);
CREATE OR REPLACE FUNCTION initial.delete_categories_A(_id integer) 
RETURNS setof void as $$
BEGIN
	set search_path to initial;
	-- delete from table on A
	delete from categories
	where categories.id = _id;
	delete from сategories_month_sum_sale_amount_cash
	where сategories_month_sum_sale_amount_cash.category_id = _id;
	RETURN;
END;
$$ LANGUAGE plpgsql;

select * from initial.categories order by id desc;