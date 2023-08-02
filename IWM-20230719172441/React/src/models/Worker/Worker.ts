import { Model } from 'react3l-common';
import { Field, MomentField } from 'react3l-decorators';
import type { Moment } from 'moment';
import { District } from 'models/District';
import { Nation } from 'models/Nation';
import { Province } from 'models/Province';
import { Sex } from 'models/Sex';
import { Status } from 'models/Status';
import { Ward } from 'models/Ward';
import { WorkerGroup } from 'models/WorkerGroup';

export class Worker extends Model
{
    @Field(Number)
    public id?: number;


    @Field(String)
    public code?: string;


    @Field(String)
    public name?: string;


    @Field(Number)
    public statusId?: number;



    @MomentField()
    public birthday?: Moment;
    @Field(String)
    public phone?: string;


    @Field(String)
    public citizenIdentificationNumber?: string;


    @Field(String)
    public email?: string;


    @Field(String)
    public address?: string;


    @Field(Number)
    public sexId?: number;


    @Field(Number)
    public workerGroupId?: number;


    @Field(Number)
    public nationId?: number;


    @Field(Number)
    public provinceId?: number;


    @Field(Number)
    public districtId?: number;


    @Field(Number)
    public wardId?: number;


    @Field(String)
    public username?: string;


    @Field(String)
    public password?: string;



    public district?: District;


    public nation?: Nation;


    public province?: Province;


    public sex?: Sex;


    public status?: Status;


    public ward?: Ward;


    public workerGroup?: WorkerGroup;



}
