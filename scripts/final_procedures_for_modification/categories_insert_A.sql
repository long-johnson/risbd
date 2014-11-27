drop function if exists initial.insert_categories_A(integer, text);
CREATE OR REPLACE FUNCTION initial.insert_categories_A(_id integer, _title text) 
RETURNS setof void as $$
BEGIN
	set search_path to initial;
	-- insert into table on A
	insert into categories(id, title)
		values(_id, _title);
	RETURN;
END;
$$ LANGUAGE plpgsql;

select * from initial.categories
where id > 200
order by id desc;

delete from initial.categories
where id = 333;

execute initial.insert_categories_A(333,'My');