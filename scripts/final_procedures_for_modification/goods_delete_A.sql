drop function if exists initial.delete_goods_A(integer);
CREATE OR REPLACE FUNCTION initial.delete_goods_A(_id integer) 
RETURNS setof void as $$
BEGIN
	set search_path to initial;
	
	-- delete from table A
	delete from goods
	where goods.id = _id;
	
	RETURN;
END;
$$ LANGUAGE plpgsql;

select * from goods
order by id desc
limit 100;
