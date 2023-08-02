import { Model } from 'react3l-common';
import { Field, MomentField } from 'react3l-decorators';
import type { Moment } from 'moment';
import { Organization } from 'models/Organization';
import { Sex } from 'models/Sex';
import { Status } from 'models/Status';

export class AppUser extends Model
{
    @Field(Number)
    public id?: number;


    @Field(String)
    public username?: string;


    @Field(String)
    public displayName?: string;


    @Field(String)
    public address?: string;


    @Field(String)
    public email?: string;


    @Field(String)
    public phone?: string;


    @Field(Number)
    public sexId?: number;



    @MomentField()
    public birthday?: Moment;
    @Field(String)
    public avatar?: string;


    @Field(String)
    public department?: string;


    @Field(Number)
    public organizationId?: number;


    @Field(Number)
    public statusId?: number;



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


    @Field(String)
    public code?: string;


    @Field(String)
    public name?: string;



    public organization?: Organization;


    public sex?: Sex;


    public status?: Status;

}
