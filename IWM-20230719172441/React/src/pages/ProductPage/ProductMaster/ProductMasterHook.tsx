/* begin general import */
import { ColumnProps } from "antd/lib/table";
import React, { useMemo } from "react";
import { useTranslation } from "react-i18next";
import {
  LayoutCell,
  LayoutHeader,
  OneLineText,
  StatusLine,
  OverflowMenu
} from "react3l-ui-library";
import { detailService } from "core/services/page-services/detail-service";
import { filterService } from "core/services/page-services/filter-service";
import { listService } from "core/services/page-services/list-service";
import { masterService } from "core/services/page-services/master-service";
import { queryStringService } from "core/services/page-services/query-string-service";
import { ListOverflowMenu } from "react3l-ui-library/build/components/OverflowMenu/OverflowMenuList";
import { importExportService } from "core/services/page-services/import-export-service";
import {
  getAntOrderType,
  tableService
} from "core/services/page-services/table-service";
import nameof from "ts-nameof.macro";

/* end general import */

/* begin helpers import */
import { formatDateTime } from "core/helpers/date-time";
import { Moment } from "moment";
import { formatNumber } from "core/helpers/number";
/* end filter import */

/* begin individual import */
import { productRepository } from "../ProductRepository";
import { Product, ProductFilter } from "models/Product";
import { Brand, BrandFilter } from "models/Brand";
import { Category, CategoryFilter } from "models/Category";
import { ProductType, ProductTypeFilter } from "models/ProductType";
import { Status, StatusFilter } from "models/Status";
import { TaxType, TaxTypeFilter } from "models/TaxType";
import { UnitOfMeasure, UnitOfMeasureFilter } from "models/UnitOfMeasure";
import { UnitOfMeasureGrouping, UnitOfMeasureGroupingFilter } from "models/UnitOfMeasureGrouping";
import { PRODUCT_ROUTE } from "config/routes";

/* end individual import */

export function useProductMasterHook() {
    const [translate] = useTranslation();
    const [visible, setVisible] = React.useState(false);
    const [modelFilter, dispatch, countFilter] =
        queryStringService.useQueryString(ProductFilter, { skip: 0, take: 10 });

   
    const {
        value: filter,
        handleChangeInputSearch,
        handleChangeAllFilter,
    } = filterService.useFilter(modelFilter, dispatch);

    const {
        list,
        count,
        loadingList,
        handleResetList,
        handleLoadList,
    } = listService.useList(productRepository.list, productRepository.count, filter, dispatch);

    const {
        handleTableChange,
        handlePagination
    } = tableService.useTable(filter, handleChangeAllFilter);

    const {
        handleAction,
        handleBulkAction,
        canBulkAction,
        rowSelection,
        selectedRowKeys,
        setSelectedRowKeys
    } = listService.useRowSelection(productRepository.delete, productRepository.bulkDelete, null, null, null, handleResetList);

    const { handleImportList, ref: importActionRef, handleClick } = importExportService.useImport(handleResetList);


    const {
        handleListExport,
        handleExportTemplateList,
    } = importExportService.useExport();

     const {
        
        history,
        handleGoCreate,
        handleGoDetail,
        handleDeleteItem,
        handleBulkDeleteItem
      } = masterService.useMasterAction(PRODUCT_ROUTE, handleAction, handleBulkAction);

    
    
      const handleClickClearAllFilter = React.useCallback(() => {
        handleChangeAllFilter(new ProductFilter());
      }, [handleChangeAllFilter]);

      const menu = React.useCallback(
        (product: Product) => {
          const list: ListOverflowMenu[] = [
            {
              title: translate("general.actions.edit"),
              action:  handleGoDetail(product?.id)  ,
              isShow: true,
            },
            {
              title: translate("general.actions.preview"),
              action:  handleGoDetail(product?.id)  ,
              isShow: true,
            },
            {
              title: translate("general.actions.delete"),
              action: handleDeleteItem(product),
              isShow: true,
            },
          ];
          return <OverflowMenu list={list}></OverflowMenu>;
        },
        [ handleGoDetail  , handleDeleteItem, translate]
      );

    const columns: ColumnProps<Product>[] = useMemo(
            () => [
                    
                    
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "code"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("products.code")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].code),
                        dataIndex: nameof(list[0].code),
                        sorter: true,
                        sortOrder: getAntOrderType<Product, ProductFilter>
                            (
                                filter,
                                nameof(list[0].code),
                            ),
                        ellipsis: true,
                        render(...params: [string, Product, number]) {
                            return (
                                <LayoutCell orderType="left" tableSize="md">
                                  <OneLineText value={params[0]} />
                                </LayoutCell>
                              );
                            },
                    },
                    
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "supplierCode"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("products.supplierCode")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].supplierCode),
                        dataIndex: nameof(list[0].supplierCode),
                        sorter: true,
                        sortOrder: getAntOrderType<Product, ProductFilter>
                            (
                                filter,
                                nameof(list[0].supplierCode),
                            ),
                        ellipsis: true,
                        render(...params: [string, Product, number]) {
                            return (
                                <LayoutCell orderType="left" tableSize="md">
                                  <OneLineText value={params[0]} />
                                </LayoutCell>
                              );
                            },
                    },
                    
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "name"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("products.name")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].name),
                        dataIndex: nameof(list[0].name),
                        sorter: true,
                        sortOrder: getAntOrderType<Product, ProductFilter>
                            (
                                filter,
                                nameof(list[0].name),
                            ),
                        ellipsis: true,
                        render(...params: [string, Product, number]) {
                            return (
                                <LayoutCell orderType="left" tableSize="md">
                                  <OneLineText value={params[0]} />
                                </LayoutCell>
                              );
                            },
                    },
                    
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "description"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("products.description")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].description),
                        dataIndex: nameof(list[0].description),
                        sorter: true,
                        sortOrder: getAntOrderType<Product, ProductFilter>
                            (
                                filter,
                                nameof(list[0].description),
                            ),
                        ellipsis: true,
                        render(...params: [string, Product, number]) {
                            return (
                                <LayoutCell orderType="left" tableSize="md">
                                  <OneLineText value={params[0]} />
                                </LayoutCell>
                              );
                            },
                    },
                    
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "scanCode"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("products.scanCode")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].scanCode),
                        dataIndex: nameof(list[0].scanCode),
                        sorter: true,
                        sortOrder: getAntOrderType<Product, ProductFilter>
                            (
                                filter,
                                nameof(list[0].scanCode),
                            ),
                        ellipsis: true,
                        render(...params: [string, Product, number]) {
                            return (
                                <LayoutCell orderType="left" tableSize="md">
                                  <OneLineText value={params[0]} />
                                </LayoutCell>
                              );
                            },
                    },
                    
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "eRPCode"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("products.eRPCode")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].eRPCode),
                        dataIndex: nameof(list[0].eRPCode),
                        sorter: true,
                        sortOrder: getAntOrderType<Product, ProductFilter>
                            (
                                filter,
                                nameof(list[0].eRPCode),
                            ),
                        ellipsis: true,
                        render(...params: [string, Product, number]) {
                            return (
                                <LayoutCell orderType="left" tableSize="md">
                                  <OneLineText value={params[0]} />
                                </LayoutCell>
                              );
                            },
                    },
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "salePrice"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("products.salePrice")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].salePrice),
                        dataIndex: nameof(list[0].salePrice),
                        sorter: true,
                        sortOrder: getAntOrderType<Product, ProductFilter>
                            (
                                filter,
                                nameof(list[0].salePrice),
                            ),
                        ellipsis: true,
                        render(...params: [number, Product, number]) {
                            return (
                                <LayoutCell orderType="left" tableSize="md">
                                  <OneLineText value={formatNumber(params[0])} />
                                </LayoutCell>
                              );
                        },
                    },
                    
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "retailPrice"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("products.retailPrice")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].retailPrice),
                        dataIndex: nameof(list[0].retailPrice),
                        sorter: true,
                        sortOrder: getAntOrderType<Product, ProductFilter>
                            (
                                filter,
                                nameof(list[0].retailPrice),
                            ),
                        ellipsis: true,
                        render(...params: [number, Product, number]) {
                            return (
                                <LayoutCell orderType="left" tableSize="md">
                                  <OneLineText value={formatNumber(params[0])} />
                                </LayoutCell>
                              );
                        },
                    },
                    
                    
                    
                    
                    
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "otherName"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("products.otherName")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].otherName),
                        dataIndex: nameof(list[0].otherName),
                        sorter: true,
                        sortOrder: getAntOrderType<Product, ProductFilter>
                            (
                                filter,
                                nameof(list[0].otherName),
                            ),
                        ellipsis: true,
                        render(...params: [string, Product, number]) {
                            return (
                                <LayoutCell orderType="left" tableSize="md">
                                  <OneLineText value={params[0]} />
                                </LayoutCell>
                              );
                            },
                    },
                    
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "technicalName"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("products.technicalName")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].technicalName),
                        dataIndex: nameof(list[0].technicalName),
                        sorter: true,
                        sortOrder: getAntOrderType<Product, ProductFilter>
                            (
                                filter,
                                nameof(list[0].technicalName),
                            ),
                        ellipsis: true,
                        render(...params: [string, Product, number]) {
                            return (
                                <LayoutCell orderType="left" tableSize="md">
                                  <OneLineText value={params[0]} />
                                </LayoutCell>
                              );
                            },
                    },
                    
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "note"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("products.note")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].note),
                        dataIndex: nameof(list[0].note),
                        sorter: true,
                        sortOrder: getAntOrderType<Product, ProductFilter>
                            (
                                filter,
                                nameof(list[0].note),
                            ),
                        ellipsis: true,
                        render(...params: [string, Product, number]) {
                            return (
                                <LayoutCell orderType="left" tableSize="md">
                                  <OneLineText value={params[0]} />
                                </LayoutCell>
                              );
                            },
                    },
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "createdAt"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("products.createdAt")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].createdAt),
                        dataIndex: nameof(list[0].createdAt),
                        sorter: true,
                        sortOrder: getAntOrderType<Product, ProductFilter>
                            (
                                filter,
                                nameof(list[0].createdAt),
                            ),
                        ellipsis: true,
                        render(...params: [Moment, Product, number]) {
                            return (
                                <LayoutCell orderType="left" tableSize="md">
                                  <OneLineText value={formatDateTime(params[0])} />
                                </LayoutCell>
                              );
                        },
                    },
                    
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "updatedAt"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("products.updatedAt")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].updatedAt),
                        dataIndex: nameof(list[0].updatedAt),
                        sorter: true,
                        sortOrder: getAntOrderType<Product, ProductFilter>
                            (
                                filter,
                                nameof(list[0].updatedAt),
                            ),
                        ellipsis: true,
                        render(...params: [Moment, Product, number]) {
                            return (
                                <LayoutCell orderType="left" tableSize="md">
                                  <OneLineText value={formatDateTime(params[0])} />
                                </LayoutCell>
                              );
                        },
                    },
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "brand"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("products.brand")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].brand),
                        dataIndex: nameof(list[0].brand),
                        sorter: true,
                        sortOrder: getAntOrderType<Product, ProductFilter>
                        (
                        filter,
                        nameof(list[0].brand),
                        ),
                        ellipsis: true,
                        render(brand: Brand) {
                            return (
                            //fill the change the render field after generate code;
                            //if status change OneLine = StatusLine 
                            <LayoutCell orderType="left" tableSize="md">
                                <OneLineText value={ brand?.name } /> 
                            </LayoutCell>
                            );
                        },
                    },
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "category"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("products.category")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].category),
                        dataIndex: nameof(list[0].category),
                        sorter: true,
                        sortOrder: getAntOrderType<Product, ProductFilter>
                        (
                        filter,
                        nameof(list[0].category),
                        ),
                        ellipsis: true,
                        render(category: Category) {
                            return (
                            //fill the change the render field after generate code;
                            //if status change OneLine = StatusLine 
                            <LayoutCell orderType="left" tableSize="md">
                                <OneLineText value={ category?.name } /> 
                            </LayoutCell>
                            );
                        },
                    },
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "productType"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("products.productType")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].productType),
                        dataIndex: nameof(list[0].productType),
                        sorter: true,
                        sortOrder: getAntOrderType<Product, ProductFilter>
                        (
                        filter,
                        nameof(list[0].productType),
                        ),
                        ellipsis: true,
                        render(productType: ProductType) {
                            return (
                            //fill the change the render field after generate code;
                            //if status change OneLine = StatusLine 
                            <LayoutCell orderType="left" tableSize="md">
                                <OneLineText value={ productType?.name } /> 
                            </LayoutCell>
                            );
                        },
                    },
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "status"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("products.status")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].status),
                        dataIndex: nameof(list[0].status),
                        sorter: true,
                        sortOrder: getAntOrderType<Product, ProductFilter>
                        (
                        filter,
                        nameof(list[0].status),
                        ),
                        ellipsis: true,
                        render(status: Status) {
                            return (
                            //fill the change the render field after generate code;
                            //if status change OneLine = StatusLine 
                            <LayoutCell orderType="left" tableSize="md">
                                <OneLineText value={ status?.name } /> 
                            </LayoutCell>
                            );
                        },
                    },
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "taxType"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("products.taxType")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].taxType),
                        dataIndex: nameof(list[0].taxType),
                        sorter: true,
                        sortOrder: getAntOrderType<Product, ProductFilter>
                        (
                        filter,
                        nameof(list[0].taxType),
                        ),
                        ellipsis: true,
                        render(taxType: TaxType) {
                            return (
                            //fill the change the render field after generate code;
                            //if status change OneLine = StatusLine 
                            <LayoutCell orderType="left" tableSize="md">
                                <OneLineText value={ taxType?.name } /> 
                            </LayoutCell>
                            );
                        },
                    },
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "unitOfMeasure"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("products.unitOfMeasure")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].unitOfMeasure),
                        dataIndex: nameof(list[0].unitOfMeasure),
                        sorter: true,
                        sortOrder: getAntOrderType<Product, ProductFilter>
                        (
                        filter,
                        nameof(list[0].unitOfMeasure),
                        ),
                        ellipsis: true,
                        render(unitOfMeasure: UnitOfMeasure) {
                            return (
                            //fill the change the render field after generate code;
                            //if status change OneLine = StatusLine 
                            <LayoutCell orderType="left" tableSize="md">
                                <OneLineText value={ unitOfMeasure?.name } /> 
                            </LayoutCell>
                            );
                        },
                    },
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "unitOfMeasureGrouping"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("products.unitOfMeasureGrouping")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].unitOfMeasureGrouping),
                        dataIndex: nameof(list[0].unitOfMeasureGrouping),
                        sorter: true,
                        sortOrder: getAntOrderType<Product, ProductFilter>
                        (
                        filter,
                        nameof(list[0].unitOfMeasureGrouping),
                        ),
                        ellipsis: true,
                        render(unitOfMeasureGrouping: UnitOfMeasureGrouping) {
                            return (
                            //fill the change the render field after generate code;
                            //if status change OneLine = StatusLine 
                            <LayoutCell orderType="left" tableSize="md">
                                <OneLineText value={ unitOfMeasureGrouping?.name } /> 
                            </LayoutCell>
                            );
                        },
                    },
                    
                    {
                        key: "action",
                        dataIndex: nameof(list[0].id),
                        fixed: "right",
                        width: 48,
                        align: "center",
                        render(id: number, product: Product) {
                            return (
                                <div className='d-flex justify-content-center button-action-table'>
                                    {menu(product)}
                                </div>
                           );
                        }
                    },
             ], [list, filter, translate, menu]);

    const listActions: any = [
        {
          title: "Tải lên",
          isShow: true,
          action: () => {
            importActionRef.current.click();
          },
        },
        {
          title: "Tải xuống",
          isShow: true,
          action: handleListExport(filter, productRepository.export),
        },
        {
          title: "Tải xuống Template",
          isShow: true,
          action: handleExportTemplateList(productRepository.exportTemplate),
        },
      ];
    return {
        visible,
        setVisible,
        handleChangeInputSearch,
        handleChangeAllFilter,
        handleClickClearAllFilter,
        importActionRef,
        handleClick,
        handleImportList,
        list,
        filter,
        countFilter,
        columns,
        count,
        loadingList,
        listActions,
        handleTableChange,
        handlePagination,
        handleBulkDeleteItem,
        canBulkAction,
        rowSelection,
        selectedRowKeys,
        setSelectedRowKeys,
        history,
        handleGoCreate,
        handleGoDetail,
    }
 }