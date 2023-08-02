import { StringFilter  } from 'react3l-advanced-filters';
import { IdFilter  } from 'react3l-advanced-filters';
import { DateFilter  } from 'react3l-advanced-filters';
import { GuidFilter  } from 'react3l-advanced-filters';
import { ModelFilter } from 'react3l-common';
import { ObjectField } from "react3l-decorators";

export class AppUserFilter extends ModelFilter  {
  @ObjectField(IdFilter)
  public id?: IdFilter = new IdFilter();
  @ObjectField(StringFilter)
  public username?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public displayName?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public address?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public email?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public phone?: StringFilter = new StringFilter();
  @ObjectField(IdFilter)
  public sexId?: IdFilter = new IdFilter();
  @ObjectField(DateFilter)
  public birthday?: DateFilter = new DateFilter();
  @ObjectField(StringFilter)
  public avatar?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public department?: StringFilter = new StringFilter();
  @ObjectField(IdFilter)
  public organizationId?: IdFilter = new IdFilter();
  @ObjectField(IdFilter)
  public statusId?: IdFilter = new IdFilter();
  @ObjectField(DateFilter)
  public createdAt?: DateFilter = new DateFilter();
  @ObjectField(DateFilter)
  public updatedAt?: DateFilter = new DateFilter();
  @ObjectField(GuidFilter)
  public rowId?: GuidFilter = new GuidFilter();
  @ObjectField(StringFilter)
  public code?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public name?: StringFilter = new StringFilter();
  public search?: string = null;
}
