/* begin general import */
import React from "react";
import nameof from "ts-nameof.macro";
import { Card, Col, Row } from "antd";
import { useTranslation } from "react-i18next";
import Drawer, { DrawerProps } from "react3l-ui-library/build/components/Drawer/Drawer";
import FormItem from "react3l-ui-library/build/components/FormItem";
import { ModelAction } from "core/services/page-services/detail-service";
import { utilService } from "core/services/common-services/util-service";
import { Model } from "react3l-common";
import { Moment } from "moment";

/* end general import */

/* begin individual import */
import InputText from "react3l-ui-library/build/components/Input/InputText";
import Select from "react3l-ui-library/build/components/Select/SingleSelect/Select";
import EnumSelect from "react3l-ui-library/build/components/Select/EnumSelect";
import { WorkerGroup } from 'models/WorkerGroup';
import { workerGroupRepository } from "../WorkerGroupRepository";
import { StatusFilter } from 'models/Status';
/* end individual import */


interface WorkerGroupDetailDrawerProps extends DrawerProps {
  model: WorkerGroup;
  handleChangeSingleField?: (config: {
    fieldName: string;
    }) => (value: any) => void
  handleChangeSelectField?: (config: {
    fieldName: string;
    }) => (idValue: number, value: Model) => void
  handleChangeTreeField?: (config: {
    fieldName: string;
   }) => (values: any[], isMultiple: boolean) => void
  handleChangeDateField?: (config: {
        fieldName: string | [string, string];
  }) => (date: Moment | [Moment, Moment]) => void;
  dispatch?: React.Dispatch<ModelAction<WorkerGroup>>;
  loading?: boolean;
}

function WorkerGroupDetailDrawer(props: WorkerGroupDetailDrawerProps) {
    const [translate] = useTranslation();

    const {
        model,
        handleChangeSingleField,
        handleChangeSelectField,
        loading,
        visible,
        handleSave,
        handleCancel,
    } = props;

    return (
        <Drawer
            {...props}
            visible={visible}
            handleSave={handleSave}
            handleCancel={handleCancel}
            handleClose={handleCancel}
            visibleFooter={true}
            loading={loading}
            title={
                model?.id
                    ? translate("general.detail.title")
                    : translate("general.actions.create")
            }
            titleButtonCancel={translate("general.actions.close")}
            titleButtonApply={translate("general.actions.save")}>

            <div className='page page__detail'>

                <div className='w-100 page__detail-tabs'>
                    <Row className='d-flex' gutter={ { xs: 8, sm: 16, md: 24, lg: 32 } }>

                        

                        <Col lg={24} className="m-b--xs m-t--xs">
                            <FormItem
                                        validateStatus={utilService.getValidateStatus(model, nameof(model.code))}
                                        message={ model.errors?.code }>
                                        <InputText  label={translate("workerGroups.code")}
                                                    type={0}
                                                    value={ model.code }
                                                    placeHolder={translate("workerGroups.placeholder.code")}
                                                    className={"tio-account_square_outlined"}
                                                    onChange={handleChangeSingleField({fieldName: nameof(model.code )})} />

                            </FormItem>
                        </Col>
                        

                        <Col lg={24} className="m-b--xs m-t--xs">
                            <FormItem
                                        validateStatus={utilService.getValidateStatus(model, nameof(model.name))}
                                        message={ model.errors?.name }>
                                        <InputText  label={translate("workerGroups.name")}
                                                    type={0}
                                                    value={ model.name }
                                                    placeHolder={translate("workerGroups.placeholder.name")}
                                                    className={"tio-account_square_outlined"}
                                                    onChange={handleChangeSingleField({fieldName: nameof(model.name )})} />

                            </FormItem>
                        </Col>
                        


                        <Col lg={24} className="m-b--xs m-t--xs">
                            <FormItem
                                        validateStatus={utilService.getValidateStatus(model, nameof(model.status))}
                                        message={ model.errors?.status } >
                                <EnumSelect
                                    placeHolder={translate("workerGroups.placeholder.status")}
                                    onChange={handleChangeSelectField({fieldName: nameof(model.status)})}
                                    getList={ workerGroupRepository.singleListStatus }
                                    type={0}
                                    label={translate("workerGroups.status")}
                                    />
                            </FormItem>
                        </Col>

                    </Row>


                </div>
            </div>
            
        </Drawer>
    );
}

export default WorkerGroupDetailDrawer;