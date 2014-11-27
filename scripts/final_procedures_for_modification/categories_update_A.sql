drop function if exists initial.update_categories_A(integer, text);
CREATE OR REPLACE FUNCTION initial.update_categories_A(_id integer, _title text) 
RETURNS setof void as $$
BEGIN
	set search_path to initial;
	-- update table on A
	update categories
	set title = _title
	where categories.id = _id;
	RETURN;
END;
$$ LANGUAGE plpgsql;

select * from initial.categories order by id desc;