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
import "./ProductDetail.scss";
/* end general import */

/* begin individual import */
import { Switch } from "antd";
import InputText from "react3l-ui-library/build/components/Input/InputText";
import Select from "react3l-ui-library/build/components/Select/SingleSelect/Select";
import InputNumber from "react3l-ui-library/build/components/Input/InputNumber";
import DatePicker from "react3l-ui-library/build/components/Input/DatePicker/DatePicker";
import TreeSelect from "react3l-ui-library/build/components/TreeSelect/TreeSelect";
import { Product } from 'models/Product';
import { PRODUCT_ROUTE } from 'config/routes';
import { productRepository } from "../ProductRepository";
import { BrandFilter } from 'models/Brand';
import { CategoryFilter } from 'models/Category';
import { ProductTypeFilter } from 'models/ProductType';
import { StatusFilter } from 'models/Status';
import { TaxTypeFilter } from 'models/TaxType';
import { UnitOfMeasureFilter } from 'models/UnitOfMeasure';
import { UnitOfMeasureGroupingFilter } from 'models/UnitOfMeasureGrouping';

/* end individual import */


function ProductDetail() {
    const [translate] = useTranslation();

    const {
        model,
        dispatch,
    } = detailService.useModel<Product>
    (Product);

    const {
    isDetail
    } = detailService.useGetIsDetail<Product>
        (
        productRepository.get,
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
    } = detailService.useActionsDetail<Product>
        (
        model,
        productRepository.save,
        handleChangeAllField,
        PRODUCT_ROUTE
        );
    return (
    <>
    <div className="page-content">
        <PageHeader title={!isDetail ? translate("products.detail.create") : translate("products.detail.update")}
                    breadcrumbItems={[ translate('products.master.header'), translate('products.master.subHeader') ]} />
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
                    <InputText label={translate("products.code")}
                                type={0}
                                value={ model.code }
                                placeHolder={translate("products.placeholder.code")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.code )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.supplierCode))}
                            message={ model.errors?.supplierCode }>
                    <InputText label={translate("products.supplierCode")}
                                type={0}
                                value={ model.supplierCode }
                                placeHolder={translate("products.placeholder.supplierCode")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.supplierCode )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.name))}
                            message={ model.errors?.name }>
                    <InputText label={translate("products.name")}
                                type={0}
                                value={ model.name }
                                placeHolder={translate("products.placeholder.name")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.name )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.description))}
                            message={ model.errors?.description }>
                    <InputText label={translate("products.description")}
                                type={0}
                                value={ model.description }
                                placeHolder={translate("products.placeholder.description")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.description )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.scanCode))}
                            message={ model.errors?.scanCode }>
                    <InputText label={translate("products.scanCode")}
                                type={0}
                                value={ model.scanCode }
                                placeHolder={translate("products.placeholder.scanCode")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.scanCode )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.eRPCode))}
                            message={ model.errors?.eRPCode }>
                    <InputText label={translate("products.eRPCode")}
                                type={0}
                                value={ model.eRPCode }
                                placeHolder={translate("products.placeholder.eRPCode")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.eRPCode )})} />

                </FormItem>
                </Col>
                







                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.salePrice))}
                            message={ model.errors?.salePrice }>
                    <InputNumber label={translate("products.salePrice")}
                                    type={0}
                                    value={ model.salePrice }
                                    placeHolder={translate("products.placeholder.salePrice")}
                                    onChange={handleChangeSingleField({fieldName: nameof(model.salePrice )})}
                                    numberType={'DECIMAL'} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.retailPrice))}
                            message={ model.errors?.retailPrice }>
                    <InputNumber label={translate("products.retailPrice")}
                                    type={0}
                                    value={ model.retailPrice }
                                    placeHolder={translate("products.placeholder.retailPrice")}
                                    onChange={handleChangeSingleField({fieldName: nameof(model.retailPrice )})}
                                    numberType={'DECIMAL'} />

                </FormItem>
                </Col>
                



                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.otherName))}
                            message={ model.errors?.otherName }>
                    <InputText label={translate("products.otherName")}
                                type={0}
                                value={ model.otherName }
                                placeHolder={translate("products.placeholder.otherName")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.otherName )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.technicalName))}
                            message={ model.errors?.technicalName }>
                    <InputText label={translate("products.technicalName")}
                                type={0}
                                value={ model.technicalName }
                                placeHolder={translate("products.placeholder.technicalName")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.technicalName )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.note))}
                            message={ model.errors?.note }>
                    <InputText label={translate("products.note")}
                                type={0}
                                value={ model.note }
                                placeHolder={translate("products.placeholder.note")}
                                className={"tio-account_square_outlined"}
                                onChange={handleChangeSingleField({fieldName: nameof(model.note )})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.isPurchasable))}
                            message={ model.errors?.isPurchasable }>

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.isSellable))}
                            message={ model.errors?.isSellable }>

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.isNew))}
                            message={ model.errors?.isNew }>

                </FormItem>
                </Col>
                


                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.createdAt))}
                            message={ model.errors?.createdAt }>
                    <DatePicker label={translate("products.createdAt")}
                                value={ model.createdAt }
                                type={0}
                                placeholder={translate("products.placeholder.createdAt")}
                                onChange={handleChangeDateField({fieldName: nameof(model.createdAt)})} />

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.updatedAt))}
                            message={ model.errors?.updatedAt }>
                    <DatePicker label={translate("products.updatedAt")}
                                value={ model.updatedAt }
                                type={0}
                                placeholder={translate("products.placeholder.updatedAt")}
                                onChange={handleChangeDateField({fieldName: nameof(model.updatedAt)})} />

                </FormItem>
                </Col>
                


                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.used))}
                            message={ model.errors?.used }>

                </FormItem>
                </Col>
                


                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.isCopyRightedProduct))}
                            message={ model.errors?.isCopyRightedProduct }>

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.isBrandProduct))}
                            message={ model.errors?.isBrandProduct }>

                </FormItem>
                </Col>
                

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.brand))}
                            message={ model.errors?.brand }>
                        <Select label={translate("products.brand")}
                            type={0}
                            classFilter={ BrandFilter }
                            searchProperty={"name"}
                            placeHolder={translate("products.placeholder.brand")}
                            getList={ productRepository.singleListBrand }
                            onChange={handleChangeSelectField({fieldName: nameof(model.brand)})}
                            value={ model.brand } />
                </FormItem>
                </Col>

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.category))}
                            message={ model.errors?.category }>
                    <TreeSelect label={translate("products.category")}
                                type={0}
                                placeHolder={translate("products.placeholder.category")}
                                selectable={true}
                                classFilter={ CategoryFilter }
                                onChange={handleChangeTreeField({fieldName: nameof(model.category)})}
                                checkStrictly={true}
                                getTreeData={ productRepository.singleListCategory }
                                item={ model.category } />
                </FormItem>
                </Col>

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.productType))}
                            message={ model.errors?.productType }>
                        <Select label={translate("products.productType")}
                            type={0}
                            classFilter={ ProductTypeFilter }
                            searchProperty={"name"}
                            placeHolder={translate("products.placeholder.productType")}
                            getList={ productRepository.singleListProductType }
                            onChange={handleChangeSelectField({fieldName: nameof(model.productType)})}
                            value={ model.productType } />
                </FormItem>
                </Col>

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.status))}
                            message={ model.errors?.status }>
                        <EnumSelect
                            placeHolder={translate("products.placeholder.status")}
                            onChange={handleChangeSelectField({fieldName: nameof(model.status)})}
                            getList={ productRepository.singleListStatus }
                            type={0}
                            label={translate("products.status")}
                            value={ model.status }
                          />
                </FormItem>
                </Col>

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.taxType))}
                            message={ model.errors?.taxType }>
                        <Select label={translate("products.taxType")}
                            type={0}
                            classFilter={ TaxTypeFilter }
                            searchProperty={"name"}
                            placeHolder={translate("products.placeholder.taxType")}
                            getList={ productRepository.singleListTaxType }
                            onChange={handleChangeSelectField({fieldName: nameof(model.taxType)})}
                            value={ model.taxType } />
                </FormItem>
                </Col>

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.unitOfMeasure))}
                            message={ model.errors?.unitOfMeasure }>
                        <Select label={translate("products.unitOfMeasure")}
                            type={0}
                            classFilter={ UnitOfMeasureFilter }
                            searchProperty={"name"}
                            placeHolder={translate("products.placeholder.unitOfMeasure")}
                            getList={ productRepository.singleListUnitOfMeasure }
                            onChange={handleChangeSelectField({fieldName: nameof(model.unitOfMeasure)})}
                            value={ model.unitOfMeasure } />
                </FormItem>
                </Col>

                <Col lg={6} className="m-b--xs m-t--xs">
                <FormItem validateStatus={utilService.getValidateStatus(model, nameof(model.unitOfMeasureGrouping))}
                            message={ model.errors?.unitOfMeasureGrouping }>
                        <Select label={translate("products.unitOfMeasureGrouping")}
                            type={0}
                            classFilter={ UnitOfMeasureGroupingFilter }
                            searchProperty={"name"}
                            placeHolder={translate("products.placeholder.unitOfMeasureGrouping")}
                            getList={ productRepository.singleListUnitOfMeasureGrouping }
                            onChange={handleChangeSelectField({fieldName: nameof(model.unitOfMeasureGrouping)})}
                            value={ model.unitOfMeasureGrouping } />
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

 export default ProductDetail;
