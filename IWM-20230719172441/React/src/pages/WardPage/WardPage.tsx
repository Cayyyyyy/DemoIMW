import React from "react";
import { Switch } from "react-router-dom";
import { WARD_MASTER_ROUTE,  } from "config/routes";
import { ProtectedRoute } from "core/pages/Authentication/ProtectedRoute";
import { authorizationService } from "core/services/common-services/authorization-service";
import WardMaster from './WardMaster/WardMaster';

function WardPage() {
  const { auth } = authorizationService.useAuthorizedRoute();
  return (
    <Switch>
        <ProtectedRoute path={ WARD_MASTER_ROUTE } key={ WARD_MASTER_ROUTE } component={ WardMaster } auth={auth(WARD_MASTER_ROUTE)} />
    </Switch>
  );
}

export { WardMaster,  };
export default WardPage;
