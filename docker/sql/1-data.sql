SET search_path = MainDB;

-- Init test data
-- Need to be remove when implement
INSERT INTO public.test(id, code, name, create_date)
VALUES ('655a9a3a-7848-43df-9019-7df88c928621', 'Nguyen Tran', 'Thanh Tam', CURRENT_TIMESTAMP),
('2ebdc0be-0680-4a69-acc6-38ccafcdfc28', 'Nguyen Tuan', '', CURRENT_TIMESTAMP),
('b304c61a-b9d0-4486-84b8-885e8a12b2c2', 'Tran Van', 'Thanh', CURRENT_TIMESTAMP),
('87c780a8-bc74-46ff-9ab9-37ff517248dc', 'Hoang', 'Tam', CURRENT_TIMESTAMP),
('c7cedea9-b0a4-45ed-b976-b474aa1d1cde', 'Le Van', 'Thanh', CURRENT_TIMESTAMP);