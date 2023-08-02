import { Brand } from './Brand'
import nameof from 'ts-nameof.macro';
import { ObjectField,  } from 'react3l-decorators';

import { Status } from 'models/Status';

ObjectField(Status)(Brand.prototype, nameof(Brand.prototype.status));

export * from './Brand';
export * from './BrandFilter';
