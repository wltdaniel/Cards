# Cards  Instructions

1. Navigate to  Cards.Api directory
2. Open the directory with powershell and  run the following command to generate the token  with Admin role rights

 *  dotnet user-jwts create --role "Admin" --name "admin@card.com".
 *  dotnet user-jwts create --role "Member" --name "member@card.com"
3. use the generated token to invoke the endpoints.

