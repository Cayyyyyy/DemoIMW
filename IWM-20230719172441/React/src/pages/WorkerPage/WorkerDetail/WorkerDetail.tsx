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
import "./WorkerDetail.scss";
/* end general import */

/* begin individual import */
import InputText from "react3l-ui-library/build/components/Input/InputText";
import Select from "react3l-ui-library/build/components/Select/SingleSelect/Select";
import DatePicker from "react3l-ui-library/build/components/Input/DatePicker/DatePicker";
import { Worker } from 'models/Worker';
import { WORKER_ROUTE } from 'config/routes';
import { workerRepository } from "../WorkerRepository";
import { DistrictFilter } from 'models/District';
import { NationFilter } from 'models/Nation';
import { ProvinceFilter } from 'models/Province';
import { SexFilter } from 'models/Sex';
import { StatusFilter } from 'models/Status';
import { WardFilter } from 'models/Ward';
import { WorkerGroupFilter } from 'models/WorkerGroup';

/* end individual import */


function WorkerDetail() {
    const [translate] = useTranslation();

    const {
        model,
        dispatch,
    } = detailService.useModel<Worker>
    (Worker);

    const {
    isDetail
    } = detailService.useGetIsDetail<Worker>
        (
        workerRepository.get,
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
    } = detailService.useActionsDetail<Worker>
        (
        model,
        workerRepository.save,
        handleChangeAllField,
        WORKER_ROUTE
        );
    return (
    <>
    <div className="page-content">
        <PageHeader title={!isDetail ? translate("workers.detail.create") : translate("workers.detail.update")}
                    breadcrumbItems={[ translate('workers.master.header'), translate('workers.master.subHeader') ]} />
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
                    <InputText label={translate("workers.code")}
                                type={0}
                                value={ model.code }
                                placeHolder={translate("workers.placeholder.code")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.code )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.name))}
                            message={ model.errors?.name }>
                    <InputText label={translate("workers.name")}
                                type={0}
                                value={ model.name }
                                placeHolder={translate("workers.placeholder.name")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.name )})} />

                </FormItem>
                </Col>
                


                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.birthday))}
                            message={ model.errors?.birthday }>
                    <DatePicker label={translate("workers.birthday")}
                                value={ model.birthday }
                                type={0}
                                placeholder={translate("workers.placeholder.birthday")}
                                onChange={handleChangeDateField({fieldName: nameof(model.birthday)})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.phone))}
                            message={ model.errors?.phone }>
                    <InputText label={translate("workers.phone")}
                                type={0}
                                value={ model.phone }
                                placeHolder={translate("workers.placeholder.phone")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.phone )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.citizenIdentificationNumber))}
                            message={ model.errors?.citizenIdentificationNumber }>
                    <InputText label={translate("workers.citizenIdentificationNumber")}
                                type={0}
                                value={ model.citizenIdentificationNumber }
                                placeHolder={translate("workers.placeholder.citizenIdentificationNumber")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.citizenIdentificationNumber )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.email))}
                            message={ model.errors?.email }>
                    <InputText label={translate("workers.email")}
                                type={0}
                                value={ model.email }
                                placeHolder={translate("workers.placeholder.email")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.email )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.address))}
                            message={ model.errors?.address }>
                    <InputText label={translate("workers.address")}
                                type={0}
                                value={ model.address }
                                placeHolder={translate("workers.placeholder.address")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.address )})} />

                </FormItem>
                </Col>
                







                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.username))}
                            message={ model.errors?.username }>
                    <InputText label={translate("workers.username")}
                                type={0}
                                value={ model.username }
                                placeHolder={translate("workers.placeholder.username")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.username )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.password))}
                            message={ model.errors?.password }>
                    <InputText label={translate("workers.password")}
                                type={0}
                                value={ model.password }
                                placeHolder={translate("workers.placeholder.password")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.password )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.district))}
                            message={ model.errors?.district }>
                        <Select label={translate("workers.district")}
                            type={0}
                            classFilter={ DistrictFilter }
                            searchProperty={"name"}
                            placeHolder={translate("workers.placeholder.district")}
                            getList={ workerRepository.singleListDistrict }
                            onChange={handleChangeSelectField({fieldName: nameof(model.district)})}
                            value={ model.district } />
                </FormItem>
                </Col>

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.nation))}
                            message={ model.errors?.nation }>
                        <Select label={translate("workers.nation")}
                            type={0}
                            classFilter={ NationFilter }
                            searchProperty={"name"}
                            placeHolder={translate("workers.placeholder.nation")}
                            getList={ workerRepository.singleListNation }
                            onChange={handleChangeSelectField({fieldName: nameof(model.nation)})}
                            value={ model.nation } />
                </FormItem>
                </Col>

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.province))}
                            message={ model.errors?.province }>
                        <Select label={translate("workers.province")}
                            type={0}
                            classFilter={ ProvinceFilter }
                            searchProperty={"name"}
                            placeHolder={translate("workers.placeholder.province")}
                            getList={ workerRepository.singleListProvince }
                            onChange={handleChangeSelectField({fieldName: nameof(model.province)})}
                            value={ model.province } />
                </FormItem>
                </Col>

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.sex))}
                            message={ model.errors?.sex }>
                        <EnumSelect
                            placeHolder={translate("workers.placeholder.sex")}
                            onChange={handleChangeSelectField({fieldName: nameof(model.sex)})}
                            getList={ workerRepository.singleListSex }
                            type={0}
                            label={translate("workers.sex")}
                            value={ model.sex }
                          />
                </FormItem>
                </Col>

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.status))}
                            message={ model.errors?.status }>
                        <EnumSelect
                            placeHolder={translate("workers.placeholder.status")}
                            onChange={handleChangeSelectField({fieldName: nameof(model.status)})}
                            getList={ workerRepository.singleListStatus }
                            type={0}
                            label={translate("workers.status")}
                            value={ model.status }
                          />
                </FormItem>
                </Col>

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.ward))}
                            message={ model.errors?.ward }>
                        <Select label={translate("workers.ward")}
                            type={0}
                            classFilter={ WardFilter }
                            searchProperty={"name"}
                            placeHolder={translate("workers.placeholder.ward")}
                            getList={ workerRepository.singleListWard }
                            onChange={handleChangeSelectField({fieldName: nameof(model.ward)})}
                            value={ model.ward } />
                </FormItem>
                </Col>

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.workerGroup))}
                            message={ model.errors?.workerGroup }>
                        <Select label={translate("workers.workerGroup")}
                            type={0}
                            classFilter={ WorkerGroupFilter }
                            searchProperty={"name"}
                            placeHolder={translate("workers.placeholder.workerGroup")}
                            getList={ workerRepository.singleListWorkerGroup }
                            onChange={handleChangeSelectField({fieldName: nameof(model.workerGroup)})}
                            value={ model.workerGroup } />
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

 export default WorkerDetail;
