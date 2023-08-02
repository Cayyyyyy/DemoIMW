import { StringFilter  } from 'react3l-advanced-filters';
import { IdFilter  } from 'react3l-advanced-filters';
import { NumberFilter  } from 'react3l-advanced-filters';
import { DateFilter  } from 'react3l-advanced-filters';
import { GuidFilter  } from 'react3l-advanced-filters';
import { ModelFilter } from 'react3l-common';
import { ObjectField } from "react3l-decorators";

export class CategoryFilter extends ModelFilter  {
  @ObjectField(IdFilter)
  public id?: IdFilter = new IdFilter();
  @ObjectField(StringFilter)
  public code?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public name?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public prefix?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public description?: StringFilter = new StringFilter();
  @ObjectField(IdFilter)
  public parentId?: IdFilter = new IdFilter();
  @ObjectField(StringFilter)
  public path?: StringFilter = new StringFilter();
  @ObjectField(NumberFilter)
  public level?: NumberFilter = new NumberFilter();
  @ObjectField(IdFilter)
  public statusId?: IdFilter = new IdFilter();
  @ObjectField(IdFilter)
  public imageId?: IdFilter = new IdFilter();
  @ObjectField(DateFilter)
  public createdAt?: DateFilter = new DateFilter();
  @ObjectField(DateFilter)
  public updatedAt?: DateFilter = new DateFilter();
  @ObjectField(GuidFilter)
  public rowId?: GuidFilter = new GuidFilter();
  @ObjectField(NumberFilter)
  public orderNumber?: NumberFilter = new NumberFilter();
  public search?: string = null;
}
