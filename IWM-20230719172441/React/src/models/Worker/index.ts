import { Worker } from './Worker'
import nameof from 'ts-nameof.macro';
import { ObjectField,  } from 'react3l-decorators';

import { District } from 'models/District';
import { Nation } from 'models/Nation';
import { Province } from 'models/Province';
import { Sex } from 'models/Sex';
import { Status } from 'models/Status';
import { Ward } from 'models/Ward';
import { WorkerGroup } from 'models/WorkerGroup';

ObjectField(District)(Worker.prototype, nameof(Worker.prototype.district));
ObjectField(Nation)(Worker.prototype, nameof(Worker.prototype.nation));
ObjectField(Province)(Worker.prototype, nameof(Worker.prototype.province));
ObjectField(Sex)(Worker.prototype, nameof(Worker.prototype.sex));
ObjectField(Status)(Worker.prototype, nameof(Worker.prototype.status));
ObjectField(Ward)(Worker.prototype, nameof(Worker.prototype.ward));
ObjectField(WorkerGroup)(Worker.prototype, nameof(Worker.prototype.workerGroup));

export * from './Worker';
export * from './WorkerFilter';
