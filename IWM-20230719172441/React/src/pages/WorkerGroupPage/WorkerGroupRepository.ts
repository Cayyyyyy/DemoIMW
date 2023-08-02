import { Repository } from "react3l-common";
import { kebabCase } from "lodash";
import { httpConfig } from 'core/config/http';
import { BASE_API_URL } from "core/config/consts";
import { Observable } from "rxjs";
import { AxiosResponse } from "axios";

import nameof from "ts-nameof.macro";


import { WorkerGroup, WorkerGroupFilter } from 'models/WorkerGroup';
import { Status, StatusFilter } from 'models/Status';

export type KeyType = string | number;

export const API_WORKER_GROUP_PREFIX: string = 'rpc/iwm/worker-group';


export class WorkerGroupRepository extends Repository {
    constructor() {
        super(httpConfig);
        this.baseURL = new URL(API_WORKER_GROUP_PREFIX, BASE_API_URL).href;      
    }

    public count = (workerGroupFilter?: WorkerGroupFilter): Observable<number> => {
        return this.http.post<number>(kebabCase(nameof(this.count)), workerGroupFilter)
          .pipe(Repository.responseDataMapper<number>());
    };

    public list = (workerGroupFilter?: WorkerGroupFilter): Observable<WorkerGroup[]> => {
        return this.http.post<WorkerGroup[]>(kebabCase(nameof(this.list)), workerGroupFilter)
            .pipe(Repository.responseMapToList<WorkerGroup>(WorkerGroup));
    };

    public get = (id: number | string): Observable<WorkerGroup> => {
        return this.http.post<WorkerGroup>
            (kebabCase(nameof(this.get)), { id })
            .pipe(Repository.responseMapToModel<WorkerGroup>(WorkerGroup));
    };

    public create = (workerGroup: WorkerGroup): Observable<WorkerGroup> => {
        return this.http.post<WorkerGroup>(kebabCase(nameof(this.create)), workerGroup)
            .pipe(Repository.responseMapToModel<WorkerGroup>(WorkerGroup));
    };

    public update = (workerGroup: WorkerGroup): Observable<WorkerGroup> => {
        return this.http.post<WorkerGroup>(kebabCase(nameof(this.update)), workerGroup)
            .pipe(Repository.responseMapToModel<WorkerGroup>(WorkerGroup));
    };

    public delete = (workerGroup: WorkerGroup): Observable<WorkerGroup> => {
        return this.http.post<WorkerGroup>(kebabCase(nameof(this.delete)), workerGroup)
            .pipe(Repository.responseMapToModel<WorkerGroup>(WorkerGroup));
    };

    public bulkDelete = (idList: KeyType[]): Observable<void> => {
        return this.http.post(kebabCase(nameof(this.bulkDelete)), idList)
            .pipe(Repository.responseDataMapper());
    };

    public save = (workerGroup: WorkerGroup): Observable<WorkerGroup> => {
        return workerGroup.id ? this.update(workerGroup) : this.create(workerGroup);
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

export const workerGroupRepository = new WorkerGroupRepository();
