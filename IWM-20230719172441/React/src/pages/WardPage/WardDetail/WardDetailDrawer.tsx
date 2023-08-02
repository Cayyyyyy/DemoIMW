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
import { Switch } from "antd";
import InputText from "react3l-ui-library/build/components/Input/InputText";
import Select from "react3l-ui-library/build/components/Select/SingleSelect/Select";
import EnumSelect from "react3l-ui-library/build/components/Select/EnumSelect";
import InputNumber from "react3l-ui-library/build/components/Input/InputNumber";
import DatePicker from "react3l-ui-library/build/components/Input/DatePicker";
import { Ward } from 'models/Ward';
import { wardRepository } from "../WardRepository";
import { DistrictFilter } from 'models/District';
import { StatusFilter } from 'models/Status';
/* end individual import */


interface WardDetailDrawerProps extends DrawerProps {
  model: Ward;
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
  dispatch?: React.Dispatch<ModelAction<Ward>>;
  loading?: boolean;
}

function WardDetailDrawer(props: WardDetailDrawerProps) {
    const [translate] = useTranslation();

    const {
        model,
        handleChangeSingleField,
        handleChangeSelectField,
        handleChangeDateField,
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
                                        <InputText  label={translate("wards.code")}
                                                    type={0}
                                                    value={ model.code }
                                                    placeHolder={translate("wards.placeholder.code")}
                                                    className={"tio-account_square_outlined"}
                                                    onChange={handleChangeSingleField({fieldName: nameof(model.code )})} />

                            </FormItem>
                        </Col>
                        

                        <Col lg={24} className="m-b--xs m-t--xs">
                            <FormItem
                                        validateStatus={utilService.getValidateStatus(model, nameof(model.name))}
                                        message={ model.errors?.name }>
                                        <InputText  label={translate("wards.name")}
                                                    type={0}
                                                    value={ model.name }
                                                    placeHolder={translate("wards.placeholder.name")}
                                                    className={"tio-account_square_outlined"}
                                                    onChange={handleChangeSingleField({fieldName: nameof(model.name )})} />

                            </FormItem>
                        </Col>
                        

                        <Col lg={24} className="m-b--xs m-t--xs">
                            <FormItem
                                        validateStatus={utilService.getValidateStatus(model, nameof(model.priority))}
                                        message={ model.errors?.priority }>
                                        <InputNumber    label={translate("wards.priority")}
                                                        type={0}
                                                        value={ model.priority }
                                                        placeHolder={translate("wards.placeholder.priority")}
                                                        onChange={handleChangeSingleField({fieldName: nameof(model.priority )})}
                                                        numberType={'DECIMAL'} />

                            </FormItem>
                        </Col>
                        



                        <Col lg={24} className="m-b--xs m-t--xs">
                            <FormItem
                                        validateStatus={utilService.getValidateStatus(model, nameof(model.createdAt))}
                                        message={ model.errors?.createdAt }>
                                        <DatePicker
                                                    label={translate("wards.createdAt")}
                                                    value={ model.createdAt }
                                                    type={0}
                                                    placeholder={translate("wards.placeholder.createdAt")}
                                                    onChange={handleChangeDateField({fieldName: nameof(model.createdAt)})} />

                            </FormItem>
                        </Col>
                        

                        <Col lg={24} className="m-b--xs m-t--xs">
                            <FormItem
                                        validateStatus={utilService.getValidateStatus(model, nameof(model.updatedAt))}
                                        message={ model.errors?.updatedAt }>
                                        <DatePicker
                                                    label={translate("wards.updatedAt")}
                                                    value={ model.updatedAt }
                                                    type={0}
                                                    placeholder={translate("wards.placeholder.updatedAt")}
                                                    onChange={handleChangeDateField({fieldName: nameof(model.updatedAt)})} />

                            </FormItem>
                        </Col>
                        



                        <Col lg={24} className="m-b--xs m-t--xs">
                            <FormItem
                                        validateStatus={utilService.getValidateStatus(model, nameof(model.used))}
                                        message={ model.errors?.used }>
                                        <Switch size='small'
                                                onChange={handleChangeSingleField({fieldName: nameof(model.used)})}
                                                checked={ model.used } />

                            </FormItem>
                        </Col>
                        

                        <Col lg={24} className="m-b--xs m-t--xs">
                            <FormItem
                                        validateStatus={utilService.getValidateStatus(model, nameof(model.district))}
                                        message={ model.errors?.district } >
                                <Select label={translate("wards.district")}
                                    type={0}
                                    classFilter={ DistrictFilter }
                                    searchProperty={"name"}
                                    placeHolder={translate("wards.placeholder.district")}
                                    getList={ wardRepository.singleListDistrict }
                                    onChange={handleChangeSelectField({fieldName: nameof(model.district)})}
                                    value={ model.district } />
                            </FormItem>
                        </Col>

                        <Col lg={24} className="m-b--xs m-t--xs">
                            <FormItem
                                        validateStatus={utilService.getValidateStatus(model, nameof(model.status))}
                                        message={ model.errors?.status } >
                                <EnumSelect
                                    placeHolder={translate("wards.placeholder.status")}
                                    onChange={handleChangeSelectField({fieldName: nameof(model.status)})}
                                    getList={ wardRepository.singleListStatus }
                                    type={0}
                                    label={translate("wards.status")}
                                    />
                            </FormItem>
                        </Col>

                    </Row>


                </div>
            </div>
            
        </Drawer>
    );
}

export default WardDetailDrawer;