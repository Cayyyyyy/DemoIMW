import { Model } from 'react3l-common';
import { Field, MomentField } from 'react3l-decorators';
import { Status } from 'models/Status';

export class WorkerGroup extends Model
{
    @Field(Number)
    public id?: number;


    @Field(String)
    public code?: string;


    @Field(String)
    public name?: string;


    @Field(Number)
    public statusId?: number;



    public status?: Status;



}
