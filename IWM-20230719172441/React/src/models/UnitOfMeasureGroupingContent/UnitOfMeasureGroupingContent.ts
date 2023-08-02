import { Model } from 'react3l-common';
import { Field, MomentField } from 'react3l-decorators';
import { UnitOfMeasure } from 'models/UnitOfMeasure';
import { UnitOfMeasureGrouping } from 'models/UnitOfMeasureGrouping';

export class UnitOfMeasureGroupingContent extends Model
{
    @Field(Number)
    public id?: number;


    @Field(Number)
    public unitOfMeasureGroupingId?: number;


    @Field(Number)
    public unitOfMeasureId?: number;


    @Field(Number)
    public factor?: number;


    @Field(String)
    public rowId?: string;



    public unitOfMeasure?: UnitOfMeasure;


    public unitOfMeasureGrouping?: UnitOfMeasureGrouping;

}
