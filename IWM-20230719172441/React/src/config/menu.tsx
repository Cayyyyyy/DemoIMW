import React, { ReactNode } from 'react';
import { translate } from "core/config/i18n";
import { TFunction } from "i18next";
import Home16 from "@carbon/icons-react/es/home/16";

import { APP_USER_MASTER_ROUTE } from 'config/routes';


import { BRAND_MASTER_ROUTE } from 'config/routes';


import { CATEGORY_MASTER_ROUTE } from 'config/routes';


import { DISTRICT_MASTER_ROUTE } from 'config/routes';


import { IMAGE_MASTER_ROUTE } from 'config/routes';


import { NATION_MASTER_ROUTE } from 'config/routes';


import { ORGANIZATION_MASTER_ROUTE } from 'config/routes';


import { PRODUCT_MASTER_ROUTE } from 'config/routes';


import { PRODUCT_TYPE_MASTER_ROUTE } from 'config/routes';


import { PROVINCE_MASTER_ROUTE } from 'config/routes';


import { PROVINCE_GROUPING_MASTER_ROUTE } from 'config/routes';





import { TAX_TYPE_MASTER_ROUTE } from 'config/routes';


import { UNIT_OF_MEASURE_MASTER_ROUTE } from 'config/routes';


import { UNIT_OF_MEASURE_GROUPING_CONTENT_MASTER_ROUTE } from 'config/routes';


import { UNIT_OF_MEASURE_GROUPING_MASTER_ROUTE } from 'config/routes';


import { WARD_MASTER_ROUTE } from 'config/routes';


import { WORKER_MASTER_ROUTE } from 'config/routes';



import { WORKER_GROUP_MASTER_ROUTE } from 'config/routes';


export interface Menu {
  name?: string | TFunction;
  icon?: string | ReactNode;
  link: string;
  children?: Menu[];
  active?: boolean;
  show?: boolean;
  checkVisible?: (mapper: Record<string, number>) => boolean;
}


export const menu: Menu[] =
[
    {
        name: 'Menu',
        icon: <Home16 />,
        link: "/dashboard",
        show: true,
        active: false,
        children: [

            {
                name: translate('menus.appUsers'),
                icon: <Home16 />,
                link: APP_USER_MASTER_ROUTE,
                active: false,
                show: true,
            },


            {
                name: translate('menus.brands'),
                icon: <Home16 />,
                link: BRAND_MASTER_ROUTE,
                active: false,
                show: true,
            },


            {
                name: translate('menus.categories'),
                icon: <Home16 />,
                link: CATEGORY_MASTER_ROUTE,
                active: false,
                show: true,
            },


            {
                name: translate('menus.districts'),
                icon: <Home16 />,
                link: DISTRICT_MASTER_ROUTE,
                active: false,
                show: true,
            },


            {
                name: translate('menus.images'),
                icon: <Home16 />,
                link: IMAGE_MASTER_ROUTE,
                active: false,
                show: true,
            },


            {
                name: translate('menus.nations'),
                icon: <Home16 />,
                link: NATION_MASTER_ROUTE,
                active: false,
                show: true,
            },


            {
                name: translate('menus.organizations'),
                icon: <Home16 />,
                link: ORGANIZATION_MASTER_ROUTE,
                active: false,
                show: true,
            },


            {
                name: translate('menus.products'),
                icon: <Home16 />,
                link: PRODUCT_MASTER_ROUTE,
                active: false,
                show: true,
            },


            {
                name: translate('menus.productTypes'),
                icon: <Home16 />,
                link: PRODUCT_TYPE_MASTER_ROUTE,
                active: false,
                show: true,
            },


            {
                name: translate('menus.provinces'),
                icon: <Home16 />,
                link: PROVINCE_MASTER_ROUTE,
                active: false,
                show: true,
            },


            {
                name: translate('menus.provinceGroupings'),
                icon: <Home16 />,
                link: PROVINCE_GROUPING_MASTER_ROUTE,
                active: false,
                show: true,
            },





            {
                name: translate('menus.taxTypes'),
                icon: <Home16 />,
                link: TAX_TYPE_MASTER_ROUTE,
                active: false,
                show: true,
            },


            {
                name: translate('menus.unitOfMeasures'),
                icon: <Home16 />,
                link: UNIT_OF_MEASURE_MASTER_ROUTE,
                active: false,
                show: true,
            },


            {
                name: translate('menus.unitOfMeasureGroupingContents'),
                icon: <Home16 />,
                link: UNIT_OF_MEASURE_GROUPING_CONTENT_MASTER_ROUTE,
                active: false,
                show: true,
            },


            {
                name: translate('menus.unitOfMeasureGroupings'),
                icon: <Home16 />,
                link: UNIT_OF_MEASURE_GROUPING_MASTER_ROUTE,
                active: false,
                show: true,
            },


            {
                name: translate('menus.wards'),
                icon: <Home16 />,
                link: WARD_MASTER_ROUTE,
                active: false,
                show: true,
            },


            {
                name: translate('menus.workers'),
                icon: <Home16 />,
                link: WORKER_MASTER_ROUTE,
                active: false,
                show: true,
            },



            {
                name: translate('menus.workerGroups'),
                icon: <Home16 />,
                link: WORKER_GROUP_MASTER_ROUTE,
                active: false,
                show: true,
            },

        ]
    }
];
