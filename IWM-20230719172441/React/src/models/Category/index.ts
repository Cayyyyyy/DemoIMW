import { Category } from './Category'
import nameof from 'ts-nameof.macro';
import { ObjectField,  } from 'react3l-decorators';

import { Image } from 'models/Image';
import { Status } from 'models/Status';

ObjectField(Image)(Category.prototype, nameof(Category.prototype.image));
ObjectField(Category)(Category.prototype, nameof(Category.prototype.parent));
ObjectField(Status)(Category.prototype, nameof(Category.prototype.status));

export * from './Category';
export * from './CategoryFilter';
