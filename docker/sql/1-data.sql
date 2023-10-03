SET search_path = dbtest;

INSERT INTO public."CustomVehicleType" ("Id", "Name", "Desc", "CreatedDate") VALUES ('090a7db5-2d5d-4c1c-a32c-27f946f8dd61', 'Type4', 'Xe loại 4', '2023-09-08 07:00:00+07');
INSERT INTO public."CustomVehicleType" ("Id", "Name", "Desc", "CreatedDate") VALUES ('a4a39e55-85c0-4761-ba64-f941111186f9', 'Type2', 'Xe loại 2', '2023-09-08 07:00:00+07');
INSERT INTO public."CustomVehicleType" ("Id", "Name", "Desc", "CreatedDate") VALUES ('be652877-ca81-4fb4-bfa1-b9cec61f9e6b', 'Type3', 'Xe loại 3', '2023-09-08 07:00:00+07');
INSERT INTO public."CustomVehicleType" ("Id", "Name", "Desc", "CreatedDate") VALUES ('fffbf4d1-8b76-4f3a-9070-0cfa0a658f08', 'Type1', 'Xe loại 1', '2023-09-08 07:00:00+07');


INSERT INTO public."FeeType" ("Id", "Name", "Amount", "Desc", "CreatedDate") VALUES ('04595036-c8a8-4800-9513-c4015b98da3b', 'DayBlock', NULL, 'Tính phí theo ngày', '2023-09-08 07:00:00+07');
INSERT INTO public."FeeType" ("Id", "Name", "Amount", "Desc", "CreatedDate") VALUES ('1143d8c3-22e2-4bd5-a690-89ca0c47b3c9', 'TimeBlock', NULL, 'Tính phí theo thời gian', '2023-09-08 07:00:00+07');
INSERT INTO public."FeeType" ("Id", "Name", "Amount", "Desc", "CreatedDate") VALUES ('30ee8597-aa3e-43e7-a1f1-559ee2d4b85e', 'Free', 0, 'Miễn phí', '2023-09-08 07:00:00+07');
INSERT INTO public."FeeType" ("Id", "Name", "Amount", "Desc", "CreatedDate") VALUES ('46b26ea4-abfd-4b9f-bdf4-ec0e434d9ffc', 'Fixed', 15000, 'Phí cố định', '2023-09-08 07:00:00+07');


INSERT INTO public."VehicleGroup" ("Id", "Name", "Desc", "CreatedDate") VALUES ('1fc5fc58-94e4-4169-a576-3cd9ecf8eb96', 'Taxi Xanh', NULL, '2023-09-08 07:00:00+07');
INSERT INTO public."VehicleGroup" ("Id", "Name", "Desc", "CreatedDate") VALUES ('ec2a686b-8adc-4053-9e2e-4942cab0168d', 'Công ty vận tải hành khách', NULL, '2023-09-08 07:00:00+07');
INSERT INTO public."VehicleGroup" ("Id", "Name", "Desc", "CreatedDate") VALUES ('efbe78bc-290b-4a01-a596-bbc62f60f5f3', 'Taxi Mai Linh', NULL, '2023-09-08 07:00:00+07');


INSERT INTO public."VehicleCategory" ("Id", "Name", "Desc", "CreatedDate") VALUES ('2b0557d0-cc6b-4fc2-a0b3-08788c9fd8c7', 'Xe ưu tiên theo tháng', NULL, '2023-09-08 07:00:00+07');
INSERT INTO public."VehicleCategory" ("Id", "Name", "Desc", "CreatedDate") VALUES ('70884a61-39f3-4e8e-b936-d5b18652d3ac', 'Xe nhượng quyền', NULL, '2023-09-08 07:00:00+07');
INSERT INTO public."VehicleCategory" ("Id", "Name", "Desc", "CreatedDate") VALUES ('82f143d3-b2ed-40d6-a59e-4fc980a24450', 'Xe nhượng quyền TCP', NULL, '2023-09-08 07:00:00+07');
INSERT INTO public."VehicleCategory" ("Id", "Name", "Desc", "CreatedDate") VALUES ('ac9b71a5-0541-4d2e-a358-6afac6d6c525', 'Xe ưu tiên theo năm', NULL, '2023-09-08 07:00:00+07');
INSERT INTO public."VehicleCategory" ("Id", "Name", "Desc", "CreatedDate") VALUES ('bd4e670d-8cae-46fa-8bac-d77ac139a044', 'Xe ưu tiên theo quý', NULL, '2023-09-08 07:00:00+07');


INSERT INTO public."TimeBlockFee" ("Id", "FromSecond", "ToSecond", "BlockDurationInSeconds", "Amount", "CustomVehicleTypeId", "CreatedDate", "BlockNumber") VALUES ('0c8d860a-c5ba-473c-a3f6-95aafd295a70', 3600, 5399, 1800, 21000, 'fffbf4d1-8b76-4f3a-9070-0cfa0a658f08', '2023-09-08 07:00:00+07', 2);
INSERT INTO public."TimeBlockFee" ("Id", "FromSecond", "ToSecond", "BlockDurationInSeconds", "Amount", "CustomVehicleTypeId", "CreatedDate", "BlockNumber") VALUES ('120fd104-b6e4-403f-87d7-811ccb1c61e4', 3600, 5399, 1800, 28000, 'a4a39e55-85c0-4761-ba64-f941111186f9', '2023-09-08 07:00:00+07', 2);
INSERT INTO public."TimeBlockFee" ("Id", "FromSecond", "ToSecond", "BlockDurationInSeconds", "Amount", "CustomVehicleTypeId", "CreatedDate", "BlockNumber") VALUES ('2e8ab3f8-8d72-4f42-831f-b0100f814a23', 600, 3599, 3000, 14000, 'fffbf4d1-8b76-4f3a-9070-0cfa0a658f08', '2023-09-08 07:00:00+07', 1);
INSERT INTO public."TimeBlockFee" ("Id", "FromSecond", "ToSecond", "BlockDurationInSeconds", "Amount", "CustomVehicleTypeId", "CreatedDate", "BlockNumber") VALUES ('3ae0b8be-525b-4ee4-9d49-d2889c6998c3', 5400, 7199, 1800, 52000, '090a7db5-2d5d-4c1c-a32c-27f946f8dd61', '2023-09-08 07:00:00+07', 3);
INSERT INTO public."TimeBlockFee" ("Id", "FromSecond", "ToSecond", "BlockDurationInSeconds", "Amount", "CustomVehicleTypeId", "CreatedDate", "BlockNumber") VALUES ('8585a134-fa8f-467e-8e66-f37e75444a65', 600, 3599, 3000, 24000, 'be652877-ca81-4fb4-bfa1-b9cec61f9e6b', '2023-09-08 07:00:00+07', 1);
INSERT INTO public."TimeBlockFee" ("Id", "FromSecond", "ToSecond", "BlockDurationInSeconds", "Amount", "CustomVehicleTypeId", "CreatedDate", "BlockNumber") VALUES ('8a8040ae-479c-4824-a0c2-3b4277d0ea9c', 600, 3599, 3000, 24000, '090a7db5-2d5d-4c1c-a32c-27f946f8dd61', '2023-09-08 07:00:00+07', 1);
INSERT INTO public."TimeBlockFee" ("Id", "FromSecond", "ToSecond", "BlockDurationInSeconds", "Amount", "CustomVehicleTypeId", "CreatedDate", "BlockNumber") VALUES ('8b000abd-8e74-47a3-8a90-299dc37fac4d', 0, 599, 600, 14000, 'be652877-ca81-4fb4-bfa1-b9cec61f9e6b', '2023-09-08 07:00:00+07', 0);
INSERT INTO public."TimeBlockFee" ("Id", "FromSecond", "ToSecond", "BlockDurationInSeconds", "Amount", "CustomVehicleTypeId", "CreatedDate", "BlockNumber") VALUES ('9302c9e0-12c2-437c-bd2d-92ed4c159e9f', 5400, 7199, 1800, 37000, 'a4a39e55-85c0-4761-ba64-f941111186f9', '2023-09-08 07:00:00+07', 3);
INSERT INTO public."TimeBlockFee" ("Id", "FromSecond", "ToSecond", "BlockDurationInSeconds", "Amount", "CustomVehicleTypeId", "CreatedDate", "BlockNumber") VALUES ('9e891bfc-2f03-4382-8b7e-6306c2757963', 5400, 7199, 1800, 52000, 'be652877-ca81-4fb4-bfa1-b9cec61f9e6b', '2023-09-08 07:00:00+07', 3);
INSERT INTO public."TimeBlockFee" ("Id", "FromSecond", "ToSecond", "BlockDurationInSeconds", "Amount", "CustomVehicleTypeId", "CreatedDate", "BlockNumber") VALUES ('ad21057b-6071-4e56-8949-ce60bf54f75b', 0, 599, 600, 9000, 'fffbf4d1-8b76-4f3a-9070-0cfa0a658f08', '2023-09-08 07:00:00+07', 0);
INSERT INTO public."TimeBlockFee" ("Id", "FromSecond", "ToSecond", "BlockDurationInSeconds", "Amount", "CustomVehicleTypeId", "CreatedDate", "BlockNumber") VALUES ('adda0b07-7bd5-470b-9f89-77bb6b5cbfb2', 3600, 5399, 1800, 38000, '090a7db5-2d5d-4c1c-a32c-27f946f8dd61', '2023-09-08 07:00:00+07', 2);
INSERT INTO public."TimeBlockFee" ("Id", "FromSecond", "ToSecond", "BlockDurationInSeconds", "Amount", "CustomVehicleTypeId", "CreatedDate", "BlockNumber") VALUES ('b3b643cb-488d-48f3-a167-ea9531db75ca', 3600, 5399, 1800, 38000, 'be652877-ca81-4fb4-bfa1-b9cec61f9e6b', '2023-09-08 07:00:00+07', 2);
INSERT INTO public."TimeBlockFee" ("Id", "FromSecond", "ToSecond", "BlockDurationInSeconds", "Amount", "CustomVehicleTypeId", "CreatedDate", "BlockNumber") VALUES ('df059c09-28aa-4134-919a-e3b3041213a4', 600, 3599, 3000, 19000, 'a4a39e55-85c0-4761-ba64-f941111186f9', '2023-09-08 07:00:00+07', 1);
INSERT INTO public."TimeBlockFee" ("Id", "FromSecond", "ToSecond", "BlockDurationInSeconds", "Amount", "CustomVehicleTypeId", "CreatedDate", "BlockNumber") VALUES ('e3772040-a29c-4a40-bd65-17d8be7211bb', 5400, 7199, 1800, 28000, 'fffbf4d1-8b76-4f3a-9070-0cfa0a658f08', '2023-09-08 07:00:00+07', 3);
INSERT INTO public."TimeBlockFee" ("Id", "FromSecond", "ToSecond", "BlockDurationInSeconds", "Amount", "CustomVehicleTypeId", "CreatedDate", "BlockNumber") VALUES ('f8d3b541-2f77-4f14-bbe6-8a3028fccd07', 0, 599, 600, 14000, 'a4a39e55-85c0-4761-ba64-f941111186f9', '2023-09-08 07:00:00+07', 0);
INSERT INTO public."TimeBlockFee" ("Id", "FromSecond", "ToSecond", "BlockDurationInSeconds", "Amount", "CustomVehicleTypeId", "CreatedDate", "BlockNumber") VALUES ('f8d9683b-7cb4-4ce6-985c-5aa0a8f944e0', 0, 599, 600, 24000, '090a7db5-2d5d-4c1c-a32c-27f946f8dd61', '2023-09-08 07:00:00+07', 0);


INSERT INTO public."FeeVehicleCategory" ("Id", "VehicleCategoryId", "FeeTypeId", "VehicleGroupId", "CustomVehicleTypeId", "PlateNumber", "RFID", "IsTCPVehicle", "ValidFrom", "ValidTo", "CreatedDate") VALUES ('1d6603bb-d361-4111-aa45-e780f50b6974', '70884a61-39f3-4e8e-b936-d5b18652d3ac', '46b26ea4-abfd-4b9f-bdf4-ec0e434d9ffc', '1fc5fc58-94e4-4169-a576-3cd9ecf8eb96', 'a4a39e55-85c0-4761-ba64-f941111186f9', '50A3008', '840326156843215625', false, '2023-01-01 07:00:00+07', '2024-01-01 06:59:59+07', '2023-09-08 07:00:00+07');
INSERT INTO public."FeeVehicleCategory" ("Id", "VehicleCategoryId", "FeeTypeId", "VehicleGroupId", "CustomVehicleTypeId", "PlateNumber", "RFID", "IsTCPVehicle", "ValidFrom", "ValidTo", "CreatedDate") VALUES ('a15041a9-1d57-4ae3-b070-2d96aaa041ec', '70884a61-39f3-4e8e-b936-d5b18652d3ac', '46b26ea4-abfd-4b9f-bdf4-ec0e434d9ffc', 'efbe78bc-290b-4a01-a596-bbc62f60f5f3', 'fffbf4d1-8b76-4f3a-9070-0cfa0a658f08', '51A3268', '843206065135832015', false, '2023-01-01 07:00:00+07', '2024-01-01 06:59:59+07', '2023-09-08 07:00:00+07');
INSERT INTO public."FeeVehicleCategory" ("Id", "VehicleCategoryId", "FeeTypeId", "VehicleGroupId", "CustomVehicleTypeId", "PlateNumber", "RFID", "IsTCPVehicle", "ValidFrom", "ValidTo", "CreatedDate") VALUES ('a743e3e1-d6aa-49c5-a63f-28ba262bc2b8', '70884a61-39f3-4e8e-b936-d5b18652d3ac', '30ee8597-aa3e-43e7-a1f1-559ee2d4b85e', 'ec2a686b-8adc-4053-9e2e-4942cab0168d', 'fffbf4d1-8b76-4f3a-9070-0cfa0a658f08', '51A0968', NULL, false, '2023-01-01 07:00:00+07', '2024-01-01 06:59:59+07', '2023-09-08 07:00:00+07');
INSERT INTO public."FeeVehicleCategory" ("Id", "VehicleCategoryId", "FeeTypeId", "VehicleGroupId", "CustomVehicleTypeId", "PlateNumber", "RFID", "IsTCPVehicle", "ValidFrom", "ValidTo", "CreatedDate") VALUES ('b780afae-6c9e-4730-a054-8ab8a876dffe', '2b0557d0-cc6b-4fc2-a0b3-08788c9fd8c7', '30ee8597-aa3e-43e7-a1f1-559ee2d4b85e', 'ec2a686b-8adc-4053-9e2e-4942cab0168d', 'be652877-ca81-4fb4-bfa1-b9cec61f9e6b', '29A3268', NULL, false, '2023-01-01 07:00:00+07', '2024-01-01 06:59:59+07', '2023-09-08 07:00:00+07');


INSERT INTO public."TimeBlockFeeFormulas" ("Id", "CustomVehicleTypeId", "FromBlockNumber", "Amount", "IntervalInSeconds", "ApplyDate", "CreatedDate") VALUES ('41369f70-ab4d-4199-a1b3-f7746fa0ff88', 'be652877-ca81-4fb4-bfa1-b9cec61f9e6b', 2, 14000, 1800, '2023-01-01 07:00:00+07', '2023-09-08 07:00:00+07');
INSERT INTO public."TimeBlockFeeFormulas" ("Id", "CustomVehicleTypeId", "FromBlockNumber", "Amount", "IntervalInSeconds", "ApplyDate", "CreatedDate") VALUES ('667b13b4-088e-4a1a-bd36-ec15e795109b', 'fffbf4d1-8b76-4f3a-9070-0cfa0a658f08', 2, 7000, 1800, '2023-01-01 07:00:00+07', '2023-09-08 07:00:00+07');
INSERT INTO public."TimeBlockFeeFormulas" ("Id", "CustomVehicleTypeId", "FromBlockNumber", "Amount", "IntervalInSeconds", "ApplyDate", "CreatedDate") VALUES ('8376b7a6-4330-4133-9e47-afd0d3f7c921', '090a7db5-2d5d-4c1c-a32c-27f946f8dd61', 2, 14000, 1800, '2023-01-01 07:00:00+07', '2023-09-08 07:00:00+07');
INSERT INTO public."TimeBlockFeeFormulas" ("Id", "CustomVehicleTypeId", "FromBlockNumber", "Amount", "IntervalInSeconds", "ApplyDate", "CreatedDate") VALUES ('98c39b48-1248-4471-ae72-22e51e456307', 'a4a39e55-85c0-4761-ba64-f941111186f9', 2, 9000, 1800, '2023-01-01 07:00:00+07', '2023-09-08 07:00:00+07');

INSERT INTO public."Fusions" ("Id", "Epoch", "Loop1", "RFID", "Cam1", "Loop2", "Cam2", "Loop3", "ReversedLoop1", "ReversedLoop2") VALUES ('47610194-3f8d-44c1-90d5-87bec44a9dd2', 20, true, true, '12A1345', true, '12A1345', true, true, true);
INSERT INTO public."Fusions" ("Id", "Epoch", "Loop1", "RFID", "Cam1", "Loop2", "Cam2", "Loop3", "ReversedLoop1", "ReversedLoop2") VALUES ('9c3c8fcd-5819-4ec8-a3aa-e71e74a83c80', 10, true, true, '12A123456', true, '12A123456', true, true, true);
INSERT INTO public."Fusions" ("Id", "Epoch", "Loop1", "RFID", "Cam1", "Loop2", "Cam2", "Loop3", "ReversedLoop1", "ReversedLoop2") VALUES ('ed7384ce-7918-4839-a9fe-4f0780a5b72c', 0, true, true, 'straaaaaing', true, 'string', true, true, true);
INSERT INTO public."Fusions" ("Id", "Epoch", "Loop1", "RFID", "Cam1", "Loop2", "Cam2", "Loop3", "ReversedLoop1", "ReversedLoop2") VALUES ('b9ec5016-f5ca-4ac8-aadb-53e5bb041fc5', 0, true, true, 'string', true, 'string', true, true, true);


INSERT INTO public."Vehicles" ("Id", "RFID", "PlateNumber", "PlateColor", "Make", "Seat", "Weight", "VehicleType", "CreatedDate") VALUES ('d4a2f1a0-5b6f-40db-ad09-6fdabbb9365f', '4156asdasd87479q', '12B5478', 'Blue', 'Toyota', 5, 2000, 'Loại 2', '2023-08-25 10:34:49.218526+07');
INSERT INTO public."Vehicles" ("Id", "RFID", "PlateNumber", "PlateColor", "Make", "Seat", "Weight", "VehicleType", "CreatedDate") VALUES ('3d21ec09-f271-4183-b691-477274f4d547', '12s4adsvsad', '12A12345', 'blue', 'Toyota', 5, 5000, 'Loai 1', NULL);
INSERT INTO public."Vehicles" ("Id", "RFID", "PlateNumber", "PlateColor", "Make", "Seat", "Weight", "VehicleType", "CreatedDate") VALUES ('a18081f7-5706-499d-acf9-680d5f13502e', '12s4adsvsad', '12A12345', 'blue', 'Toyota', 5, 5000, 'Loai 1', NULL);
INSERT INTO public."Vehicles" ("Id", "RFID", "PlateNumber", "PlateColor", "Make", "Seat", "Weight", "VehicleType", "CreatedDate") VALUES ('8fd84949-94b8-4607-856d-b38564fe7bac', '123asdas48v6aaswd', '12A123456', 'White', 'Toyotra', 5, 5000, 'Loai 2', '2023-09-13 10:11:58.84279+07');


INSERT INTO public."AppConfig" ("Id", "IsApply", "AppName", "HeaderHeading", "HeaderSubHeading", "HeaderLine1", "HeaderLine2", "FooterLine1", "FooterLine2", "CreatedDate", "StationCode") VALUES ('2c0f4a72-0c59-4a76-a379-4be0bc5ebd08', true, 'Default app config', 'Cảng hàng không quốc tế Tân Sơn Nhất', 'CN tổng Công ty hàng không việt - CTCP', 'ĐC: 58 Trường Sơn, Phường 2, Quận Tân Bình, TP. HCM', 'ĐT: 123456789 MST: 0312451145112', 'TP HCM, ', 'Người nộp', '2023-09-27 13:55:24.173983', '03');


