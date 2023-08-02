#!/bin/bash
sed -i "s,{DOMAIN},${DOMAIN},g" appsettings.json
sed -i "s/{SQL_IP}/${SQL_IP}/g" appsettings.json
sed -i "s/{SQL_DB}/${SQL_DB}/g" appsettings.json
sed -i "s/{SQL_USER}/${SQL_USER}/g" appsettings.json
sed -i "s/{SQL_PASS}/${SQL_PASS}/g" appsettings.json
sed -i "s/{MG_IP}/${MG_IP}/g" appsettings.json
sed -i "s/{MG_USER}/${MG_USER}/g" appsettings.json
sed -i "s/{MG_PASS}/${MG_PASS}/g" appsettings.json
sed -i "s,{ELASTIC_ENDPOINT},${ELASTIC_ENDPOINT},g" appsettings.json
sed -i "s/{ELASTIC_USERNAME}/${ELASTIC_USERNAME}/g" appsettings.json
sed -i "s/{ELASTIC_PASSWORD}/${ELASTIC_PASSWORD}/g" appsettings.json
sed -i "s/{TRUESIGHT_LICENSE}/${TRUESIGHT_LICENSE}/g" appsettings.json
sed -i "s/{PUBLIC_RSA_KEY}/${PUBLIC_RSA_KEY}/g" appsettings.json
sed -i "s/{ON_CLOUD}/${ON_CLOUD}/g" appsettings.json
sed -i "s,{MASTER_CLOUD_DOMAIN},${MASTER_CLOUD_DOMAIN},g" appsettings.json

PROJECT_NAME="IWM"
dotnet ${PROJECT_NAME}.dll --urls http://0.0.0.0:8080 --launch-profile Production