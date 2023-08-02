import { ProvinceGrouping } from './ProvinceGrouping'
import nameof from 'ts-nameof.macro';
import { ObjectField,  } from 'react3l-decorators';

import { Status } from 'models/Status';

ObjectField(ProvinceGrouping)(ProvinceGrouping.prototype, nameof(ProvinceGrouping.prototype.parent));
ObjectField(Status)(ProvinceGrouping.prototype, nameof(ProvinceGrouping.prototype.status));

export * from './ProvinceGrouping';
export * from './ProvinceGroupingFilter';
