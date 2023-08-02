import { Organization } from './Organization'
import nameof from 'ts-nameof.macro';
import { ObjectField,  } from 'react3l-decorators';

import { Status } from 'models/Status';

ObjectField(Organization)(Organization.prototype, nameof(Organization.prototype.parent));
ObjectField(Status)(Organization.prototype, nameof(Organization.prototype.status));

export * from './Organization';
export * from './OrganizationFilter';
