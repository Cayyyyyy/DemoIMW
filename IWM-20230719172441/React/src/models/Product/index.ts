import { Product } from './Product'
import nameof from 'ts-nameof.macro';
import { ObjectField,  } from 'react3l-decorators';

import { Brand } from 'models/Brand';
import { Category } from 'models/Category';
import { ProductType } from 'models/ProductType';
import { Status } from 'models/Status';
import { TaxType } from 'models/TaxType';
import { UnitOfMeasure } from 'models/UnitOfMeasure';
import { UnitOfMeasureGrouping } from 'models/UnitOfMeasureGrouping';

ObjectField(Brand)(Product.prototype, nameof(Product.prototype.brand));
ObjectField(Category)(Product.prototype, nameof(Product.prototype.category));
ObjectField(ProductType)(Product.prototype, nameof(Product.prototype.productType));
ObjectField(Status)(Product.prototype, nameof(Product.prototype.status));
ObjectField(TaxType)(Product.prototype, nameof(Product.prototype.taxType));
ObjectField(UnitOfMeasure)(Product.prototype, nameof(Product.prototype.unitOfMeasure));
ObjectField(UnitOfMeasureGrouping)(Product.prototype, nameof(Product.prototype.unitOfMeasureGrouping));

export * from './Product';
export * from './ProductFilter';
