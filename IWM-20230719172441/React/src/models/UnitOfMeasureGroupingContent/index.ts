import { UnitOfMeasureGroupingContent } from './UnitOfMeasureGroupingContent'
import nameof from 'ts-nameof.macro';
import { ObjectField,  } from 'react3l-decorators';

import { UnitOfMeasure } from 'models/UnitOfMeasure';
import { UnitOfMeasureGrouping } from 'models/UnitOfMeasureGrouping';

ObjectField(UnitOfMeasure)(UnitOfMeasureGroupingContent.prototype, nameof(UnitOfMeasureGroupingContent.prototype.unitOfMeasure));
ObjectField(UnitOfMeasureGrouping)(UnitOfMeasureGroupingContent.prototype, nameof(UnitOfMeasureGroupingContent.prototype.unitOfMeasureGrouping));

export * from './UnitOfMeasureGroupingContent';
export * from './UnitOfMeasureGroupingContentFilter';
