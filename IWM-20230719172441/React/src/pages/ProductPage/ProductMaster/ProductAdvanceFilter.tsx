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
import { productRepository } from "../ProductRepository";
import { Product, ProductFilter } from "models/Product";
import { Brand, BrandFilter } from "models/Brand";
import { Category, CategoryFilter } from "models/Category";
import { ProductType, ProductTypeFilter } from "models/ProductType";
import { Status, StatusFilter } from "models/Status";
import { TaxType, TaxTypeFilter } from "models/TaxType";
import { UnitOfMeasure, UnitOfMeasureFilter } from "models/UnitOfMeasure";
import { UnitOfMeasureGrouping, UnitOfMeasureGroupingFilter } from "models/UnitOfMeasureGrouping";
/* end individual import */


export interface  ProductFilterProps {
  filter?: any;
  setVisible?: any;
  handleChangeAllFilter?: (data: any) => void;
  visible?: boolean;
}

function ProductAdvanceFilter(props: ProductFilterProps) {
    const [translate] = useTranslation();

     const { visible, filter, setVisible, handleChangeAllFilter } = props;

    const { modelFilter, dispatchFilter } = filterService.useModelFilter(
        ProductFilter,
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
          payload: new ProductFilter(),
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
                            label={ translate('products.status')}
                            values={modelFilter?.statusId?.in}
                            onChange={handleChangeCheckboxFilter({
                              fieldName: "status",
                              fieldType: "in",
                              classFilter: IdFilter,
                            })}
                            getList={ productRepository.filterListStatus  }
                          />
                        </div>
                      
                    







            </FilterPanel.Left>
            <FilterPanel.Right>
                <Row gutter={ { xs: 8, sm: 16, md: 24, lg: 32 } }>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('products.code')}
                                         value={modelFilter["code"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "code" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('products.placeholder.code')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('products.supplierCode')}
                                         value={modelFilter["supplierCode"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "supplierCode" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('products.placeholder.supplierCode')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('products.name')}
                                         value={modelFilter["name"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "name" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('products.placeholder.name')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('products.description')}
                                         value={modelFilter["description"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "description" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('products.placeholder.description')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('products.scanCode')}
                                         value={modelFilter["scanCode"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "scanCode" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('products.placeholder.scanCode')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('products.eRPCode')}
                                         value={modelFilter["eRPCode"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "eRPCode" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('products.placeholder.eRPCode')} />
                    </Col>
                     


                    

 


                    

 


                    

 


                    

 


                    

 


                    

 


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceNumberFilter label={ translate('products.salePrice')}
                                         value={modelFilter["salePrice"]["equal"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "salePrice" ,
                                         fieldType: "equal" ,
                                         classFilter:NumberFilter,} )}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('products.placeholder.salePrice')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceNumberFilter label={ translate('products.retailPrice')}
                                         value={modelFilter["retailPrice"]["equal"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "retailPrice" ,
                                         fieldType: "equal" ,
                                         classFilter:NumberFilter,} )}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('products.placeholder.retailPrice')} />
                    </Col>
                     


                    

 


                    

 


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('products.otherName')}
                                         value={modelFilter["otherName"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "otherName" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('products.placeholder.otherName')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('products.technicalName')}
                                         value={modelFilter["technicalName"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "technicalName" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('products.placeholder.technicalName')} />
                    </Col>
                     


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceStringFilter label={ translate('products.note')}
                                         value={modelFilter["note"]["contain"]}
                                         onChange={handleChangeInputFilter({
                                         fieldName: "note" ,
                                         fieldType: "contain" ,
                                         classFilter: StringFilter,})}
                                         bgColor="white"
                                         type={0}
                                         isSmall
                                         placeHolder={translate('products.placeholder.note')} />
                    </Col>
                     


                    




                    




                    




                    

 


                    



                    <Col lg={8} className="m-b--md">
                    <AdvanceDateRangeFilter label={ translate('products.createdAt')}
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
                    <AdvanceDateRangeFilter label={ translate('products.updatedAt')}
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
                    <AdvanceIdMultipleFilter values={modelFilter["brandValue"]}
                                     onChange={handleChangeMultipleSelectFilter({
                                     fieldName: "brand" ,
                                     fieldType: "in" ,
                                     classFilter: IdFilter,
                                     })}
                                     bgColor="white"
                                     type={0}
                                     isSmall
                                     classFilter={ BrandFilter }
                                     getList={ productRepository.filterListBrand }
                                     placeHolder={translate('products.placeholder.brand')}
                                     label={ translate('products.brand')} />
                    </Col>
                    




                    
                    <Col lg={8} className="m-b--md">
                    <AdvanceIdMultipleFilter values={modelFilter["categoryValue"]}
                                     onChange={handleChangeMultipleSelectFilter({
                                     fieldName: "category" ,
                                     fieldType: "in" ,
                                     classFilter: IdFilter,
                                     })}
                                     bgColor="white"
                                     type={0}
                                     isSmall
                                     classFilter={ CategoryFilter }
                                     getList={ productRepository.filterListCategory }
                                     placeHolder={translate('products.placeholder.category')}
                                     label={ translate('products.category')} />
                    </Col>
                    




                    
                    <Col lg={8} className="m-b--md">
                    <AdvanceIdMultipleFilter values={modelFilter["productTypeValue"]}
                                     onChange={handleChangeMultipleSelectFilter({
                                     fieldName: "productType" ,
                                     fieldType: "in" ,
                                     classFilter: IdFilter,
                                     })}
                                     bgColor="white"
                                     type={0}
                                     isSmall
                                     classFilter={ ProductTypeFilter }
                                     getList={ productRepository.filterListProductType }
                                     placeHolder={translate('products.placeholder.productType')}
                                     label={ translate('products.productType')} />
                    </Col>
                    




                    




                    
                    <Col lg={8} className="m-b--md">
                    <AdvanceIdMultipleFilter values={modelFilter["taxTypeValue"]}
                                     onChange={handleChangeMultipleSelectFilter({
                                     fieldName: "taxType" ,
                                     fieldType: "in" ,
                                     classFilter: IdFilter,
                                     })}
                                     bgColor="white"
                                     type={0}
                                     isSmall
                                     classFilter={ TaxTypeFilter }
                                     getList={ productRepository.filterListTaxType }
                                     placeHolder={translate('products.placeholder.taxType')}
                                     label={ translate('products.taxType')} />
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
                                     getList={ productRepository.filterListUnitOfMeasure }
                                     placeHolder={translate('products.placeholder.unitOfMeasure')}
                                     label={ translate('products.unitOfMeasure')} />
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
                                     getList={ productRepository.filterListUnitOfMeasureGrouping }
                                     placeHolder={translate('products.placeholder.unitOfMeasureGrouping')}
                                     label={ translate('products.unitOfMeasureGrouping')} />
                    </Col>
                    

                </Row>
            </FilterPanel.Right>
        </FilterPanel>
        );

    }

export default ProductAdvanceFilter;
