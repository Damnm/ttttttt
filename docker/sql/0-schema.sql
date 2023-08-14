SET search_path = MainDB;

-- Init test schema
-- Need to be remove when implement
CREATE TABLE IF NOT EXISTS public.test
(
    id uuid NOT NULL,
    code character varying(255) COLLATE pg_catalog."default",
    name character varying(255) COLLATE pg_catalog."default",
    create_date timestamp with time zone,
    CONSTRAINT test_pkey PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.test
    OWNER to postgres;