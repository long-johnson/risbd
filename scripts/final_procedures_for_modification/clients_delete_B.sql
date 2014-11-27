drop function if exists initial.delete_clients_B(integer);
CREATE OR REPLACE FUNCTION initial.delete_clients_B(_id integer) 
RETURNS setof void as $$
DECLARE _count_clients integer;
BEGIN
	set search_path to initial;
	
	-- check if this client_id is not used in orders_cash
	select check_for_key.count_clients into _count_clients
	from
		public.dblink('host=students.ami.nstu.ru dbname=risbd4 user=risbd4 password=ris14bd4',
		'
			set search_path to initial;
			SELECT count(*)
			FROM orders_cash
			where orders_cash.client_id = ' || _id || ';
		') as check_for_key(count_clients integer);
	if _count_clients <> 0
	then
		raise exception 'Foreign key violation (_id) is used in orders_cash(A)';
		return;
	end if;
	
	-- delete from table B
	delete from clients
	where clients.id = _id;
	
	-- delete from table A
	perform * from public.dblink('host=students.ami.nstu.ru dbname=risbd4 user=risbd4 password=ris14bd4',
	'
		select * from 
		initial.delete_clients_A(' || _id || ');
	') as T(info text);

	RETURN;
END;
$$ LANGUAGE plpgsql;

select * from initial.delete_clients_B(50002);
select * from initial.clients order by surname, name limit 20;