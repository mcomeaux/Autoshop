## Description
The solution uses the mediator pattern extensively with CQRS for the database queries.
The Api project contains all of the endpoints and the Application project contains all of the methods and queries
The Infrastructure project facilitates some of the setup required for Entity Framework

# Autoshop DB
To Create and populate the database, Create a new database named Autoshop.
Edit the appsettings file under the dbUpdater project and point the connection string at the new DB you just created.
Run the dbUpdater project.
The project is currently using a SQLite db file that is checked into the repo.
