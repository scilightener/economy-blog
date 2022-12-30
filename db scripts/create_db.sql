 --drop table Posts
 --drop table Users
 --drop table UsersFavoriteTopics
 --drop table News
 --drop table Topics
 --drop table Sessions
-- uncomment if already created

create table Topics(
	id int primary key identity(0,1),
	name varchar(20) not null,
	description varchar(max) not null
)

create table Users(
	id int primary key identity(1, 1),
	login varchar(20) not null unique,
	password varchar(max) not null,
	first_name varchar(30) default 'John',
	last_name varchar(30) default 'Doe',
	age int default 42,
	education varchar(100) default 'None',
	job varchar(50) default 'None',
	risk_index int default 0,
	favorite_topics int
)

create table UsersFavoriteTopics(
	id int foreign key references Users(id) primary key identity(1, 1),
	topic1 int foreign key references Topics(id),
	topic2 int foreign key references Topics(id),
	topic3 int foreign key references Topics(id),
	topic4 int foreign key references Topics(id),
	topic5 int foreign key references Topics(id)
)

create table Posts(
	id int primary key identity(1, 1),
	title varchar(100),
	text varchar(1000),
	author varchar(20) foreign key references Users(login),
	publication_date datetime
)

create table News(
	id int primary key identity(1, 1),
	title varchar(100),
	text varchar(max),
	publication_date datetime,
	topic1 int foreign key references Topics(id),
	topic2 int foreign key references Topics(id),
	topic3 int foreign key references Topics(id)
)

create table Sessions(
	guid varchar(70) primary key,
	id int,
	login varchar(20),
	created_at datetime
)