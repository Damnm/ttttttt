SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 232 (class 1259 OID 18050)
-- Name: AppConfig; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."AppConfig" (
    "Id" uuid NOT NULL,
    "IsApply" boolean NOT NULL,
    "AppName" character varying(250) NOT NULL,
    "HeaderHeading" character varying(250),
    "HeaderSubHeading" character varying(250),
    "HeaderLine1" character varying(250),
    "HeaderLine2" character varying(250),
    "FooterLine1" character varying(250),
    "FooterLine2" character varying(250),
    "CreatedDate" timestamp without time zone,
    "StationCode" character varying(10)
);


ALTER TABLE public."AppConfig" OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 17464)
-- Name: CustomVehicleType; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."CustomVehicleType" (
    "Id" uuid NOT NULL,
    "Name" character varying(50) NOT NULL,
    "Desc" character varying(255),
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."CustomVehicleType" OWNER TO postgres;

--
-- TOC entry 231 (class 1259 OID 18026)
-- Name: ETCCheckout; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."ETCCheckout" (
    "Id" uuid NOT NULL,
    "PaymentId" uuid NOT NULL,
    "ServiceProvider" character varying(50) NOT NULL,
    "TransactionId" character varying(50) NOT NULL,
    "TransactionStatus" character varying(50) NOT NULL,
    "Amount" double precision NOT NULL,
    "RFID" character varying(50),
    "PlateNumber" character varying(20),
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."ETCCheckout" OWNER TO postgres;

--
-- TOC entry 228 (class 1259 OID 17570)
-- Name: Fee; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Fee" (
    "Id" uuid NOT NULL,
    "ObjectId" uuid NOT NULL,
    "LaneInId" character varying(10),
    "LaneInDate" timestamp without time zone,
    "LaneInEpoch" bigint,
    "LaneOutId" character varying(10),
    "LaneOutDate" timestamp without time zone,
    "LaneOutEpoch" bigint,
    "Duration" integer NOT NULL,
    "RFID" character varying(50),
    "Make" character varying(150),
    "Model" character varying(150),
    "PlateNumber" character varying(20),
    "PlateColour" character varying(50),
    "CustomVehicleTypeId" uuid,
    "Seat" integer,
    "Weight" integer,
    "LaneInPlateNumberPhotoUrl" character varying(255),
    "LaneInVehiclePhotoUrl" character varying(255),
    "LaneOutPlateNumberPhotoUrl" character varying(255),
    "LaneOutVehiclePhotoUrl" character varying(255),
    "ConfidenceScore" real,
    "Amount" double precision NOT NULL,
    "VehicleCategoryId" uuid,
    "TicketTypeId" character varying(50),
    "TicketId" character varying(50),
    "ShiftId" uuid,
    "EmployeeId" character varying(20),
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."Fee" OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 17469)
-- Name: FeeType; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."FeeType" (
    "Id" uuid NOT NULL,
    "Name" character varying(50) NOT NULL,
    "Amount" double precision,
    "Desc" character varying(255),
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."FeeType" OWNER TO postgres;

--
-- TOC entry 226 (class 1259 OID 17494)
-- Name: FeeVehicleCategory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."FeeVehicleCategory" (
    "Id" uuid NOT NULL,
    "VehicleCategoryId" uuid NOT NULL,
    "FeeTypeId" uuid NOT NULL,
    "VehicleGroupId" uuid NOT NULL,
    "CustomVehicleTypeId" uuid NOT NULL,
    "PlateNumber" character varying(20),
    "RFID" character varying(50),
    "IsTCPVehicle" boolean NOT NULL,
    "ValidFrom" timestamp without time zone NOT NULL,
    "ValidTo" timestamp without time zone,
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."FeeVehicleCategory" OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 17459)
-- Name: Fusions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Fusions" (
    "Id" uuid NOT NULL,
    "Epoch" bigint NOT NULL,
    "Loop1" boolean NOT NULL,
    "RFID" boolean NOT NULL,
    "Cam1" character varying(15),
    "Loop2" boolean NOT NULL,
    "Cam2" character varying(15),
    "Loop3" boolean NOT NULL,
    "ReversedLoop1" boolean NOT NULL,
    "ReversedLoop2" boolean NOT NULL
);


ALTER TABLE public."Fusions" OWNER TO postgres;

--
-- TOC entry 215 (class 1259 OID 17417)
-- Name: LaneInCameraTransactionLogs; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."LaneInCameraTransactionLogs" (
    "Id" uuid NOT NULL,
    "Epoch" double precision NOT NULL,
    "RFID" text,
    "CameraReaderMacAddr" text,
    "CameraReaderIPAddr" text,
    "LaneInId" uuid NOT NULL,
    "Make" text,
    "Model" text,
    "PlateNumber" text,
    "PlateColour" text,
    "VehicleType" text,
    "Seat" integer,
    "Weight" integer,
    "PlateNumberPhotoUrl" text,
    "VehiclePhotoUrl" text,
    "ConfidenceScore" double precision NOT NULL,
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."LaneInCameraTransactionLogs" OWNER TO postgres;

--
-- TOC entry 216 (class 1259 OID 17424)
-- Name: LaneInRFIDTransactionLogs; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."LaneInRFIDTransactionLogs" (
    "Id" uuid NOT NULL,
    "Epoch" double precision NOT NULL,
    "RFID" text,
    "RFIDReaderMacAddr" text,
    "RFIDReaderIPAddr" text,
    "LaneInId" uuid NOT NULL,
    "Make" text,
    "Model" text,
    "PlateNumber" text,
    "PlateColour" text,
    "VehicleType" text,
    "Seat" integer NOT NULL,
    "Weight" integer NOT NULL,
    "PlateNumberPhotoUrl" text,
    "VehiclePhotoUrl" text,
    "ConfidenceScore" double precision NOT NULL,
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."LaneInRFIDTransactionLogs" OWNER TO postgres;

--
-- TOC entry 233 (class 1259 OID 18059)
-- Name: ManualBarrierControl; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."ManualBarrierControl" (
    "Id" uuid NOT NULL,
    "EmployeeId" character varying(10),
    "Action" integer NOT NULL,
    "LaneOutId" character varying(10),
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."ManualBarrierControl" OWNER TO postgres;

--
-- TOC entry 229 (class 1259 OID 17981)
-- Name: Payment; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Payment" (
    "Id" uuid NOT NULL,
    "LaneInId" character varying(10),
    "FeeId" uuid,
    "LaneOutId" character varying(10),
    "Duration" integer NOT NULL,
    "RFID" character varying(50),
    "Make" character varying(150),
    "Model" character varying(150),
    "PlateNumber" character varying(20),
    "VehicleTypeId" text,
    "CustomVehicleTypeId" uuid,
    "Amount" double precision NOT NULL,
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."Payment" OWNER TO postgres;

--
-- TOC entry 230 (class 1259 OID 17988)
-- Name: PaymentStatus; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."PaymentStatus" (
    "Id" uuid NOT NULL,
    "PaymentId" uuid NOT NULL,
    "Amount" double precision NOT NULL,
    "Currency" character varying(10) NOT NULL,
    "PaymentMethod" character varying(50) NOT NULL,
    "PaymentDate" timestamp without time zone NOT NULL,
    "Status" character varying(50) NOT NULL,
    "PaymentReferenceId" character varying(50),
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."PaymentStatus" OWNER TO postgres;

--
-- TOC entry 225 (class 1259 OID 17484)
-- Name: TimeBlockFee; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."TimeBlockFee" (
    "Id" uuid NOT NULL,
    "FromSecond" bigint NOT NULL,
    "ToSecond" bigint NOT NULL,
    "BlockDurationInSeconds" integer,
    "Amount" double precision,
    "CustomVehicleTypeId" uuid DEFAULT '00000000-0000-0000-0000-000000000000'::uuid NOT NULL,
    "CreatedDate" timestamp without time zone,
    "BlockNumber" integer DEFAULT 0 NOT NULL
);


ALTER TABLE public."TimeBlockFee" OWNER TO postgres;

--
-- TOC entry 227 (class 1259 OID 17559)
-- Name: TimeBlockFeeFormulas; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."TimeBlockFeeFormulas" (
    "Id" uuid NOT NULL,
    "CustomVehicleTypeId" uuid NOT NULL,
    "FromBlockNumber" integer NOT NULL,
    "Amount" double precision NOT NULL,
    "IntervalInSeconds" bigint NOT NULL,
    "ApplyDate" timestamp without time zone NOT NULL,
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."TimeBlockFeeFormulas" OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 17474)
-- Name: VehicleCategory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."VehicleCategory" (
    "Id" uuid NOT NULL,
    "Name" character varying(100),
    "Desc" character varying(255),
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."VehicleCategory" OWNER TO postgres;

--
-- TOC entry 224 (class 1259 OID 17479)
-- Name: VehicleGroup; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."VehicleGroup" (
    "Id" uuid NOT NULL,
    "Name" character varying(100),
    "Desc" character varying(255),
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."VehicleGroup" OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 17431)
-- Name: VehiclePaymentTransactions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."VehiclePaymentTransactions" (
    "Id" uuid NOT NULL,
    "VehicleTransactionId" uuid NOT NULL,
    "PaymentType" text,
    "IsSuccessful" boolean NOT NULL,
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."VehiclePaymentTransactions" OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 17452)
-- Name: VehicleTransactionModels; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."VehicleTransactionModels" (
    "Id" uuid NOT NULL,
    "LaneInId" uuid NOT NULL,
    "LaneInDate" timestamp without time zone NOT NULL,
    "LaneOutId" uuid NOT NULL,
    "LaneOutDate" timestamp without time zone NOT NULL,
    "Duration" integer NOT NULL,
    "PlateNumber" text,
    "RFID" text,
    "LaneInPlateNumberPhotoURL" text,
    "LaneInVehiclePhotoURL" text,
    "LaneOutPlateNumberPhotoURL" text,
    "LaneOutVehiclePhotoURL" text,
    "PaymentMethod" text,
    "Amount" double precision NOT NULL,
    "TicketId" uuid NOT NULL,
    "ExternalEmployeeId" uuid NOT NULL,
    "ShiftId" uuid NOT NULL,
    "VehicleId" uuid NOT NULL,
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."VehicleTransactionModels" OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 17445)
-- Name: Vehicles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Vehicles" (
    "Id" uuid NOT NULL,
    "RFID" text,
    "PlateNumber" text,
    "PlateColor" text,
    "Make" text,
    "Seat" integer,
    "Weight" integer,
    "VehicleType" text,
    "CreatedDate" timestamp without time zone,
    "Model" text
);


ALTER TABLE public."Vehicles" OWNER TO postgres;

--
-- TOC entry 214 (class 1259 OID 17412)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;


--
-- TOC entry 3318 (class 2606 OID 18056)
-- Name: AppConfig PK_AppConfig; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AppConfig"
    ADD CONSTRAINT "PK_AppConfig" PRIMARY KEY ("Id");


--
-- TOC entry 3265 (class 2606 OID 17468)
-- Name: CustomVehicleType PK_CustomVehicleType; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."CustomVehicleType"
    ADD CONSTRAINT "PK_CustomVehicleType" PRIMARY KEY ("Id");


--
-- TOC entry 3315 (class 2606 OID 18030)
-- Name: ETCCheckout PK_ETCCheckout; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ETCCheckout"
    ADD CONSTRAINT "PK_ETCCheckout" PRIMARY KEY ("Id");


--
-- TOC entry 3298 (class 2606 OID 17793)
-- Name: Fee PK_Fee; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Fee"
    ADD CONSTRAINT "PK_Fee" PRIMARY KEY ("Id");


--
-- TOC entry 3267 (class 2606 OID 17473)
-- Name: FeeType PK_FeeType; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."FeeType"
    ADD CONSTRAINT "PK_FeeType" PRIMARY KEY ("Id");


--
-- TOC entry 3283 (class 2606 OID 17498)
-- Name: FeeVehicleCategory PK_FeeVehicleCategory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."FeeVehicleCategory"
    ADD CONSTRAINT "PK_FeeVehicleCategory" PRIMARY KEY ("Id");


--
-- TOC entry 3263 (class 2606 OID 17463)
-- Name: Fusions PK_Fusions; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Fusions"
    ADD CONSTRAINT "PK_Fusions" PRIMARY KEY ("Id");


--
-- TOC entry 3253 (class 2606 OID 17423)
-- Name: LaneInCameraTransactionLogs PK_LaneInCameraTransactionLogs; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."LaneInCameraTransactionLogs"
    ADD CONSTRAINT "PK_LaneInCameraTransactionLogs" PRIMARY KEY ("Id");


--
-- TOC entry 3255 (class 2606 OID 17430)
-- Name: LaneInRFIDTransactionLogs PK_LaneInRFIDTransactionLogs; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."LaneInRFIDTransactionLogs"
    ADD CONSTRAINT "PK_LaneInRFIDTransactionLogs" PRIMARY KEY ("Id");


--
-- TOC entry 3320 (class 2606 OID 18065)
-- Name: ManualBarrierControl PK_ManualBarrierControl; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ManualBarrierControl"
    ADD CONSTRAINT "PK_ManualBarrierControl" PRIMARY KEY ("Id");


--
-- TOC entry 3306 (class 2606 OID 17987)
-- Name: Payment PK_Payment; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Payment"
    ADD CONSTRAINT "PK_Payment" PRIMARY KEY ("Id");


--
-- TOC entry 3310 (class 2606 OID 17994)
-- Name: PaymentStatus PK_PaymentStatus; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."PaymentStatus"
    ADD CONSTRAINT "PK_PaymentStatus" PRIMARY KEY ("Id");


--
-- TOC entry 3275 (class 2606 OID 17488)
-- Name: TimeBlockFee PK_TimeBlockFee; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TimeBlockFee"
    ADD CONSTRAINT "PK_TimeBlockFee" PRIMARY KEY ("Id");


--
-- TOC entry 3286 (class 2606 OID 17563)
-- Name: TimeBlockFeeFormulas PK_TimeBlockFeeFormulas; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TimeBlockFeeFormulas"
    ADD CONSTRAINT "PK_TimeBlockFeeFormulas" PRIMARY KEY ("Id");


--
-- TOC entry 3269 (class 2606 OID 17478)
-- Name: VehicleCategory PK_VehicleCategory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."VehicleCategory"
    ADD CONSTRAINT "PK_VehicleCategory" PRIMARY KEY ("Id");


--
-- TOC entry 3271 (class 2606 OID 17483)
-- Name: VehicleGroup PK_VehicleGroup; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."VehicleGroup"
    ADD CONSTRAINT "PK_VehicleGroup" PRIMARY KEY ("Id");


--
-- TOC entry 3257 (class 2606 OID 17437)
-- Name: VehiclePaymentTransactions PK_VehiclePaymentTransactions; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."VehiclePaymentTransactions"
    ADD CONSTRAINT "PK_VehiclePaymentTransactions" PRIMARY KEY ("Id");


--
-- TOC entry 3261 (class 2606 OID 17458)
-- Name: VehicleTransactionModels PK_VehicleTransactionModels; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."VehicleTransactionModels"
    ADD CONSTRAINT "PK_VehicleTransactionModels" PRIMARY KEY ("Id");


--
-- TOC entry 3259 (class 2606 OID 17451)
-- Name: Vehicles PK_Vehicles; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Vehicles"
    ADD CONSTRAINT "PK_Vehicles" PRIMARY KEY ("Id");


--
-- TOC entry 3251 (class 2606 OID 17416)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 3316 (class 1259 OID 18057)
-- Name: IX_AppConfig_IsApply; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_AppConfig_IsApply" ON public."AppConfig" USING btree ("IsApply");


--
-- TOC entry 3311 (class 1259 OID 18036)
-- Name: IX_ETCCheckout_PaymentId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ETCCheckout_PaymentId" ON public."ETCCheckout" USING btree ("PaymentId");


--
-- TOC entry 3312 (class 1259 OID 18037)
-- Name: IX_ETCCheckout_TransactionId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ETCCheckout_TransactionId" ON public."ETCCheckout" USING btree ("TransactionId");


--
-- TOC entry 3313 (class 1259 OID 18038)
-- Name: IX_ETCCheckout_TransactionId_RFID_PlateNumber; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ETCCheckout_TransactionId_RFID_PlateNumber" ON public."ETCCheckout" USING btree ("TransactionId", "RFID", "PlateNumber");


--
-- TOC entry 3276 (class 1259 OID 17519)
-- Name: IX_FeeVehicleCategory_CustomVehicleTypeId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_FeeVehicleCategory_CustomVehicleTypeId" ON public."FeeVehicleCategory" USING btree ("CustomVehicleTypeId");


--
-- TOC entry 3277 (class 1259 OID 17520)
-- Name: IX_FeeVehicleCategory_FeeTypeId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_FeeVehicleCategory_FeeTypeId" ON public."FeeVehicleCategory" USING btree ("FeeTypeId");


--
-- TOC entry 3278 (class 1259 OID 17521)
-- Name: IX_FeeVehicleCategory_PlateNumber; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_FeeVehicleCategory_PlateNumber" ON public."FeeVehicleCategory" USING btree ("PlateNumber");


--
-- TOC entry 3279 (class 1259 OID 17522)
-- Name: IX_FeeVehicleCategory_RFID; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_FeeVehicleCategory_RFID" ON public."FeeVehicleCategory" USING btree ("RFID");


--
-- TOC entry 3280 (class 1259 OID 17523)
-- Name: IX_FeeVehicleCategory_VehicleCategoryId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_FeeVehicleCategory_VehicleCategoryId" ON public."FeeVehicleCategory" USING btree ("VehicleCategoryId");


--
-- TOC entry 3281 (class 1259 OID 17524)
-- Name: IX_FeeVehicleCategory_VehicleGroupId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_FeeVehicleCategory_VehicleGroupId" ON public."FeeVehicleCategory" USING btree ("VehicleGroupId");


--
-- TOC entry 3287 (class 1259 OID 17582)
-- Name: IX_Fee_CustomVehicleTypeId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Fee_CustomVehicleTypeId" ON public."Fee" USING btree ("CustomVehicleTypeId");


--
-- TOC entry 3288 (class 1259 OID 17924)
-- Name: IX_Fee_LaneInDate; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Fee_LaneInDate" ON public."Fee" USING btree ("LaneInDate");


--
-- TOC entry 3289 (class 1259 OID 17584)
-- Name: IX_Fee_LaneInEpoch; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Fee_LaneInEpoch" ON public."Fee" USING btree ("LaneInEpoch");


--
-- TOC entry 3290 (class 1259 OID 17776)
-- Name: IX_Fee_LaneInId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Fee_LaneInId" ON public."Fee" USING btree ("LaneInId");


--
-- TOC entry 3291 (class 1259 OID 17907)
-- Name: IX_Fee_LaneOutDate; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Fee_LaneOutDate" ON public."Fee" USING btree ("LaneOutDate");


--
-- TOC entry 3292 (class 1259 OID 17587)
-- Name: IX_Fee_LaneOutEpoch; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Fee_LaneOutEpoch" ON public."Fee" USING btree ("LaneOutEpoch");


--
-- TOC entry 3293 (class 1259 OID 17730)
-- Name: IX_Fee_LaneOutId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Fee_LaneOutId" ON public."Fee" USING btree ("LaneOutId");


--
-- TOC entry 3294 (class 1259 OID 17639)
-- Name: IX_Fee_PlateNumber; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Fee_PlateNumber" ON public."Fee" USING btree ("PlateNumber");


--
-- TOC entry 3295 (class 1259 OID 17623)
-- Name: IX_Fee_RFID; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Fee_RFID" ON public."Fee" USING btree ("RFID");


--
-- TOC entry 3296 (class 1259 OID 17607)
-- Name: IX_Fee_TicketId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Fee_TicketId" ON public."Fee" USING btree ("TicketId");


--
-- TOC entry 3307 (class 1259 OID 18005)
-- Name: IX_PaymentStatus_PaymentId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_PaymentStatus_PaymentId" ON public."PaymentStatus" USING btree ("PaymentId");


--
-- TOC entry 3308 (class 1259 OID 18007)
-- Name: IX_PaymentStatus_PaymentReferenceId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_PaymentStatus_PaymentReferenceId" ON public."PaymentStatus" USING btree ("PaymentReferenceId");


--
-- TOC entry 3299 (class 1259 OID 18039)
-- Name: IX_Payment_CustomVehicleTypeId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Payment_CustomVehicleTypeId" ON public."Payment" USING btree ("CustomVehicleTypeId");


--
-- TOC entry 3300 (class 1259 OID 18000)
-- Name: IX_Payment_FeeId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Payment_FeeId" ON public."Payment" USING btree ("FeeId");


--
-- TOC entry 3301 (class 1259 OID 18001)
-- Name: IX_Payment_LaneInId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Payment_LaneInId" ON public."Payment" USING btree ("LaneInId");


--
-- TOC entry 3302 (class 1259 OID 18002)
-- Name: IX_Payment_LaneOutId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Payment_LaneOutId" ON public."Payment" USING btree ("LaneOutId");


--
-- TOC entry 3303 (class 1259 OID 18003)
-- Name: IX_Payment_PlateNumber; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Payment_PlateNumber" ON public."Payment" USING btree ("PlateNumber");


--
-- TOC entry 3304 (class 1259 OID 18004)
-- Name: IX_Payment_RFID; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Payment_RFID" ON public."Payment" USING btree ("RFID");


--
-- TOC entry 3284 (class 1259 OID 17569)
-- Name: IX_TimeBlockFeeFormulas_CustomVehicleTypeId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_TimeBlockFeeFormulas_CustomVehicleTypeId" ON public."TimeBlockFeeFormulas" USING btree ("CustomVehicleTypeId");


--
-- TOC entry 3272 (class 1259 OID 17553)
-- Name: IX_TimeBlockFee_CustomVehicleTypeId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_TimeBlockFee_CustomVehicleTypeId" ON public."TimeBlockFee" USING btree ("CustomVehicleTypeId");


--
-- TOC entry 3273 (class 1259 OID 17534)
-- Name: IX_TimeBlockFee_FromSecond_ToSecond; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_TimeBlockFee_FromSecond_ToSecond" ON public."TimeBlockFee" USING btree ("FromSecond", "ToSecond");


--
-- TOC entry 3331 (class 2606 OID 18031)
-- Name: ETCCheckout FK_ETCCheckout_Payment_PaymentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ETCCheckout"
    ADD CONSTRAINT "FK_ETCCheckout_Payment_PaymentId" FOREIGN KEY ("PaymentId") REFERENCES public."Payment"("Id") ON DELETE CASCADE;


--
-- TOC entry 3322 (class 2606 OID 17499)
-- Name: FeeVehicleCategory FK_FeeVehicleCategory_CustomVehicleType_CustomVehicleTypeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."FeeVehicleCategory"
    ADD CONSTRAINT "FK_FeeVehicleCategory_CustomVehicleType_CustomVehicleTypeId" FOREIGN KEY ("CustomVehicleTypeId") REFERENCES public."CustomVehicleType"("Id") ON DELETE CASCADE;


--
-- TOC entry 3323 (class 2606 OID 17504)
-- Name: FeeVehicleCategory FK_FeeVehicleCategory_FeeType_FeeTypeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."FeeVehicleCategory"
    ADD CONSTRAINT "FK_FeeVehicleCategory_FeeType_FeeTypeId" FOREIGN KEY ("FeeTypeId") REFERENCES public."FeeType"("Id") ON DELETE CASCADE;


--
-- TOC entry 3324 (class 2606 OID 17509)
-- Name: FeeVehicleCategory FK_FeeVehicleCategory_VehicleCategory_VehicleCategoryId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."FeeVehicleCategory"
    ADD CONSTRAINT "FK_FeeVehicleCategory_VehicleCategory_VehicleCategoryId" FOREIGN KEY ("VehicleCategoryId") REFERENCES public."VehicleCategory"("Id") ON DELETE CASCADE;


--
-- TOC entry 3325 (class 2606 OID 17514)
-- Name: FeeVehicleCategory FK_FeeVehicleCategory_VehicleGroup_VehicleGroupId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."FeeVehicleCategory"
    ADD CONSTRAINT "FK_FeeVehicleCategory_VehicleGroup_VehicleGroupId" FOREIGN KEY ("VehicleGroupId") REFERENCES public."VehicleGroup"("Id") ON DELETE CASCADE;


--
-- TOC entry 3327 (class 2606 OID 17794)
-- Name: Fee FK_Fee_CustomVehicleType_CustomVehicleTypeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Fee"
    ADD CONSTRAINT "FK_Fee_CustomVehicleType_CustomVehicleTypeId" FOREIGN KEY ("CustomVehicleTypeId") REFERENCES public."CustomVehicleType"("Id");


--
-- TOC entry 3330 (class 2606 OID 17995)
-- Name: PaymentStatus FK_PaymentStatus_Payment_PaymentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."PaymentStatus"
    ADD CONSTRAINT "FK_PaymentStatus_Payment_PaymentId" FOREIGN KEY ("PaymentId") REFERENCES public."Payment"("Id") ON DELETE CASCADE;


--
-- TOC entry 3328 (class 2606 OID 18040)
-- Name: Payment FK_Payment_CustomVehicleType_CustomVehicleTypeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Payment"
    ADD CONSTRAINT "FK_Payment_CustomVehicleType_CustomVehicleTypeId" FOREIGN KEY ("CustomVehicleTypeId") REFERENCES public."CustomVehicleType"("Id");


--
-- TOC entry 3329 (class 2606 OID 18045)
-- Name: Payment FK_Payment_Fee_FeeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Payment"
    ADD CONSTRAINT "FK_Payment_Fee_FeeId" FOREIGN KEY ("FeeId") REFERENCES public."Fee"("Id");


--
-- TOC entry 3326 (class 2606 OID 17564)
-- Name: TimeBlockFeeFormulas FK_TimeBlockFeeFormulas_CustomVehicleType_CustomVehicleTypeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TimeBlockFeeFormulas"
    ADD CONSTRAINT "FK_TimeBlockFeeFormulas_CustomVehicleType_CustomVehicleTypeId" FOREIGN KEY ("CustomVehicleTypeId") REFERENCES public."CustomVehicleType"("Id") ON DELETE CASCADE;


--
-- TOC entry 3321 (class 2606 OID 17554)
-- Name: TimeBlockFee FK_TimeBlockFee_CustomVehicleType_CustomVehicleTypeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TimeBlockFee"
    ADD CONSTRAINT "FK_TimeBlockFee_CustomVehicleType_CustomVehicleTypeId" FOREIGN KEY ("CustomVehicleTypeId") REFERENCES public."CustomVehicleType"("Id") ON DELETE CASCADE;


-- Completed on 2023-09-28 14:46:45

--
-- PostgreSQL database dump complete
--

