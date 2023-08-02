import { StringFilter  } from 'react3l-advanced-filters';
import { IdFilter  } from 'react3l-advanced-filters';
import { DateFilter  } from 'react3l-advanced-filters';
import { GuidFilter  } from 'react3l-advanced-filters';
import { ModelFilter } from 'react3l-common';
import { ObjectField } from "react3l-decorators";

export class ImageFilter extends ModelFilter  {
  @ObjectField(IdFilter)
  public id?: IdFilter = new IdFilter();
  @ObjectField(StringFilter)
  public name?: StringFilter = new StringFilter();
  @ObjectField(StringFilter)
  public url?: StringFilter = new StringFilter();
  @ObjectField(DateFilter)
  public createdAt?: DateFilter = new DateFilter();
  @ObjectField(DateFilter)
  public updatedAt?: DateFilter = new DateFilter();
  @ObjectField(StringFilter)
  public thumbnailUrl?: StringFilter = new StringFilter();
  @ObjectField(GuidFilter)
  public rowId?: GuidFilter = new GuidFilter();
  public search?: string = null;
}
