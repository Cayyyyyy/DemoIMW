import { WorkerDistrictWorkingMapping } from './WorkerDistrictWorkingMapping'
import nameof from 'ts-nameof.macro';
import { ObjectField,  } from 'react3l-decorators';

import { District } from 'models/District';
import { Worker } from 'models/Worker';

ObjectField(District)(WorkerDistrictWorkingMapping.prototype, nameof(WorkerDistrictWorkingMapping.prototype.district));
ObjectField(Worker)(WorkerDistrictWorkingMapping.prototype, nameof(WorkerDistrictWorkingMapping.prototype.worker));

export * from './WorkerDistrictWorkingMapping';
export * from './WorkerDistrictWorkingMappingFilter';
