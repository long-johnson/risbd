drop function if exists initial.delete_goods_B(integer);
CREATE OR REPLACE FUNCTION initial.delete_goods_B(_id integer) 
RETURNS setof void as $$
DECLARE _count_goods integer;
BEGIN
	set search_path to initial;
	
	-- check that row to delete from goods: goods.id is neither in orders_cash (A) nor in orders_non_cash (B - auto)
	
	select check_for_key.count_goods into _count_goods
	from
		public.dblink('host=students.ami.nstu.ru dbname=risbd4 user=risbd4 password=ris14bd4',
		'
			set search_path to initial;
			SELECT count(*)
			FROM orders_cash
			where orders_cash.goods_id = ' || _id || ';
		') as check_for_key(count_goods integer);
	if _count_goods <> 0
	then
		raise exception 'Foreign key violation (_id) is used in orders_cash(A)';
		return;
	end if;
	
	-- delete from table B
	delete from goods
	where goods.id = _id;
	
	-- delete from table A
	perform * from public.dblink('host=students.ami.nstu.ru dbname=risbd4 user=risbd4 password=ris14bd4',
	'
		select * from
		initial.delete_goods_A(' || _id || ');
	') as T(info text);
	
	RETURN;
END;
$$ LANGUAGE plpgsql;

select * from initial.delete_goods_B(10168)

select * from initial.select_all_goods_B();
select * from goods
order by id desc
limit 100;