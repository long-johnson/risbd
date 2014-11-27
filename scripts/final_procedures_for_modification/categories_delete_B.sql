drop function if exists initial.delete_categories_B(integer);
CREATE OR REPLACE FUNCTION initial.delete_categories_B(_id integer) 
RETURNS setof void as $$
BEGIN
	set search_path to initial;
	-- delete from table on B
	delete from categories
	where categories.id = _id;
	-- delete from table on A
	perform * from public.dblink('host=students.ami.nstu.ru dbname=risbd4 user=risbd4 password=ris14bd4',
	'
		select * from
		initial.delete_categories_A(' || _id ||  ');
	') as T(info text);
	RETURN;
END;
$$ LANGUAGE plpgsql;

select * from delete_categories_B(333);

select * from initial.categories order by id desc;