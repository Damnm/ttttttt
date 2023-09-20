SET search_path = dbtest;

-- Table: public.CustomVehicleType
CREATE TABLE IF NOT EXISTS public."CustomVehicleType"
(
    "Id" uuid NOT NULL,
    "Name" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "Desc" character varying(255) COLLATE pg_catalog."default",
    "CreatedDate" timestamp without time zone,
    CONSTRAINT "PK_CustomVehicleType" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."CustomVehicleType"
    OWNER to postgres;

-- Table: public.FeeType
CREATE TABLE IF NOT EXISTS public."FeeType"
(
    "Id" uuid NOT NULL,
    "Name" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "Amount" double precision,
    "Desc" character varying(255) COLLATE pg_catalog."default",
    "CreatedDate" timestamp without time zone,
    CONSTRAINT "PK_FeeType" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."FeeType"
    OWNER to postgres;


-- Table: public.VehicleCategory
CREATE TABLE IF NOT EXISTS public."VehicleCategory"
(
    "Id" uuid NOT NULL,
    "Name" character varying(100) COLLATE pg_catalog."default",
    "Desc" character varying(255) COLLATE pg_catalog."default",
    "CreatedDate" timestamp without time zone,
    CONSTRAINT "PK_VehicleCategory" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."VehicleCategory"
    OWNER to postgres;


-- Table: public.VehicleGroup
CREATE TABLE IF NOT EXISTS public."VehicleGroup"
(
    "Id" uuid NOT NULL,
    "Name" character varying(100) COLLATE pg_catalog."default",
    "Desc" character varying(255) COLLATE pg_catalog."default",
    "CreatedDate" timestamp without time zone,
    CONSTRAINT "PK_VehicleGroup" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."VehicleGroup"
    OWNER to postgres;

-- Table: public.TimeBlockFee
CREATE TABLE IF NOT EXISTS public."TimeBlockFee"
(
    "Id" uuid NOT NULL,
    "CustomVehicleTypeId" uuid DEFAULT '00000000-0000-0000-0000-000000000000'::uuid NOT NULL,
    "FromSecond" bigint NOT NULL,
    "ToSecond" bigint NOT NULL,
    "BlockDurationInSeconds" integer,
    "Amount" double precision,
    "CreatedDate" timestamp without time zone,
    "BlockNumber" integer DEFAULT 0 NOT NULL,
    CONSTRAINT "PK_TimeBlockFee" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_TimeBlockFee_CustomVehicleType_CustomVehicleTypeId" FOREIGN KEY ("CustomVehicleTypeId")
        REFERENCES public."CustomVehicleType" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."TimeBlockFee"
    OWNER to postgres;
-- Index: IX_TimeBlockFee_CustomVehicleTypeId
CREATE INDEX IF NOT EXISTS "IX_TimeBlockFee_CustomVehicleTypeId"
    ON public."TimeBlockFee" USING btree
    ("CustomVehicleTypeId" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: IX_TimeBlockFee_FromSecond_ToSecond
CREATE INDEX IF NOT EXISTS "IX_TimeBlockFee_FromSecond_ToSecond"
    ON public."TimeBlockFee" USING btree
    ("FromSecond" ASC NULLS LAST, "ToSecond" ASC NULLS LAST)
    TABLESPACE pg_default;


-- Table: public.FeeVehicleCategory
CREATE TABLE IF NOT EXISTS public."FeeVehicleCategory"
(
    "Id" uuid NOT NULL,
    "VehicleCategoryId" uuid NOT NULL,
    "FeeTypeId" uuid NOT NULL,
    "VehicleGroupId" uuid NOT NULL,
    "CustomVehicleTypeId" uuid NOT NULL,
    "PlateNumber" character varying(20) COLLATE pg_catalog."default",
    "RFID" character varying(50) COLLATE pg_catalog."default",
    "IsTCPVehicle" boolean NOT NULL,
    "ValidFrom" timestamp without time zone NOT NULL,
    "ValidTo" timestamp without time zone,
    "CreatedDate" timestamp without time zone,
    CONSTRAINT "PK_FeeVehicleCategory" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_FeeVehicleCategory_CustomVehicleType_CustomVehicleTypeId" FOREIGN KEY ("CustomVehicleTypeId")
        REFERENCES public."CustomVehicleType" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    CONSTRAINT "FK_FeeVehicleCategory_FeeType_FeeTypeId" FOREIGN KEY ("FeeTypeId")
        REFERENCES public."FeeType" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    CONSTRAINT "FK_FeeVehicleCategory_VehicleCategory_VehicleCategoryId" FOREIGN KEY ("VehicleCategoryId")
        REFERENCES public."VehicleCategory" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    CONSTRAINT "FK_FeeVehicleCategory_VehicleGroup_VehicleGroupId" FOREIGN KEY ("VehicleGroupId")
        REFERENCES public."VehicleGroup" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."FeeVehicleCategory"
    OWNER to postgres;
-- Index: IX_FeeVehicleCategory_CustomVehicleTypeId
CREATE INDEX IF NOT EXISTS "IX_FeeVehicleCategory_CustomVehicleTypeId"
    ON public."FeeVehicleCategory" USING btree
    ("CustomVehicleTypeId" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: IX_FeeVehicleCategory_FeeTypeId
CREATE INDEX IF NOT EXISTS "IX_FeeVehicleCategory_FeeTypeId"
    ON public."FeeVehicleCategory" USING btree
    ("FeeTypeId" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: IX_FeeVehicleCategory_PlateNumber
CREATE INDEX IF NOT EXISTS "IX_FeeVehicleCategory_PlateNumber"
    ON public."FeeVehicleCategory" USING btree
    ("PlateNumber" COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: IX_FeeVehicleCategory_RFID
CREATE INDEX IF NOT EXISTS "IX_FeeVehicleCategory_RFID"
    ON public."FeeVehicleCategory" USING btree
    ("RFID" COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: IX_FeeVehicleCategory_VehicleCategoryId
CREATE INDEX IF NOT EXISTS "IX_FeeVehicleCategory_VehicleCategoryId"
    ON public."FeeVehicleCategory" USING btree
    ("VehicleCategoryId" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: IX_FeeVehicleCategory_VehicleGroupId
CREATE INDEX IF NOT EXISTS "IX_FeeVehicleCategory_VehicleGroupId"
    ON public."FeeVehicleCategory" USING btree
    ("VehicleGroupId" ASC NULLS LAST)
    TABLESPACE pg_default;


-- Table: public.Vehicles
CREATE TABLE IF NOT EXISTS public."Vehicles"
(
    "Id" uuid NOT NULL,
    "RFID" text COLLATE pg_catalog."default",
    "PlateNumber" text COLLATE pg_catalog."default",
    "PlateColor" text COLLATE pg_catalog."default",
    "Make" text COLLATE pg_catalog."default",
    "Seat" integer,
    "Weight" integer,
    "VehicleType" text COLLATE pg_catalog."default",
    "CreatedDate" timestamp without time zone,
    CONSTRAINT "PK_Vehicles" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."Vehicles"
    OWNER to postgres;

-- Table: public.Fusions

-- DROP TABLE IF EXISTS public."Fusions";

CREATE TABLE IF NOT EXISTS public."Fusions"
(
    "Id" uuid NOT NULL,
    "Epoch" real NOT NULL,
    "Loop1" boolean NOT NULL,
    "RFID" boolean NOT NULL,
    "Cam1" character varying(15) COLLATE pg_catalog."default",
    "Loop2" boolean NOT NULL,
    "Cam2" character varying(15) COLLATE pg_catalog."default",
    "Loop3" boolean NOT NULL,
    "ReversedLoop1" boolean NOT NULL,
    "ReversedLoop2" boolean NOT NULL,
    CONSTRAINT "PK_Fusions" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."Fusions"
    OWNER to postgres;

-- Table: public.TimeBlockFeeFormulas
CREATE TABLE IF NOT EXISTS public."TimeBlockFeeFormulas"
(
    "Id" uuid NOT NULL,
    "CustomVehicleTypeId" uuid NOT NULL,
    "FromBlockNumber" integer NOT NULL,
    "Amount" double precision NOT NULL,
    "IntervalInSeconds" bigint NOT NULL,
    "ApplyDate" timestamp without time zone NOT NULL,
    "CreatedDate" timestamp without time zone,
    CONSTRAINT "PK_TimeBlockFeeFormulas" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_TimeBlockFeeFormulas_CustomVehicleType_CustomVehicleTypeId" FOREIGN KEY ("CustomVehicleTypeId")
        REFERENCES public."CustomVehicleType" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."TimeBlockFeeFormulas"
    OWNER to postgres;
-- Index: IX_TimeBlockFeeFormulas_CustomVehicleTypeId
CREATE INDEX IF NOT EXISTS "IX_TimeBlockFeeFormulas_CustomVehicleTypeId"
    ON public."TimeBlockFeeFormulas" USING btree
    ("CustomVehicleTypeId" ASC NULLS LAST)
    TABLESPACE pg_default;






-- Table: public.Fee

-- DROP TABLE IF EXISTS public."Fee";

CREATE TABLE IF NOT EXISTS public."Fee"
(
    "Id" uuid NOT NULL,
    "ObjectId" uuid NOT NULL,
    "LaneInId" character varying(10) COLLATE pg_catalog."default",
    "LaneInDate" timestamp without time zone,
    "LaneInEpoch" bigint,
    "LaneOutId" character varying(10) COLLATE pg_catalog."default",
    "LaneOutDate" timestamp without time zone,
    "LaneOutEpoch" bigint,
    "Duration" integer NOT NULL,
    "RFID" character varying(50) COLLATE pg_catalog."default",
    "Make" character varying(150) COLLATE pg_catalog."default",
    "Model" character varying(150) COLLATE pg_catalog."default",
    "PlateNumber" character varying(20) COLLATE pg_catalog."default",
    "PlateColour" character varying(50) COLLATE pg_catalog."default",
    "CustomVehicleTypeId" uuid,
    "Seat" integer,
    "Weight" integer,
    "LaneInPlateNumberPhotoUrl" character varying(255) COLLATE pg_catalog."default",
    "LaneInVehiclePhotoUrl" character varying(255) COLLATE pg_catalog."default",
    "LaneOutPlateNumberPhotoUrl" character varying(255) COLLATE pg_catalog."default",
    "LaneOutVehiclePhotoUrl" character varying(255) COLLATE pg_catalog."default",
    "ConfidenceScore" real,
    "Amount" double precision NOT NULL,
    "VehicleCategoryId" uuid,
    "TicketTypeId" character varying(50) COLLATE pg_catalog."default",
    "TicketId" character varying(50) COLLATE pg_catalog."default",
    "ShiftId" uuid,
    "EmployeeId" character varying(20),
    "CreatedDate" timestamp without time zone,
    CONSTRAINT "PK_Fee" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Fee_CustomVehicleType_CustomVehicleTypeId" FOREIGN KEY ("CustomVehicleTypeId")
        REFERENCES public."CustomVehicleType" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."Fee"
    OWNER to postgres;
-- Index: IX_Fee_CustomVehicleTypeId

-- DROP INDEX IF EXISTS public."IX_Fee_CustomVehicleTypeId";

CREATE INDEX IF NOT EXISTS "IX_Fee_CustomVehicleTypeId"
    ON public."Fee" USING btree
    ("CustomVehicleTypeId" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: IX_Fee_LaneInDate

-- DROP INDEX IF EXISTS public."IX_Fee_LaneInDate";

CREATE INDEX IF NOT EXISTS "IX_Fee_LaneInDate"
    ON public."Fee" USING btree
    ("LaneInDate" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: IX_Fee_LaneInEpoch

-- DROP INDEX IF EXISTS public."IX_Fee_LaneInEpoch";

CREATE INDEX IF NOT EXISTS "IX_Fee_LaneInEpoch"
    ON public."Fee" USING btree
    ("LaneInEpoch" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: IX_Fee_LaneInId

-- DROP INDEX IF EXISTS public."IX_Fee_LaneInId";

CREATE INDEX IF NOT EXISTS "IX_Fee_LaneInId"
    ON public."Fee" USING btree
    ("LaneInId" COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: IX_Fee_LaneOutDate

-- DROP INDEX IF EXISTS public."IX_Fee_LaneOutDate";

CREATE INDEX IF NOT EXISTS "IX_Fee_LaneOutDate"
    ON public."Fee" USING btree
    ("LaneOutDate" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: IX_Fee_LaneOutEpoch

-- DROP INDEX IF EXISTS public."IX_Fee_LaneOutEpoch";

CREATE INDEX IF NOT EXISTS "IX_Fee_LaneOutEpoch"
    ON public."Fee" USING btree
    ("LaneOutEpoch" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: IX_Fee_LaneOutId

-- DROP INDEX IF EXISTS public."IX_Fee_LaneOutId";

CREATE INDEX IF NOT EXISTS "IX_Fee_LaneOutId"
    ON public."Fee" USING btree
    ("LaneOutId" COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: IX_Fee_PlateNumber

-- DROP INDEX IF EXISTS public."IX_Fee_PlateNumber";

CREATE INDEX IF NOT EXISTS "IX_Fee_PlateNumber"
    ON public."Fee" USING btree
    ("PlateNumber" COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: IX_Fee_RFID

-- DROP INDEX IF EXISTS public."IX_Fee_RFID";

CREATE INDEX IF NOT EXISTS "IX_Fee_RFID"
    ON public."Fee" USING btree
    ("RFID" COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: IX_Fee_TicketId

-- DROP INDEX IF EXISTS public."IX_Fee_TicketId";

CREATE INDEX IF NOT EXISTS "IX_Fee_TicketId"
    ON public."Fee" USING btree
    ("TicketId" COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;