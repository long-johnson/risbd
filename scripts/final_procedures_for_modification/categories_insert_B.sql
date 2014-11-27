drop function if exists initial.insert_categories_B(text);
CREATE OR REPLACE FUNCTION initial.insert_categories_B(_title text) 
RETURNS setof void as $$
DECLARE new_id integer;
BEGIN
	set search_path to initial;
	-- get maximal id
	SELECT categories.id into new_id 
	FROM categories 
	ORDER BY id DESC 
	LIMIT 1;
	-- new_id is max_id + 1
	new_id := new_id + 1;
	-- insert into table on B
	insert into categories(id, title)
		values(new_id, _title);
	-- insert into table on A
	perform * from public.dblink('host=students.ami.nstu.ru dbname=risbd4 user=risbd4 password=ris14bd4',
	'
		select * from initial.insert_categories_A(' || new_id ||  ', ''' || _title || ''');
	') as T1(test text);
END;
$$ LANGUAGE plpgsql;

select * from
initial.categories
order by id desc;

select * from initial.insert_categories_B('My1');