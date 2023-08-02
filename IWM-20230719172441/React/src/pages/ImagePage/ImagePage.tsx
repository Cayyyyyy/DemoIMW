import React from "react";
import { Switch } from "react-router-dom";
import { IMAGE_MASTER_ROUTE,  } from "config/routes";
import { ProtectedRoute } from "core/pages/Authentication/ProtectedRoute";
import { authorizationService } from "core/services/common-services/authorization-service";
import ImageMaster from './ImageMaster/ImageMaster';

function ImagePage() {
  const { auth } = authorizationService.useAuthorizedRoute();
  return (
    <Switch>
        <ProtectedRoute path={ IMAGE_MASTER_ROUTE } key={ IMAGE_MASTER_ROUTE } component={ ImageMaster } auth={auth(IMAGE_MASTER_ROUTE)} />
    </Switch>
  );
}

export { ImageMaster,  };
export default ImagePage;
