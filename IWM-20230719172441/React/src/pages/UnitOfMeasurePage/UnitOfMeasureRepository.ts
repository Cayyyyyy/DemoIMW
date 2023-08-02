import { Repository } from "react3l-common";
import { kebabCase } from "lodash";
import { httpConfig } from 'core/config/http';
import { BASE_API_URL } from "core/config/consts";
import { Observable } from "rxjs";
import { AxiosResponse } from "axios";

import nameof from "ts-nameof.macro";


import { UnitOfMeasure, UnitOfMeasureFilter } from 'models/UnitOfMeasure';
import { Status, StatusFilter } from 'models/Status';

export type KeyType = string | number;

export const API_UNIT_OF_MEASURE_PREFIX: string = 'rpc/iwm/unit-of-measure';


export class UnitOfMeasureRepository extends Repository {
    constructor() {
        super(httpConfig);
        this.baseURL = new URL(API_UNIT_OF_MEASURE_PREFIX, BASE_API_URL).href;      
    }

    public count = (unitOfMeasureFilter?: UnitOfMeasureFilter): Observable<number> => {
        return this.http.post<number>(kebabCase(nameof(this.count)), unitOfMeasureFilter)
          .pipe(Repository.responseDataMapper<number>());
    };

    public list = (unitOfMeasureFilter?: UnitOfMeasureFilter): Observable<UnitOfMeasure[]> => {
        return this.http.post<UnitOfMeasure[]>(kebabCase(nameof(this.list)), unitOfMeasureFilter)
            .pipe(Repository.responseMapToList<UnitOfMeasure>(UnitOfMeasure));
    };

    public get = (id: number | string): Observable<UnitOfMeasure> => {
        return this.http.post<UnitOfMeasure>
            (kebabCase(nameof(this.get)), { id })
            .pipe(Repository.responseMapToModel<UnitOfMeasure>(UnitOfMeasure));
    };

    public create = (unitOfMeasure: UnitOfMeasure): Observable<UnitOfMeasure> => {
        return this.http.post<UnitOfMeasure>(kebabCase(nameof(this.create)), unitOfMeasure)
            .pipe(Repository.responseMapToModel<UnitOfMeasure>(UnitOfMeasure));
    };

    public update = (unitOfMeasure: UnitOfMeasure): Observable<UnitOfMeasure> => {
        return this.http.post<UnitOfMeasure>(kebabCase(nameof(this.update)), unitOfMeasure)
            .pipe(Repository.responseMapToModel<UnitOfMeasure>(UnitOfMeasure));
    };

    public delete = (unitOfMeasure: UnitOfMeasure): Observable<UnitOfMeasure> => {
        return this.http.post<UnitOfMeasure>(kebabCase(nameof(this.delete)), unitOfMeasure)
            .pipe(Repository.responseMapToModel<UnitOfMeasure>(UnitOfMeasure));
    };

    public bulkDelete = (idList: KeyType[]): Observable<void> => {
        return this.http.post(kebabCase(nameof(this.bulkDelete)), idList)
            .pipe(Repository.responseDataMapper());
    };

    public save = (unitOfMeasure: UnitOfMeasure): Observable<UnitOfMeasure> => {
        return unitOfMeasure.id ? this.update(unitOfMeasure) : this.create(unitOfMeasure);
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

export const unitOfMeasureRepository = new UnitOfMeasureRepository();
