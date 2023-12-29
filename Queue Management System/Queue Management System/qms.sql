--
-- PostgreSQL database dump
--

-- Dumped from database version 16.1
-- Dumped by pg_dump version 16.1

-- Started on 2023-12-29 20:40:40

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
-- TOC entry 217 (class 1259 OID 16413)
-- Name: servedcustomers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.servedcustomers (
    "TransactionID" integer NOT NULL,
    "TicketNumber" text NOT NULL,
    "ServicePointID" text NOT NULL,
    "ShowedUp" boolean,
    "TimeShowedUp" timestamp with time zone,
    "TimeQueued" timestamp with time zone,
    "TimeFinished" timestamp with time zone
);


ALTER TABLE public.servedcustomers OWNER TO postgres;

--
-- TOC entry 216 (class 1259 OID 16412)
-- Name: servedcustomers_ID_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."servedcustomers_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."servedcustomers_ID_seq" OWNER TO postgres;

--
-- TOC entry 4809 (class 0 OID 0)
-- Dependencies: 216
-- Name: servedcustomers_ID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."servedcustomers_ID_seq" OWNED BY public.servedcustomers."TransactionID";


--
-- TOC entry 215 (class 1259 OID 16404)
-- Name: servicepoints; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.servicepoints (
    "ID" text NOT NULL,
    "Description" text,
    "PassKey" text DEFAULT 'pass'::text NOT NULL
);


ALTER TABLE public.servicepoints OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 16432)
-- Name: serviceproviders; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.serviceproviders (
    "ID" integer NOT NULL,
    "Name" text NOT NULL,
    "Email" text NOT NULL,
    "Password" text DEFAULT 'pass'::text NOT NULL,
    "IsAdmin" boolean DEFAULT false NOT NULL
);


ALTER TABLE public.serviceproviders OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 16431)
-- Name: serviceproviders_ID_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."serviceproviders_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."serviceproviders_ID_seq" OWNER TO postgres;

--
-- TOC entry 4810 (class 0 OID 0)
-- Dependencies: 218
-- Name: serviceproviders_ID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."serviceproviders_ID_seq" OWNED BY public.serviceproviders."ID";


--
-- TOC entry 4644 (class 2604 OID 16416)
-- Name: servedcustomers TransactionID; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.servedcustomers ALTER COLUMN "TransactionID" SET DEFAULT nextval('public."servedcustomers_ID_seq"'::regclass);


--
-- TOC entry 4645 (class 2604 OID 16435)
-- Name: serviceproviders ID; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.serviceproviders ALTER COLUMN "ID" SET DEFAULT nextval('public."serviceproviders_ID_seq"'::regclass);


--
-- TOC entry 4801 (class 0 OID 16413)
-- Dependencies: 217
-- Data for Name: servedcustomers; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.servedcustomers ("TransactionID", "TicketNumber", "ServicePointID", "ShowedUp", "TimeShowedUp", "TimeQueued", "TimeFinished") FROM stdin;
3	03dcfba1-906c-4a92-af52-6d6af787dfb6	sp001	t	2023-12-26 12:10:31+03	2023-12-26 12:09:59+03	2023-12-26 12:10:32+03
4	22657716-fbd0-48a5-bcda-b0b443c6d4b2	sp001	f	1999-12-31 23:59:59+03	2023-12-26 12:10:02+03	1999-12-31 23:59:59+03
5	0ace2cee-2dad-48fc-888a-f4fd33e62dcf	sp009	t	2023-12-26 12:11:57+03	2023-12-26 12:10:09+03	2023-12-26 12:11:59+03
6	3264e091-f0d3-4f6d-a6ff-8bb819807c99	sp006	f	1999-12-31 23:59:59+03	2023-12-27 14:26:09+03	1999-12-31 23:59:59+03
7	9e80f3b5-a330-4444-a4bb-93bb84219da5	sp006	f	1999-12-31 23:59:59+03	2023-12-27 14:27:24+03	1999-12-31 23:59:59+03
8	5274b67d-bbc0-462e-a2fa-11224d630f40	sp009	f	1999-12-31 23:59:59+03	2023-12-27 14:30:43+03	1999-12-31 23:59:59+03
9	d784a1c3-06cd-420f-9234-b704c58771a4	sp001	f	1999-12-31 23:59:59+03	2023-12-27 14:31:29+03	1999-12-31 23:59:59+03
10	d48c2ddb-bd59-4d17-bbcf-b55af27360c5	sp001	f	1999-12-31 23:59:59+03	2023-12-27 14:32:33+03	1999-12-31 23:59:59+03
11	541d4947-adf4-4885-8c79-61e9efa98602	sp009	f	1999-12-31 23:59:59+03	2023-12-27 14:40:57+03	1999-12-31 23:59:59+03
12	5dc2e3c7-300b-46e2-b5d1-1ef1f38336f9	sp001	f	1999-12-31 23:59:59+03	2023-12-27 14:45:53+03	1999-12-31 23:59:59+03
13	f8bf7f0d-6a82-4416-87e7-115f0cbd6a73	sp001	f	1999-12-31 23:59:59+03	2023-12-27 14:54:37+03	1999-12-31 23:59:59+03
14	45b65c4f-80c7-4201-b860-e27f5aafb2fb	sp001	f	1999-12-31 23:59:59+03	2023-12-27 14:56:01+03	1999-12-31 23:59:59+03
15	2c78462b-d38c-415b-a76b-77a271ed5a55	sp001	f	1999-12-31 23:59:59+03	2023-12-27 15:35:02+03	1999-12-31 23:59:59+03
16	15f1def1-7812-4148-9567-b7166b8bb95c	sp006	f	1999-12-31 23:59:59+03	2023-12-28 13:39:00+03	1999-12-31 23:59:59+03
17	a56fd595-e1b6-4f9e-b469-3c5111aaf4ae	sp009	t	2023-12-28 13:56:14+03	2023-12-28 13:54:50+03	2023-12-28 13:56:16+03
18	3a8349d0-d9ec-47ac-bdb0-1f5268e51b56	sp001	f	1999-12-31 23:59:59+03	2023-12-28 14:01:10+03	1999-12-31 23:59:59+03
20	16a157c1-9d69-4491-8bb6-db8c21cbde44	sp002	f	1999-12-31 23:59:59+03	2023-12-28 14:05:32+03	1999-12-31 23:59:59+03
19	a3735203-775e-4ac4-acf1-da270274ddd8	sp009	t	2023-12-28 14:10:46+03	2023-12-28 14:05:15+03	2023-12-28 14:10:48+03
21	b955f661-d15e-42ea-91c4-833a2db3f581	sp001	f	1999-12-31 23:59:59+03	2023-12-28 15:43:24+03	1999-12-31 23:59:59+03
22	d8b20b79-106a-4bb9-a656-6dcbcd136b9b	sp001	f	1999-12-31 23:59:59+03	2023-12-28 15:43:35+03	1999-12-31 23:59:59+03
23	e021b5ab-0dba-4a2d-9e12-e4613a2f41f6	sp001	f	1999-12-31 23:59:59+03	2023-12-28 15:43:42+03	1999-12-31 23:59:59+03
24	9920baaf-1c33-484f-9742-cd3ad8c988f9	sp001	f	1999-12-31 23:59:59+03	2023-12-28 15:45:59+03	1999-12-31 23:59:59+03
25	6a2762a5-da15-44c7-96ef-a8c19fd6f072	sp001	f	1999-12-31 23:59:59+03	2023-12-28 15:46:05+03	1999-12-31 23:59:59+03
26	e8f86093-32df-4ffd-ad10-586326297f49	sp001	f	1999-12-31 23:59:59+03	2023-12-28 15:46:10+03	1999-12-31 23:59:59+03
27	6055f103-4c94-40a3-aed2-0611ca470a8f	sp001	t	2023-12-28 15:50:28+03	2023-12-28 15:49:37+03	2023-12-28 15:50:30+03
28	b1b1299a-dcb7-4cab-97d4-ec1861f0c660	sp001	f	1999-12-31 23:59:59+03	2023-12-28 15:49:45+03	1999-12-31 23:59:59+03
29	15c7f6f4-6fd0-483f-a5f7-77659572230e	sp001	f	1999-12-31 23:59:59+03	2023-12-29 16:13:20+03	1999-12-31 23:59:59+03
30	0ae5048f-1d67-4fbb-98a4-d25751151cbe	sp001	f	1999-12-31 23:59:59+03	2023-12-29 16:21:28+03	1999-12-31 23:59:59+03
31	6fce11f1-aad1-4865-a58e-7cde3480e736	sp001	f	1999-12-31 23:59:59+03	2023-12-29 16:39:13+03	1999-12-31 23:59:59+03
32	f9a14c17-1602-4337-b2ac-851ef215110f	sp001	f	1999-12-31 23:59:59+03	2023-12-29 16:40:19+03	1999-12-31 23:59:59+03
33	a49a33b9-cd30-4cf4-9531-6b40930315f3	sp001	f	1999-12-31 23:59:59+03	2023-12-29 16:42:53+03	1999-12-31 23:59:59+03
34	00f732c2-4d06-41cc-9b53-dfbb8ee2d2f1	sp001	f	1999-12-31 23:59:59+03	2023-12-29 16:58:33+03	1999-12-31 23:59:59+03
35	04f08a6f-5748-44be-8388-6f1ee5d54911	sp009	f	1999-12-31 23:59:59+03	2023-12-29 17:03:24+03	1999-12-31 23:59:59+03
36	b21757b2-d599-4a3e-92b1-ea4cf0e80171	sp001	f	1999-12-31 23:59:59+03	2023-12-29 17:06:19+03	1999-12-31 23:59:59+03
37	355b3074-778b-4591-9084-e4b43f32f93b	sp001	f	1999-12-31 23:59:59+03	2023-12-29 17:54:28+03	1999-12-31 23:59:59+03
38	ca45ea2a-d7ee-48ba-a28c-19b03bfd457c	sp001	f	1999-12-31 23:59:59+03	2023-12-29 17:54:34+03	1999-12-31 23:59:59+03
39	0ec536d0-f905-475a-b854-cb4cae5c1844	sp001	f	1999-12-31 23:59:59+03	2023-12-29 17:54:36+03	1999-12-31 23:59:59+03
40	ecf82356-eb68-4447-a31b-906202ad3cb8	sp001	f	1999-12-31 23:59:59+03	2023-12-29 17:59:21+03	1999-12-31 23:59:59+03
41	c0e6f7a0-1042-4d57-83ff-d294cba44c99	sp001	f	1999-12-31 23:59:59+03	2023-12-29 17:59:26+03	1999-12-31 23:59:59+03
42	5f178e15-bcf6-4291-9269-cabf78f77c13	sp001	f	1999-12-31 23:59:59+03	2023-12-29 17:59:29+03	1999-12-31 23:59:59+03
\.


--
-- TOC entry 4799 (class 0 OID 16404)
-- Dependencies: 215
-- Data for Name: servicepoints; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.servicepoints ("ID", "Description", "PassKey") FROM stdin;
sp001	servicepoint 1	pass
sp002	servicepoint2	pass
sp009	servicepoint9	danilo
\.


--
-- TOC entry 4803 (class 0 OID 16432)
-- Dependencies: 219
-- Data for Name: serviceproviders; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.serviceproviders ("ID", "Name", "Email", "Password", "IsAdmin") FROM stdin;
1	dante	dante@gmail.com	danilo	t
2	messi	messi@yahoo.com	messi10	f
3	kolo	kolos@hotmail.com	koloniol	f
\.


--
-- TOC entry 4811 (class 0 OID 0)
-- Dependencies: 216
-- Name: servedcustomers_ID_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."servedcustomers_ID_seq"', 42, true);


--
-- TOC entry 4812 (class 0 OID 0)
-- Dependencies: 218
-- Name: serviceproviders_ID_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."serviceproviders_ID_seq"', 5, true);


--
-- TOC entry 4651 (class 2606 OID 16420)
-- Name: servedcustomers servedcustomers_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.servedcustomers
    ADD CONSTRAINT servedcustomers_pkey PRIMARY KEY ("TransactionID");


--
-- TOC entry 4649 (class 2606 OID 16411)
-- Name: servicepoints servicepoints_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.servicepoints
    ADD CONSTRAINT servicepoints_pkey PRIMARY KEY ("ID");


--
-- TOC entry 4653 (class 2606 OID 16443)
-- Name: serviceproviders serviceproviders_Email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.serviceproviders
    ADD CONSTRAINT "serviceproviders_Email_key" UNIQUE ("Email");


--
-- TOC entry 4655 (class 2606 OID 16441)
-- Name: serviceproviders serviceproviders_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.serviceproviders
    ADD CONSTRAINT serviceproviders_pkey PRIMARY KEY ("ID");


-- Completed on 2023-12-29 20:40:40

--
-- PostgreSQL database dump complete
--

