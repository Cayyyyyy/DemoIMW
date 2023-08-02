import React from "react";
import { Switch } from "react-router-dom";
import { APP_USER_MASTER_ROUTE, APP_USER_DETAIL_ROUTE } from "config/routes";
import { ProtectedRoute } from "core/pages/Authentication/ProtectedRoute";
import { authorizationService } from "core/services/common-services/authorization-service";
import AppUserDetail from './AppUserDetail/AppUserDetail';
import AppUserMaster from './AppUserMaster/AppUserMaster';

function AppUserPage() {
  const { auth } = authorizationService.useAuthorizedRoute();
  return (
    <Switch>
        <ProtectedRoute path={ APP_USER_MASTER_ROUTE } key={ APP_USER_MASTER_ROUTE } component={ AppUserMaster } auth={auth(APP_USER_MASTER_ROUTE)} />
        <ProtectedRoute path={ APP_USER_DETAIL_ROUTE } key={ APP_USER_DETAIL_ROUTE } component={ AppUserDetail } auth={auth(APP_USER_DETAIL_ROUTE)} />
    </Switch>
  );
}

export { AppUserMaster, AppUserDetail };
export default AppUserPage;
