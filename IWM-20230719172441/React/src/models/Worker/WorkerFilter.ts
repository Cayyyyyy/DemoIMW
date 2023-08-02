import { StringFilter  } from 'react3l-advanced-filters';
import { IdFilter  } from 'react3l-advanced-filters';
import { DateFilter  } from 'react3l-advanced-filters';
import { ModelFilter } from 'react3l-common';
import { ObjectField } from "react3l-decorators";

export class WorkerFilter extends ModelFilter  {
  @ObjectField(IdFilter)
  public id?: IdFilter = new IdFilter();
  @ObjectField(StringFilter)
  public code?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public name?: StringFilter = new StringFilter();
  @ObjectField(IdFilter)
  public statusId?: IdFilter = new IdFilter();
  @ObjectField(DateFilter)
  public birthday?: DateFilter = new DateFilter();
  @ObjectField(StringFilter)
  public phone?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public citizenIdentificationNumber?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public email?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public address?: StringFilter = new StringFilter();
  @ObjectField(IdFilter)
  public sexId?: IdFilter = new IdFilter();
  @ObjectField(IdFilter)
  public workerGroupId?: IdFilter = new IdFilter();
  @ObjectField(IdFilter)
  public nationId?: IdFilter = new IdFilter();
  @ObjectField(IdFilter)
  public provinceId?: IdFilter = new IdFilter();
  @ObjectField(IdFilter)
  public districtId?: IdFilter = new IdFilter();
  @ObjectField(IdFilter)
  public wardId?: IdFilter = new IdFilter();
  @ObjectField(StringFilter)
  public username?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public password?: StringFilter = new StringFilter();
  public search?: string = null;
}
