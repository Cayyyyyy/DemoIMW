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
import AdvanceNumberFilter from "react3l-ui-library/build/components/AdvanceFilter/AdvanceNumberFilter";
import { formatNumber } from "core/helpers/number";
import AdvanceDateRangeFilter from "react3l-ui-library/build/components/AdvanceFilter/AdvanceDateRangeFilter";
import { formatDateTime } from "core/helpers/date-time";

import CheckboxWrapper from "react3l-ui-library/build/components/Checkbox";

/* end filter import */

/* begin individual import */
import { wardRepository } from "../WardRepository";
import { Ward, WardFilter } from "models/Ward";
import { District, DistrictFilter } from "models/District";
import { Status, StatusFilter } from "models/Status";
/* end individual import */


export interface  WardFilterProps {
  filter?: any;
  setVisible?: any;
  handleChangeAllFilter?: (data: any) => void;
  visible?: boolean;
}

function WardAdvanceFilter(props: WardFilterProps) {
    const [translate] = useTranslation();

     const { visible, filter, setVisible, handleChangeAllFilter } = props;

    const { modelFilter, dispatchFilter } = filterService.useModelFilter(
        WardFilter,
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
          payload: new WardFilter(),
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
                            label={ translate('wards.status')}
                            values={modelFilter?.statusId?.in}
                            onChange={handleChangeCheckboxFilter({
                              fieldName: "status",
                              fieldType: "in",
                              classFilter: IdFilter,
                            })}
                            getList={ wardRepository.filterListStatus  }
                          />
                        </div>
                      
                    



            </FilterPanel.Left>
            <FilterPanel.Right>
                <Row gutter={ { xs: 8, sm: 16, md: 24, lg: 32 } }>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('wards.code')}
                                         value={modelFilter["code"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "code" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('wards.placeholder.code')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('wards.name')}
                                         value={modelFilter["name"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "name" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('wards.placeholder.name')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceNumberFilter label={ translate('wards.priority')}
                                         value={modelFilter["priority"]["equal"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "priority" ,
                                         fieldType: "equal" ,
                                         classFilter:NumberFilter,} )}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('wards.placeholder.priority')} />
                    </Col>
                     


                    

 


                    

 


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceDateRangeFilter label={ translate('wards.createdAt')}
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
                    <AdvanceDateRangeFilter label={ translate('wards.updatedAt')}
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
                    <AdvanceIdMultipleFilter values={modelFilter["districtValue"]}
                                     onChange={handleChangeMultipleSelectFilter({
                                     fieldName: "district" ,
                                     fieldType: "in" ,
                                     classFilter: IdFilter,
                                     })}
                                     bgColor="white"
                                     type={0}
                                     isSmall
                                     classFilter={ DistrictFilter }
                                     getList={ wardRepository.filterListDistrict }
                                     placeHolder={translate('wards.placeholder.district')}
                                     label={ translate('wards.district')} />
                    </Col>
                    




                    




                    

                </Row>
            </FilterPanel.Right>
        </FilterPanel>
        );

    }

export default WardAdvanceFilter;
