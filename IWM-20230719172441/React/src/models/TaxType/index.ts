import { TaxType } from './TaxType'
import nameof from 'ts-nameof.macro';
import { ObjectField,  } from 'react3l-decorators';

import { Status } from 'models/Status';

ObjectField(Status)(TaxType.prototype, nameof(TaxType.prototype.status));

export * from './TaxType';
export * from './TaxTypeFilter';
