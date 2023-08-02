import { Model } from 'react3l-common';
import { Field, MomentField } from 'react3l-decorators';
import type { Moment } from 'moment';
import { Brand } from 'models/Brand';
import { Category } from 'models/Category';
import { ProductType } from 'models/ProductType';
import { Status } from 'models/Status';
import { TaxType } from 'models/TaxType';
import { UnitOfMeasure } from 'models/UnitOfMeasure';
import { UnitOfMeasureGrouping } from 'models/UnitOfMeasureGrouping';

export class Product extends Model
{
    @Field(Number)
    public id?: number;


    @Field(String)
    public code?: string;


    @Field(String)
    public supplierCode?: string;


    @Field(String)
    public name?: string;


    @Field(String)
    public description?: string;


    @Field(String)
    public scanCode?: string;


    @Field(String)
    public eRPCode?: string;


    @Field(Number)
    public categoryId?: number;


    @Field(Number)
    public productTypeId?: number;


    @Field(Number)
    public brandId?: number;


    @Field(Number)
    public unitOfMeasureId?: number;


    @Field(Number)
    public codeGeneratorRuleId?: number;


    @Field(Number)
    public unitOfMeasureGroupingId?: number;


    @Field(Number)
    public salePrice?: number;


    @Field(Number)
    public retailPrice?: number;


    @Field(Number)
    public taxTypeId?: number;


    @Field(Number)
    public statusId?: number;


    @Field(String)
    public otherName?: string;


    @Field(String)
    public technicalName?: string;


    @Field(String)
    public note?: string;


    @Field(Boolean)
    public isPurchasable?: boolean;


    @Field(Boolean)
    public isSellable?: boolean;


    @Field(Boolean)
    public isNew?: boolean;


    @Field(Number)
    public usedVariationId?: number;



    @MomentField()
    public createdAt?: Moment;

    @MomentField()
    public updatedAt?: Moment;

    @MomentField()
    public deletedAt?: Moment;
    @Field(Boolean)
    public used?: boolean;


    @Field(String)
    public rowId?: string;


    @Field(Boolean)
    public isCopyRightedProduct?: boolean;


    @Field(Boolean)
    public isBrandProduct?: boolean;



    public brand?: Brand;


    public category?: Category;


    public productType?: ProductType;


    public status?: Status;


    public taxType?: TaxType;


    public unitOfMeasure?: UnitOfMeasure;


    public unitOfMeasureGrouping?: UnitOfMeasureGrouping;

}
