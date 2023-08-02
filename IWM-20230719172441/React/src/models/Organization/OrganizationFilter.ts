import { StringFilter  } from 'react3l-advanced-filters';
import { IdFilter  } from 'react3l-advanced-filters';
import { NumberFilter  } from 'react3l-advanced-filters';
import { DateFilter  } from 'react3l-advanced-filters';
import { GuidFilter  } from 'react3l-advanced-filters';
import { ModelFilter } from 'react3l-common';
import { ObjectField } from "react3l-decorators";

export class OrganizationFilter extends ModelFilter  {
  @ObjectField(IdFilter)
  public id?: IdFilter = new IdFilter();
  @ObjectField(StringFilter)
  public code?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public name?: StringFilter = new StringFilter();
  @ObjectField(IdFilter)
  public parentId?: IdFilter = new IdFilter();
  @ObjectField(StringFilter)
  public path?: StringFilter = new StringFilter();
  @ObjectField(NumberFilter)
  public level?: NumberFilter = new NumberFilter();
  @ObjectField(IdFilter)
  public statusId?: IdFilter = new IdFilter();
  @ObjectField(StringFilter)
  public phone?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public email?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public address?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public taxCode?: StringFilter = new StringFilter();
  @ObjectField(DateFilter)
  public createdAt?: DateFilter = new DateFilter();
  @ObjectField(DateFilter)
  public updatedAt?: DateFilter = new DateFilter();
  @ObjectField(GuidFilter)
  public rowId?: GuidFilter = new GuidFilter();
  public search?: string = null;
}
