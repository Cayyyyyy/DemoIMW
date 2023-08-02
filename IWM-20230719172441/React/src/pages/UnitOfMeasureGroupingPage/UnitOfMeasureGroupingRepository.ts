import { Repository } from "react3l-common";
import { kebabCase } from "lodash";
import { httpConfig } from 'core/config/http';
import { BASE_API_URL } from "core/config/consts";
import { Observable } from "rxjs";
import { AxiosResponse } from "axios";

import nameof from "ts-nameof.macro";


import { UnitOfMeasureGrouping, UnitOfMeasureGroupingFilter } from 'models/UnitOfMeasureGrouping';
import { Status, StatusFilter } from 'models/Status';
import { UnitOfMeasure, UnitOfMeasureFilter } from 'models/UnitOfMeasure';

export type KeyType = string | number;

export const API_UNIT_OF_MEASURE_GROUPING_PREFIX: string = 'rpc/iwm/unit-of-measure-grouping';


export class UnitOfMeasureGroupingRepository extends Repository {
    constructor() {
        super(httpConfig);
        this.baseURL = new URL(API_UNIT_OF_MEASURE_GROUPING_PREFIX, BASE_API_URL).href;      
    }

    public count = (unitOfMeasureGroupingFilter?: UnitOfMeasureGroupingFilter): Observable<number> => {
        return this.http.post<number>(kebabCase(nameof(this.count)), unitOfMeasureGroupingFilter)
          .pipe(Repository.responseDataMapper<number>());
    };

    public list = (unitOfMeasureGroupingFilter?: UnitOfMeasureGroupingFilter): Observable<UnitOfMeasureGrouping[]> => {
        return this.http.post<UnitOfMeasureGrouping[]>(kebabCase(nameof(this.list)), unitOfMeasureGroupingFilter)
            .pipe(Repository.responseMapToList<UnitOfMeasureGrouping>(UnitOfMeasureGrouping));
    };

    public get = (id: number | string): Observable<UnitOfMeasureGrouping> => {
        return this.http.post<UnitOfMeasureGrouping>
            (kebabCase(nameof(this.get)), { id })
            .pipe(Repository.responseMapToModel<UnitOfMeasureGrouping>(UnitOfMeasureGrouping));
    };

    public create = (unitOfMeasureGrouping: UnitOfMeasureGrouping): Observable<UnitOfMeasureGrouping> => {
        return this.http.post<UnitOfMeasureGrouping>(kebabCase(nameof(this.create)), unitOfMeasureGrouping)
            .pipe(Repository.responseMapToModel<UnitOfMeasureGrouping>(UnitOfMeasureGrouping));
    };

    public update = (unitOfMeasureGrouping: UnitOfMeasureGrouping): Observable<UnitOfMeasureGrouping> => {
        return this.http.post<UnitOfMeasureGrouping>(kebabCase(nameof(this.update)), unitOfMeasureGrouping)
            .pipe(Repository.responseMapToModel<UnitOfMeasureGrouping>(UnitOfMeasureGrouping));
    };

    public delete = (unitOfMeasureGrouping: UnitOfMeasureGrouping): Observable<UnitOfMeasureGrouping> => {
        return this.http.post<UnitOfMeasureGrouping>(kebabCase(nameof(this.delete)), unitOfMeasureGrouping)
            .pipe(Repository.responseMapToModel<UnitOfMeasureGrouping>(UnitOfMeasureGrouping));
    };

    public bulkDelete = (idList: KeyType[]): Observable<void> => {
        return this.http.post(kebabCase(nameof(this.bulkDelete)), idList)
            .pipe(Repository.responseDataMapper());
    };

    public save = (unitOfMeasureGrouping: UnitOfMeasureGrouping): Observable<UnitOfMeasureGrouping> => {
        return unitOfMeasureGrouping.id ? this.update(unitOfMeasureGrouping) : this.create(unitOfMeasureGrouping);
    };

    public singleListStatus = (): Observable<Status[]> => {
        return this.http.post<Status[]>(kebabCase(nameof(this.singleListStatus)), new StatusFilter())
            .pipe(Repository.responseMapToList<Status>(Status));
    };

    public filterListStatus = (): Observable<Status[]> => {
        return this.http.post<Status[]>(kebabCase(nameof(this.filterListStatus)), new StatusFilter())
            .pipe(Repository.responseMapToList<Status>(Status));
    };
    public singleListUnitOfMeasure = (unitOfMeasureFilter: UnitOfMeasureFilter): Observable<UnitOfMeasure[]> => {
        return this.http.post<UnitOfMeasure[]>(kebabCase(nameof(this.singleListUnitOfMeasure)), unitOfMeasureFilter)
            .pipe(Repository.responseMapToList<UnitOfMeasure>(UnitOfMeasure));
    }
    public filterListUnitOfMeasure = (unitOfMeasureFilter: UnitOfMeasureFilter): Observable<UnitOfMeasure[]> => {
        return this.http.post<UnitOfMeasure[]>(kebabCase(nameof(this.filterListUnitOfMeasure)), unitOfMeasureFilter)
            .pipe(Repository.responseMapToList<UnitOfMeasure>(UnitOfMeasure));
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

export const unitOfMeasureGroupingRepository = new UnitOfMeasureGroupingRepository();
