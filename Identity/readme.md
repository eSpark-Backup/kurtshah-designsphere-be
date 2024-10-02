run cmd

first run sts project in development for auto local context migration (device table) 

cd ".\designsphere_identity.STS.Identity\"
dotnet run --project "designsphere_identity.STS.Identity.csproj"

cd ".\designsphere_identity.Admin.Api\"
dotnet run --project "designsphere_identity.Admin.Api.csproj"

cd ".\designsphere_identity.Admin\"
dotnet run --project "designsphere_identity.Admin.csproj"
