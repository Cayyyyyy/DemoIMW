import React from "react";
import { Switch } from "react-router-dom";
import { UNIT_OF_MEASURE_GROUPING_CONTENT_MASTER_ROUTE,  } from "config/routes";
import { ProtectedRoute } from "core/pages/Authentication/ProtectedRoute";
import { authorizationService } from "core/services/common-services/authorization-service";
import UnitOfMeasureGroupingContentMaster from './UnitOfMeasureGroupingContentMaster/UnitOfMeasureGroupingContentMaster';

function UnitOfMeasureGroupingContentPage() {
  const { auth } = authorizationService.useAuthorizedRoute();
  return (
    <Switch>
        <ProtectedRoute path={ UNIT_OF_MEASURE_GROUPING_CONTENT_MASTER_ROUTE } key={ UNIT_OF_MEASURE_GROUPING_CONTENT_MASTER_ROUTE } component={ UnitOfMeasureGroupingContentMaster } auth={auth(UNIT_OF_MEASURE_GROUPING_CONTENT_MASTER_ROUTE)} />
    </Switch>
  );
}

export { UnitOfMeasureGroupingContentMaster,  };
export default UnitOfMeasureGroupingContentPage;
