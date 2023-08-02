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
import "./AppUserDetail.scss";
/* end general import */

/* begin individual import */
import { Switch } from "antd";
import InputText from "react3l-ui-library/build/components/Input/InputText";
import Select from "react3l-ui-library/build/components/Select/SingleSelect/Select";
import DatePicker from "react3l-ui-library/build/components/Input/DatePicker/DatePicker";
import TreeSelect from "react3l-ui-library/build/components/TreeSelect/TreeSelect";
import { AppUser } from 'models/AppUser';
import { APP_USER_ROUTE } from 'config/routes';
import { appUserRepository } from "../AppUserRepository";
import { OrganizationFilter } from 'models/Organization';
import { SexFilter } from 'models/Sex';
import { StatusFilter } from 'models/Status';

/* end individual import */


function AppUserDetail() {
    const [translate] = useTranslation();

    const {
        model,
        dispatch,
    } = detailService.useModel<AppUser>
    (AppUser);

    const {
    isDetail
    } = detailService.useGetIsDetail<AppUser>
        (
        appUserRepository.get,
        dispatch
        );


    const {
    handleChangeDateField,
    handleChangeAllField,
    handleChangeSingleField,
    handleChangeTreeField,
    handleChangeSelectField,
    } = fieldService.useField(model, dispatch);


    const {
    loading,
    setLoading,
    handleSaveModel,
    handleGoMaster
    } = detailService.useActionsDetail<AppUser>
        (
        model,
        appUserRepository.save,
        handleChangeAllField,
        APP_USER_ROUTE
        );
    return (
    <>
    <div className="page-content">
        <PageHeader title={!isDetail ? translate("appUsers.detail.create") : translate("appUsers.detail.update")}
                    breadcrumbItems={[ translate('appUsers.master.header'), translate('appUsers.master.subHeader') ]} />
        <LayoutDetail>
            <div className="page-detail__title p-b--lg">
                {!isDetail
                ? translate("general.actions.create")
                : translate("general.detail.title")}
            </div>
            <Row gutter={ { xs: 8, sm: 16, md: 24, lg: 32 } }>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.username))}
                            message={ model.errors?.username }>
                    <InputText label={translate("appUsers.username")}
                                type={0}
                                value={ model.username }
                                placeHolder={translate("appUsers.placeholder.username")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.username )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.displayName))}
                            message={ model.errors?.displayName }>
                    <InputText label={translate("appUsers.displayName")}
                                type={0}
                                value={ model.displayName }
                                placeHolder={translate("appUsers.placeholder.displayName")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.displayName )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.address))}
                            message={ model.errors?.address }>
                    <InputText label={translate("appUsers.address")}
                                type={0}
                                value={ model.address }
                                placeHolder={translate("appUsers.placeholder.address")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.address )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.email))}
                            message={ model.errors?.email }>
                    <InputText label={translate("appUsers.email")}
                                type={0}
                                value={ model.email }
                                placeHolder={translate("appUsers.placeholder.email")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.email )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.phone))}
                            message={ model.errors?.phone }>
                    <InputText label={translate("appUsers.phone")}
                                type={0}
                                value={ model.phone }
                                placeHolder={translate("appUsers.placeholder.phone")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.phone )})} />

                </FormItem>
                </Col>
                


                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.birthday))}
                            message={ model.errors?.birthday }>
                    <DatePicker label={translate("appUsers.birthday")}
                                value={ model.birthday }
                                type={0}
                                placeholder={translate("appUsers.placeholder.birthday")}
                                onChange={handleChangeDateField({fieldName: nameof(model.birthday)})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.avatar))}
                            message={ model.errors?.avatar }>
                    <InputText label={translate("appUsers.avatar")}
                                type={0}
                                value={ model.avatar }
                                placeHolder={translate("appUsers.placeholder.avatar")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.avatar )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.department))}
                            message={ model.errors?.department }>
                    <InputText label={translate("appUsers.department")}
                                type={0}
                                value={ model.department }
                                placeHolder={translate("appUsers.placeholder.department")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.department )})} />

                </FormItem>
                </Col>
                



                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.createdAt))}
                            message={ model.errors?.createdAt }>
                    <DatePicker label={translate("appUsers.createdAt")}
                                value={ model.createdAt }
                                type={0}
                                placeholder={translate("appUsers.placeholder.createdAt")}
                                onChange={handleChangeDateField({fieldName: nameof(model.createdAt)})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.updatedAt))}
                            message={ model.errors?.updatedAt }>
                    <DatePicker label={translate("appUsers.updatedAt")}
                                value={ model.updatedAt }
                                type={0}
                                placeholder={translate("appUsers.placeholder.updatedAt")}
                                onChange={handleChangeDateField({fieldName: nameof(model.updatedAt)})} />

                </FormItem>
                </Col>
                



                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.used))}
                            message={ model.errors?.used }>

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.code))}
                            message={ model.errors?.code }>
                    <InputText label={translate("appUsers.code")}
                                type={0}
                                value={ model.code }
                                placeHolder={translate("appUsers.placeholder.code")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.code )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.name))}
                            message={ model.errors?.name }>
                    <InputText label={translate("appUsers.name")}
                                type={0}
                                value={ model.name }
                                placeHolder={translate("appUsers.placeholder.name")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.name )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.organization))}
                            message={ model.errors?.organization }>
                    <TreeSelect label={translate("appUsers.organization")}
                                type={0}
                                placeHolder={translate("appUsers.placeholder.organization")}
                                selectable={true}
                                classFilter={ OrganizationFilter }
                                onChange={handleChangeTreeField({fieldName: nameof(model.organization)})}
                                checkStrictly={true}
                                getTreeData={ appUserRepository.singleListOrganization }
                                item={ model.organization } />
                </FormItem>
                </Col>

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.sex))}
                            message={ model.errors?.sex }>
                        <EnumSelect
                            placeHolder={translate("appUsers.placeholder.sex")}
                            onChange={handleChangeSelectField({fieldName: nameof(model.sex)})}
                            getList={ appUserRepository.singleListSex }
                            type={0}
                            label={translate("appUsers.sex")}
                            value={ model.sex }
                          />
                </FormItem>
                </Col>

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.status))}
                            message={ model.errors?.status }>
                        <EnumSelect
                            placeHolder={translate("appUsers.placeholder.status")}
                            onChange={handleChangeSelectField({fieldName: nameof(model.status)})}
                            getList={ appUserRepository.singleListStatus }
                            type={0}
                            label={translate("appUsers.status")}
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

 export default AppUserDetail;
