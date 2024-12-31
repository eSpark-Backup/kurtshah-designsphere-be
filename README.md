# DesignSphere
dotnet ef --startup-project ..\DesignSphere.Api\ migrations add Initial --context AppDbContext
dotnet ef --startup-project ..\DesignSphere.Api\ migrations add Initial2 --context SQLContext
dotnet ef --startup-project ..\DesignSphere.Api\ database update

Identity deafult password
admin
!QAZxsw2

# docker compose 
## Windows 
- choco install mkcert
- cd .\shared\nginx\certs

mkcert --install
copy $env:LOCALAPPDATA\mkcert\rootCA.pem ./cacerts.pem
copy $env:LOCALAPPDATA\mkcert\rootCA.pem ./cacerts.crt

mkcert -cert-file designsphere.local.crt -key-file designsphere.local.key designsphere.local *.designsphere.local
mkcert -pkcs12 designsphere.local.pfx designsphere.local *.designsphere.local

### Add lines to host file 
127.0.0.1 admin.designsphere.local
127.0.0.1 admin-api.designsphere.local
127.0.0.1 sts.designsphere.local
127.0.0.1 api.designsphere.local


docker-compose build
docker-compose up -d