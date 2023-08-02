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
import AdvanceIdMultipleFilter from "react3l-ui-library/build/components/AdvanceFilter/AdvanceIdMultipleFilter";
import AdvanceNumberFilter from "react3l-ui-library/build/components/AdvanceFilter/AdvanceNumberFilter";
import { formatNumber } from "core/helpers/number";

import CheckboxWrapper from "react3l-ui-library/build/components/Checkbox";

/* end filter import */

/* begin individual import */
import { unitOfMeasureGroupingContentRepository } from "../UnitOfMeasureGroupingContentRepository";
import { UnitOfMeasureGroupingContent, UnitOfMeasureGroupingContentFilter } from "models/UnitOfMeasureGroupingContent";
import { UnitOfMeasure, UnitOfMeasureFilter } from "models/UnitOfMeasure";
import { UnitOfMeasureGrouping, UnitOfMeasureGroupingFilter } from "models/UnitOfMeasureGrouping";
/* end individual import */


export interface  UnitOfMeasureGroupingContentFilterProps {
  filter?: any;
  setVisible?: any;
  handleChangeAllFilter?: (data: any) => void;
  visible?: boolean;
}

function UnitOfMeasureGroupingContentAdvanceFilter(props: UnitOfMeasureGroupingContentFilterProps) {
    const [translate] = useTranslation();

     const { visible, filter, setVisible, handleChangeAllFilter } = props;

    const { modelFilter, dispatchFilter } = filterService.useModelFilter(
        UnitOfMeasureGroupingContentFilter,
        filter
      );

    const {
        handleChangeInputFilter,
        handleChangeMultipleSelectFilter,
        handleChangeCheckboxFilter,
    } = filterService.useFilter(modelFilter, dispatchFilter);


      const handleSaveModelFilter = React.useCallback(() => {
        handleChangeAllFilter(modelFilter);
        setVisible(false);
      }, [handleChangeAllFilter, modelFilter, setVisible]);

      const handleClearModelFilter = React.useCallback(() => {
        dispatchFilter({
          type: 0,
          payload: new UnitOfMeasureGroupingContentFilter(),
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
                













            </FilterPanel.Left>
            <FilterPanel.Right>
                <Row gutter={ { xs: 8, sm: 16, md: 24, lg: 32 } }>
                     


                    

 


                    

 


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceNumberFilter label={ translate('unitOfMeasureGroupingContents.factor')}
                                         value={modelFilter["factor"]["equal"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "factor" ,
                                         fieldType: "equal" ,
                                         classFilter:NumberFilter,} )}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('unitOfMeasureGroupingContents.placeholder.factor')} />
                    </Col>
                     


                    

 


                    




                    
                    <Col lg={8} className="m-b--md">
                    <AdvanceIdMultipleFilter values={modelFilter["unitOfMeasureValue"]}
                                     onChange={handleChangeMultipleSelectFilter({
                                     fieldName: "unitOfMeasure" ,
                                     fieldType: "in" ,
                                     classFilter: IdFilter,
                                     })}
                                     bgColor="white"
                                     type={0}
                                     isSmall
                                     classFilter={ UnitOfMeasureFilter }
                                     getList={ unitOfMeasureGroupingContentRepository.filterListUnitOfMeasure }
                                     placeHolder={translate('unitOfMeasureGroupingContents.placeholder.unitOfMeasure')}
                                     label={ translate('unitOfMeasureGroupingContents.unitOfMeasure')} />
                    </Col>
                    




                    
                    <Col lg={8} className="m-b--md">
                    <AdvanceIdMultipleFilter values={modelFilter["unitOfMeasureGroupingValue"]}
                                     onChange={handleChangeMultipleSelectFilter({
                                     fieldName: "unitOfMeasureGrouping" ,
                                     fieldType: "in" ,
                                     classFilter: IdFilter,
                                     })}
                                     bgColor="white"
                                     type={0}
                                     isSmall
                                     classFilter={ UnitOfMeasureGroupingFilter }
                                     getList={ unitOfMeasureGroupingContentRepository.filterListUnitOfMeasureGrouping }
                                     placeHolder={translate('unitOfMeasureGroupingContents.placeholder.unitOfMeasureGrouping')}
                                     label={ translate('unitOfMeasureGroupingContents.unitOfMeasureGrouping')} />
                    </Col>
                    

                </Row>
            </FilterPanel.Right>
        </FilterPanel>
        );

    }

export default UnitOfMeasureGroupingContentAdvanceFilter;
