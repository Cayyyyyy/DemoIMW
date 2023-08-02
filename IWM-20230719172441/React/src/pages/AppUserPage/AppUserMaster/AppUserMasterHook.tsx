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
import { appUserRepository } from "../AppUserRepository";
import { AppUser, AppUserFilter } from "models/AppUser";
import { Organization, OrganizationFilter } from "models/Organization";
import { Sex, SexFilter } from "models/Sex";
import { Status, StatusFilter } from "models/Status";
import { APP_USER_ROUTE } from "config/routes";

/* end individual import */

export function useAppUserMasterHook() {
    const [translate] = useTranslation();
    const [visible, setVisible] = React.useState(false);
    const [modelFilter, dispatch, countFilter] =
        queryStringService.useQueryString(AppUserFilter, { skip: 0, take: 10 });

   
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
    } = listService.useList(appUserRepository.list, appUserRepository.count, filter, dispatch);

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
    } = listService.useRowSelection(appUserRepository.delete, appUserRepository.bulkDelete, null, null, null, handleResetList);

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
      } = masterService.useMasterAction(APP_USER_ROUTE, handleAction, handleBulkAction);

    
    
      const handleClickClearAllFilter = React.useCallback(() => {
        handleChangeAllFilter(new AppUserFilter());
      }, [handleChangeAllFilter]);

      const menu = React.useCallback(
        (appUser: AppUser) => {
          const list: ListOverflowMenu[] = [
            {
              title: translate("general.actions.edit"),
              action:  handleGoDetail(appUser?.id)  ,
              isShow: true,
            },
            {
              title: translate("general.actions.preview"),
              action:  handleGoDetail(appUser?.id)  ,
              isShow: true,
            },
            {
              title: translate("general.actions.delete"),
              action: handleDeleteItem(appUser),
              isShow: true,
            },
          ];
          return <OverflowMenu list={list}></OverflowMenu>;
        },
        [ handleGoDetail  , handleDeleteItem, translate]
      );

    const columns: ColumnProps<AppUser>[] = useMemo(
            () => [
                    
                    
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "username"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("appUsers.username")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].username),
                        dataIndex: nameof(list[0].username),
                        sorter: true,
                        sortOrder: getAntOrderType<AppUser, AppUserFilter>
                            (
                                filter,
                                nameof(list[0].username),
                            ),
                        ellipsis: true,
                        render(...params: [string, AppUser, number]) {
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
                                ({ column }) => column.key === "displayName"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("appUsers.displayName")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].displayName),
                        dataIndex: nameof(list[0].displayName),
                        sorter: true,
                        sortOrder: getAntOrderType<AppUser, AppUserFilter>
                            (
                                filter,
                                nameof(list[0].displayName),
                            ),
                        ellipsis: true,
                        render(...params: [string, AppUser, number]) {
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
                                                    title={translate("appUsers.address")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].address),
                        dataIndex: nameof(list[0].address),
                        sorter: true,
                        sortOrder: getAntOrderType<AppUser, AppUserFilter>
                            (
                                filter,
                                nameof(list[0].address),
                            ),
                        ellipsis: true,
                        render(...params: [string, AppUser, number]) {
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
                                                    title={translate("appUsers.email")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].email),
                        dataIndex: nameof(list[0].email),
                        sorter: true,
                        sortOrder: getAntOrderType<AppUser, AppUserFilter>
                            (
                                filter,
                                nameof(list[0].email),
                            ),
                        ellipsis: true,
                        render(...params: [string, AppUser, number]) {
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
                                ({ column }) => column.key === "phone"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("appUsers.phone")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].phone),
                        dataIndex: nameof(list[0].phone),
                        sorter: true,
                        sortOrder: getAntOrderType<AppUser, AppUserFilter>
                            (
                                filter,
                                nameof(list[0].phone),
                            ),
                        ellipsis: true,
                        render(...params: [string, AppUser, number]) {
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
                                                  title={translate("appUsers.birthday")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].birthday),
                        dataIndex: nameof(list[0].birthday),
                        sorter: true,
                        sortOrder: getAntOrderType<AppUser, AppUserFilter>
                            (
                                filter,
                                nameof(list[0].birthday),
                            ),
                        ellipsis: true,
                        render(...params: [Moment, AppUser, number]) {
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
                                ({ column }) => column.key === "avatar"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("appUsers.avatar")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].avatar),
                        dataIndex: nameof(list[0].avatar),
                        sorter: true,
                        sortOrder: getAntOrderType<AppUser, AppUserFilter>
                            (
                                filter,
                                nameof(list[0].avatar),
                            ),
                        ellipsis: true,
                        render(...params: [string, AppUser, number]) {
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
                                ({ column }) => column.key === "department"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("appUsers.department")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].department),
                        dataIndex: nameof(list[0].department),
                        sorter: true,
                        sortOrder: getAntOrderType<AppUser, AppUserFilter>
                            (
                                filter,
                                nameof(list[0].department),
                            ),
                        ellipsis: true,
                        render(...params: [string, AppUser, number]) {
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
                                                  title={translate("appUsers.createdAt")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].createdAt),
                        dataIndex: nameof(list[0].createdAt),
                        sorter: true,
                        sortOrder: getAntOrderType<AppUser, AppUserFilter>
                            (
                                filter,
                                nameof(list[0].createdAt),
                            ),
                        ellipsis: true,
                        render(...params: [Moment, AppUser, number]) {
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
                                                  title={translate("appUsers.updatedAt")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].updatedAt),
                        dataIndex: nameof(list[0].updatedAt),
                        sorter: true,
                        sortOrder: getAntOrderType<AppUser, AppUserFilter>
                            (
                                filter,
                                nameof(list[0].updatedAt),
                            ),
                        ellipsis: true,
                        render(...params: [Moment, AppUser, number]) {
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
                                ({ column }) => column.key === "code"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("appUsers.code")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].code),
                        dataIndex: nameof(list[0].code),
                        sorter: true,
                        sortOrder: getAntOrderType<AppUser, AppUserFilter>
                            (
                                filter,
                                nameof(list[0].code),
                            ),
                        ellipsis: true,
                        render(...params: [string, AppUser, number]) {
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
                                                    title={translate("appUsers.name")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].name),
                        dataIndex: nameof(list[0].name),
                        sorter: true,
                        sortOrder: getAntOrderType<AppUser, AppUserFilter>
                            (
                                filter,
                                nameof(list[0].name),
                            ),
                        ellipsis: true,
                        render(...params: [string, AppUser, number]) {
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
                                ({ column }) => column.key === "organization"
                              );
                              return (
                                <div>
                                    <LayoutHeader orderType='left'
                                                  title={translate("appUsers.organization")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].organization),
                        dataIndex: nameof(list[0].organization),
                        sorter: true,
                        sortOrder: getAntOrderType<AppUser, AppUserFilter>
                        (
                        filter,
                        nameof(list[0].organization),
                        ),
                        ellipsis: true,
                        render(organization: Organization) {
                            return (
                            //fill the change the render field after generate code;
                            //if status change OneLine = StatusLine 
                            <LayoutCell orderType="left" tableSize="md">
                                <OneLineText value={ organization?.name } /> 
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
                                                  title={translate("appUsers.sex")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].sex),
                        dataIndex: nameof(list[0].sex),
                        sorter: true,
                        sortOrder: getAntOrderType<AppUser, AppUserFilter>
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
                                                  title={translate("appUsers.status")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].status),
                        dataIndex: nameof(list[0].status),
                        sorter: true,
                        sortOrder: getAntOrderType<AppUser, AppUserFilter>
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
                        key: "action",
                        dataIndex: nameof(list[0].id),
                        fixed: "right",
                        width: 48,
                        align: "center",
                        render(id: number, appUser: AppUser) {
                            return (
                                <div className='d-flex justify-content-center button-action-table'>
                                    {menu(appUser)}
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
          action: handleListExport(filter, appUserRepository.export),
        },
        {
          title: "Tải xuống Template",
          isShow: true,
          action: handleExportTemplateList(appUserRepository.exportTemplate),
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