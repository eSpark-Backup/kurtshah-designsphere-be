# Prereq
run ps with admin
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass

# Migration
cd build
.\add-migrations.ps1 -migration DbInit2 -migrationProviderName PostgreSQL
.\add-migrations.ps1 -migration DbInit3 -migrationProviderName PostgreSQL
# run admin for migration 

dotnet ef migrations add exuser --context MylocalContext 

# Deployment
# create secret one time 
kubectl create namespace dev-ns-designsphere
cd /mnt/shared_server/installation/cert/
kubectl create secret tls designsphere.io -n dev-ns-designsphere --key designsphere.io.key --cert designsphere.io.crt
kubectl create secret generic dev-sc-identity-ssl -n dev-ns-designsphere --from-file=certificate=./designsphere.io.pfx
# Everytime when new build
docker-compose build
docker tag dev-designsphere-stsidentity localhost:5500/designsphere/dev-designsphere-stsidentity
docker push localhost:5500/designsphere/dev-designsphere-stsidentity

kubectl rollout restart deployment/dev-pd-designsphere-stsidentity -n dev-ns-designsphere



# Production
docker tag dev-designsphere-stsidentity localhost:5500/designsphere/prd-designsphere-stsidentity
docker push localhost:5500/designsphere/prd-designsphere-stsidentity

kubectl rollout restart deployment/prd-pod-designsphere-stsidentity -n prd-ns-designsphere

cd /mnt/shared_server/installation/production/services/espark/designsphere/identity-sts
k apply -f deploy.yaml