import { Model } from 'react3l-common';
import { Field, MomentField } from 'react3l-decorators';
import type { Moment } from 'moment';
import { Status } from 'models/Status';
import { UnitOfMeasure } from 'models/UnitOfMeasure';

export class UnitOfMeasureGrouping extends Model
{
    @Field(Number)
    public id?: number;


    @Field(String)
    public code?: string;


    @Field(String)
    public name?: string;


    @Field(String)
    public description?: string;


    @Field(Number)
    public unitOfMeasureId?: number;


    @Field(Number)
    public statusId?: number;



    @MomentField()
    public createdAt?: Moment;

    @MomentField()
    public updatedAt?: Moment;

    @MomentField()
    public deletedAt?: Moment;
    @Field(Boolean)
    public used?: boolean;


    @Field(String)
    public rowId?: string;



    public status?: Status;


    public unitOfMeasure?: UnitOfMeasure;





}
