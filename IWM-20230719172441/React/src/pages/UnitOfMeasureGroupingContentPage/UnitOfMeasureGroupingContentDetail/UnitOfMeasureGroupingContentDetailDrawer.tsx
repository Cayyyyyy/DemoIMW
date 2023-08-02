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
import Select from "react3l-ui-library/build/components/Select/SingleSelect/Select";
import EnumSelect from "react3l-ui-library/build/components/Select/EnumSelect";
import InputNumber from "react3l-ui-library/build/components/Input/InputNumber";
import { UnitOfMeasureGroupingContent } from 'models/UnitOfMeasureGroupingContent';
import { unitOfMeasureGroupingContentRepository } from "../UnitOfMeasureGroupingContentRepository";
import { UnitOfMeasureFilter } from 'models/UnitOfMeasure';
import { UnitOfMeasureGroupingFilter } from 'models/UnitOfMeasureGrouping';
/* end individual import */


interface UnitOfMeasureGroupingContentDetailDrawerProps extends DrawerProps {
  model: UnitOfMeasureGroupingContent;
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
  dispatch?: React.Dispatch<ModelAction<UnitOfMeasureGroupingContent>>;
  loading?: boolean;
}

function UnitOfMeasureGroupingContentDetailDrawer(props: UnitOfMeasureGroupingContentDetailDrawerProps) {
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
                                        validateStatus={utilService.getValidateStatus(model, nameof(model.factor))}
                                        message={ model.errors?.factor }>
                                        <InputNumber    label={translate("unitOfMeasureGroupingContents.factor")}
                                                        type={0}
                                                        value={ model.factor }
                                                        placeHolder={translate("unitOfMeasureGroupingContents.placeholder.factor")}
                                                        onChange={handleChangeSingleField({fieldName: nameof(model.factor )})}
                                                        numberType={'DECIMAL'} />

                            </FormItem>
                        </Col>
                        


                        <Col lg={24} className="m-b--xs m-t--xs">
                            <FormItem
                                        validateStatus={utilService.getValidateStatus(model, nameof(model.unitOfMeasure))}
                                        message={ model.errors?.unitOfMeasure } >
                                <Select label={translate("unitOfMeasureGroupingContents.unitOfMeasure")}
                                    type={0}
                                    classFilter={ UnitOfMeasureFilter }
                                    searchProperty={"name"}
                                    placeHolder={translate("unitOfMeasureGroupingContents.placeholder.unitOfMeasure")}
                                    getList={ unitOfMeasureGroupingContentRepository.singleListUnitOfMeasure }
                                    onChange={handleChangeSelectField({fieldName: nameof(model.unitOfMeasure)})}
                                    value={ model.unitOfMeasure } />
                            </FormItem>
                        </Col>

                        <Col lg={24} className="m-b--xs m-t--xs">
                            <FormItem
                                        validateStatus={utilService.getValidateStatus(model, nameof(model.unitOfMeasureGrouping))}
                                        message={ model.errors?.unitOfMeasureGrouping } >
                                <Select label={translate("unitOfMeasureGroupingContents.unitOfMeasureGrouping")}
                                    type={0}
                                    classFilter={ UnitOfMeasureGroupingFilter }
                                    searchProperty={"name"}
                                    placeHolder={translate("unitOfMeasureGroupingContents.placeholder.unitOfMeasureGrouping")}
                                    getList={ unitOfMeasureGroupingContentRepository.singleListUnitOfMeasureGrouping }
                                    onChange={handleChangeSelectField({fieldName: nameof(model.unitOfMeasureGrouping)})}
                                    value={ model.unitOfMeasureGrouping } />
                            </FormItem>
                        </Col>
                    </Row>


                </div>
            </div>
            
        </Drawer>
    );
}

export default UnitOfMeasureGroupingContentDetailDrawer;