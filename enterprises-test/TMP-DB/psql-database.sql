create table enterprises (
	id bigint not null primary key generated always as identity,
	created_by varchar(250) not null,
	created_date timestamp not null,
	modified_by varchar(250) null,
	modified_date timestamp null,
	status boolean not null default(true),
	address varchar(250) null,
	name varchar(250) not null,
	phone varchar(10) null
);

create table departments (
	id bigint not null primary key generated always as identity,
	created_by varchar(250) not null,
	created_date timestamp not null,
	modified_by varchar(250) null,
	modified_date timestamp null,
	status boolean not null default(true),
	description varchar(200) null,
	name varchar(150) not null,
	phone varchar(10) null,
	id_enterprise bigint not null,
	constraint FK_enterprises_id_enterprise foreign key (id_enterprise) references enterprises(id)
);

create table employees (
	id bigint not null primary key generated always as identity,
	created_by varchar(250) not null,
	created_date timestamp not null,
	modified_by varchar(250) null,
	modified_date timestamp null,
	status boolean not null default(true),
	age int not null,
	email varchar(150) null,
	name varchar(150) not null,
	position varchar(150) not null,
	surname varchar(100) not null
);

create table departments_employees (
	id bigint not null primary key generated always as identity,
	created_by varchar(250) not null,
	created_date timestamp not null,
	modified_by varchar(250) null,
	modified_date timestamp null,
	status boolean not null default(true),
	id_department bigint not null,
	id_employee bigint not null,
	constraint FK_departments_id_department foreign key (id_department) references departments(id),
	constraint FK_employees_id_employee foreign key (id_employee) references employees(id)
);


