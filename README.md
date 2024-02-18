# Cards  Instructions
#Navigate to  Cards.Api directory 
#Open the directory with powershell and  run the following command to generate the token  with Admin role rights
dotnet user-jwts create --role "Admin" --name "admin@card.com"  or dotnet user-jwts create --role "Member" --name "member@card.com"
#use the generated token to invoke the endpoints.
