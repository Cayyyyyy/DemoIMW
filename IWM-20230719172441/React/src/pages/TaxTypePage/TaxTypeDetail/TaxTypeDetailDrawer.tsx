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
import { TaxType } from 'models/TaxType';
import { taxTypeRepository } from "../TaxTypeRepository";
import { StatusFilter } from 'models/Status';
/* end individual import */


interface TaxTypeDetailDrawerProps extends DrawerProps {
  model: TaxType;
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
  dispatch?: React.Dispatch<ModelAction<TaxType>>;
  loading?: boolean;
}

function TaxTypeDetailDrawer(props: TaxTypeDetailDrawerProps) {
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
                                        <InputText  label={translate("taxTypes.code")}
                                                    type={0}
                                                    value={ model.code }
                                                    placeHolder={translate("taxTypes.placeholder.code")}
                                                    className={"tio-account_square_outlined"}
                                                    onChange={handleChangeSingleField({fieldName: nameof(model.code )})} />

                            </FormItem>
                        </Col>
                        

                        <Col lg={24} className="m-b--xs m-t--xs">
                            <FormItem
                                        validateStatus={utilService.getValidateStatus(model, nameof(model.name))}
                                        message={ model.errors?.name }>
                                        <InputText  label={translate("taxTypes.name")}
                                                    type={0}
                                                    value={ model.name }
                                                    placeHolder={translate("taxTypes.placeholder.name")}
                                                    className={"tio-account_square_outlined"}
                                                    onChange={handleChangeSingleField({fieldName: nameof(model.name )})} />

                            </FormItem>
                        </Col>
                        

                        <Col lg={24} className="m-b--xs m-t--xs">
                            <FormItem
                                        validateStatus={utilService.getValidateStatus(model, nameof(model.percentage))}
                                        message={ model.errors?.percentage }>
                                        <InputNumber    label={translate("taxTypes.percentage")}
                                                        type={0}
                                                        value={ model.percentage }
                                                        placeHolder={translate("taxTypes.placeholder.percentage")}
                                                        onChange={handleChangeSingleField({fieldName: nameof(model.percentage )})}
                                                        numberType={'DECIMAL'} />

                            </FormItem>
                        </Col>
                        


                        <Col lg={24} className="m-b--xs m-t--xs">
                            <FormItem
                                        validateStatus={utilService.getValidateStatus(model, nameof(model.createdAt))}
                                        message={ model.errors?.createdAt }>
                                        <DatePicker
                                                    label={translate("taxTypes.createdAt")}
                                                    value={ model.createdAt }
                                                    type={0}
                                                    placeholder={translate("taxTypes.placeholder.createdAt")}
                                                    onChange={handleChangeDateField({fieldName: nameof(model.createdAt)})} />

                            </FormItem>
                        </Col>
                        

                        <Col lg={24} className="m-b--xs m-t--xs">
                            <FormItem
                                        validateStatus={utilService.getValidateStatus(model, nameof(model.updatedAt))}
                                        message={ model.errors?.updatedAt }>
                                        <DatePicker
                                                    label={translate("taxTypes.updatedAt")}
                                                    value={ model.updatedAt }
                                                    type={0}
                                                    placeholder={translate("taxTypes.placeholder.updatedAt")}
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
                                        validateStatus={utilService.getValidateStatus(model, nameof(model.status))}
                                        message={ model.errors?.status } >
                                <EnumSelect
                                    placeHolder={translate("taxTypes.placeholder.status")}
                                    onChange={handleChangeSelectField({fieldName: nameof(model.status)})}
                                    getList={ taxTypeRepository.singleListStatus }
                                    type={0}
                                    label={translate("taxTypes.status")}
                                    />
                            </FormItem>
                        </Col>

                    </Row>


                </div>
            </div>
            
        </Drawer>
    );
}

export default TaxTypeDetailDrawer;