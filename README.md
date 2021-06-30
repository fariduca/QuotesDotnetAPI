# Quotes Dotnet API
This is a .Net Core web api with features to store, retrieve, edit, and delete quotes. In addition, there is a feature to subscribe via email or phone number to daily quotes and a clean-up feature which deletes quotes that were created more than 24 hours ago. 

This repo also uses SwaggerUI to interact with the endpoints. 

In order to install, clone this repo and run `dotnet build`. Then run `dotnet run` and go to `localhost:5000/swagger` in a browser to view the SwaggerUI. 

# To-Do
1. Configure sending email and sms
2. Add more validation
