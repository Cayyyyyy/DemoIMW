import React from "react";
import { Switch } from "react-router-dom";
import { BRAND_MASTER_ROUTE,  } from "config/routes";
import { ProtectedRoute } from "core/pages/Authentication/ProtectedRoute";
import { authorizationService } from "core/services/common-services/authorization-service";
import BrandMaster from './BrandMaster/BrandMaster';

function BrandPage() {
  const { auth } = authorizationService.useAuthorizedRoute();
  return (
    <Switch>
        <ProtectedRoute path={ BRAND_MASTER_ROUTE } key={ BRAND_MASTER_ROUTE } component={ BrandMaster } auth={auth(BRAND_MASTER_ROUTE)} />
    </Switch>
  );
}

export { BrandMaster,  };
export default BrandPage;
