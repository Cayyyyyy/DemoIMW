import React from "react";
import { Switch } from "react-router-dom";
import { PRODUCT_MASTER_ROUTE, PRODUCT_DETAIL_ROUTE } from "config/routes";
import { ProtectedRoute } from "core/pages/Authentication/ProtectedRoute";
import { authorizationService } from "core/services/common-services/authorization-service";
import ProductDetail from './ProductDetail/ProductDetail';
import ProductMaster from './ProductMaster/ProductMaster';

function ProductPage() {
  const { auth } = authorizationService.useAuthorizedRoute();
  return (
    <Switch>
        <ProtectedRoute path={ PRODUCT_MASTER_ROUTE } key={ PRODUCT_MASTER_ROUTE } component={ ProductMaster } auth={auth(PRODUCT_MASTER_ROUTE)} />
        <ProtectedRoute path={ PRODUCT_DETAIL_ROUTE } key={ PRODUCT_DETAIL_ROUTE } component={ ProductDetail } auth={auth(PRODUCT_DETAIL_ROUTE)} />
    </Switch>
  );
}

export { ProductMaster, ProductDetail };
export default ProductPage;
