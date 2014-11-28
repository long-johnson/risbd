drop function if exists initial.insert_goods_B(integer, integer, text, numeric, text);
CREATE OR REPLACE FUNCTION initial.insert_goods_B(_category_id integer, _company_id integer, _model text, _price numeric(12,2), _description text) 
RETURNS setof void as $$
DECLARE new_id integer;
BEGIN
	set search_path to initial;
	
	-- get maximal id
	SELECT goods.id into new_id 
	FROM goods
	ORDER BY id DESC 
	LIMIT 1;
	-- new_id is max_id + 1
	new_id := new_id + 1;
	
	-- insert into table B
	insert into goods(id, category_id, company_id, model, price, description)
		values(new_id, _category_id, _company_id, _model, _price, _description);
	
	-- insert into table A
	perform * from public.dblink('host=students.ami.nstu.ru dbname=risbd4 user=risbd4 password=ris14bd4',
	'
		select * from 
		initial.insert_goods_A(' || new_id || ' , ' || _category_id || ' , ' || _company_id || ' ,''' || _model || ''', ' || _price  || ' , ''' ||  _description  || ''');
	') as T(info text);
	
	RETURN;
END;
$$ LANGUAGE plpgsql;

select * from insert_goods_B(1,1,'0AAA-MY',666,'Cool')

select * from initial.select_all_goods_B();
select * from goods
order by id desc
limit 100;