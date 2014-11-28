drop function if exists initial.update_clients_sum_sale_amount(integer, numeric(12,2));
CREATE OR REPLACE FUNCTION initial.update_clients_sum_sale_amount(_client_id integer, _sale_amount_diff numeric(12,2))
RETURNS setof void as $$
BEGIN
	set search_path to initial;
	
	update clients
	set clients.sum_sale_amount = clients.sum_sale_amount + _sale_amount_diff
	where clients.id = _client_id;
	
	RETURN;
END;
$$ LANGUAGE plpgsql;