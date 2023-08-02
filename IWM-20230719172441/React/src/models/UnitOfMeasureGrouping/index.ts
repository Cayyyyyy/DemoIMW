import { UnitOfMeasureGrouping } from './UnitOfMeasureGrouping'
import nameof from 'ts-nameof.macro';
import { ObjectField,  } from 'react3l-decorators';

import { Status } from 'models/Status';
import { UnitOfMeasure } from 'models/UnitOfMeasure';

ObjectField(Status)(UnitOfMeasureGrouping.prototype, nameof(UnitOfMeasureGrouping.prototype.status));
ObjectField(UnitOfMeasure)(UnitOfMeasureGrouping.prototype, nameof(UnitOfMeasureGrouping.prototype.unitOfMeasure));

export * from './UnitOfMeasureGrouping';
export * from './UnitOfMeasureGroupingFilter';
