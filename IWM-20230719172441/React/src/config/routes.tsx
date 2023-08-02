import React from "react";
import { ROOT_ROUTE } from "core/config/consts";
import { join } from "path";

import AppUserPage from 'pages/AppUserPage/AppUserPage';


import BrandPage from 'pages/BrandPage/BrandPage';


import CategoryTreePage from 'pages/CategoryTreePage/CategoryTreePage';


import DistrictPage from 'pages/DistrictPage/DistrictPage';


import ImagePage from 'pages/ImagePage/ImagePage';


import NationPage from 'pages/NationPage/NationPage';


import OrganizationTreePage from 'pages/OrganizationTreePage/OrganizationTreePage';


import ProductPage from 'pages/ProductPage/ProductPage';


import ProductTypePage from 'pages/ProductTypePage/ProductTypePage';


import ProvincePage from 'pages/ProvincePage/ProvincePage';


import ProvinceGroupingTreePage from 'pages/ProvinceGroupingTreePage/ProvinceGroupingTreePage';





import TaxTypePage from 'pages/TaxTypePage/TaxTypePage';


import UnitOfMeasurePage from 'pages/UnitOfMeasurePage/UnitOfMeasurePage';


import UnitOfMeasureGroupingContentPage from 'pages/UnitOfMeasureGroupingContentPage/UnitOfMeasureGroupingContentPage';


import UnitOfMeasureGroupingPage from 'pages/UnitOfMeasureGroupingPage/UnitOfMeasureGroupingPage';


import WardPage from 'pages/WardPage/WardPage';


import WorkerPage from 'pages/WorkerPage/WorkerPage';



import WorkerGroupPage from 'pages/WorkerGroupPage/WorkerGroupPage';


export interface Route {
    path: string;
    component:
    | ((props?: any) => JSX.Element)
    | React.LazyExoticComponent<( props?: any ) => JSX.Element>;
    exact?: boolean;
}

/* Routes */

export const LOGIN_ROUTE: string = '/login';
export const LOGOUT_ROUTE: string = '/logout';
export const FORBIDENT_ROUTE: string = '/403';
export const NOT_FOUND_ROUTE: string = '/404';


export const APP_USER_ROUTE: string = ROOT_ROUTE ? ROOT_ROUTE + '/app-user' : '/app-user';
export const APP_USER_MASTER_ROUTE: string = join(
APP_USER_ROUTE,
  "app-user-master",
);
export const APP_USER_DETAIL_ROUTE: string = join(
APP_USER_ROUTE,
  "app-user-detail",
);


export const BRAND_ROUTE: string = ROOT_ROUTE ? ROOT_ROUTE + '/brand' : '/brand';
export const BRAND_MASTER_ROUTE: string = join(
BRAND_ROUTE,
  "brand-master",
);


export const CATEGORY_ROUTE: string = ROOT_ROUTE ? ROOT_ROUTE + '/category' : '/category';
export const CATEGORY_MASTER_ROUTE: string = join(
CATEGORY_ROUTE,
  "category-master",
);
export const CATEGORY_DETAIL_ROUTE: string = join(
CATEGORY_ROUTE,
  "category-detail",
);


export const DISTRICT_ROUTE: string = ROOT_ROUTE ? ROOT_ROUTE + '/district' : '/district';
export const DISTRICT_MASTER_ROUTE: string = join(
DISTRICT_ROUTE,
  "district-master",
);
export const DISTRICT_DETAIL_ROUTE: string = join(
DISTRICT_ROUTE,
  "district-detail",
);


export const IMAGE_ROUTE: string = ROOT_ROUTE ? ROOT_ROUTE + '/image' : '/image';
export const IMAGE_MASTER_ROUTE: string = join(
IMAGE_ROUTE,
  "image-master",
);


export const NATION_ROUTE: string = ROOT_ROUTE ? ROOT_ROUTE + '/nation' : '/nation';
export const NATION_MASTER_ROUTE: string = join(
NATION_ROUTE,
  "nation-master",
);


export const ORGANIZATION_ROUTE: string = ROOT_ROUTE ? ROOT_ROUTE + '/organization' : '/organization';
export const ORGANIZATION_MASTER_ROUTE: string = join(
ORGANIZATION_ROUTE,
  "organization-master",
);
export const ORGANIZATION_DETAIL_ROUTE: string = join(
ORGANIZATION_ROUTE,
  "organization-detail",
);


export const PRODUCT_ROUTE: string = ROOT_ROUTE ? ROOT_ROUTE + '/product' : '/product';
export const PRODUCT_MASTER_ROUTE: string = join(
PRODUCT_ROUTE,
  "product-master",
);
export const PRODUCT_DETAIL_ROUTE: string = join(
PRODUCT_ROUTE,
  "product-detail",
);


export const PRODUCT_TYPE_ROUTE: string = ROOT_ROUTE ? ROOT_ROUTE + '/product-type' : '/product-type';
export const PRODUCT_TYPE_MASTER_ROUTE: string = join(
PRODUCT_TYPE_ROUTE,
  "product-type-master",
);


export const PROVINCE_ROUTE: string = ROOT_ROUTE ? ROOT_ROUTE + '/province' : '/province';
export const PROVINCE_MASTER_ROUTE: string = join(
PROVINCE_ROUTE,
  "province-master",
);
export const PROVINCE_DETAIL_ROUTE: string = join(
PROVINCE_ROUTE,
  "province-detail",
);


export const PROVINCE_GROUPING_ROUTE: string = ROOT_ROUTE ? ROOT_ROUTE + '/province-grouping' : '/province-grouping';
export const PROVINCE_GROUPING_MASTER_ROUTE: string = join(
PROVINCE_GROUPING_ROUTE,
  "province-grouping-master",
);
export const PROVINCE_GROUPING_DETAIL_ROUTE: string = join(
PROVINCE_GROUPING_ROUTE,
  "province-grouping-detail",
);





export const TAX_TYPE_ROUTE: string = ROOT_ROUTE ? ROOT_ROUTE + '/tax-type' : '/tax-type';
export const TAX_TYPE_MASTER_ROUTE: string = join(
TAX_TYPE_ROUTE,
  "tax-type-master",
);


export const UNIT_OF_MEASURE_ROUTE: string = ROOT_ROUTE ? ROOT_ROUTE + '/unit-of-measure' : '/unit-of-measure';
export const UNIT_OF_MEASURE_MASTER_ROUTE: string = join(
UNIT_OF_MEASURE_ROUTE,
  "unit-of-measure-master",
);
export const UNIT_OF_MEASURE_DETAIL_ROUTE: string = join(
UNIT_OF_MEASURE_ROUTE,
  "unit-of-measure-detail",
);


export const UNIT_OF_MEASURE_GROUPING_CONTENT_ROUTE: string = ROOT_ROUTE ? ROOT_ROUTE + '/unit-of-measure-grouping-content' : '/unit-of-measure-grouping-content';
export const UNIT_OF_MEASURE_GROUPING_CONTENT_MASTER_ROUTE: string = join(
UNIT_OF_MEASURE_GROUPING_CONTENT_ROUTE,
  "unit-of-measure-grouping-content-master",
);


export const UNIT_OF_MEASURE_GROUPING_ROUTE: string = ROOT_ROUTE ? ROOT_ROUTE + '/unit-of-measure-grouping' : '/unit-of-measure-grouping';
export const UNIT_OF_MEASURE_GROUPING_MASTER_ROUTE: string = join(
UNIT_OF_MEASURE_GROUPING_ROUTE,
  "unit-of-measure-grouping-master",
);
export const UNIT_OF_MEASURE_GROUPING_DETAIL_ROUTE: string = join(
UNIT_OF_MEASURE_GROUPING_ROUTE,
  "unit-of-measure-grouping-detail",
);


export const WARD_ROUTE: string = ROOT_ROUTE ? ROOT_ROUTE + '/ward' : '/ward';
export const WARD_MASTER_ROUTE: string = join(
WARD_ROUTE,
  "ward-master",
);


export const WORKER_ROUTE: string = ROOT_ROUTE ? ROOT_ROUTE + '/worker' : '/worker';
export const WORKER_MASTER_ROUTE: string = join(
WORKER_ROUTE,
  "worker-master",
);
export const WORKER_DETAIL_ROUTE: string = join(
WORKER_ROUTE,
  "worker-detail",
);



export const WORKER_GROUP_ROUTE: string = ROOT_ROUTE ? ROOT_ROUTE + '/worker-group' : '/worker-group';
export const WORKER_GROUP_MASTER_ROUTE: string = join(
WORKER_GROUP_ROUTE,
  "worker-group-master",
);


/* Routes Component */
const userRoutes: Route[] =
[
    
            {
                path: APP_USER_ROUTE,
                component: AppUserPage,
            },
    
    
            {
                path: BRAND_ROUTE,
                component: BrandPage,
            },
    
    
            {
                path: CATEGORY_ROUTE,
                component: CategoryTreePage,
            },
    
    
            {
                path: DISTRICT_ROUTE,
                component: DistrictPage,
            },
    
    
            {
                path: IMAGE_ROUTE,
                component: ImagePage,
            },
    
    
            {
                path: NATION_ROUTE,
                component: NationPage,
            },
    
    
            {
                path: ORGANIZATION_ROUTE,
                component: OrganizationTreePage,
            },
    
    
            {
                path: PRODUCT_ROUTE,
                component: ProductPage,
            },
    
    
            {
                path: PRODUCT_TYPE_ROUTE,
                component: ProductTypePage,
            },
    
    
            {
                path: PROVINCE_ROUTE,
                component: ProvincePage,
            },
    
    
            {
                path: PROVINCE_GROUPING_ROUTE,
                component: ProvinceGroupingTreePage,
            },
    
    
    
    
    
            {
                path: TAX_TYPE_ROUTE,
                component: TaxTypePage,
            },
    
    
            {
                path: UNIT_OF_MEASURE_ROUTE,
                component: UnitOfMeasurePage,
            },
    
    
            {
                path: UNIT_OF_MEASURE_GROUPING_CONTENT_ROUTE,
                component: UnitOfMeasureGroupingContentPage,
            },
    
    
            {
                path: UNIT_OF_MEASURE_GROUPING_ROUTE,
                component: UnitOfMeasureGroupingPage,
            },
    
    
            {
                path: WARD_ROUTE,
                component: WardPage,
            },
    
    
            {
                path: WORKER_ROUTE,
                component: WorkerPage,
            },
    
    
    
            {
                path: WORKER_GROUP_ROUTE,
                component: WorkerGroupPage,
            },
    
];
export { userRoutes };