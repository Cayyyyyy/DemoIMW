import { IdFilter  } from 'react3l-advanced-filters';
import { NumberFilter  } from 'react3l-advanced-filters';
import { GuidFilter  } from 'react3l-advanced-filters';
import { ModelFilter } from 'react3l-common';
import { ObjectField } from "react3l-decorators";

export class UnitOfMeasureGroupingContentFilter extends ModelFilter  {
  @ObjectField(IdFilter)
  public id?: IdFilter = new IdFilter();
  @ObjectField(IdFilter)
  public unitOfMeasureGroupingId?: IdFilter = new IdFilter();
  @ObjectField(IdFilter)
  public unitOfMeasureId?: IdFilter = new IdFilter();
  @ObjectField(NumberFilter)
  public factor?: NumberFilter = new NumberFilter();
  @ObjectField(GuidFilter)
  public rowId?: GuidFilter = new GuidFilter();
  public search?: string = null;
}
