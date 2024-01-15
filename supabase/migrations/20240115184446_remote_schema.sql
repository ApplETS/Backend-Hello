
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

CREATE EXTENSION IF NOT EXISTS "pgsodium" WITH SCHEMA "pgsodium";

CREATE SCHEMA IF NOT EXISTS "supabase_migrations";

ALTER SCHEMA "supabase_migrations" OWNER TO "postgres";

CREATE EXTENSION IF NOT EXISTS "pg_graphql" WITH SCHEMA "graphql";

CREATE EXTENSION IF NOT EXISTS "pg_stat_statements" WITH SCHEMA "extensions";

CREATE EXTENSION IF NOT EXISTS "pgcrypto" WITH SCHEMA "extensions";

CREATE EXTENSION IF NOT EXISTS "pgjwt" WITH SCHEMA "extensions";

CREATE EXTENSION IF NOT EXISTS "supabase_vault" WITH SCHEMA "vault";

CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA "extensions";

SET default_tablespace = '';

SET default_table_access_method = "heap";

CREATE TABLE IF NOT EXISTS "public"."event" (
    "id" bigint NOT NULL,
    "title" text NOT NULL,
    "content" text NOT NULL,
    "image_url" text,
    "state" text NOT NULL,
    "moderator_id" bigint,
    "organizer_id" bigint NOT NULL,
    "publication_date" timestamp with time zone NOT NULL,
    "event_date" timestamp with time zone NOT NULL
);

ALTER TABLE "public"."event" OWNER TO "postgres";

ALTER TABLE "public"."event" ALTER COLUMN "id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME "public"."event_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);

CREATE TABLE IF NOT EXISTS "public"."moderator" (
    "id" bigint NOT NULL,
    "name" text NOT NULL,
    "email" text NOT NULL
);

ALTER TABLE "public"."moderator" OWNER TO "postgres";

ALTER TABLE "public"."moderator" ALTER COLUMN "id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME "public"."moderator_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);

CREATE TABLE IF NOT EXISTS "public"."organizer" (
    "id" bigint NOT NULL,
    "name" text NOT NULL,
    "email" text NOT NULL,
    "organisation" text NOT NULL,
    "activity_area" text NOT NULL
);

ALTER TABLE "public"."organizer" OWNER TO "postgres";

ALTER TABLE "public"."organizer" ALTER COLUMN "id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME "public"."organizer_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);

CREATE TABLE IF NOT EXISTS "public"."publication" (
    "id" bigint NOT NULL,
    "title" text NOT NULL,
    "content" text NOT NULL,
    "image_url" text,
    "state" text NOT NULL,
    "moderator_id" bigint,
    "organizer_id" bigint NOT NULL,
    "publication_date" timestamp with time zone NOT NULL
);

ALTER TABLE "public"."publication" OWNER TO "postgres";

ALTER TABLE "public"."publication" ALTER COLUMN "id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME "public"."publication_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);

CREATE TABLE IF NOT EXISTS "public"."report" (
    "id" bigint NOT NULL,
    "reason" text NOT NULL,
    "date" timestamp with time zone DEFAULT timezone('utc'::text, now()) NOT NULL
);

ALTER TABLE "public"."report" OWNER TO "postgres";

ALTER TABLE "public"."report" ALTER COLUMN "id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME "public"."report_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);

CREATE TABLE IF NOT EXISTS "public"."tag" (
    "id" bigint NOT NULL,
    "name" text NOT NULL,
    "priority_value" integer NOT NULL,
    "parent_tag_id" bigint
);

ALTER TABLE "public"."tag" OWNER TO "postgres";

ALTER TABLE "public"."tag" ALTER COLUMN "id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME "public"."tag_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);

CREATE TABLE IF NOT EXISTS "supabase_migrations"."schema_migrations" (
    "version" text NOT NULL,
    "statements" text[],
    "name" text
);

ALTER TABLE "supabase_migrations"."schema_migrations" OWNER TO "postgres";

ALTER TABLE ONLY "public"."event"
    ADD CONSTRAINT "event_pkey" PRIMARY KEY ("id");

ALTER TABLE ONLY "public"."moderator"
    ADD CONSTRAINT "moderator_pkey" PRIMARY KEY ("id");

ALTER TABLE ONLY "public"."organizer"
    ADD CONSTRAINT "organizer_pkey" PRIMARY KEY ("id");

ALTER TABLE ONLY "public"."publication"
    ADD CONSTRAINT "publication_pkey" PRIMARY KEY ("id");

ALTER TABLE ONLY "public"."report"
    ADD CONSTRAINT "report_pkey" PRIMARY KEY ("id");

ALTER TABLE ONLY "public"."tag"
    ADD CONSTRAINT "tag_pkey" PRIMARY KEY ("id");

ALTER TABLE ONLY "supabase_migrations"."schema_migrations"
    ADD CONSTRAINT "schema_migrations_pkey" PRIMARY KEY ("version");

ALTER TABLE ONLY "public"."event"
    ADD CONSTRAINT "event_moderator_id_fkey" FOREIGN KEY (moderator_id) REFERENCES public.moderator(id);

ALTER TABLE ONLY "public"."event"
    ADD CONSTRAINT "event_organizer_id_fkey" FOREIGN KEY (organizer_id) REFERENCES public.organizer(id);

ALTER TABLE ONLY "public"."publication"
    ADD CONSTRAINT "publication_moderator_id_fkey" FOREIGN KEY (moderator_id) REFERENCES public.moderator(id);

ALTER TABLE ONLY "public"."publication"
    ADD CONSTRAINT "publication_organizer_id_fkey" FOREIGN KEY (organizer_id) REFERENCES public.organizer(id);

ALTER TABLE ONLY "public"."tag"
    ADD CONSTRAINT "tag_parent_tag_id_fkey" FOREIGN KEY (parent_tag_id) REFERENCES public.tag(id);

GRANT USAGE ON SCHEMA "public" TO "postgres";
GRANT USAGE ON SCHEMA "public" TO "anon";
GRANT USAGE ON SCHEMA "public" TO "authenticated";
GRANT USAGE ON SCHEMA "public" TO "service_role";

GRANT ALL ON TABLE "public"."event" TO "anon";
GRANT ALL ON TABLE "public"."event" TO "authenticated";
GRANT ALL ON TABLE "public"."event" TO "service_role";

GRANT ALL ON SEQUENCE "public"."event_id_seq" TO "anon";
GRANT ALL ON SEQUENCE "public"."event_id_seq" TO "authenticated";
GRANT ALL ON SEQUENCE "public"."event_id_seq" TO "service_role";

GRANT ALL ON TABLE "public"."moderator" TO "anon";
GRANT ALL ON TABLE "public"."moderator" TO "authenticated";
GRANT ALL ON TABLE "public"."moderator" TO "service_role";

GRANT ALL ON SEQUENCE "public"."moderator_id_seq" TO "anon";
GRANT ALL ON SEQUENCE "public"."moderator_id_seq" TO "authenticated";
GRANT ALL ON SEQUENCE "public"."moderator_id_seq" TO "service_role";

GRANT ALL ON TABLE "public"."organizer" TO "anon";
GRANT ALL ON TABLE "public"."organizer" TO "authenticated";
GRANT ALL ON TABLE "public"."organizer" TO "service_role";

GRANT ALL ON SEQUENCE "public"."organizer_id_seq" TO "anon";
GRANT ALL ON SEQUENCE "public"."organizer_id_seq" TO "authenticated";
GRANT ALL ON SEQUENCE "public"."organizer_id_seq" TO "service_role";

GRANT ALL ON TABLE "public"."publication" TO "anon";
GRANT ALL ON TABLE "public"."publication" TO "authenticated";
GRANT ALL ON TABLE "public"."publication" TO "service_role";

GRANT ALL ON SEQUENCE "public"."publication_id_seq" TO "anon";
GRANT ALL ON SEQUENCE "public"."publication_id_seq" TO "authenticated";
GRANT ALL ON SEQUENCE "public"."publication_id_seq" TO "service_role";

GRANT ALL ON TABLE "public"."report" TO "anon";
GRANT ALL ON TABLE "public"."report" TO "authenticated";
GRANT ALL ON TABLE "public"."report" TO "service_role";

GRANT ALL ON SEQUENCE "public"."report_id_seq" TO "anon";
GRANT ALL ON SEQUENCE "public"."report_id_seq" TO "authenticated";
GRANT ALL ON SEQUENCE "public"."report_id_seq" TO "service_role";

GRANT ALL ON TABLE "public"."tag" TO "anon";
GRANT ALL ON TABLE "public"."tag" TO "authenticated";
GRANT ALL ON TABLE "public"."tag" TO "service_role";

GRANT ALL ON SEQUENCE "public"."tag_id_seq" TO "anon";
GRANT ALL ON SEQUENCE "public"."tag_id_seq" TO "authenticated";
GRANT ALL ON SEQUENCE "public"."tag_id_seq" TO "service_role";

ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON SEQUENCES  TO "postgres";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON SEQUENCES  TO "anon";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON SEQUENCES  TO "authenticated";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON SEQUENCES  TO "service_role";

ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON FUNCTIONS  TO "postgres";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON FUNCTIONS  TO "anon";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON FUNCTIONS  TO "authenticated";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON FUNCTIONS  TO "service_role";

ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON TABLES  TO "postgres";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON TABLES  TO "anon";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON TABLES  TO "authenticated";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON TABLES  TO "service_role";

RESET ALL;
