import { AppUser } from './AppUser'
import nameof from 'ts-nameof.macro';
import { ObjectField,  } from 'react3l-decorators';

import { Organization } from 'models/Organization';
import { Sex } from 'models/Sex';
import { Status } from 'models/Status';

ObjectField(Organization)(AppUser.prototype, nameof(AppUser.prototype.organization));
ObjectField(Sex)(AppUser.prototype, nameof(AppUser.prototype.sex));
ObjectField(Status)(AppUser.prototype, nameof(AppUser.prototype.status));

export * from './AppUser';
export * from './AppUserFilter';
