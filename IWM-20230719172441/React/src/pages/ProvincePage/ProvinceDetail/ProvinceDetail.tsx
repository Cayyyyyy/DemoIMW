/* begin general import */
import React from "react";
import Close16 from "@carbon/icons-react/es/close/16";
import Send16 from "@carbon/icons-react/es/send/16";
import { Col, Row } from "antd";
import PageHeader from "components/PageHeader/PageHeader";
import { useTranslation } from "react-i18next";
import nameof from "ts-nameof.macro";
import { detailService } from "core/services/page-services/detail-service";
import { fieldService } from "core/services/page-services/field-service";
import { utilService } from "core/services/common-services/util-service";
import FormItem from "react3l-ui-library/build/components/FormItem";
import { Button, EnumSelect  } from "react3l-ui-library";
import LayoutDetail from "components/LayoutDetail/LayoutDetail";
import "./ProvinceDetail.scss";
/* end general import */

/* begin individual import */
import { Switch } from "antd";
import InputText from "react3l-ui-library/build/components/Input/InputText";
import Select from "react3l-ui-library/build/components/Select/SingleSelect/Select";
import InputNumber from "react3l-ui-library/build/components/Input/InputNumber";
import DatePicker from "react3l-ui-library/build/components/Input/DatePicker/DatePicker";
import { Province } from 'models/Province';
import { PROVINCE_ROUTE } from 'config/routes';
import { provinceRepository } from "../ProvinceRepository";
import { StatusFilter } from 'models/Status';

/* end individual import */


function ProvinceDetail() {
    const [translate] = useTranslation();

    const {
        model,
        dispatch,
    } = detailService.useModel<Province>
    (Province);

    const {
    isDetail
    } = detailService.useGetIsDetail<Province>
        (
        provinceRepository.get,
        dispatch
        );


    const {
    handleChangeDateField,
    handleChangeAllField,
    handleChangeSingleField,
    handleChangeSelectField,
    } = fieldService.useField(model, dispatch);


    const {
    loading,
    setLoading,
    handleSaveModel,
    handleGoMaster
    } = detailService.useActionsDetail<Province>
        (
        model,
        provinceRepository.save,
        handleChangeAllField,
        PROVINCE_ROUTE
        );
    return (
    <>
    <div className="page-content">
        <PageHeader title={!isDetail ? translate("provinces.detail.create") : translate("provinces.detail.update")}
                    breadcrumbItems={[ translate('provinces.master.header'), translate('provinces.master.subHeader') ]} />
        <LayoutDetail>
            <div className="page-detail__title p-b--lg">
                {!isDetail
                ? translate("general.actions.create")
                : translate("general.detail.title")}
            </div>
            <Row gutter={ { xs: 8, sm: 16, md: 24, lg: 32 } }>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.code))}
                            message={ model.errors?.code }>
                    <InputText label={translate("provinces.code")}
                                type={0}
                                value={ model.code }
                                placeHolder={translate("provinces.placeholder.code")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.code )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.name))}
                            message={ model.errors?.name }>
                    <InputText label={translate("provinces.name")}
                                type={0}
                                value={ model.name }
                                placeHolder={translate("provinces.placeholder.name")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.name )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.priority))}
                            message={ model.errors?.priority }>
                    <InputNumber label={translate("provinces.priority")}
                                    type={0}
                                    value={ model.priority }
                                    placeHolder={translate("provinces.placeholder.priority")}
                                    onChange={handleChangeSingleField({fieldName: nameof(model.priority )})}
                                    numberType={'DECIMAL'} />

                </FormItem>
                </Col>
                


                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.createdAt))}
                            message={ model.errors?.createdAt }>
                    <DatePicker label={translate("provinces.createdAt")}
                                value={ model.createdAt }
                                type={0}
                                placeholder={translate("provinces.placeholder.createdAt")}
                                onChange={handleChangeDateField({fieldName: nameof(model.createdAt)})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.updatedAt))}
                            message={ model.errors?.updatedAt }>
                    <DatePicker label={translate("provinces.updatedAt")}
                                value={ model.updatedAt }
                                type={0}
                                placeholder={translate("provinces.placeholder.updatedAt")}
                                onChange={handleChangeDateField({fieldName: nameof(model.updatedAt)})} />

                </FormItem>
                </Col>
                



                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.used))}
                            message={ model.errors?.used }>

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.status))}
                            message={ model.errors?.status }>
                        <EnumSelect
                            placeHolder={translate("provinces.placeholder.status")}
                            onChange={handleChangeSelectField({fieldName: nameof(model.status)})}
                            getList={ provinceRepository.singleListStatus }
                            type={0}
                            label={translate("provinces.status")}
                            value={ model.status }
                          />
                </FormItem>
                </Col>



            </Row>
        </LayoutDetail>
        <LayoutDetail.Footer>
            <div className="app-footer__full d-flex justify-content-end align-items-center">
                <div className="app-footer__actions d-flex justify-content-end">
                    <Button type="secondary" className="btn--lg" icon={<Close16 />} onClick={handleGoMaster}>
                    {translate("general.actions.close")}
                </Button>
                    <Button type="primary" className="btn--lg" icon={<Send16 />} onClick={handleSaveModel}>
                    {translate("general.actions.save")}
                </Button>
                </div>
            </div>
        </LayoutDetail.Footer>
    </div>

    </>
    );
  }

 export default ProvinceDetail;
