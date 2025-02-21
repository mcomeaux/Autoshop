# Problem Statement
An Automotive repair shop currently uses a notebook to intake customer vehicles, document the work to be done
and determine payment. This project will attempt to create a more robust and streamlined process that allows a 
user to search for customer information, track work being done and give a history of previous work and payments.

## Description
The solution uses the mediator pattern extensively with CQRS for the database queries.
The Api project contains all of the endpoints and the Application project contains all of the methods and queries
The Infrastructure project facilitates some of the setup required for Entity Framework

The launchsettings.json is currently configured to run on http://localhost:5053
You can view the swagger docs at http://localhost:5053/swagger/index.html

## Autoshop DB
To Create and populate the database, Create a new database named Autoshop.
Edit the appsettings file under the dbUpdater project and point the connection string at the new DB you just created.
Run the dbUpdater project.
The project is currently using a SQLite db file that is checked into the repo.
