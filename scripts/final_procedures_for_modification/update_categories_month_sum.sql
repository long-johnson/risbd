drop function if exists initial.update_categories_month_sum_A(integer, integer, numeric(12,2));
CREATE OR REPLACE FUNCTION initial.update_categories_month_sum_A(_category_id integer, _on_sale_month integer, _sale_amount_diff numeric(12,2))
RETURNS setof void as $$
DECLARE
count_entries integer;
current_value integer;
new_value integer;
BEGIN
	set search_path to initial;
	
	-- find if there is already (_category_id, _on_sale_month) tuple in "categories_month_sum_sale_amount_cash"
	select count(*)
	into count_entries
	from categories_month_sum_sale_amount_cash
	where categories_month_sum_sale_amount_cash.category_id = _category_id and categories_month_sum_sale_amount_cash.on_sale_month = _on_sale_month
	group by categories_month_sum_sale_amount_cash.category_id, categories_month_sum_sale_amount_cash.on_sale_month;
	
	-- если нет вхождения и разница отрицательная: ошибка
	if count_entries = 0 and _sale_amount_diff < 0
	then
		raise exception 'Trying to decrease value that doesnt exits in categories_month_sum_sale_amount_cash';
		return;
	end if;
	
	-- если нет вхождения, создадим его
	if count_entries = 0
	then
		insert into categories_month_sum_sale_amount_cash(category_id, on_sale_month, sum_sale_amount)
			values(_category_id, _on_sale_month, 0);
	end if;
	
	-- найдем текущее значение
	select categories_month_sum_sale_amount_cash.sum_sale_amount
	into current_value
	from categories_month_sum_sale_amount_cash
	where categories_month_sum_sale_amount_cash.category_id = _category_id and categories_month_sum_sale_amount_cash.on_sale_month = _on_sale_month;
	
	-- найдем новое значение
	new_value := current_value + _sale_amount_diff;
	
	if new_value > 0	-- new_value > 0 : прибавим разницу к текущему вхождению
	then
		update categories_month_sum_sale_amount_cash
		set categories_month_sum_sale_amount_cash.sum_sale_amount = new_value
		where categories_month_sum_sale_amount_cash.category_id = _category_id and categories_month_sum_sale_amount_cash.on_sale_month = _on_sale_month;
	else				-- new_value <= 0 : удалим данную строку
		delete from categories_month_sum_sale_amount_cash
		where categories_month_sum_sale_amount_cash.category_id = _category_id and categories_month_sum_sale_amount_cash.on_sale_month = _on_sale_month;
	end if;
	
	RETURN;
END;
$$ LANGUAGE plpgsql;