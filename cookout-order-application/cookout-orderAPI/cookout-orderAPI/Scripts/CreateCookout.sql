-- Switch to the system (aka master) database
USE master;
GO

-- Delete the dvdstore database (IF EXISTS)
DROP DATABASE IF EXISTS cookoutDatabase;
GO

-- Create a new dvdstore database
CREATE DATABASE cookoutDatabase;
GO

-- Switch to the dvdstore database
USE cookoutDatabase
GO


BEGIN TRANSACTION;

CREATE TABLE role_table (
	role_id integer identity NOT NULL,
	role_name varchar(20) NOT NULL,

	CONSTRAINT pk_role_id PRIMARY KEY (role_id),
);

CREATE TABLE users (
    app_user_id integer identity NOT NULL,
	app_user_name varchar(64) NOT NULL,
	email varchar(64) NOT NULL,
	user_hash varchar(64) NOT NULL,
	user_salt varchar(64) NOT NULL,
	role_id integer NOT NULL,

	CONSTRAINT pk_app_user_id PRIMARY KEY (app_user_id),
	CONSTRAINT fk_role_id FOREIGN KEY (role_id) REFERENCES role_table(role_id),
);

CREATE TABLE event_table (
    event_id integer identity NOT NULL,
    app_host_id integer NOT NULL,
	event_name varchar(64) NOT NULL,
	event_description varchar(64) NOT NULL,
	event_date datetime NOT NULL,
    
    CONSTRAINT pk_event_id PRIMARY KEY (event_id),
	CONSTRAINT fk_app_user_id_host FOREIGN KEY (app_host_id) REFERENCES users(app_user_id),
);

CREATE TABLE guest (
	guest_id integer identity NOT NULL,
	event_id integer NOT NULL,
	app_user_id integer NOT NULL,
	--confirmed_rsvp bit NOT NULL,

	CONSTRAINT pk_guest_id PRIMARY KEY (guest_id), 
	CONSTRAINT fk_app_user_id_guest FOREIGN KEY (app_user_id) REFERENCES users(app_user_id),
	CONSTRAINT fk_event_id_guest FOREIGN KEY (event_id) REFERENCES event_table(event_id),
);

CREATE TABLE orders (
	order_id integer identity NOT NULL,
	app_user_id integer NOT NULL,
	event_id integer NOT NULL,
	completed bit NOT NULL,
	
	CONSTRAINT pk_order_id PRIMARY KEY (order_id),
	CONSTRAINT fk_event_id_orders FOREIGN KEY (event_id) REFERENCES event_table(event_id),
	CONSTRAINT fk_app_user_id FOREIGN KEY (app_user_id) REFERENCES users(app_user_id),
	);

CREATE TABLE item (
	item_id integer identity NOT NULL,
	item_name varchar(64) NOT NULL,
	item_description varchar(120) NOT NULL,

	CONSTRAINT pk_item_id PRIMARY KEY (item_id),
);

CREATE TABLE order_items (
	order_items_id int identity NOT NULL,
	order_id integer NOT NULL,
	item_id integer NOT NULL,
	item_quantity integer NOT NULL,

	CONSTRAINT pk_order_items_id PRIMARY KEY (order_items_id),
	CONSTRAINT fk_order_id FOREIGN KEY (order_id) REFERENCES orders(order_id),
	CONSTRAINT fk_item_id_order_items FOREIGN KEY (item_id) REFERENCES item(item_id),
);

CREATE TABLE event_item (
	item_id integer NOT NULL,
	event_id integer NOT NULL,

	CONSTRAINT fk_item_id_event_item FOREIGN KEY (item_id) REFERENCES item(item_id),
	CONSTRAINT fk_event_id_event_item FOREIGN KEY (event_id) REFERENCES event_table(event_id),
);
INSERT INTO role_table (role_name) values('guest');
INSERT INTO role_table (role_name) values('host');
INSERT INTO role_table (role_name) values('chef');

INSERT INTO item (item_name, item_description) values('cheeseburger', 'burger with cheese');
INSERT INTO item (item_name, item_description) values('burger', 'plain ol burger');
INSERT INTO item (item_name, item_description) values('fries', 'fried potato wedges');
INSERT INTO item (item_name, item_description) values('hot dog', 'frankenfurter on a bun');
INSERT INTO item (item_name, item_description) values('chili dog', 'hot dog with more heartburn');
INSERT INTO item (item_name, item_description) values('hipster salad', 'traditional salad with some unnecessary ingredient no one asked for');
INSERT INTO item (item_name, item_description) values('soft pretzel', 'giant pretzel');


COMMIT TRANSACTION;