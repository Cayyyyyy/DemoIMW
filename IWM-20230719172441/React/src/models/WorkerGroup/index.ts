import { WorkerGroup } from './WorkerGroup'
import nameof from 'ts-nameof.macro';
import { ObjectField,  } from 'react3l-decorators';

import { Status } from 'models/Status';

ObjectField(Status)(WorkerGroup.prototype, nameof(WorkerGroup.prototype.status));

export * from './WorkerGroup';
export * from './WorkerGroupFilter';
