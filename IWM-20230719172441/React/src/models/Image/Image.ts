import { Model } from 'react3l-common';
import { Field, MomentField } from 'react3l-decorators';
import type { Moment } from 'moment';

export class Image extends Model
{
    @Field(Number)
    public id?: number;


    @Field(String)
    public name?: string;


    @Field(String)
    public url?: string;



    @MomentField()
    public createdAt?: Moment;

    @MomentField()
    public updatedAt?: Moment;

    @MomentField()
    public deletedAt?: Moment;
    @Field(String)
    public thumbnailUrl?: string;


    @Field(String)
    public rowId?: string;




}
