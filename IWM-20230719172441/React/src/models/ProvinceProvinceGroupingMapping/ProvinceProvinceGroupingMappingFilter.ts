import { IdFilter  } from 'react3l-advanced-filters';
import { ModelFilter } from 'react3l-common';
import { ObjectField } from "react3l-decorators";

export class ProvinceProvinceGroupingMappingFilter extends ModelFilter  {
  @ObjectField(IdFilter)
  public provinceGroupingId?: IdFilter = new IdFilter();
  @ObjectField(IdFilter)
  public provinceId?: IdFilter = new IdFilter();
  public search?: string = null;
}
