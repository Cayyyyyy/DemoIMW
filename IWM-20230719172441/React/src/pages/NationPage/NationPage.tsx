import React from "react";
import { Switch } from "react-router-dom";
import { NATION_MASTER_ROUTE,  } from "config/routes";
import { ProtectedRoute } from "core/pages/Authentication/ProtectedRoute";
import { authorizationService } from "core/services/common-services/authorization-service";
import NationMaster from './NationMaster/NationMaster';

function NationPage() {
  const { auth } = authorizationService.useAuthorizedRoute();
  return (
    <Switch>
        <ProtectedRoute path={ NATION_MASTER_ROUTE } key={ NATION_MASTER_ROUTE } component={ NationMaster } auth={auth(NATION_MASTER_ROUTE)} />
    </Switch>
  );
}

export { NationMaster,  };
export default NationPage;
