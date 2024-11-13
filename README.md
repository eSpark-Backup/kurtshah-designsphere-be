## Migrations
### DesignSphere API
- dotnet ef --startup-project ..\DesignSphere.Api\ migrations add Initial2 --context SQLContext
- dotnet ef --startup-project ..\DesignSphere.Api\ database update

## Endpoints 
### main API
- dev : htps://localhost:5001
- staging : htps://devapi.medchg.com
- production : htps://api.medchg.com

### Identity STS APP
- dev : htps://localhost:43... (use default)
- staging : htps://devauth.medchg.com
- production : htps://auth.medchg.com

### Identity Admin APP
- dev : htps://localhost:43...
- staging : htps://devauthadmin.medchg.com
- production : htps://authadmin.medchg.com

### Identity Admin API 
- dev : htps://localhost:43...
- staging : htps://devauthapi.medchg.com
- production : htps://authapi.medchg.com

### Database
- dev : localhost:5432
- staging : no need will be configured via kubernates script 
