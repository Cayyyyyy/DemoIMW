import { IdFilter  } from 'react3l-advanced-filters';
import { ModelFilter } from 'react3l-common';
import { ObjectField } from "react3l-decorators";

export class WorkerDistrictWorkingMappingFilter extends ModelFilter  {
  @ObjectField(IdFilter)
  public workerId?: IdFilter = new IdFilter();
  @ObjectField(IdFilter)
  public districtId?: IdFilter = new IdFilter();
  public search?: string = null;
}
