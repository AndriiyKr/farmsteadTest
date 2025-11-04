BEGIN;

CREATE TABLE IF NOT EXISTS public.users
(
    id bigint PRIMARY KEY NOT NULL,
    email character varying(254) NOT NULL,
    password character varying(128) NOT NULL,
    username character varying(128) NOT NULL,
    is_Superuser boolean NOT NULL DEFAULT false,
    firstname character varying(128),
    lastname character varying(128),
    is_Active boolean NOT NULL DEFAULT true,
    avatar character varying(100)
);

CREATE TABLE IF NOT EXISTS public.maps
(
    id bigint PRIMARY KEY NOT NULL,
	name character varying(50),
    map jsonb NOT NULL,
    is_Private boolean NOT NULL DEFAULT false,
    user_id bigint NOT NULL,
	CONSTRAINT map_user_fk
    	FOREIGN KEY (user_id)
        REFERENCES public.users (id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS public.trees
(
    id bigint PRIMARY KEY NOT NULL,
    name character varying(50) NOT NULL,
    image text NOT NULL
);


CREATE TABLE IF NOT EXISTS public.tree_sorts
(
    id bigint PRIMARY KEY NOT NULL,
    name character varying(150) NOT NULL,
    ground_type character varying(15) NOT NULL,
    tree_id bigint NOT NULL,
	CONSTRAINT sorts_tree_fk
    	FOREIGN KEY (tree_id)
        REFERENCES public.trees (id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS public.vegetables
(
    id bigint PRIMARY KEY NOT NULL,
    name character varying(50) NOT NULL
);

CREATE TABLE IF NOT EXISTS public.trees_incompatibility
(
    tree1_id bigint NOT NULL,
    tree2_id bigint NOT NULL,
    CONSTRAINT tree_incompatibility_id PRIMARY KEY (tree1_id, tree2_id),
	CONSTRAINT trees_incompatibility_unique UNIQUE (tree1_id, tree2_id),
    CONSTRAINT trees_incompatibility_check CHECK (tree1_id < tree2_id),
	CONSTRAINT incompatibility_tree1_fk
    	FOREIGN KEY (tree1_id)
        REFERENCES public.trees (id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
	CONSTRAINT incompatibility_tree2_fk
    	FOREIGN KEY (tree2_id)
        REFERENCES public.trees (id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS public.veg_sorts
(
    id bigint PRIMARY KEY NOT NULL,
    name character varying(100) NOT NULL,
    image text NOT NULL,
    ground_type character varying(15) NOT NULL,
    veg_id bigint NOT NULL,
	CONSTRAINT sorts_veg_fk
    	FOREIGN KEY (veg_id)
        REFERENCES public.vegetables (id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);


CREATE TABLE IF NOT EXISTS public.veg_incompatibility
(
    veg1_id bigint NOT NULL,
    veg2_id bigint NOT NULL,
    CONSTRAINT veg_incompatibility_id PRIMARY KEY (veg1_id, veg2_id),
	CONSTRAINT veg_incompatibility_unique UNIQUE (veg1_id, veg2_id),
    CONSTRAINT veg_incompatibility_check CHECK (veg1_id < veg2_id),
	CONSTRAINT incompatibility_veg1_fk
    	FOREIGN KEY (veg1_id)
        REFERENCES public.vegetables (id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
	CONSTRAINT incompatibility_veg2_fk
    	FOREIGN KEY (veg2_id)
        REFERENCES public.vegetables (id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS public.flowers
(
    id bigint PRIMARY KEY NOT NULL,
    name character varying(50) NOT NULL,
    ground_type character varying(15) NOT NULL,
    image text NOT NULL
);

END;