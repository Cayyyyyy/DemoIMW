import { Model } from 'react3l-common';
import { Field, MomentField } from 'react3l-decorators';
import type { Moment } from 'moment';
import { Image } from 'models/Image';
import { Status } from 'models/Status';

export class Category extends Model
{
    @Field(Number)
    public id?: number;


    @Field(String)
    public code?: string;


    @Field(String)
    public name?: string;


    @Field(String)
    public prefix?: string;


    @Field(String)
    public description?: string;


    @Field(Number)
    public parentId?: number;


    @Field(String)
    public path?: string;


    @Field(Number)
    public level?: number;


    @Field(Boolean)
    public hasChildren?: boolean;


    @Field(Number)
    public statusId?: number;


    @Field(Number)
    public imageId?: number;



    @MomentField()
    public createdAt?: Moment;

    @MomentField()
    public updatedAt?: Moment;

    @MomentField()
    public deletedAt?: Moment;
    @Field(String)
    public rowId?: string;


    @Field(Boolean)
    public used?: boolean;


    @Field(Number)
    public orderNumber?: number;



    public image?: Image;


    public parent?: Category;


    public status?: Status;





}
