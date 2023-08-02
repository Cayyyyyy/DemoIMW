import { Model } from 'react3l-common';
import { Field, MomentField } from 'react3l-decorators';

export class Status extends Model
{
    @Field(Number)
    public id?: number;


    @Field(String)
    public code?: string;


    @Field(String)
    public name?: string;


    @Field(String)
    public color?: string;


































}
