import React from "react";
import { Switch } from "react-router-dom";
import { UNIT_OF_MEASURE_GROUPING_MASTER_ROUTE, UNIT_OF_MEASURE_GROUPING_DETAIL_ROUTE } from "config/routes";
import { ProtectedRoute } from "core/pages/Authentication/ProtectedRoute";
import { authorizationService } from "core/services/common-services/authorization-service";
import UnitOfMeasureGroupingDetail from './UnitOfMeasureGroupingDetail/UnitOfMeasureGroupingDetail';
import UnitOfMeasureGroupingMaster from './UnitOfMeasureGroupingMaster/UnitOfMeasureGroupingMaster';

function UnitOfMeasureGroupingPage() {
  const { auth } = authorizationService.useAuthorizedRoute();
  return (
    <Switch>
        <ProtectedRoute path={ UNIT_OF_MEASURE_GROUPING_MASTER_ROUTE } key={ UNIT_OF_MEASURE_GROUPING_MASTER_ROUTE } component={ UnitOfMeasureGroupingMaster } auth={auth(UNIT_OF_MEASURE_GROUPING_MASTER_ROUTE)} />
        <ProtectedRoute path={ UNIT_OF_MEASURE_GROUPING_DETAIL_ROUTE } key={ UNIT_OF_MEASURE_GROUPING_DETAIL_ROUTE } component={ UnitOfMeasureGroupingDetail } auth={auth(UNIT_OF_MEASURE_GROUPING_DETAIL_ROUTE)} />
    </Switch>
  );
}

export { UnitOfMeasureGroupingMaster, UnitOfMeasureGroupingDetail };
export default UnitOfMeasureGroupingPage;
