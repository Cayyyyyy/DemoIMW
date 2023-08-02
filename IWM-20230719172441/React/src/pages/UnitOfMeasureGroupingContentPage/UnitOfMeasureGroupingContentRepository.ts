import { Repository } from "react3l-common";
import { kebabCase } from "lodash";
import { httpConfig } from 'core/config/http';
import { BASE_API_URL } from "core/config/consts";
import { Observable } from "rxjs";
import { AxiosResponse } from "axios";

import nameof from "ts-nameof.macro";


import { UnitOfMeasureGroupingContent, UnitOfMeasureGroupingContentFilter } from 'models/UnitOfMeasureGroupingContent';
import { UnitOfMeasure, UnitOfMeasureFilter } from 'models/UnitOfMeasure';
import { UnitOfMeasureGrouping, UnitOfMeasureGroupingFilter } from 'models/UnitOfMeasureGrouping';

export type KeyType = string | number;

export const API_UNIT_OF_MEASURE_GROUPING_CONTENT_PREFIX: string = 'rpc/iwm/unit-of-measure-grouping-content';


export class UnitOfMeasureGroupingContentRepository extends Repository {
    constructor() {
        super(httpConfig);
        this.baseURL = new URL(API_UNIT_OF_MEASURE_GROUPING_CONTENT_PREFIX, BASE_API_URL).href;      
    }

    public count = (unitOfMeasureGroupingContentFilter?: UnitOfMeasureGroupingContentFilter): Observable<number> => {
        return this.http.post<number>(kebabCase(nameof(this.count)), unitOfMeasureGroupingContentFilter)
          .pipe(Repository.responseDataMapper<number>());
    };

    public list = (unitOfMeasureGroupingContentFilter?: UnitOfMeasureGroupingContentFilter): Observable<UnitOfMeasureGroupingContent[]> => {
        return this.http.post<UnitOfMeasureGroupingContent[]>(kebabCase(nameof(this.list)), unitOfMeasureGroupingContentFilter)
            .pipe(Repository.responseMapToList<UnitOfMeasureGroupingContent>(UnitOfMeasureGroupingContent));
    };

    public get = (id: number | string): Observable<UnitOfMeasureGroupingContent> => {
        return this.http.post<UnitOfMeasureGroupingContent>
            (kebabCase(nameof(this.get)), { id })
            .pipe(Repository.responseMapToModel<UnitOfMeasureGroupingContent>(UnitOfMeasureGroupingContent));
    };

    public create = (unitOfMeasureGroupingContent: UnitOfMeasureGroupingContent): Observable<UnitOfMeasureGroupingContent> => {
        return this.http.post<UnitOfMeasureGroupingContent>(kebabCase(nameof(this.create)), unitOfMeasureGroupingContent)
            .pipe(Repository.responseMapToModel<UnitOfMeasureGroupingContent>(UnitOfMeasureGroupingContent));
    };

    public update = (unitOfMeasureGroupingContent: UnitOfMeasureGroupingContent): Observable<UnitOfMeasureGroupingContent> => {
        return this.http.post<UnitOfMeasureGroupingContent>(kebabCase(nameof(this.update)), unitOfMeasureGroupingContent)
            .pipe(Repository.responseMapToModel<UnitOfMeasureGroupingContent>(UnitOfMeasureGroupingContent));
    };

    public delete = (unitOfMeasureGroupingContent: UnitOfMeasureGroupingContent): Observable<UnitOfMeasureGroupingContent> => {
        return this.http.post<UnitOfMeasureGroupingContent>(kebabCase(nameof(this.delete)), unitOfMeasureGroupingContent)
            .pipe(Repository.responseMapToModel<UnitOfMeasureGroupingContent>(UnitOfMeasureGroupingContent));
    };

    public bulkDelete = (idList: KeyType[]): Observable<void> => {
        return this.http.post(kebabCase(nameof(this.bulkDelete)), idList)
            .pipe(Repository.responseDataMapper());
    };

    public save = (unitOfMeasureGroupingContent: UnitOfMeasureGroupingContent): Observable<UnitOfMeasureGroupingContent> => {
        return unitOfMeasureGroupingContent.id ? this.update(unitOfMeasureGroupingContent) : this.create(unitOfMeasureGroupingContent);
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

export const unitOfMeasureGroupingContentRepository = new UnitOfMeasureGroupingContentRepository();
