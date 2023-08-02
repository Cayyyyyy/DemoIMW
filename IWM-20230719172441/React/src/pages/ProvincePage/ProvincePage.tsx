import React from "react";
import { Switch } from "react-router-dom";
import { PROVINCE_MASTER_ROUTE, PROVINCE_DETAIL_ROUTE } from "config/routes";
import { ProtectedRoute } from "core/pages/Authentication/ProtectedRoute";
import { authorizationService } from "core/services/common-services/authorization-service";
import ProvinceDetail from './ProvinceDetail/ProvinceDetail';
import ProvinceMaster from './ProvinceMaster/ProvinceMaster';

function ProvincePage() {
  const { auth } = authorizationService.useAuthorizedRoute();
  return (
    <Switch>
        <ProtectedRoute path={ PROVINCE_MASTER_ROUTE } key={ PROVINCE_MASTER_ROUTE } component={ ProvinceMaster } auth={auth(PROVINCE_MASTER_ROUTE)} />
        <ProtectedRoute path={ PROVINCE_DETAIL_ROUTE } key={ PROVINCE_DETAIL_ROUTE } component={ ProvinceDetail } auth={auth(PROVINCE_DETAIL_ROUTE)} />
    </Switch>
  );
}

export { ProvinceMaster, ProvinceDetail };
export default ProvincePage;
