import { Repository } from "react3l-common";
import { kebabCase } from "lodash";
import { httpConfig } from 'core/config/http';
import { BASE_API_URL } from "core/config/consts";
import { Observable } from "rxjs";
import { AxiosResponse } from "axios";

import nameof from "ts-nameof.macro";


import { ProductType, ProductTypeFilter } from 'models/ProductType';
import { Status, StatusFilter } from 'models/Status';

export type KeyType = string | number;

export const API_PRODUCT_TYPE_PREFIX: string = 'rpc/iwm/product-type';


export class ProductTypeRepository extends Repository {
    constructor() {
        super(httpConfig);
        this.baseURL = new URL(API_PRODUCT_TYPE_PREFIX, BASE_API_URL).href;      
    }

    public count = (productTypeFilter?: ProductTypeFilter): Observable<number> => {
        return this.http.post<number>(kebabCase(nameof(this.count)), productTypeFilter)
          .pipe(Repository.responseDataMapper<number>());
    };

    public list = (productTypeFilter?: ProductTypeFilter): Observable<ProductType[]> => {
        return this.http.post<ProductType[]>(kebabCase(nameof(this.list)), productTypeFilter)
            .pipe(Repository.responseMapToList<ProductType>(ProductType));
    };

    public get = (id: number | string): Observable<ProductType> => {
        return this.http.post<ProductType>
            (kebabCase(nameof(this.get)), { id })
            .pipe(Repository.responseMapToModel<ProductType>(ProductType));
    };

    public create = (productType: ProductType): Observable<ProductType> => {
        return this.http.post<ProductType>(kebabCase(nameof(this.create)), productType)
            .pipe(Repository.responseMapToModel<ProductType>(ProductType));
    };

    public update = (productType: ProductType): Observable<ProductType> => {
        return this.http.post<ProductType>(kebabCase(nameof(this.update)), productType)
            .pipe(Repository.responseMapToModel<ProductType>(ProductType));
    };

    public delete = (productType: ProductType): Observable<ProductType> => {
        return this.http.post<ProductType>(kebabCase(nameof(this.delete)), productType)
            .pipe(Repository.responseMapToModel<ProductType>(ProductType));
    };

    public bulkDelete = (idList: KeyType[]): Observable<void> => {
        return this.http.post(kebabCase(nameof(this.bulkDelete)), idList)
            .pipe(Repository.responseDataMapper());
    };

    public save = (productType: ProductType): Observable<ProductType> => {
        return productType.id ? this.update(productType) : this.create(productType);
    };

    public singleListStatus = (): Observable<Status[]> => {
        return this.http.post<Status[]>(kebabCase(nameof(this.singleListStatus)), new StatusFilter())
            .pipe(Repository.responseMapToList<Status>(Status));
    };

    public filterListStatus = (): Observable<Status[]> => {
        return this.http.post<Status[]>(kebabCase(nameof(this.filterListStatus)), new StatusFilter())
            .pipe(Repository.responseMapToList<Status>(Status));
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

export const productTypeRepository = new ProductTypeRepository();
