drop function if exists initial.delete_companies_B(integer);
CREATE OR REPLACE FUNCTION initial.delete_companies_B(_id integer) 
RETURNS setof void as $$
BEGIN
	set search_path to initial;
	
	-- delete from table B
	delete from companies
	where companies.id = _id;
	
	-- delete from table A
	perform * from public.dblink('host=students.ami.nstu.ru dbname=risbd4 user=risbd4 password=ris14bd4',
	'
		select * from 
		initial.delete_companies_A(' || _id || ');
	') as T(info text);
	
	RETURN;
END;
$$ LANGUAGE plpgsql;

select * from delete_companies_B(599); 

select * from companies order by name;