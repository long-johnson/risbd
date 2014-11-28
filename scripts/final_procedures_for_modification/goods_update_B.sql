drop function if exists initial.update_goods_B(integer, integer, integer, text, numeric, text);
CREATE OR REPLACE FUNCTION initial.update_goods_B(_id integer, _category_id integer, _company_id integer, _model text, _price numeric(12,2), _description text) 
RETURNS setof void as $$
BEGIN
	set search_path to initial;
	
	-- update table B
	update goods
	set category_id = _category_id, company_id = _company_id, 
		model = _model, price = _price, description = _description
	where goods.id = _id;
	
	-- update table A
	perform * from public.dblink('host=students.ami.nstu.ru dbname=risbd4 user=risbd4 password=ris14bd4',
	'
		select * from 
		initial.update_goods_A(' || _id || ' , ' || _category_id || ' , ' || _company_id || ' ,''' || _model || ''', ' || _price  || ' , ''' ||  _description  || ''');
	') as T(info text);
	
	RETURN;
END;
$$ LANGUAGE plpgsql;

select * from initial.update_goods_B(10168,1,1,'0BBB-MY',555,'Cool')

select * from initial.select_all_goods_B();
select * from goods
order by id desc
limit 100;