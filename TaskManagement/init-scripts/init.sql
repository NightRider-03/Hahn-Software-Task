-- Table: public.tasks

-- DROP TABLE IF EXISTS public.tasks;

CREATE TABLE IF NOT EXISTS public.tasks
(
    "Id" uuid NOT NULL,
    "Title" character varying(200) COLLATE pg_catalog."default" NOT NULL,
    "Description" character varying(1000) COLLATE pg_catalog."default" NOT NULL,
    "Status" text COLLATE pg_catalog."default" NOT NULL,
    "DueDate" timestamp with time zone,
    "Priority" integer NOT NULL DEFAULT 1,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT now(),
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_tasks" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.tasks
    OWNER to postgres;
-- Index: IX_tasks_DueDate

-- DROP INDEX IF EXISTS public."IX_tasks_DueDate";

CREATE INDEX IF NOT EXISTS "IX_tasks_DueDate"
    ON public.tasks USING btree
    ("DueDate" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: IX_tasks_Priority

-- DROP INDEX IF EXISTS public."IX_tasks_Priority";

CREATE INDEX IF NOT EXISTS "IX_tasks_Priority"
    ON public.tasks USING btree
    ("Priority" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: IX_tasks_Status

-- DROP INDEX IF EXISTS public."IX_tasks_Status";

CREATE INDEX IF NOT EXISTS "IX_tasks_Status"
    ON public.tasks USING btree
    ("Status" COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;