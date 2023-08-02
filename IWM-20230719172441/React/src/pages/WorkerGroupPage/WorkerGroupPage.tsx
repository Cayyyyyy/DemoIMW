import React from "react";
import { Switch } from "react-router-dom";
import { WORKER_GROUP_MASTER_ROUTE,  } from "config/routes";
import { ProtectedRoute } from "core/pages/Authentication/ProtectedRoute";
import { authorizationService } from "core/services/common-services/authorization-service";
import WorkerGroupMaster from './WorkerGroupMaster/WorkerGroupMaster';

function WorkerGroupPage() {
  const { auth } = authorizationService.useAuthorizedRoute();
  return (
    <Switch>
        <ProtectedRoute path={ WORKER_GROUP_MASTER_ROUTE } key={ WORKER_GROUP_MASTER_ROUTE } component={ WorkerGroupMaster } auth={auth(WORKER_GROUP_MASTER_ROUTE)} />
    </Switch>
  );
}

export { WorkerGroupMaster,  };
export default WorkerGroupPage;
