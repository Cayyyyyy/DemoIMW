import { ProductType } from './ProductType'
import nameof from 'ts-nameof.macro';
import { ObjectField,  } from 'react3l-decorators';

import { Status } from 'models/Status';

ObjectField(Status)(ProductType.prototype, nameof(ProductType.prototype.status));

export * from './ProductType';
export * from './ProductTypeFilter';
