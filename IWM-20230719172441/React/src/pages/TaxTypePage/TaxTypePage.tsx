import React from "react";
import { Switch } from "react-router-dom";
import { TAX_TYPE_MASTER_ROUTE,  } from "config/routes";
import { ProtectedRoute } from "core/pages/Authentication/ProtectedRoute";
import { authorizationService } from "core/services/common-services/authorization-service";
import TaxTypeMaster from './TaxTypeMaster/TaxTypeMaster';

function TaxTypePage() {
  const { auth } = authorizationService.useAuthorizedRoute();
  return (
    <Switch>
        <ProtectedRoute path={ TAX_TYPE_MASTER_ROUTE } key={ TAX_TYPE_MASTER_ROUTE } component={ TaxTypeMaster } auth={auth(TAX_TYPE_MASTER_ROUTE)} />
    </Switch>
  );
}

export { TaxTypeMaster,  };
export default TaxTypePage;
