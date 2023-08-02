import { Model } from 'react3l-common';
import { Field, MomentField } from 'react3l-decorators';
import type { Moment } from 'moment';
import { Status } from 'models/Status';

export class UnitOfMeasure extends Model
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
    public statusId?: number;


    @Field(Boolean)
    public isDecimal?: boolean;



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


    @Field(String)
    public erpCode?: string;



    public status?: Status;







}
