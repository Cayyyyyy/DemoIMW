import { Repository } from "react3l-common";
import { kebabCase } from "lodash";
import { httpConfig } from 'core/config/http';
import { BASE_API_URL } from "core/config/consts";
import { Observable } from "rxjs";
import { AxiosResponse } from "axios";

import nameof from "ts-nameof.macro";


import { Brand, BrandFilter } from 'models/Brand';
import { Status, StatusFilter } from 'models/Status';

export type KeyType = string | number;

export const API_BRAND_PREFIX: string = 'rpc/iwm/brand';


export class BrandRepository extends Repository {
    constructor() {
        super(httpConfig);
        this.baseURL = new URL(API_BRAND_PREFIX, BASE_API_URL).href;      
    }

    public count = (brandFilter?: BrandFilter): Observable<number> => {
        return this.http.post<number>(kebabCase(nameof(this.count)), brandFilter)
          .pipe(Repository.responseDataMapper<number>());
    };

    public list = (brandFilter?: BrandFilter): Observable<Brand[]> => {
        return this.http.post<Brand[]>(kebabCase(nameof(this.list)), brandFilter)
            .pipe(Repository.responseMapToList<Brand>(Brand));
    };

    public get = (id: number | string): Observable<Brand> => {
        return this.http.post<Brand>
            (kebabCase(nameof(this.get)), { id })
            .pipe(Repository.responseMapToModel<Brand>(Brand));
    };

    public create = (brand: Brand): Observable<Brand> => {
        return this.http.post<Brand>(kebabCase(nameof(this.create)), brand)
            .pipe(Repository.responseMapToModel<Brand>(Brand));
    };

    public update = (brand: Brand): Observable<Brand> => {
        return this.http.post<Brand>(kebabCase(nameof(this.update)), brand)
            .pipe(Repository.responseMapToModel<Brand>(Brand));
    };

    public delete = (brand: Brand): Observable<Brand> => {
        return this.http.post<Brand>(kebabCase(nameof(this.delete)), brand)
            .pipe(Repository.responseMapToModel<Brand>(Brand));
    };

    public bulkDelete = (idList: KeyType[]): Observable<void> => {
        return this.http.post(kebabCase(nameof(this.bulkDelete)), idList)
            .pipe(Repository.responseDataMapper());
    };

    public save = (brand: Brand): Observable<Brand> => {
        return brand.id ? this.update(brand) : this.create(brand);
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

export const brandRepository = new BrandRepository();
