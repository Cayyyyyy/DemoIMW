import React from "react";
import { Switch } from "react-router-dom";
import { PRODUCT_TYPE_MASTER_ROUTE,  } from "config/routes";
import { ProtectedRoute } from "core/pages/Authentication/ProtectedRoute";
import { authorizationService } from "core/services/common-services/authorization-service";
import ProductTypeMaster from './ProductTypeMaster/ProductTypeMaster';

function ProductTypePage() {
  const { auth } = authorizationService.useAuthorizedRoute();
  return (
    <Switch>
        <ProtectedRoute path={ PRODUCT_TYPE_MASTER_ROUTE } key={ PRODUCT_TYPE_MASTER_ROUTE } component={ ProductTypeMaster } auth={auth(PRODUCT_TYPE_MASTER_ROUTE)} />
    </Switch>
  );
}

export { ProductTypeMaster,  };
export default ProductTypePage;
