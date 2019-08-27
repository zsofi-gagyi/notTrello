To run this locally, one needs to have installed the following tools:
-Visual Studio
-MySQL (for example MySQL Workbench https://www.mysql.com/products/workbench/)

It's also necessary to set the following environmental variables for the database:
-"TaskManagerHOST" 
-"TaskManagerDATABASE" 
-"TaskManagerUSERNAME"
-"TaskManagerPASSWORD"

Token generation requires setting the following environmental variable, as security key:
-"TODOTOKENSECRET" (any string that is longer than 128 bits will work)

While this is not necessary for running the app, to see the Google Authentication feature 
in acion, one can request a set of secrets from Google 
	       (https://developers.google.com/identity/sign-in/web/sign-in#before_you_begin),
and store them as the following user-secrets: 
-"Authentication:Google:ClientIdTaskManager" 
-"Authentication:Google:ClientSecretTaskManager"

For running the end-to-end tests, it's necessary to have installed:
-Cypress (see https://docs.cypress.io/guides/getting-started/installing-cypress.html)






