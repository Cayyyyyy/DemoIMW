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
/* end filter import */

/* begin individual import */
import { unitOfMeasureGroupingRepository } from "../UnitOfMeasureGroupingRepository";
import { UnitOfMeasureGrouping, UnitOfMeasureGroupingFilter } from "models/UnitOfMeasureGrouping";
import { Status, StatusFilter } from "models/Status";
import { UnitOfMeasure, UnitOfMeasureFilter } from "models/UnitOfMeasure";
import { UNIT_OF_MEASURE_GROUPING_ROUTE } from "config/routes";

/* end individual import */

export function useUnitOfMeasureGroupingMasterHook() {
    const [translate] = useTranslation();
    const [visible, setVisible] = React.useState(false);
    const [modelFilter, dispatch, countFilter] =
        queryStringService.useQueryString(UnitOfMeasureGroupingFilter, { skip: 0, take: 10 });

   
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
    } = listService.useList(unitOfMeasureGroupingRepository.list, unitOfMeasureGroupingRepository.count, filter, dispatch);

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
    } = listService.useRowSelection(unitOfMeasureGroupingRepository.delete, unitOfMeasureGroupingRepository.bulkDelete, null, null, null, handleResetList);

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
      } = masterService.useMasterAction(UNIT_OF_MEASURE_GROUPING_ROUTE, handleAction, handleBulkAction);

    
    
      const handleClickClearAllFilter = React.useCallback(() => {
        handleChangeAllFilter(new UnitOfMeasureGroupingFilter());
      }, [handleChangeAllFilter]);

      const menu = React.useCallback(
        (unitOfMeasureGrouping: UnitOfMeasureGrouping) => {
          const list: ListOverflowMenu[] = [
            {
              title: translate("general.actions.edit"),
              action:  handleGoDetail(unitOfMeasureGrouping?.id)  ,
              isShow: true,
            },
            {
              title: translate("general.actions.preview"),
              action:  handleGoDetail(unitOfMeasureGrouping?.id)  ,
              isShow: true,
            },
            {
              title: translate("general.actions.delete"),
              action: handleDeleteItem(unitOfMeasureGrouping),
              isShow: true,
            },
          ];
          return <OverflowMenu list={list}></OverflowMenu>;
        },
        [ handleGoDetail  , handleDeleteItem, translate]
      );

    const columns: ColumnProps<UnitOfMeasureGrouping>[] = useMemo(
            () => [
                    
                    
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "code"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("unitOfMeasureGroupings.code")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].code),
                        dataIndex: nameof(list[0].code),
                        sorter: true,
                        sortOrder: getAntOrderType<UnitOfMeasureGrouping, UnitOfMeasureGroupingFilter>
                            (
                                filter,
                                nameof(list[0].code),
                            ),
                        ellipsis: true,
                        render(...params: [string, UnitOfMeasureGrouping, number]) {
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
                                                    title={translate("unitOfMeasureGroupings.name")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].name),
                        dataIndex: nameof(list[0].name),
                        sorter: true,
                        sortOrder: getAntOrderType<UnitOfMeasureGrouping, UnitOfMeasureGroupingFilter>
                            (
                                filter,
                                nameof(list[0].name),
                            ),
                        ellipsis: true,
                        render(...params: [string, UnitOfMeasureGrouping, number]) {
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
                                                    title={translate("unitOfMeasureGroupings.description")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].description),
                        dataIndex: nameof(list[0].description),
                        sorter: true,
                        sortOrder: getAntOrderType<UnitOfMeasureGrouping, UnitOfMeasureGroupingFilter>
                            (
                                filter,
                                nameof(list[0].description),
                            ),
                        ellipsis: true,
                        render(...params: [string, UnitOfMeasureGrouping, number]) {
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
                                                  title={translate("unitOfMeasureGroupings.createdAt")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].createdAt),
                        dataIndex: nameof(list[0].createdAt),
                        sorter: true,
                        sortOrder: getAntOrderType<UnitOfMeasureGrouping, UnitOfMeasureGroupingFilter>
                            (
                                filter,
                                nameof(list[0].createdAt),
                            ),
                        ellipsis: true,
                        render(...params: [Moment, UnitOfMeasureGrouping, number]) {
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
                                                  title={translate("unitOfMeasureGroupings.updatedAt")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].updatedAt),
                        dataIndex: nameof(list[0].updatedAt),
                        sorter: true,
                        sortOrder: getAntOrderType<UnitOfMeasureGrouping, UnitOfMeasureGroupingFilter>
                            (
                                filter,
                                nameof(list[0].updatedAt),
                            ),
                        ellipsis: true,
                        render(...params: [Moment, UnitOfMeasureGrouping, number]) {
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
                                ({ column }) => column.key === "status"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("unitOfMeasureGroupings.status")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].status),
                        dataIndex: nameof(list[0].status),
                        sorter: true,
                        sortOrder: getAntOrderType<UnitOfMeasureGrouping, UnitOfMeasureGroupingFilter>
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
                                ({ column }) => column.key === "unitOfMeasure"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("unitOfMeasureGroupings.unitOfMeasure")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].unitOfMeasure),
                        dataIndex: nameof(list[0].unitOfMeasure),
                        sorter: true,
                        sortOrder: getAntOrderType<UnitOfMeasureGrouping, UnitOfMeasureGroupingFilter>
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
                        key: "action",
                        dataIndex: nameof(list[0].id),
                        fixed: "right",
                        width: 48,
                        align: "center",
                        render(id: number, unitOfMeasureGrouping: UnitOfMeasureGrouping) {
                            return (
                                <div className='d-flex justify-content-center button-action-table'>
                                    {menu(unitOfMeasureGrouping)}
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
          action: handleListExport(filter, unitOfMeasureGroupingRepository.export),
        },
        {
          title: "Tải xuống Template",
          isShow: true,
          action: handleExportTemplateList(unitOfMeasureGroupingRepository.exportTemplate),
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