import { Model } from 'react3l-common';
import { Field, MomentField } from 'react3l-decorators';
import type { Moment } from 'moment';
import { Status } from 'models/Status';

export class ProvinceGrouping extends Model
{
    @Field(Number)
    public id?: number;


    @Field(String)
    public code?: string;


    @Field(String)
    public name?: string;


    @Field(Number)
    public statusId?: number;


    @Field(Number)
    public parentId?: number;


    @Field(Boolean)
    public hasChildren?: boolean;


    @Field(Number)
    public level?: number;


    @Field(String)
    public path?: string;



    @MomentField()
    public createdAt?: Moment;

    @MomentField()
    public updatedAt?: Moment;

    @MomentField()
    public deletedAt?: Moment;
    @Field(String)
    public rowId?: string;



    public parent?: ProvinceGrouping;


    public status?: Status;





}
