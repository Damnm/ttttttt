--
-- PostgreSQL database dump
--

-- Dumped from database version 15.3
-- Dumped by pg_dump version 15.3

-- Started on 2023-10-11 21:47:40

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
-- TOC entry 230 (class 1259 OID 25709)
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
-- TOC entry 233 (class 1259 OID 25738)
-- Name: Barcode; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Barcode" (
    "Id" uuid NOT NULL,
    "EmployeeId" character varying(50),
    "ActionCode" character varying(20),
    "ActionDesc" character varying(100),
    "CreatedDate" timestamp without time zone,
    "BarcodeAction" character varying(50) DEFAULT ''::character varying NOT NULL
);


ALTER TABLE public."Barcode" OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 25096)
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
-- TOC entry 229 (class 1259 OID 25679)
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
-- TOC entry 226 (class 1259 OID 25203)
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
-- TOC entry 220 (class 1259 OID 25101)
-- Name: FeeType; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."FeeType" (
    "Id" uuid NOT NULL,
    "FeeName" character varying(50) NOT NULL,
    "Amount" double precision,
    "Desc" character varying(255),
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."FeeType" OWNER TO postgres;

--
-- TOC entry 224 (class 1259 OID 25126)
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
-- TOC entry 218 (class 1259 OID 24638)
-- Name: Fusion; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Fusion" (
    "Id" uuid NOT NULL,
    "Epoch" bigint NOT NULL,
    "Loop1" boolean NOT NULL,
    "RFID" boolean NOT NULL,
    "ANPRCam1" character varying(15),
    "Loop2" boolean NOT NULL,
    "CCTVCam2" character varying(15),
    "Loop3" boolean NOT NULL,
    "ReversedLoop1" boolean NOT NULL,
    "ReversedLoop2" boolean NOT NULL
);


ALTER TABLE public."Fusion" OWNER TO postgres;

--
-- TOC entry 215 (class 1259 OID 24596)
-- Name: LaneInCameraTransactionLog; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."LaneInCameraTransactionLog" (
    "Id" uuid NOT NULL,
    "Epoch" bigint NOT NULL,
    "RFID" character varying(50),
    "CamerraIPAddr" character varying(50),
    "CameraMacAddr" character varying(50),
    "LaneInId" character varying(10),
    "Make" character varying(150),
    "Model" character varying(150),
    "PlateNumber" character varying(20),
    "PlateColour" character varying(50),
    "VehicleType" character varying(20),
    "Seat" integer,
    "Weight" integer,
    "PlateNumberPhotoUrl" character varying(255),
    "VehiclePhotoUrl" character varying(255),
    "ConfidenceScore" double precision NOT NULL,
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."LaneInCameraTransactionLog" OWNER TO postgres;

--
-- TOC entry 216 (class 1259 OID 24603)
-- Name: LaneInRFIDTransactionLog; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."LaneInRFIDTransactionLog" (
    "Id" uuid NOT NULL,
    "Epoch" bigint NOT NULL,
    "RFID" character varying(50),
    "RFIDReaderMacAddr" character varying(50),
    "RFIDReaderIPAddr" character varying(50),
    "LaneInId" character varying(10),
    "Make" character varying(150),
    "Model" character varying(150),
    "PlateNumber" character varying(20),
    "PlateColour" character varying(50),
    "VehicleType" character varying(20),
    "Seat" integer,
    "Weight" integer,
    "PlateNumberPhotoUrl" character varying(255),
    "VehiclePhotoUrl" character varying(255),
    "ConfidenceScore" double precision NOT NULL,
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."LaneInRFIDTransactionLog" OWNER TO postgres;

--
-- TOC entry 232 (class 1259 OID 25725)
-- Name: ManualBarrierControl; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."ManualBarrierControl" (
    "Id" uuid NOT NULL,
    "EmployeeId" character varying(20),
    "Action" character varying(50) NOT NULL,
    "LaneOutId" character varying(10) NOT NULL,
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."ManualBarrierControl" OWNER TO postgres;

--
-- TOC entry 231 (class 1259 OID 25717)
-- Name: ManualBarrierControls; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."ManualBarrierControls" (
    "Id" uuid NOT NULL,
    "EmployeeId" uuid,
    "Action" integer NOT NULL,
    "LaneOutId" text NOT NULL,
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."ManualBarrierControls" OWNER TO postgres;

--
-- TOC entry 227 (class 1259 OID 25634)
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
    "VehicleType" character varying(20),
    "CustomVehicleTypeId" uuid,
    "Amount" double precision NOT NULL,
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."Payment" OWNER TO postgres;

--
-- TOC entry 228 (class 1259 OID 25641)
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
    "TransactionId" character varying(50),
    "CreatedDate" timestamp without time zone,
    "Reason" character varying(255)
);


ALTER TABLE public."PaymentStatus" OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 25116)
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
-- TOC entry 225 (class 1259 OID 25192)
-- Name: TimeBlockFeeFormula; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."TimeBlockFeeFormula" (
    "Id" uuid NOT NULL,
    "CustomVehicleTypeId" uuid NOT NULL,
    "FromBlockNumber" integer NOT NULL,
    "Amount" double precision NOT NULL,
    "IntervalInSeconds" bigint NOT NULL,
    "ApplyDate" timestamp without time zone NOT NULL,
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."TimeBlockFeeFormula" OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 24624)
-- Name: Vehicle; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Vehicle" (
    "Id" uuid NOT NULL,
    "RFID" character varying(50),
    "PlateNumber" character varying(20),
    "PlateColor" character varying(50),
    "Make" character varying(150),
    "Seat" integer,
    "Weight" integer,
    "VehicleType" character varying(20),
    "CreatedDate" timestamp without time zone,
    "Model" character varying(150)
);


ALTER TABLE public."Vehicle" OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 25106)
-- Name: VehicleCategory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."VehicleCategory" (
    "Id" uuid NOT NULL,
    "VehicleCategoryName" character varying(100),
    "Desc" character varying(255),
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."VehicleCategory" OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 25111)
-- Name: VehicleGroup; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."VehicleGroup" (
    "Id" uuid NOT NULL,
    "GroupName" character varying(100),
    "Desc" character varying(255),
    "CreatedDate" timestamp without time zone
);


ALTER TABLE public."VehicleGroup" OWNER TO postgres;

--
-- TOC entry 214 (class 1259 OID 24591)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 3315 (class 2606 OID 25715)
-- Name: AppConfig PK_AppConfig; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AppConfig"
    ADD CONSTRAINT "PK_AppConfig" PRIMARY KEY ("Id");


--
-- TOC entry 3321 (class 2606 OID 25744)
-- Name: Barcode PK_Barcode; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Barcode"
    ADD CONSTRAINT "PK_Barcode" PRIMARY KEY ("Id");


--
-- TOC entry 3262 (class 2606 OID 25100)
-- Name: CustomVehicleType PK_CustomVehicleType; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."CustomVehicleType"
    ADD CONSTRAINT "PK_CustomVehicleType" PRIMARY KEY ("Id");


--
-- TOC entry 3312 (class 2606 OID 25683)
-- Name: ETCCheckout PK_ETCCheckout; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ETCCheckout"
    ADD CONSTRAINT "PK_ETCCheckout" PRIMARY KEY ("Id");


--
-- TOC entry 3295 (class 2606 OID 25445)
-- Name: Fee PK_Fee; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Fee"
    ADD CONSTRAINT "PK_Fee" PRIMARY KEY ("Id");


--
-- TOC entry 3264 (class 2606 OID 25105)
-- Name: FeeType PK_FeeType; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."FeeType"
    ADD CONSTRAINT "PK_FeeType" PRIMARY KEY ("Id");


--
-- TOC entry 3280 (class 2606 OID 25130)
-- Name: FeeVehicleCategory PK_FeeVehicleCategory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."FeeVehicleCategory"
    ADD CONSTRAINT "PK_FeeVehicleCategory" PRIMARY KEY ("Id");


--
-- TOC entry 3260 (class 2606 OID 25926)
-- Name: Fusion PK_Fusion; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Fusion"
    ADD CONSTRAINT "PK_Fusion" PRIMARY KEY ("Id");


--
-- TOC entry 3254 (class 2606 OID 25918)
-- Name: LaneInCameraTransactionLog PK_LaneInCameraTransactionLog; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."LaneInCameraTransactionLog"
    ADD CONSTRAINT "PK_LaneInCameraTransactionLog" PRIMARY KEY ("Id");


--
-- TOC entry 3256 (class 2606 OID 25916)
-- Name: LaneInRFIDTransactionLog PK_LaneInRFIDTransactionLog; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."LaneInRFIDTransactionLog"
    ADD CONSTRAINT "PK_LaneInRFIDTransactionLog" PRIMARY KEY ("Id");


--
-- TOC entry 3319 (class 2606 OID 25731)
-- Name: ManualBarrierControl PK_ManualBarrierControl; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ManualBarrierControl"
    ADD CONSTRAINT "PK_ManualBarrierControl" PRIMARY KEY ("Id");


--
-- TOC entry 3317 (class 2606 OID 25723)
-- Name: ManualBarrierControls PK_ManualBarrierControls; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ManualBarrierControls"
    ADD CONSTRAINT "PK_ManualBarrierControls" PRIMARY KEY ("Id");


--
-- TOC entry 3303 (class 2606 OID 25640)
-- Name: Payment PK_Payment; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Payment"
    ADD CONSTRAINT "PK_Payment" PRIMARY KEY ("Id");


--
-- TOC entry 3307 (class 2606 OID 25647)
-- Name: PaymentStatus PK_PaymentStatus; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."PaymentStatus"
    ADD CONSTRAINT "PK_PaymentStatus" PRIMARY KEY ("Id");


--
-- TOC entry 3272 (class 2606 OID 25120)
-- Name: TimeBlockFee PK_TimeBlockFee; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TimeBlockFee"
    ADD CONSTRAINT "PK_TimeBlockFee" PRIMARY KEY ("Id");


--
-- TOC entry 3283 (class 2606 OID 25914)
-- Name: TimeBlockFeeFormula PK_TimeBlockFeeFormula; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TimeBlockFeeFormula"
    ADD CONSTRAINT "PK_TimeBlockFeeFormula" PRIMARY KEY ("Id");


--
-- TOC entry 3258 (class 2606 OID 25912)
-- Name: Vehicle PK_Vehicle; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Vehicle"
    ADD CONSTRAINT "PK_Vehicle" PRIMARY KEY ("Id");


--
-- TOC entry 3266 (class 2606 OID 25110)
-- Name: VehicleCategory PK_VehicleCategory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."VehicleCategory"
    ADD CONSTRAINT "PK_VehicleCategory" PRIMARY KEY ("Id");


--
-- TOC entry 3268 (class 2606 OID 25115)
-- Name: VehicleGroup PK_VehicleGroup; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."VehicleGroup"
    ADD CONSTRAINT "PK_VehicleGroup" PRIMARY KEY ("Id");


--
-- TOC entry 3252 (class 2606 OID 24595)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 3313 (class 1259 OID 25716)
-- Name: IX_AppConfig_IsApply; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_AppConfig_IsApply" ON public."AppConfig" USING btree ("IsApply");


--
-- TOC entry 3308 (class 1259 OID 25689)
-- Name: IX_ETCCheckout_PaymentId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ETCCheckout_PaymentId" ON public."ETCCheckout" USING btree ("PaymentId");


--
-- TOC entry 3309 (class 1259 OID 25690)
-- Name: IX_ETCCheckout_TransactionId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ETCCheckout_TransactionId" ON public."ETCCheckout" USING btree ("TransactionId");


--
-- TOC entry 3310 (class 1259 OID 25691)
-- Name: IX_ETCCheckout_TransactionId_RFID_PlateNumber; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ETCCheckout_TransactionId_RFID_PlateNumber" ON public."ETCCheckout" USING btree ("TransactionId", "RFID", "PlateNumber");


--
-- TOC entry 3273 (class 1259 OID 25151)
-- Name: IX_FeeVehicleCategory_CustomVehicleTypeId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_FeeVehicleCategory_CustomVehicleTypeId" ON public."FeeVehicleCategory" USING btree ("CustomVehicleTypeId");


--
-- TOC entry 3274 (class 1259 OID 25152)
-- Name: IX_FeeVehicleCategory_FeeTypeId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_FeeVehicleCategory_FeeTypeId" ON public."FeeVehicleCategory" USING btree ("FeeTypeId");


--
-- TOC entry 3275 (class 1259 OID 25153)
-- Name: IX_FeeVehicleCategory_PlateNumber; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_FeeVehicleCategory_PlateNumber" ON public."FeeVehicleCategory" USING btree ("PlateNumber");


--
-- TOC entry 3276 (class 1259 OID 25154)
-- Name: IX_FeeVehicleCategory_RFID; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_FeeVehicleCategory_RFID" ON public."FeeVehicleCategory" USING btree ("RFID");


--
-- TOC entry 3277 (class 1259 OID 25155)
-- Name: IX_FeeVehicleCategory_VehicleCategoryId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_FeeVehicleCategory_VehicleCategoryId" ON public."FeeVehicleCategory" USING btree ("VehicleCategoryId");


--
-- TOC entry 3278 (class 1259 OID 25156)
-- Name: IX_FeeVehicleCategory_VehicleGroupId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_FeeVehicleCategory_VehicleGroupId" ON public."FeeVehicleCategory" USING btree ("VehicleGroupId");


--
-- TOC entry 3284 (class 1259 OID 25215)
-- Name: IX_Fee_CustomVehicleTypeId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Fee_CustomVehicleTypeId" ON public."Fee" USING btree ("CustomVehicleTypeId");


--
-- TOC entry 3285 (class 1259 OID 25576)
-- Name: IX_Fee_LaneInDate; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Fee_LaneInDate" ON public."Fee" USING btree ("LaneInDate");


--
-- TOC entry 3286 (class 1259 OID 25217)
-- Name: IX_Fee_LaneInEpoch; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Fee_LaneInEpoch" ON public."Fee" USING btree ("LaneInEpoch");


--
-- TOC entry 3287 (class 1259 OID 25443)
-- Name: IX_Fee_LaneInId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Fee_LaneInId" ON public."Fee" USING btree ("LaneInId");


--
-- TOC entry 3288 (class 1259 OID 25559)
-- Name: IX_Fee_LaneOutDate; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Fee_LaneOutDate" ON public."Fee" USING btree ("LaneOutDate");


--
-- TOC entry 3289 (class 1259 OID 25220)
-- Name: IX_Fee_LaneOutEpoch; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Fee_LaneOutEpoch" ON public."Fee" USING btree ("LaneOutEpoch");


--
-- TOC entry 3290 (class 1259 OID 25442)
-- Name: IX_Fee_LaneOutId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Fee_LaneOutId" ON public."Fee" USING btree ("LaneOutId");


--
-- TOC entry 3291 (class 1259 OID 25441)
-- Name: IX_Fee_PlateNumber; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Fee_PlateNumber" ON public."Fee" USING btree ("PlateNumber");


--
-- TOC entry 3292 (class 1259 OID 25440)
-- Name: IX_Fee_RFID; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Fee_RFID" ON public."Fee" USING btree ("RFID");


--
-- TOC entry 3293 (class 1259 OID 25439)
-- Name: IX_Fee_TicketId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Fee_TicketId" ON public."Fee" USING btree ("TicketId");


--
-- TOC entry 3304 (class 1259 OID 25658)
-- Name: IX_PaymentStatus_PaymentId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_PaymentStatus_PaymentId" ON public."PaymentStatus" USING btree ("PaymentId");


--
-- TOC entry 3305 (class 1259 OID 25660)
-- Name: IX_PaymentStatus_TransactionId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_PaymentStatus_TransactionId" ON public."PaymentStatus" USING btree ("TransactionId");


--
-- TOC entry 3296 (class 1259 OID 25698)
-- Name: IX_Payment_CustomVehicleTypeId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Payment_CustomVehicleTypeId" ON public."Payment" USING btree ("CustomVehicleTypeId");


--
-- TOC entry 3297 (class 1259 OID 25653)
-- Name: IX_Payment_FeeId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Payment_FeeId" ON public."Payment" USING btree ("FeeId");


--
-- TOC entry 3298 (class 1259 OID 25654)
-- Name: IX_Payment_LaneInId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Payment_LaneInId" ON public."Payment" USING btree ("LaneInId");


--
-- TOC entry 3299 (class 1259 OID 25655)
-- Name: IX_Payment_LaneOutId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Payment_LaneOutId" ON public."Payment" USING btree ("LaneOutId");


--
-- TOC entry 3300 (class 1259 OID 25656)
-- Name: IX_Payment_PlateNumber; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Payment_PlateNumber" ON public."Payment" USING btree ("PlateNumber");


--
-- TOC entry 3301 (class 1259 OID 25657)
-- Name: IX_Payment_RFID; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Payment_RFID" ON public."Payment" USING btree ("RFID");


--
-- TOC entry 3281 (class 1259 OID 25202)
-- Name: IX_TimeBlockFeeFormula_CustomVehicleTypeId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_TimeBlockFeeFormula_CustomVehicleTypeId" ON public."TimeBlockFeeFormula" USING btree ("CustomVehicleTypeId");


--
-- TOC entry 3269 (class 1259 OID 25185)
-- Name: IX_TimeBlockFee_CustomVehicleTypeId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_TimeBlockFee_CustomVehicleTypeId" ON public."TimeBlockFee" USING btree ("CustomVehicleTypeId");


--
-- TOC entry 3270 (class 1259 OID 25166)
-- Name: IX_TimeBlockFee_FromSecond_ToSecond; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_TimeBlockFee_FromSecond_ToSecond" ON public."TimeBlockFee" USING btree ("FromSecond", "ToSecond");


--
-- TOC entry 3332 (class 2606 OID 25684)
-- Name: ETCCheckout FK_ETCCheckout_Payment_PaymentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ETCCheckout"
    ADD CONSTRAINT "FK_ETCCheckout_Payment_PaymentId" FOREIGN KEY ("PaymentId") REFERENCES public."Payment"("Id") ON DELETE CASCADE;


--
-- TOC entry 3323 (class 2606 OID 25131)
-- Name: FeeVehicleCategory FK_FeeVehicleCategory_CustomVehicleType_CustomVehicleTypeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."FeeVehicleCategory"
    ADD CONSTRAINT "FK_FeeVehicleCategory_CustomVehicleType_CustomVehicleTypeId" FOREIGN KEY ("CustomVehicleTypeId") REFERENCES public."CustomVehicleType"("Id") ON DELETE CASCADE;


--
-- TOC entry 3324 (class 2606 OID 25136)
-- Name: FeeVehicleCategory FK_FeeVehicleCategory_FeeType_FeeTypeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."FeeVehicleCategory"
    ADD CONSTRAINT "FK_FeeVehicleCategory_FeeType_FeeTypeId" FOREIGN KEY ("FeeTypeId") REFERENCES public."FeeType"("Id") ON DELETE CASCADE;


--
-- TOC entry 3325 (class 2606 OID 25141)
-- Name: FeeVehicleCategory FK_FeeVehicleCategory_VehicleCategory_VehicleCategoryId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."FeeVehicleCategory"
    ADD CONSTRAINT "FK_FeeVehicleCategory_VehicleCategory_VehicleCategoryId" FOREIGN KEY ("VehicleCategoryId") REFERENCES public."VehicleCategory"("Id") ON DELETE CASCADE;


--
-- TOC entry 3326 (class 2606 OID 25146)
-- Name: FeeVehicleCategory FK_FeeVehicleCategory_VehicleGroup_VehicleGroupId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."FeeVehicleCategory"
    ADD CONSTRAINT "FK_FeeVehicleCategory_VehicleGroup_VehicleGroupId" FOREIGN KEY ("VehicleGroupId") REFERENCES public."VehicleGroup"("Id") ON DELETE CASCADE;


--
-- TOC entry 3328 (class 2606 OID 25446)
-- Name: Fee FK_Fee_CustomVehicleType_CustomVehicleTypeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Fee"
    ADD CONSTRAINT "FK_Fee_CustomVehicleType_CustomVehicleTypeId" FOREIGN KEY ("CustomVehicleTypeId") REFERENCES public."CustomVehicleType"("Id");


--
-- TOC entry 3331 (class 2606 OID 25648)
-- Name: PaymentStatus FK_PaymentStatus_Payment_PaymentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."PaymentStatus"
    ADD CONSTRAINT "FK_PaymentStatus_Payment_PaymentId" FOREIGN KEY ("PaymentId") REFERENCES public."Payment"("Id") ON DELETE CASCADE;


--
-- TOC entry 3329 (class 2606 OID 25699)
-- Name: Payment FK_Payment_CustomVehicleType_CustomVehicleTypeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Payment"
    ADD CONSTRAINT "FK_Payment_CustomVehicleType_CustomVehicleTypeId" FOREIGN KEY ("CustomVehicleTypeId") REFERENCES public."CustomVehicleType"("Id");


--
-- TOC entry 3330 (class 2606 OID 25704)
-- Name: Payment FK_Payment_Fee_FeeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Payment"
    ADD CONSTRAINT "FK_Payment_Fee_FeeId" FOREIGN KEY ("FeeId") REFERENCES public."Fee"("Id");


--
-- TOC entry 3327 (class 2606 OID 25919)
-- Name: TimeBlockFeeFormula FK_TimeBlockFeeFormula_CustomVehicleType_CustomVehicleTypeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TimeBlockFeeFormula"
    ADD CONSTRAINT "FK_TimeBlockFeeFormula_CustomVehicleType_CustomVehicleTypeId" FOREIGN KEY ("CustomVehicleTypeId") REFERENCES public."CustomVehicleType"("Id") ON DELETE CASCADE;


--
-- TOC entry 3322 (class 2606 OID 25186)
-- Name: TimeBlockFee FK_TimeBlockFee_CustomVehicleType_CustomVehicleTypeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TimeBlockFee"
    ADD CONSTRAINT "FK_TimeBlockFee_CustomVehicleType_CustomVehicleTypeId" FOREIGN KEY ("CustomVehicleTypeId") REFERENCES public."CustomVehicleType"("Id") ON DELETE CASCADE;


-- Completed on 2023-10-11 21:47:41

--
-- PostgreSQL database dump complete
--

