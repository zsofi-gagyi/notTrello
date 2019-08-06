I began working on this project when I have been studying how to 
do things in ASP.NET that I already knew in Spring. Later it grew 
into a practice project that covered new topics, so I'm moving it
here. Earlier versions can be found in the commits of my "translate
to C#" directory.

Features:
-generates JWTokens and requires them for the API endpoints
-uses a custom middleware, with the pipeline branching and rejoining
-has integration tests that verify data extracted from generated HTML
-serves static pages without using a controller