Amazon review data was taken from pet supplies category here: https://snap.stanford.edu/data/web-Amazon-links.html 
and truncated to more manageable size.

If running via docker-compose, it is required to first generate and trust development certificates by executing:

`dotnet dev-certs https -ep %USERPROFILE%\https\devcerts.pfx -p qwerty`

`dotnet dev-certs https --trust`

If valid ApiKey for Azure Text Analytics and service endpoint are specified in appsettings.json file the ReviewGen.API will use Azure Text Analytics API for sentiment analysis. 
If key/endpoint URI is missing, will fallback to alternative inferior implementation of sentiment analysis.

Simply run `docker-compose up` and navigate to http://localhost:4040
