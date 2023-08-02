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
import { imageRepository } from "../ImageRepository";
import { Image, ImageFilter } from "models/Image";
import { IMAGE_ROUTE } from "config/routes";

/* end individual import */

export function useImageMasterHook() {
    const [translate] = useTranslation();
    const [visible, setVisible] = React.useState(false);
    const [modelFilter, dispatch, countFilter] =
        queryStringService.useQueryString(ImageFilter, { skip: 0, take: 10 });

   
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
    } = listService.useList(imageRepository.list, imageRepository.count, filter, dispatch);

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
    } = listService.useRowSelection(imageRepository.delete, imageRepository.bulkDelete, null, null, null, handleResetList);

    const { handleImportList, ref: importActionRef, handleClick } = importExportService.useImport(handleResetList);


    const {
        handleListExport,
        handleExportTemplateList,
    } = importExportService.useExport();

     const {
        
        handleDeleteItem,
        handleBulkDeleteItem
      } = masterService.useMasterAction(IMAGE_ROUTE, handleAction, handleBulkAction);

    
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
        handleChangeDateField,
    } = detailService.useDetailModal(
        Image,
        imageRepository.get,
        imageRepository.save,
        handleLoadList,
    );
    
      const handleClickClearAllFilter = React.useCallback(() => {
        handleChangeAllFilter(new ImageFilter());
      }, [handleChangeAllFilter]);

      const menu = React.useCallback(
        (image: Image) => {
          const list: ListOverflowMenu[] = [
            {
              title: translate("general.actions.edit"),
              action:   ()=>handleOpenDetailModal(image?.id),
              isShow: true,
            },
            {
              title: translate("general.actions.preview"),
              action:   ()=>handleOpenDetailModal(image?.id),
              isShow: true,
            },
            {
              title: translate("general.actions.delete"),
              action: handleDeleteItem(image),
              isShow: true,
            },
          ];
          return <OverflowMenu list={list}></OverflowMenu>;
        },
        [  handleOpenDetailModal, handleDeleteItem, translate]
      );

    const columns: ColumnProps<Image>[] = useMemo(
            () => [
                    
                    
                    
                    
                    {
                        title: ({ sortColumns }) => {
                              const sortedColumn = sortColumns?.find(
                                ({ column }) => column.key === "name"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("images.name")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].name),
                        dataIndex: nameof(list[0].name),
                        sorter: true,
                        sortOrder: getAntOrderType<Image, ImageFilter>
                            (
                                filter,
                                nameof(list[0].name),
                            ),
                        ellipsis: true,
                        render(...params: [string, Image, number]) {
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
                                ({ column }) => column.key === "url"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("images.url")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].url),
                        dataIndex: nameof(list[0].url),
                        sorter: true,
                        sortOrder: getAntOrderType<Image, ImageFilter>
                            (
                                filter,
                                nameof(list[0].url),
                            ),
                        ellipsis: true,
                        render(...params: [string, Image, number]) {
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
                                                  title={translate("images.createdAt")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].createdAt),
                        dataIndex: nameof(list[0].createdAt),
                        sorter: true,
                        sortOrder: getAntOrderType<Image, ImageFilter>
                            (
                                filter,
                                nameof(list[0].createdAt),
                            ),
                        ellipsis: true,
                        render(...params: [Moment, Image, number]) {
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
                                                  title={translate("images.updatedAt")}
                                                  sortedColumn={sortedColumn}
                                                  isSorter />
                                </div>
                              );
                            },
                        key: nameof(list[0].updatedAt),
                        dataIndex: nameof(list[0].updatedAt),
                        sorter: true,
                        sortOrder: getAntOrderType<Image, ImageFilter>
                            (
                                filter,
                                nameof(list[0].updatedAt),
                            ),
                        ellipsis: true,
                        render(...params: [Moment, Image, number]) {
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
                                ({ column }) => column.key === "thumbnailUrl"
                              );
                              return (
                                  <div>
                                      <LayoutHeader orderType='left'
                                                    title={translate("images.thumbnailUrl")}
                                                    sortedColumn={sortedColumn}
                                                    isSorter />
                                  </div>
                              );
                            },
                        key: nameof(list[0].thumbnailUrl),
                        dataIndex: nameof(list[0].thumbnailUrl),
                        sorter: true,
                        sortOrder: getAntOrderType<Image, ImageFilter>
                            (
                                filter,
                                nameof(list[0].thumbnailUrl),
                            ),
                        ellipsis: true,
                        render(...params: [string, Image, number]) {
                            return (
                                <LayoutCell orderType="left" tableSize="md">
                                  <OneLineText value={params[0]} />
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
                        render(id: number, image: Image) {
                            return (
                                <div className='d-flex justify-content-center button-action-table'>
                                    {menu(image)}
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
          action: handleListExport(filter, imageRepository.export),
        },
        {
          title: "Tải xuống Template",
          isShow: true,
          action: handleExportTemplateList(imageRepository.exportTemplate),
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
        handleChangeDateField,
        handleCloseDetailModal,
        handleOpenDetailModal,
    }
 }