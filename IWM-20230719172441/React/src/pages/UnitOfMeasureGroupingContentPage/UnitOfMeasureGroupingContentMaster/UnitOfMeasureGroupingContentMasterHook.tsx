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
import { formatNumber } from "core/helpers/number";
/* end filter import */

/* begin individual import */
import { unitOfMeasureGroupingContentRepository } from "../UnitOfMeasureGroupingContentRepository";
import { UnitOfMeasureGroupingContent, UnitOfMeasureGroupingContentFilter } from "models/UnitOfMeasureGroupingContent";
import { UnitOfMeasure, UnitOfMeasureFilter } from "models/UnitOfMeasure";
import { UnitOfMeasureGrouping, UnitOfMeasureGroupingFilter } from "models/UnitOfMeasureGrouping";
import { UNIT_OF_MEASURE_GROUPING_CONTENT_ROUTE } from "config/routes";

/* end individual import */

export function useUnitOfMeasureGroupingContentMasterHook() {
    const [translate] = useTranslation();
    const [visible, setVisible] = React.useState(false);
    const [modelFilter, dispatch, countFilter] =
        queryStringService.useQueryString(UnitOfMeasureGroupingContentFilter, { skip: 0, take: 10 });

   
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
    } = listService.useList(unitOfMeasureGroupingContentRepository.list, unitOfMeasureGroupingContentRepository.count, filter, dispatch);

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
    } = listService.useRowSelection(unitOfMeasureGroupingContentRepository.delete, unitOfMeasureGroupingContentRepository.bulkDelete, null, null, null, handleResetList);

    const { handleImportList, ref: importActionRef, handleClick } = importExportService.useImport(handleResetList);


    const {
        handleListExport,
        handleExportTemplateList,
    } = importExportService.useExport();

     const {
        
        handleDeleteItem,
        handleBulkDeleteItem
      } = masterService.useMasterAction(UNIT_OF_MEASURE_GROUPING_CONTENT_ROUTE, handleAction, handleBulkAction);

    
    const {
        model,
        dispatch: dispatchModal,
        isOpenDetailModal,
        handleOpenDetailModal,
        handleCloseDetailModal,
        handleSaveModel,
        loadingModel,
        handleChangeSingleField,
        handleChangeSelectField,
    } = detailService.useDetailModal(
        UnitOfMeasureGroupingContent,
        unitOfMeasureGroupingContentRepository.get,
        unitOfMeasureGroupingContentRepository.save,
        handleLoadList,
    );
    
      const handleClickClearAllFilter = React.useCallback(() => {
        handleChangeAllFilter(new UnitOfMeasureGroupingContentFilter());
      }, [handleChangeAllFilter]);

      const menu = React.useCallback(
        (unitOfMeasureGroupingContent: UnitOfMeasureGroupingContent) => {
          const list: ListOverflowMenu[] = [
            {
              title: translate("general.actions.edit"),
              action:   ()=>handleOpenDetailModal(unitOfMeasureGroupingContent?.id),
              isShow: true,
            },
            {
              title: translate("general.actions.preview"),
              action:   ()=>handleOpenDetailModal(unitOfMeasureGroupingContent?.id),
              isShow: true,
            },
            {
              title: translate("general.actions.delete"),
              action: handleDeleteItem(unitOfMeasureGroupingContent),
              isShow: true,
            },
          ];
          return <OverflowMenu list={list}></OverflowMenu>;
        },
        [  handleOpenDetailModal, handleDeleteItem, translate]
      );

    const columns: ColumnProps<UnitOfMeasureGroupingContent>[] = useMemo(
            () => [
                    
                    
                    
                    
                    
                    
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "factor"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("unitOfMeasureGroupingContents.factor")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].factor),
                        dataIndex: nameof(list[0].factor),
                        sorter: true,
                        sortOrder: getAntOrderType<UnitOfMeasureGroupingContent, UnitOfMeasureGroupingContentFilter>
                            (
                                filter,
                                nameof(list[0].factor),
                            ),
                        ellipsis: true,
                        render(...params: [number, UnitOfMeasureGroupingContent, number]) {
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
                                ({ column }) => column.key === "unitOfMeasure"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("unitOfMeasureGroupingContents.unitOfMeasure")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].unitOfMeasure),
                        dataIndex: nameof(list[0].unitOfMeasure),
                        sorter: true,
                        sortOrder: getAntOrderType<UnitOfMeasureGroupingContent, UnitOfMeasureGroupingContentFilter>
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
                                                  title={translate("unitOfMeasureGroupingContents.unitOfMeasureGrouping")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].unitOfMeasureGrouping),
                        dataIndex: nameof(list[0].unitOfMeasureGrouping),
                        sorter: true,
                        sortOrder: getAntOrderType<UnitOfMeasureGroupingContent, UnitOfMeasureGroupingContentFilter>
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
                        render(id: number, unitOfMeasureGroupingContent: UnitOfMeasureGroupingContent) {
                            return (
                                <div className='d-flex justify-content-center button-action-table'>
                                    {menu(unitOfMeasureGroupingContent)}
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
          action: handleListExport(filter, unitOfMeasureGroupingContentRepository.export),
        },
        {
          title: "Tải xuống Template",
          isShow: true,
          action: handleExportTemplateList(unitOfMeasureGroupingContentRepository.exportTemplate),
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
        model,
        dispatchModal,
        isOpenDetailModal,
        handleSaveModel,
        loadingModel,
        handleChangeSingleField,
        handleChangeSelectField,
        handleCloseDetailModal,
        handleOpenDetailModal,
    }
 }