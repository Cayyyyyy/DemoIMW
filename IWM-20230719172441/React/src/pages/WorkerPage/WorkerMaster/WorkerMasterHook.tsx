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
import { workerRepository } from "../WorkerRepository";
import { Worker, WorkerFilter } from "models/Worker";
import { District, DistrictFilter } from "models/District";
import { Nation, NationFilter } from "models/Nation";
import { Province, ProvinceFilter } from "models/Province";
import { Sex, SexFilter } from "models/Sex";
import { Status, StatusFilter } from "models/Status";
import { Ward, WardFilter } from "models/Ward";
import { WorkerGroup, WorkerGroupFilter } from "models/WorkerGroup";
import { WORKER_ROUTE } from "config/routes";

/* end individual import */

export function useWorkerMasterHook() {
    const [translate] = useTranslation();
    const [visible, setVisible] = React.useState(false);
    const [modelFilter, dispatch, countFilter] =
        queryStringService.useQueryString(WorkerFilter, { skip: 0, take: 10 });

   
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
    } = listService.useList(workerRepository.list, workerRepository.count, filter, dispatch);

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
    } = listService.useRowSelection(workerRepository.delete, workerRepository.bulkDelete, null, null, null, handleResetList);

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
      } = masterService.useMasterAction(WORKER_ROUTE, handleAction, handleBulkAction);

    
    
      const handleClickClearAllFilter = React.useCallback(() => {
        handleChangeAllFilter(new WorkerFilter());
      }, [handleChangeAllFilter]);

      const menu = React.useCallback(
        (worker: Worker) => {
          const list: ListOverflowMenu[] = [
            {
              title: translate("general.actions.edit"),
              action:  handleGoDetail(worker?.id)  ,
              isShow: true,
            },
            {
              title: translate("general.actions.preview"),
              action:  handleGoDetail(worker?.id)  ,
              isShow: true,
            },
            {
              title: translate("general.actions.delete"),
              action: handleDeleteItem(worker),
              isShow: true,
            },
          ];
          return <OverflowMenu list={list}></OverflowMenu>;
        },
        [ handleGoDetail  , handleDeleteItem, translate]
      );

    const columns: ColumnProps<Worker>[] = useMemo(
            () => [
                    
                    
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "code"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("workers.code")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].code),
                        dataIndex: nameof(list[0].code),
                        sorter: true,
                        sortOrder: getAntOrderType<Worker, WorkerFilter>
                            (
                                filter,
                                nameof(list[0].code),
                            ),
                        ellipsis: true,
                        render(...params: [string, Worker, number]) {
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
                                                    title={translate("workers.name")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].name),
                        dataIndex: nameof(list[0].name),
                        sorter: true,
                        sortOrder: getAntOrderType<Worker, WorkerFilter>
                            (
                                filter,
                                nameof(list[0].name),
                            ),
                        ellipsis: true,
                        render(...params: [string, Worker, number]) {
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
                                ({ column }) => column.key === "birthday"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("workers.birthday")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].birthday),
                        dataIndex: nameof(list[0].birthday),
                        sorter: true,
                        sortOrder: getAntOrderType<Worker, WorkerFilter>
                            (
                                filter,
                                nameof(list[0].birthday),
                            ),
                        ellipsis: true,
                        render(...params: [Moment, Worker, number]) {
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
                                ({ column }) => column.key === "phone"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("workers.phone")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].phone),
                        dataIndex: nameof(list[0].phone),
                        sorter: true,
                        sortOrder: getAntOrderType<Worker, WorkerFilter>
                            (
                                filter,
                                nameof(list[0].phone),
                            ),
                        ellipsis: true,
                        render(...params: [string, Worker, number]) {
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
                                ({ column }) => column.key === "citizenIdentificationNumber"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("workers.citizenIdentificationNumber")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].citizenIdentificationNumber),
                        dataIndex: nameof(list[0].citizenIdentificationNumber),
                        sorter: true,
                        sortOrder: getAntOrderType<Worker, WorkerFilter>
                            (
                                filter,
                                nameof(list[0].citizenIdentificationNumber),
                            ),
                        ellipsis: true,
                        render(...params: [string, Worker, number]) {
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
                                ({ column }) => column.key === "email"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("workers.email")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].email),
                        dataIndex: nameof(list[0].email),
                        sorter: true,
                        sortOrder: getAntOrderType<Worker, WorkerFilter>
                            (
                                filter,
                                nameof(list[0].email),
                            ),
                        ellipsis: true,
                        render(...params: [string, Worker, number]) {
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
                                ({ column }) => column.key === "address"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("workers.address")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].address),
                        dataIndex: nameof(list[0].address),
                        sorter: true,
                        sortOrder: getAntOrderType<Worker, WorkerFilter>
                            (
                                filter,
                                nameof(list[0].address),
                            ),
                        ellipsis: true,
                        render(...params: [string, Worker, number]) {
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
                                ({ column }) => column.key === "username"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("workers.username")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].username),
                        dataIndex: nameof(list[0].username),
                        sorter: true,
                        sortOrder: getAntOrderType<Worker, WorkerFilter>
                            (
                                filter,
                                nameof(list[0].username),
                            ),
                        ellipsis: true,
                        render(...params: [string, Worker, number]) {
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
                                ({ column }) => column.key === "password"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("workers.password")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].password),
                        dataIndex: nameof(list[0].password),
                        sorter: true,
                        sortOrder: getAntOrderType<Worker, WorkerFilter>
                            (
                                filter,
                                nameof(list[0].password),
                            ),
                        ellipsis: true,
                        render(...params: [string, Worker, number]) {
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
                                ({ column }) => column.key === "district"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("workers.district")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].district),
                        dataIndex: nameof(list[0].district),
                        sorter: true,
                        sortOrder: getAntOrderType<Worker, WorkerFilter>
                        (
                        filter,
                        nameof(list[0].district),
                        ),
                        ellipsis: true,
                        render(district: District) {
                            return (
                            //fill the change the render field after generate code;
                            //if status change OneLine = StatusLine 
                            <LayoutCell orderType="left" tableSize="md">
                                <OneLineText value={ district?.name } /> 
                            </LayoutCell>
                            );
                        },
                    },
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "nation"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("workers.nation")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].nation),
                        dataIndex: nameof(list[0].nation),
                        sorter: true,
                        sortOrder: getAntOrderType<Worker, WorkerFilter>
                        (
                        filter,
                        nameof(list[0].nation),
                        ),
                        ellipsis: true,
                        render(nation: Nation) {
                            return (
                            //fill the change the render field after generate code;
                            //if status change OneLine = StatusLine 
                            <LayoutCell orderType="left" tableSize="md">
                                <OneLineText value={ nation?.name } /> 
                            </LayoutCell>
                            );
                        },
                    },
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "province"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("workers.province")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].province),
                        dataIndex: nameof(list[0].province),
                        sorter: true,
                        sortOrder: getAntOrderType<Worker, WorkerFilter>
                        (
                        filter,
                        nameof(list[0].province),
                        ),
                        ellipsis: true,
                        render(province: Province) {
                            return (
                            //fill the change the render field after generate code;
                            //if status change OneLine = StatusLine 
                            <LayoutCell orderType="left" tableSize="md">
                                <OneLineText value={ province?.name } /> 
                            </LayoutCell>
                            );
                        },
                    },
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "sex"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("workers.sex")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].sex),
                        dataIndex: nameof(list[0].sex),
                        sorter: true,
                        sortOrder: getAntOrderType<Worker, WorkerFilter>
                        (
                        filter,
                        nameof(list[0].sex),
                        ),
                        ellipsis: true,
                        render(sex: Sex) {
                            return (
                            //fill the change the render field after generate code;
                            //if status change OneLine = StatusLine 
                            <LayoutCell orderType="left" tableSize="md">
                                <OneLineText value={ sex?.name } /> 
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
                                                  title={translate("workers.status")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].status),
                        dataIndex: nameof(list[0].status),
                        sorter: true,
                        sortOrder: getAntOrderType<Worker, WorkerFilter>
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
                                ({ column }) => column.key === "ward"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("workers.ward")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].ward),
                        dataIndex: nameof(list[0].ward),
                        sorter: true,
                        sortOrder: getAntOrderType<Worker, WorkerFilter>
                        (
                        filter,
                        nameof(list[0].ward),
                        ),
                        ellipsis: true,
                        render(ward: Ward) {
                            return (
                            //fill the change the render field after generate code;
                            //if status change OneLine = StatusLine 
                            <LayoutCell orderType="left" tableSize="md">
                                <OneLineText value={ ward?.name } /> 
                            </LayoutCell>
                            );
                        },
                    },
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "workerGroup"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("workers.workerGroup")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].workerGroup),
                        dataIndex: nameof(list[0].workerGroup),
                        sorter: true,
                        sortOrder: getAntOrderType<Worker, WorkerFilter>
                        (
                        filter,
                        nameof(list[0].workerGroup),
                        ),
                        ellipsis: true,
                        render(workerGroup: WorkerGroup) {
                            return (
                            //fill the change the render field after generate code;
                            //if status change OneLine = StatusLine 
                            <LayoutCell orderType="left" tableSize="md">
                                <OneLineText value={ workerGroup?.name } /> 
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
                        render(id: number, worker: Worker) {
                            return (
                                <div className='d-flex justify-content-center button-action-table'>
                                    {menu(worker)}
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
          action: handleListExport(filter, workerRepository.export),
        },
        {
          title: "Tải xuống Template",
          isShow: true,
          action: handleExportTemplateList(workerRepository.exportTemplate),
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