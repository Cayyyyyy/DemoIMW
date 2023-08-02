import { Repository } from "react3l-common";
import { kebabCase } from "lodash";
import { httpConfig } from 'core/config/http';
import { BASE_API_URL } from "core/config/consts";
import { Observable } from "rxjs";
import { AxiosResponse } from "axios";

import nameof from "ts-nameof.macro";


import { TaxType, TaxTypeFilter } from 'models/TaxType';
import { Status, StatusFilter } from 'models/Status';

export type KeyType = string | number;

export const API_TAX_TYPE_PREFIX: string = 'rpc/iwm/tax-type';


export class TaxTypeRepository extends Repository {
    constructor() {
        super(httpConfig);
        this.baseURL = new URL(API_TAX_TYPE_PREFIX, BASE_API_URL).href;      
    }

    public count = (taxTypeFilter?: TaxTypeFilter): Observable<number> => {
        return this.http.post<number>(kebabCase(nameof(this.count)), taxTypeFilter)
          .pipe(Repository.responseDataMapper<number>());
    };

    public list = (taxTypeFilter?: TaxTypeFilter): Observable<TaxType[]> => {
        return this.http.post<TaxType[]>(kebabCase(nameof(this.list)), taxTypeFilter)
            .pipe(Repository.responseMapToList<TaxType>(TaxType));
    };

    public get = (id: number | string): Observable<TaxType> => {
        return this.http.post<TaxType>
            (kebabCase(nameof(this.get)), { id })
            .pipe(Repository.responseMapToModel<TaxType>(TaxType));
    };

    public create = (taxType: TaxType): Observable<TaxType> => {
        return this.http.post<TaxType>(kebabCase(nameof(this.create)), taxType)
            .pipe(Repository.responseMapToModel<TaxType>(TaxType));
    };

    public update = (taxType: TaxType): Observable<TaxType> => {
        return this.http.post<TaxType>(kebabCase(nameof(this.update)), taxType)
            .pipe(Repository.responseMapToModel<TaxType>(TaxType));
    };

    public delete = (taxType: TaxType): Observable<TaxType> => {
        return this.http.post<TaxType>(kebabCase(nameof(this.delete)), taxType)
            .pipe(Repository.responseMapToModel<TaxType>(TaxType));
    };

    public bulkDelete = (idList: KeyType[]): Observable<void> => {
        return this.http.post(kebabCase(nameof(this.bulkDelete)), idList)
            .pipe(Repository.responseDataMapper());
    };

    public save = (taxType: TaxType): Observable<TaxType> => {
        return taxType.id ? this.update(taxType) : this.create(taxType);
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

export const taxTypeRepository = new TaxTypeRepository();
