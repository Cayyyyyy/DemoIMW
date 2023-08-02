import { Repository } from "react3l-common";
import { kebabCase } from "lodash";
import { httpConfig } from 'core/config/http';
import { BASE_API_URL } from "core/config/consts";
import { Observable } from "rxjs";
import { AxiosResponse } from "axios";

import nameof from "ts-nameof.macro";


import { Product, ProductFilter } from 'models/Product';
import { Brand, BrandFilter } from 'models/Brand';
import { Category, CategoryFilter } from 'models/Category';
import { ProductType, ProductTypeFilter } from 'models/ProductType';
import { Status, StatusFilter } from 'models/Status';
import { TaxType, TaxTypeFilter } from 'models/TaxType';
import { UnitOfMeasure, UnitOfMeasureFilter } from 'models/UnitOfMeasure';
import { UnitOfMeasureGrouping, UnitOfMeasureGroupingFilter } from 'models/UnitOfMeasureGrouping';

export type KeyType = string | number;

export const API_PRODUCT_PREFIX: string = 'rpc/iwm/product';


export class ProductRepository extends Repository {
    constructor() {
        super(httpConfig);
        this.baseURL = new URL(API_PRODUCT_PREFIX, BASE_API_URL).href;      
    }

    public count = (productFilter?: ProductFilter): Observable<number> => {
        return this.http.post<number>(kebabCase(nameof(this.count)), productFilter)
          .pipe(Repository.responseDataMapper<number>());
    };

    public list = (productFilter?: ProductFilter): Observable<Product[]> => {
        return this.http.post<Product[]>(kebabCase(nameof(this.list)), productFilter)
            .pipe(Repository.responseMapToList<Product>(Product));
    };

    public get = (id: number | string): Observable<Product> => {
        return this.http.post<Product>
            (kebabCase(nameof(this.get)), { id })
            .pipe(Repository.responseMapToModel<Product>(Product));
    };

    public create = (product: Product): Observable<Product> => {
        return this.http.post<Product>(kebabCase(nameof(this.create)), product)
            .pipe(Repository.responseMapToModel<Product>(Product));
    };

    public update = (product: Product): Observable<Product> => {
        return this.http.post<Product>(kebabCase(nameof(this.update)), product)
            .pipe(Repository.responseMapToModel<Product>(Product));
    };

    public delete = (product: Product): Observable<Product> => {
        return this.http.post<Product>(kebabCase(nameof(this.delete)), product)
            .pipe(Repository.responseMapToModel<Product>(Product));
    };

    public bulkDelete = (idList: KeyType[]): Observable<void> => {
        return this.http.post(kebabCase(nameof(this.bulkDelete)), idList)
            .pipe(Repository.responseDataMapper());
    };

    public save = (product: Product): Observable<Product> => {
        return product.id ? this.update(product) : this.create(product);
    };

    public singleListBrand = (brandFilter: BrandFilter): Observable<Brand[]> => {
        return this.http.post<Brand[]>(kebabCase(nameof(this.singleListBrand)), brandFilter)
            .pipe(Repository.responseMapToList<Brand>(Brand));
    }
    public filterListBrand = (brandFilter: BrandFilter): Observable<Brand[]> => {
        return this.http.post<Brand[]>(kebabCase(nameof(this.filterListBrand)), brandFilter)
            .pipe(Repository.responseMapToList<Brand>(Brand));
    };
    public singleListCategory = (categoryFilter: CategoryFilter): Observable<Category[]> => {
        return this.http.post<Category[]>(kebabCase(nameof(this.singleListCategory)), categoryFilter)
            .pipe(Repository.responseMapToList<Category>(Category));
    }
    public filterListCategory = (categoryFilter: CategoryFilter): Observable<Category[]> => {
        return this.http.post<Category[]>(kebabCase(nameof(this.filterListCategory)), categoryFilter)
            .pipe(Repository.responseMapToList<Category>(Category));
    };
    public singleListProductType = (productTypeFilter: ProductTypeFilter): Observable<ProductType[]> => {
        return this.http.post<ProductType[]>(kebabCase(nameof(this.singleListProductType)), productTypeFilter)
            .pipe(Repository.responseMapToList<ProductType>(ProductType));
    }
    public filterListProductType = (productTypeFilter: ProductTypeFilter): Observable<ProductType[]> => {
        return this.http.post<ProductType[]>(kebabCase(nameof(this.filterListProductType)), productTypeFilter)
            .pipe(Repository.responseMapToList<ProductType>(ProductType));
    };
    public singleListStatus = (): Observable<Status[]> => {
        return this.http.post<Status[]>(kebabCase(nameof(this.singleListStatus)), new StatusFilter())
            .pipe(Repository.responseMapToList<Status>(Status));
    };

    public filterListStatus = (): Observable<Status[]> => {
        return this.http.post<Status[]>(kebabCase(nameof(this.filterListStatus)), new StatusFilter())
            .pipe(Repository.responseMapToList<Status>(Status));
    };
    public singleListTaxType = (taxTypeFilter: TaxTypeFilter): Observable<TaxType[]> => {
        return this.http.post<TaxType[]>(kebabCase(nameof(this.singleListTaxType)), taxTypeFilter)
            .pipe(Repository.responseMapToList<TaxType>(TaxType));
    }
    public filterListTaxType = (taxTypeFilter: TaxTypeFilter): Observable<TaxType[]> => {
        return this.http.post<TaxType[]>(kebabCase(nameof(this.filterListTaxType)), taxTypeFilter)
            .pipe(Repository.responseMapToList<TaxType>(TaxType));
    };
    public singleListUnitOfMeasure = (unitOfMeasureFilter: UnitOfMeasureFilter): Observable<UnitOfMeasure[]> => {
        return this.http.post<UnitOfMeasure[]>(kebabCase(nameof(this.singleListUnitOfMeasure)), unitOfMeasureFilter)
            .pipe(Repository.responseMapToList<UnitOfMeasure>(UnitOfMeasure));
    }
    public filterListUnitOfMeasure = (unitOfMeasureFilter: UnitOfMeasureFilter): Observable<UnitOfMeasure[]> => {
        return this.http.post<UnitOfMeasure[]>(kebabCase(nameof(this.filterListUnitOfMeasure)), unitOfMeasureFilter)
            .pipe(Repository.responseMapToList<UnitOfMeasure>(UnitOfMeasure));
    };
    public singleListUnitOfMeasureGrouping = (unitOfMeasureGroupingFilter: UnitOfMeasureGroupingFilter): Observable<UnitOfMeasureGrouping[]> => {
        return this.http.post<UnitOfMeasureGrouping[]>(kebabCase(nameof(this.singleListUnitOfMeasureGrouping)), unitOfMeasureGroupingFilter)
            .pipe(Repository.responseMapToList<UnitOfMeasureGrouping>(UnitOfMeasureGrouping));
    }
    public filterListUnitOfMeasureGrouping = (unitOfMeasureGroupingFilter: UnitOfMeasureGroupingFilter): Observable<UnitOfMeasureGrouping[]> => {
        return this.http.post<UnitOfMeasureGrouping[]>(kebabCase(nameof(this.filterListUnitOfMeasureGrouping)), unitOfMeasureGroupingFilter)
            .pipe(Repository.responseMapToList<UnitOfMeasureGrouping>(UnitOfMeasureGrouping));
    };
    
    public import = (file: File, name: string = nameof(file)): Observable<void> => {
        const formData: FormData = new FormData();
        formData.append(name, file as Blob);
        return this.http.post<void>(kebabCase(nameof(this.import)), formData)
            .pipe(Repository.responseDataMapper<any>());
    };

    public export = (filter: any): Observable<AxiosResponse<any>> => {
        return this.http.post('export', filter, {
          responseType: 'arraybuffer',
        });
    };

    public exportTemplate = (): Observable<AxiosResponse<any>> => {
        return this.http.post('export-template', {}, {
          responseType: 'arraybuffer',
        });
    };

}

export const productRepository = new ProductRepository();
