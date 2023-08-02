import React from "react";
import { Switch } from "react-router-dom";
import { UNIT_OF_MEASURE_MASTER_ROUTE, UNIT_OF_MEASURE_DETAIL_ROUTE } from "config/routes";
import { ProtectedRoute } from "core/pages/Authentication/ProtectedRoute";
import { authorizationService } from "core/services/common-services/authorization-service";
import UnitOfMeasureDetail from './UnitOfMeasureDetail/UnitOfMeasureDetail';
import UnitOfMeasureMaster from './UnitOfMeasureMaster/UnitOfMeasureMaster';

function UnitOfMeasurePage() {
  const { auth } = authorizationService.useAuthorizedRoute();
  return (
    <Switch>
        <ProtectedRoute path={ UNIT_OF_MEASURE_MASTER_ROUTE } key={ UNIT_OF_MEASURE_MASTER_ROUTE } component={ UnitOfMeasureMaster } auth={auth(UNIT_OF_MEASURE_MASTER_ROUTE)} />
        <ProtectedRoute path={ UNIT_OF_MEASURE_DETAIL_ROUTE } key={ UNIT_OF_MEASURE_DETAIL_ROUTE } component={ UnitOfMeasureDetail } auth={auth(UNIT_OF_MEASURE_DETAIL_ROUTE)} />
    </Switch>
  );
}

export { UnitOfMeasureMaster, UnitOfMeasureDetail };
export default UnitOfMeasurePage;
