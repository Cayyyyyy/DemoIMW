import React from "react";
import { Switch } from "react-router-dom";
import { WORKER_MASTER_ROUTE, WORKER_DETAIL_ROUTE } from "config/routes";
import { ProtectedRoute } from "core/pages/Authentication/ProtectedRoute";
import { authorizationService } from "core/services/common-services/authorization-service";
import WorkerDetail from './WorkerDetail/WorkerDetail';
import WorkerMaster from './WorkerMaster/WorkerMaster';

function WorkerPage() {
  const { auth } = authorizationService.useAuthorizedRoute();
  return (
    <Switch>
        <ProtectedRoute path={ WORKER_MASTER_ROUTE } key={ WORKER_MASTER_ROUTE } component={ WorkerMaster } auth={auth(WORKER_MASTER_ROUTE)} />
        <ProtectedRoute path={ WORKER_DETAIL_ROUTE } key={ WORKER_DETAIL_ROUTE } component={ WorkerDetail } auth={auth(WORKER_DETAIL_ROUTE)} />
    </Switch>
  );
}

export { WorkerMaster, WorkerDetail };
export default WorkerPage;
