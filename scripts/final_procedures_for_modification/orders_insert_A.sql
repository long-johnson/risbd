drop function if exists initial.insert_orders_A(integer, integer, integer, date, numeric(12,2), integer, integer, text, integer);
CREATE OR REPLACE FUNCTION initial.insert_orders_A(_id integer, _goods_id integer, _client_id integer, _on_sale_date date, _sale_amount numeric(12,2), 
												_payment_method_id integer, _sale_type_id integer, _details text, _on_sale_month integer) 
RETURNS setof void as $$
declare _category_id integer;
BEGIN
	set search_path to initial;
	
	-- insert into table A
	insert into orders_cash(id, goods_id, client_id, on_sale_date, sale_amount, payment_method_id, sale_type_id, details, on_sale_month)
		values(_id, _goods_id, _client_id, _on_sale_date, _sale_amount, _payment_method_id, _sale_type_id, _details, _on_sale_month);
	
	-- get _category_id to change
	select goods.category_id
	into _category_id
	from goods
	where goods.id = _goods_id;
	
	-- update categories_month_sum_sale_amount_cash
	perform * from initial.update_categories_month_sum_A(_category_id, _on_sale_month, _sale_amount) as T(info text); 
	
	-- update clients.sum_sale_amount
	perform * from initial.update_clients_sum_sale_amount(_client_id, _sale_amount) as T(info text);
	
	RETURN;
END;
$$ LANGUAGE plpgsql;

select * from orders_cash
order by id desc
limit 100
