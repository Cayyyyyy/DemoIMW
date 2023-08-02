import React from "react";
import { Switch } from "react-router-dom";
import { DISTRICT_MASTER_ROUTE, DISTRICT_DETAIL_ROUTE } from "config/routes";
import { ProtectedRoute } from "core/pages/Authentication/ProtectedRoute";
import { authorizationService } from "core/services/common-services/authorization-service";
import DistrictDetail from './DistrictDetail/DistrictDetail';
import DistrictMaster from './DistrictMaster/DistrictMaster';

function DistrictPage() {
  const { auth } = authorizationService.useAuthorizedRoute();
  return (
    <Switch>
        <ProtectedRoute path={ DISTRICT_MASTER_ROUTE } key={ DISTRICT_MASTER_ROUTE } component={ DistrictMaster } auth={auth(DISTRICT_MASTER_ROUTE)} />
        <ProtectedRoute path={ DISTRICT_DETAIL_ROUTE } key={ DISTRICT_DETAIL_ROUTE } component={ DistrictDetail } auth={auth(DISTRICT_DETAIL_ROUTE)} />
    </Switch>
  );
}

export { DistrictMaster, DistrictDetail };
export default DistrictPage;
