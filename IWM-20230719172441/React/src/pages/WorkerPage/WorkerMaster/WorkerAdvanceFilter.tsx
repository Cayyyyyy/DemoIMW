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
import { workerRepository } from "../WorkerRepository";
import { Worker, WorkerFilter } from "models/Worker";
import { District, DistrictFilter } from "models/District";
import { Nation, NationFilter } from "models/Nation";
import { Province, ProvinceFilter } from "models/Province";
import { Sex, SexFilter } from "models/Sex";
import { Status, StatusFilter } from "models/Status";
import { Ward, WardFilter } from "models/Ward";
import { WorkerGroup, WorkerGroupFilter } from "models/WorkerGroup";
/* end individual import */


export interface  WorkerFilterProps {
  filter?: any;
  setVisible?: any;
  handleChangeAllFilter?: (data: any) => void;
  visible?: boolean;
}

function WorkerAdvanceFilter(props: WorkerFilterProps) {
    const [translate] = useTranslation();

     const { visible, filter, setVisible, handleChangeAllFilter } = props;

    const { modelFilter, dispatchFilter } = filterService.useModelFilter(
        WorkerFilter,
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
          payload: new WorkerFilter(),
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
                            label={ translate('workers.sex')}
                            values={modelFilter?.sexId?.in}
                            onChange={handleChangeCheckboxFilter({
                              fieldName: "sex",
                              fieldType: "in",
                              classFilter: IdFilter,
                            })}
                            getList={ workerRepository.filterListSex  }
                          />
                        </div>
                      
                    


                        <div className="m-t--2xl">
                          <CheckboxWrapper.Group
                            label={ translate('workers.status')}
                            values={modelFilter?.statusId?.in}
                            onChange={handleChangeCheckboxFilter({
                              fieldName: "status",
                              fieldType: "in",
                              classFilter: IdFilter,
                            })}
                            getList={ workerRepository.filterListStatus  }
                          />
                        </div>
                      
                    







            </FilterPanel.Left>
            <FilterPanel.Right>
                <Row gutter={ { xs: 8, sm: 16, md: 24, lg: 32 } }>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('workers.code')}
                                         value={modelFilter["code"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "code" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('workers.placeholder.code')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('workers.name')}
                                         value={modelFilter["name"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "name" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('workers.placeholder.name')} />
                    </Col>
                     


                    

 


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceDateRangeFilter label={ translate('workers.birthday')}
                                                  onChange={handleChangeDateFilter({
                                                  fieldName: "birthday" ,
                                                  fieldType: ["greaterEqual", "lessEqual" ],
                                                  })}
                                                  values={[
                                                  modelFilter?.birthday?.greaterEqual,
                                                  modelFilter?.birthday?.lessEqual,
                                                  ]}
                                                  bgColor="white"
                                                  type={0}
                                                  isSmall />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('workers.phone')}
                                         value={modelFilter["phone"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "phone" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('workers.placeholder.phone')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('workers.citizenIdentificationNumber')}
                                         value={modelFilter["citizenIdentificationNumber"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "citizenIdentificationNumber" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('workers.placeholder.citizenIdentificationNumber')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('workers.email')}
                                         value={modelFilter["email"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "email" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('workers.placeholder.email')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('workers.address')}
                                         value={modelFilter["address"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "address" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('workers.placeholder.address')} />
                    </Col>
                     


                    

 


                    

 


                    

 


                    

 


                    

 


                    

 


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('workers.username')}
                                         value={modelFilter["username"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "username" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('workers.placeholder.username')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('workers.password')}
                                         value={modelFilter["password"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "password" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('workers.placeholder.password')} />
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
                                     getList={ workerRepository.filterListDistrict }
                                     placeHolder={translate('workers.placeholder.district')}
                                     label={ translate('workers.district')} />
                    </Col>
                    




                    
                    <Col lg={8} className="m-b--md">
                    <AdvanceIdMultipleFilter values={modelFilter["nationValue"]}
                                     onChange={handleChangeMultipleSelectFilter({
                                     fieldName: "nation" ,
                                     fieldType: "in" ,
                                     classFilter: IdFilter,
                                     })}
                                     bgColor="white"
                                     type={0}
                                     isSmall
                                     classFilter={ NationFilter }
                                     getList={ workerRepository.filterListNation }
                                     placeHolder={translate('workers.placeholder.nation')}
                                     label={ translate('workers.nation')} />
                    </Col>
                    




                    
                    <Col lg={8} className="m-b--md">
                    <AdvanceIdMultipleFilter values={modelFilter["provinceValue"]}
                                     onChange={handleChangeMultipleSelectFilter({
                                     fieldName: "province" ,
                                     fieldType: "in" ,
                                     classFilter: IdFilter,
                                     })}
                                     bgColor="white"
                                     type={0}
                                     isSmall
                                     classFilter={ ProvinceFilter }
                                     getList={ workerRepository.filterListProvince }
                                     placeHolder={translate('workers.placeholder.province')}
                                     label={ translate('workers.province')} />
                    </Col>
                    




                    




                    




                    
                    <Col lg={8} className="m-b--md">
                    <AdvanceIdMultipleFilter values={modelFilter["wardValue"]}
                                     onChange={handleChangeMultipleSelectFilter({
                                     fieldName: "ward" ,
                                     fieldType: "in" ,
                                     classFilter: IdFilter,
                                     })}
                                     bgColor="white"
                                     type={0}
                                     isSmall
                                     classFilter={ WardFilter }
                                     getList={ workerRepository.filterListWard }
                                     placeHolder={translate('workers.placeholder.ward')}
                                     label={ translate('workers.ward')} />
                    </Col>
                    




                    
                    <Col lg={8} className="m-b--md">
                    <AdvanceIdMultipleFilter values={modelFilter["workerGroupValue"]}
                                     onChange={handleChangeMultipleSelectFilter({
                                     fieldName: "workerGroup" ,
                                     fieldType: "in" ,
                                     classFilter: IdFilter,
                                     })}
                                     bgColor="white"
                                     type={0}
                                     isSmall
                                     classFilter={ WorkerGroupFilter }
                                     getList={ workerRepository.filterListWorkerGroup }
                                     placeHolder={translate('workers.placeholder.workerGroup')}
                                     label={ translate('workers.workerGroup')} />
                    </Col>
                    




                    

                </Row>
            </FilterPanel.Right>
        </FilterPanel>
        );

    }

export default WorkerAdvanceFilter;
