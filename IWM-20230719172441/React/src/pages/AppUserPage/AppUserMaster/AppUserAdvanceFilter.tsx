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
import { appUserRepository } from "../AppUserRepository";
import { AppUser, AppUserFilter } from "models/AppUser";
import { Organization, OrganizationFilter } from "models/Organization";
import { Sex, SexFilter } from "models/Sex";
import { Status, StatusFilter } from "models/Status";
/* end individual import */


export interface  AppUserFilterProps {
  filter?: any;
  setVisible?: any;
  handleChangeAllFilter?: (data: any) => void;
  visible?: boolean;
}

function AppUserAdvanceFilter(props: AppUserFilterProps) {
    const [translate] = useTranslation();

     const { visible, filter, setVisible, handleChangeAllFilter } = props;

    const { modelFilter, dispatchFilter } = filterService.useModelFilter(
        AppUserFilter,
        filter
      );

    const {
        handleChangeInputFilter,
        handleChangeMultipleSelectFilter,
        handleChangeCheckboxFilter,
        handleChangeDateFilter,
        handleChangeSingleTreeFilter,
    } = filterService.useFilter(modelFilter, dispatchFilter);


      const handleSaveModelFilter = React.useCallback(() => {
        handleChangeAllFilter(modelFilter);
        setVisible(false);
      }, [handleChangeAllFilter, modelFilter, setVisible]);

      const handleClearModelFilter = React.useCallback(() => {
        dispatchFilter({
          type: 0,
          payload: new AppUserFilter(),
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
                            label={ translate('appUsers.sex')}
                            values={modelFilter?.sexId?.in}
                            onChange={handleChangeCheckboxFilter({
                              fieldName: "sex",
                              fieldType: "in",
                              classFilter: IdFilter,
                            })}
                            getList={ appUserRepository.filterListSex  }
                          />
                        </div>
                      
                    


                        <div className="m-t--2xl">
                          <CheckboxWrapper.Group
                            label={ translate('appUsers.status')}
                            values={modelFilter?.statusId?.in}
                            onChange={handleChangeCheckboxFilter({
                              fieldName: "status",
                              fieldType: "in",
                              classFilter: IdFilter,
                            })}
                            getList={ appUserRepository.filterListStatus  }
                          />
                        </div>
                      
                    

            </FilterPanel.Left>
            <FilterPanel.Right>
                <Row gutter={ { xs: 8, sm: 16, md: 24, lg: 32 } }>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('appUsers.username')}
                                         value={modelFilter["username"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "username" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('appUsers.placeholder.username')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('appUsers.displayName')}
                                         value={modelFilter["displayName"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "displayName" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('appUsers.placeholder.displayName')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('appUsers.address')}
                                         value={modelFilter["address"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "address" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('appUsers.placeholder.address')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('appUsers.email')}
                                         value={modelFilter["email"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "email" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('appUsers.placeholder.email')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('appUsers.phone')}
                                         value={modelFilter["phone"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "phone" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('appUsers.placeholder.phone')} />
                    </Col>
                     


                    

 


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceDateRangeFilter label={ translate('appUsers.birthday')}
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
                    <AdvanceStringFilter label={ translate('appUsers.avatar')}
                                         value={modelFilter["avatar"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "avatar" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('appUsers.placeholder.avatar')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('appUsers.department')}
                                         value={modelFilter["department"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "department" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('appUsers.placeholder.department')} />
                    </Col>
                     


                    

 


                    

 


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceDateRangeFilter label={ translate('appUsers.createdAt')}
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
                    <AdvanceDateRangeFilter label={ translate('appUsers.updatedAt')}
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
                    <AdvanceStringFilter label={ translate('appUsers.code')}
                                         value={modelFilter["code"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "code" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('appUsers.placeholder.code')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('appUsers.name')}
                                         value={modelFilter["name"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "name" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('appUsers.placeholder.name')} />
                    </Col>
                     


                    




                    
                    <Col lg={8} className="m-b--md">
                    <AdvanceIdMultipleFilter values={modelFilter["organizationValue"]}
                                     onChange={handleChangeMultipleSelectFilter({
                                     fieldName: "organization" ,
                                     fieldType: "in" ,
                                     classFilter: IdFilter,
                                     })}
                                     bgColor="white"
                                     type={0}
                                     isSmall
                                     classFilter={ OrganizationFilter }
                                     getList={ appUserRepository.filterListOrganization }
                                     placeHolder={translate('appUsers.placeholder.organization')}
                                     label={ translate('appUsers.organization')} />
                    </Col>
                    




                    




                    

                </Row>
            </FilterPanel.Right>
        </FilterPanel>
        );

    }

export default AppUserAdvanceFilter;
