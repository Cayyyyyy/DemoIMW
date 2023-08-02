/* begin general import */
import React from "react";
import { useTranslation } from "react-i18next";
import { IdFilter, NumberFilter, StringFilter } from "react3l-advanced-filters";
import { filterService } from "core/services/page-services/filter-service";
import Search16 from "@carbon/icons-react/es/search/16";
import { Col, Row } from "antd";
import FilterPanel from "components/FilterPanel/FilterPanel";
/* end general import */

/* begin filter import */
import AdvanceStringFilter from "react3l-ui-library/build/components/AdvanceFilter/AdvanceStringFilter";
import AdvanceIdMultipleFilter from "react3l-ui-library/build/components/AdvanceFilter/AdvanceIdMultipleFilter";
import AdvanceDateRangeFilter from "react3l-ui-library/build/components/AdvanceFilter/AdvanceDateRangeFilter";
import { formatDateTime } from "core/helpers/date-time";

import CheckboxWrapper from "react3l-ui-library/build/components/Checkbox";

/* end filter import */

/* begin individual import */
import { unitOfMeasureRepository } from "../UnitOfMeasureRepository";
import { UnitOfMeasure, UnitOfMeasureFilter } from "models/UnitOfMeasure";
import { Status, StatusFilter } from "models/Status";
/* end individual import */


export interface  UnitOfMeasureFilterProps {
  filter?: any;
  setVisible?: any;
  handleChangeAllFilter?: (data: any) => void;
  visible?: boolean;
}

function UnitOfMeasureAdvanceFilter(props: UnitOfMeasureFilterProps) {
    const [translate] = useTranslation();

     const { visible, filter, setVisible, handleChangeAllFilter } = props;

    const { modelFilter, dispatchFilter } = filterService.useModelFilter(
        UnitOfMeasureFilter,
        filter
      );

    const {
        handleChangeInputFilter,
        handleChangeMultipleSelectFilter,
        handleChangeCheckboxFilter,
        handleChangeDateFilter,
    } = filterService.useFilter(modelFilter, dispatchFilter);


      const handleSaveModelFilter = React.useCallback(() => {
        handleChangeAllFilter(modelFilter);
        setVisible(false);
      }, [handleChangeAllFilter, modelFilter, setVisible]);

      const handleClearModelFilter = React.useCallback(() => {
        dispatchFilter({
          type: 0,
          payload: new UnitOfMeasureFilter(),
        });
      }, [dispatchFilter]);



    return (
        <FilterPanel
              handleAppplyFilter={handleSaveModelFilter}
              handleResetFilter={handleClearModelFilter}
              titleButtonCancel={translate("general.actions.resetFilter")}
              titleButtonApply={translate("general.actions.applyFilter")}
              setVisible={setVisible}
              visible={visible}
            >
            <FilterPanel.Left>
                
























                        <div className="m-t--2xl">
                          <CheckboxWrapper.Group
                            label={ translate('unitOfMeasures.status')}
                            values={modelFilter?.statusId?.in}
                            onChange={handleChangeCheckboxFilter({
                              fieldName: "status",
                              fieldType: "in",
                              classFilter: IdFilter,
                            })}
                            getList={ unitOfMeasureRepository.filterListStatus  }
                          />
                        </div>
                      
                    







            </FilterPanel.Left>
            <FilterPanel.Right>
                <Row gutter={ { xs: 8, sm: 16, md: 24, lg: 32 } }>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('unitOfMeasures.code')}
                                         value={modelFilter["code"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "code" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('unitOfMeasures.placeholder.code')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('unitOfMeasures.name')}
                                         value={modelFilter["name"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "name" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('unitOfMeasures.placeholder.name')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('unitOfMeasures.description')}
                                         value={modelFilter["description"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "description" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('unitOfMeasures.placeholder.description')} />
                    </Col>
                     


                    

 


                    




                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceDateRangeFilter label={ translate('unitOfMeasures.createdAt')}
                                                  onChange={handleChangeDateFilter({
                                                  fieldName: "createdAt" ,
                                                  fieldType: ["greaterEqual", "lessEqual" ],
                                                  })}
                                                  values={[
                                                  modelFilter?.createdAt?.greaterEqual,
                                                  modelFilter?.createdAt?.lessEqual,
                                                  ]}
                                                  bgColor="white"
                                                  type={0}
                                                  isSmall />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceDateRangeFilter label={ translate('unitOfMeasures.updatedAt')}
                                                  onChange={handleChangeDateFilter({
                                                  fieldName: "updatedAt" ,
                                                  fieldType: ["greaterEqual", "lessEqual" ],
                                                  })}
                                                  values={[
                                                  modelFilter?.updatedAt?.greaterEqual,
                                                  modelFilter?.updatedAt?.lessEqual,
                                                  ]}
                                                  bgColor="white"
                                                  type={0}
                                                  isSmall />
                    </Col>
                     


                    




                    




                    

 


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('unitOfMeasures.erpCode')}
                                         value={modelFilter["erpCode"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "erpCode" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('unitOfMeasures.placeholder.erpCode')} />
                    </Col>
                     


                    




                    




                    




                    




                    

                </Row>
            </FilterPanel.Right>
        </FilterPanel>
        );

    }

export default UnitOfMeasureAdvanceFilter;
