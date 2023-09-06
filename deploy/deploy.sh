#!/bin/bash

az webapp up \
    -n $AZ_WEBAPP \
    -g $AZ_RESOURCE_GROUP \
    -l $AZ_LOCATION \
    -p $AZ_PLAN \
    --runtime ${AZ_WEBAPP_RUNTIME:-"dotnet:7"} \
    --os ${AZ_WEBAPP_OS:-windows} \
    --sku ${AZ_WEBAPP_SKU:-B1}

az webapp config appsettings set \
    -n $AZ_WEBAPP \
    -g $AZ_RESOURCE_GROUP \
    --settings auth0_AUTHENTICATION_SECRET=$AUTH0_CLIENT_SECRET \
    -o none

az webapp auth openid-connect add \
    -n $AZ_WEBAPP \
    -g $AZ_RESOURCE_GROUP \
    --provider-name auth0 \
    --client-id $AUTH0_CLIENT_ID \
    --secret-setting auth0_AUTHENTICATION_SECRET \
    --openid-configuration "https://$AUTH0_DOMAIN.auth0.com/.well-known/openid-configuration" \
    --scopes "openid,profile,email"

az webapp auth update \
    -n $AZ_WEBAPP \
    -g $AZ_RESOURCE_GROUP \
    --enabled true \
    --enable-token-store true \
    --action RedirectToLoginPage \
    --redirect-provider auth0
