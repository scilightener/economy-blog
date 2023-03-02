# economy-blog

# What it is
* This is an Economic blog - a place where you can communicate on topics of interest to you with other authorized users, read site news published by the administration, and filter news by topics of interest to you

# How to run this project
1) Create a database in MS SQL SERVER named `dev_basics_sem1`
2) Run the scripts (from the `db_scripts` folder) create_db.sql and Topics_INSERT.sql in the newly created database
3) Change the data source in `_connectionString` in the constructor in the `EconomyBlog\ORM\MyORM.cs` file to the one you have in the `connectionString` of your created database
4) Being in the root of the project directory (i.e. economy-blog\EconomyBlog), start `dotnet run` in the terminal (or add the Views folder to the following directory: `economy-blog\EconomyBlog\bin\Debug\net6.0` and run the project in your IDE )
5) Go to http://localhost:7700/register/ and register

# Demo

> https://user-images.githubusercontent.com/45340222/222579317-35f83e28-8953-4c37-8b50-c319eb3896a2.mp4

# Details
* Unathorized users can't see news or profiles of other users (neither the profile of itself for obvious reasons) and some other site functionality
* An example of the error message, got by an unaouthorized attempt to go to the `Secret` section
> <img width="1024" alt="error_example" src="https://user-images.githubusercontent.com/45340222/222579765-ad240fe1-ed0f-4d25-a042-5055caf75bcc.png">
* There were some adaptive design attempts but... well, let's just say that it is usable on mobile
* Every user can edit any post made by him, but cannot delete it (and of course, can't edit another user's post)
* If you're authorized under admin account, you can post news that every user will see on the `news` page

# Implementation details
* Written entirely in C# using pure html+css
* Written without use of any framework. All functionality of the server (reacting to HttpRequests) is written only using the HttpListener class
* ORM, DAO, dynamic routing, authorization check and other stuff implemented without the use of external libraries
