drop function if exists initial.update_categories_B(integer, text);
CREATE OR REPLACE FUNCTION initial.update_categories_B(_id integer, _title text) 
RETURNS setof void as $$
BEGIN
	set search_path to initial;
	-- update table on B
	update categories
	set title = _title
	where categories.id = _id;
	-- update table on A
	perform * from public.dblink('host=students.ami.nstu.ru dbname=risbd4 user=risbd4 password=ris14bd4',
	'
		select * from
		initial.update_categories_A(' || _id ||  ', ''' || _title || ''');
	') as T1(test text);
	RETURN;
END;
$$ LANGUAGE plpgsql;

select * from update_categories_B(333, 'MyNew');

select * from initial.categories order by id desc;