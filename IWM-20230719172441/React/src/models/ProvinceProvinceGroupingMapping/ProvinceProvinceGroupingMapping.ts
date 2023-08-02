import { Model } from 'react3l-common';
import { Field, MomentField } from 'react3l-decorators';
import { Province } from 'models/Province';
import { ProvinceGrouping } from 'models/ProvinceGrouping';

export class ProvinceProvinceGroupingMapping extends Model
{
    @Field(Number)
    public provinceGroupingId?: number;


    @Field(Number)
    public provinceId?: number;



    public province?: Province;


    public provinceGrouping?: ProvinceGrouping;

}
