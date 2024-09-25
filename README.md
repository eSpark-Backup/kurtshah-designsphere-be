# DesignSphere
dotnet ef --startup-project ..\DesignSphere.Api\ migrations add Initial --context AppDbContext
dotnet ef --startup-project ..\DesignSphere.Api\ migrations add Initial2 --context SQLContext
dotnet ef --startup-project ..\DesignSphere.Api\ database update