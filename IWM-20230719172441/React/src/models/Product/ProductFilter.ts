import { StringFilter  } from 'react3l-advanced-filters';
import { IdFilter  } from 'react3l-advanced-filters';
import { NumberFilter  } from 'react3l-advanced-filters';
import { DateFilter  } from 'react3l-advanced-filters';
import { GuidFilter  } from 'react3l-advanced-filters';
import { ModelFilter } from 'react3l-common';
import { ObjectField } from "react3l-decorators";

export class ProductFilter extends ModelFilter  {
  @ObjectField(IdFilter)
  public id?: IdFilter = new IdFilter();
  @ObjectField(StringFilter)
  public code?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public supplierCode?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public name?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public description?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public scanCode?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public eRPCode?: StringFilter = new StringFilter();
  @ObjectField(IdFilter)
  public categoryId?: IdFilter = new IdFilter();
  @ObjectField(IdFilter)
  public productTypeId?: IdFilter = new IdFilter();
  @ObjectField(IdFilter)
  public brandId?: IdFilter = new IdFilter();
  @ObjectField(IdFilter)
  public unitOfMeasureId?: IdFilter = new IdFilter();
  @ObjectField(IdFilter)
  public codeGeneratorRuleId?: IdFilter = new IdFilter();
  @ObjectField(IdFilter)
  public unitOfMeasureGroupingId?: IdFilter = new IdFilter();
  @ObjectField(NumberFilter)
  public salePrice?: NumberFilter = new NumberFilter();
  @ObjectField(NumberFilter)
  public retailPrice?: NumberFilter = new NumberFilter();
  @ObjectField(IdFilter)
  public taxTypeId?: IdFilter = new IdFilter();
  @ObjectField(IdFilter)
  public statusId?: IdFilter = new IdFilter();
  @ObjectField(StringFilter)
  public otherName?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public technicalName?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public note?: StringFilter = new StringFilter();
  @ObjectField(IdFilter)
  public usedVariationId?: IdFilter = new IdFilter();
  @ObjectField(DateFilter)
  public createdAt?: DateFilter = new DateFilter();
  @ObjectField(DateFilter)
  public updatedAt?: DateFilter = new DateFilter();
  @ObjectField(GuidFilter)
  public rowId?: GuidFilter = new GuidFilter();
  public search?: string = null;
}
