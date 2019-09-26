 <img src="https://github.com/zsofi-gagyi/notTrello/blob/master/screenshots/screenshot.png" width="870px"></img> 

<h2>TaskManager</h2>
<h3>Deployed <a href="https://taskmanagerstudyapp.azurewebsites.net">here</a></h3>
<br/>
<br/>
<p>This is a simple Trello clone.</p> 
<p>It's hosted on Azure.</p>
<p>This is a version which can be run locally. The deployed version can be found on the "deployed" branch. This version uses</p>

- user authentication (optionally with Google) and role-based authorization with ASP.NET Core Identity, using a cookie
- MySQL database used with EntityFramework and Automapper
- REST* API using JWT tokens and Newtonsoft.Json.Schema
- RazorView (and to a lesser extent, RazorPages) for HTML templating
- css and minor bits of ready-made Javascript found on the net
- unit tests with Moq, integration tests with XUnit and AngleSharp, and end-to-end tests with Cypress
- deployment on Azure

*except HATEOAS
