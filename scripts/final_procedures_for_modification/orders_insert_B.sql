drop function if exists initial.insert_orders_B(integer, integer, date, numeric(12,2), integer, integer, text);
CREATE OR REPLACE FUNCTION initial.insert_orders_B(_goods_id integer, _client_id integer, _on_sale_date date, _sale_amount numeric(12,2), 
												_payment_method_id integer, _sale_type_id integer, _details text) 
RETURNS setof void as $$
declare max_id_A integer;
declare max_id_B integer;
declare new_id integer;
declare _on_sale_month integer;
BEGIN
	set search_path to initial;
	
	-- get maximal id from B
	SELECT orders_non_cash.id into max_id_B 
	FROM orders_non_cash
	ORDER BY id DESC 
	LIMIT 1;
	
	-- get maximal id from A
	select get_max_id.max_id into max_id_A
	from
		public.dblink('host=students.ami.nstu.ru dbname=risbd4 user=risbd4 password=ris14bd4',
		'
			set search_path to initial;
			SELECT orders_cash.id 
			FROM orders_cash
			ORDER BY orders_cash.id DESC 
			LIMIT 1;
		') as get_max_id(max_id integer);
	
	-- new_id is max(max_id_A, max_id_B) + 1
	if max_id_B > max_id_A
	then
		new_id := max_id_B + 1;
	else
		new_id := max_id_A + 1;
	end if;
	
	-- extract month from date
	_on_sale_month := date_part('month', _on_sale_date);
	
	-- insert in accordance with payment method
	if (_payment_method_id = 1)
	then
		-- insert into table B
		insert into orders_non_cash(id, goods_id, client_id, on_sale_date, sale_amount, payment_method_id, sale_type_id, details, on_sale_month)
			values(new_id, _goods_id, _client_id, _on_sale_date, _sale_amount, _payment_method_id, _sale_type_id, _details, _on_sale_month);
		return;
	end if;
	
	-- insert into table A
	perform * from public.dblink('host=students.ami.nstu.ru dbname=risbd4 user=risbd4 password=ris14bd4',
	'
		select * from 
		initial.insert_orders_A(' || new_id || ' , ' || _goods_id || ' , ' || _client_id || ' , ''' || _on_sale_date || ''', '
		|| _sale_amount  || ' , ' ||  _payment_method_id  || ' , ' ||  _sale_type_id  || ' , ''' ||  _details  || ''' , ' ||  _on_sale_month  || '
		);
	') as T(info text);
	/*end if;*/
	
	RETURN;
END;
$$ LANGUAGE plpgsql;

select * from initial.insert_orders_B(1, 1, '01-01-1993', 666, 2, 1, 'details')

select * from orders_non_cash
order by id desc
limit 100


