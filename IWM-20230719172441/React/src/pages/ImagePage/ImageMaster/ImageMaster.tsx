/* begin general import */
import Add16 from "@carbon/icons-react/es/add/16";
import Close16 from "@carbon/icons-react/es/close/16";
import Filter16 from "@carbon/icons-react/es/filter/16";
import Settings16 from "@carbon/icons-react/es/settings/16";
import TrashCan16 from "@carbon/icons-react/es/trash-can/16";
import { useTranslation } from "react-i18next";
import {
  ActionBarComponent, Button,
  OverflowMenu,
  Pagination,
  StandardTable,
  Tag,
} from "react3l-ui-library";
import InputSearch from "react3l-ui-library/build/components/Input/InputSearch";
import nameof from "ts-nameof.macro";

import { Dropdown } from "antd";
import classNames from "classnames";
import LayoutMaster from "components/LayoutMaster/LayoutMaster";
import PageHeader from "components/PageHeader/PageHeader";
import { DEFAULT_PAGE_SIZE_OPTION } from "core/config/consts";

import ImageAdvanceFilter from "./ImageAdvanceFilter";
import { Image, ImageFilter } from "models/Image";
import "./ImageMaster.scss";
/* end general import */

/* begin individual import */
import { imageRepository } from "../ImageRepository";
import ImageDetailDrawer from "../ImageDetail/ImageDetailDrawer";
import { useImageMasterHook } from "./ImageMasterHook";
/* end individual import */

function ImageMaster() {
    const [translate] = useTranslation();
    const {
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

        } = useImageMasterHook();
    
    
    return (
    <>
    <div className="page-content">
        <PageHeader title={translate("images.master.subHeader")}
                    breadcrumbItems={[ translate("images.master.header"), translate("images.master.subHeader") ]} />
        <LayoutMaster>
            <LayoutMaster.Title
                title={translate("images.master.title")}
                description={translate("images.master.subHeader")}
              />
            <LayoutMaster.Actions>
                <div className="page-master__view d-flex align-items-center">
             
                </div>
                <div className="page-master__filter-action-search d-flex align-items-center">
                  <InputSearch
                    value={filter?.search}
                    classFilter={ ImageFilter }
                    placeHolder={translate("general.placeholder.search")}
                    onChange={handleChangeInputSearch}
                    position="right"
                  />
                </div>
                <div className="page-master__actions  d-flex align-items-center">
                  {countFilter > 0 && (
                    <Tag
                      value={countFilter.toString()}
                      action={handleClickClearAllFilter}
                      className="page-master__filter-tag"
                    />
                  )}

                  <Dropdown
                    dropdownRender={() => (
                      <ImageAdvanceFilter
                        filter={filter}
                        handleChangeAllFilter={handleChangeAllFilter}
                        setVisible={setVisible}
                        visible={visible}
                      />
                    )}
                    open={visible}
                    trigger={["click"]}
                    placement="bottomRight"
                    overlayClassName="page-master__filter"
                  >
                    {!visible ? (
                      <button
                        className={classNames(
                          "btn-component btn-only-icon btn--icon-only-ghost btn--xl btn-filter"
                        )}
                        onClick={() => setVisible(true)}
                      >
                        <Filter16 />
                      </button>
                    ) : (
                      <button
                        className={classNames(
                          "btn-component btn-only-icon btn--icon-only-ghost btn--xl btn-filter",
                          {
                            "btn--shadow": visible,
                          }
                        )}
                        onClick={() => setVisible(false)}
                      >
                        <Close16 />
                      </button>
                    )}
                  </Dropdown>
                  <Button
                    type="icon-only-ghost"
                    icon={<Settings16 />}
                    className="btn--xl"
                  />
                  <OverflowMenu size="xl" list={listActions}></OverflowMenu>
                  <Button
                    type="primary"
                    className="btn--lg"
                    icon={<Add16 />}
                      onClick={() => handleOpenDetailModal(null)}
                  >
                    {translate("general.actions.create")}
                  </Button>
                </div>
            </LayoutMaster.Actions>
            <LayoutMaster.Content>
                <ActionBarComponent
                  selectedRowKeys={selectedRowKeys}
                  setSelectedRowKeys={setSelectedRowKeys}
                >
                  <Button
                    icon={<TrashCan16 />}
                    type="ghost-primary"
                    className="btn--lg"
                    disabled={!canBulkAction}
                    onClick={handleBulkDeleteItem(selectedRowKeys)}
                  >
                    {translate("general.actions.delete")}
                  </Button>
                </ActionBarComponent>
                <div className="page-master__table">
                  <StandardTable
                    rowKey={nameof(list[0].id)}
                    columns={columns}
                    dataSource={list}
                    isDragable={true}
                    tableSize={"md"}
                    onChange={handleTableChange}
                    loading={loadingList}
                    rowSelection={rowSelection}
                    scroll={ { x: 1500, y: 500 } }
                  />
                  <div className="page-master__pagination">
                    <Pagination
                      skip={filter?.skip}
                      take={filter?.take}
                      total={count}
                      onChange={handlePagination}
                      pageSizeOptions={DEFAULT_PAGE_SIZE_OPTION}
                    />
                  </div>
                </div>
          </LayoutMaster.Content>
       </LayoutMaster>
    </div>
    <input
        ref={importActionRef}
        type="file"
        style={ { display: "none" } }
        id="master-import"
        onClick={handleClick}
        onChange={handleImportList(imageRepository.import)}
    />
    { visible &&
    <ImageAdvanceFilter visible={visible}
                                   filter={filter}
                                   setVisible={setVisible}
                                   handleChangeAllFilter={handleChangeAllFilter} />
    }
    <ImageDetailDrawer model={model}
                                 visible={isOpenDetailModal}
                                 handleSave={handleSaveModel}
                                 handleCancel={handleCloseDetailModal}
                                 handleChangeSingleField={handleChangeSingleField}
                                  handleChangeSelectField={handleChangeSelectField}
                                 
                                  handleChangeDateField={handleChangeDateField}
                                 dispatch={dispatchModal}
                                 loading={loadingModel}
                                 visibleFooter={true} />
    </>);
 }
 export default ImageMaster;
