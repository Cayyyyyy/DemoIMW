import { UnitOfMeasure } from './UnitOfMeasure'
import nameof from 'ts-nameof.macro';
import { ObjectField,  } from 'react3l-decorators';

import { Status } from 'models/Status';

ObjectField(Status)(UnitOfMeasure.prototype, nameof(UnitOfMeasure.prototype.status));

export * from './UnitOfMeasure';
export * from './UnitOfMeasureFilter';
