import { Model } from 'react3l-common';
import { Field, MomentField } from 'react3l-decorators';
import { District } from 'models/District';
import { Worker } from 'models/Worker';

export class WorkerDistrictWorkingMapping extends Model
{
    @Field(Number)
    public workerId?: number;


    @Field(Number)
    public districtId?: number;



    public district?: District;


    public worker?: Worker;

}
