import { Repository } from "react3l-common";
import { kebabCase } from "lodash";
import { httpConfig } from 'core/config/http';
import { BASE_API_URL } from "core/config/consts";
import { Observable } from "rxjs";
import { AxiosResponse } from "axios";

import nameof from "ts-nameof.macro";


import { Worker, WorkerFilter } from 'models/Worker';
import { District, DistrictFilter } from 'models/District';
import { Nation, NationFilter } from 'models/Nation';
import { Province, ProvinceFilter } from 'models/Province';
import { Sex, SexFilter } from 'models/Sex';
import { Status, StatusFilter } from 'models/Status';
import { Ward, WardFilter } from 'models/Ward';
import { WorkerGroup, WorkerGroupFilter } from 'models/WorkerGroup';

export type KeyType = string | number;

export const API_WORKER_PREFIX: string = 'rpc/iwm/worker';


export class WorkerRepository extends Repository {
    constructor() {
        super(httpConfig);
        this.baseURL = new URL(API_WORKER_PREFIX, BASE_API_URL).href;      
    }

    public count = (workerFilter?: WorkerFilter): Observable<number> => {
        return this.http.post<number>(kebabCase(nameof(this.count)), workerFilter)
          .pipe(Repository.responseDataMapper<number>());
    };

    public list = (workerFilter?: WorkerFilter): Observable<Worker[]> => {
        return this.http.post<Worker[]>(kebabCase(nameof(this.list)), workerFilter)
            .pipe(Repository.responseMapToList<Worker>(Worker));
    };

    public get = (id: number | string): Observable<Worker> => {
        return this.http.post<Worker>
            (kebabCase(nameof(this.get)), { id })
            .pipe(Repository.responseMapToModel<Worker>(Worker));
    };

    public create = (worker: Worker): Observable<Worker> => {
        return this.http.post<Worker>(kebabCase(nameof(this.create)), worker)
            .pipe(Repository.responseMapToModel<Worker>(Worker));
    };

    public update = (worker: Worker): Observable<Worker> => {
        return this.http.post<Worker>(kebabCase(nameof(this.update)), worker)
            .pipe(Repository.responseMapToModel<Worker>(Worker));
    };

    public delete = (worker: Worker): Observable<Worker> => {
        return this.http.post<Worker>(kebabCase(nameof(this.delete)), worker)
            .pipe(Repository.responseMapToModel<Worker>(Worker));
    };

    public bulkDelete = (idList: KeyType[]): Observable<void> => {
        return this.http.post(kebabCase(nameof(this.bulkDelete)), idList)
            .pipe(Repository.responseDataMapper());
    };

    public save = (worker: Worker): Observable<Worker> => {
        return worker.id ? this.update(worker) : this.create(worker);
    };

    public singleListDistrict = (districtFilter: DistrictFilter): Observable<District[]> => {
        return this.http.post<District[]>(kebabCase(nameof(this.singleListDistrict)), districtFilter)
            .pipe(Repository.responseMapToList<District>(District));
    }
    public filterListDistrict = (districtFilter: DistrictFilter): Observable<District[]> => {
        return this.http.post<District[]>(kebabCase(nameof(this.filterListDistrict)), districtFilter)
            .pipe(Repository.responseMapToList<District>(District));
    };
    public singleListNation = (nationFilter: NationFilter): Observable<Nation[]> => {
        return this.http.post<Nation[]>(kebabCase(nameof(this.singleListNation)), nationFilter)
            .pipe(Repository.responseMapToList<Nation>(Nation));
    }
    public filterListNation = (nationFilter: NationFilter): Observable<Nation[]> => {
        return this.http.post<Nation[]>(kebabCase(nameof(this.filterListNation)), nationFilter)
            .pipe(Repository.responseMapToList<Nation>(Nation));
    };
    public singleListProvince = (provinceFilter: ProvinceFilter): Observable<Province[]> => {
        return this.http.post<Province[]>(kebabCase(nameof(this.singleListProvince)), provinceFilter)
            .pipe(Repository.responseMapToList<Province>(Province));
    }
    public filterListProvince = (provinceFilter: ProvinceFilter): Observable<Province[]> => {
        return this.http.post<Province[]>(kebabCase(nameof(this.filterListProvince)), provinceFilter)
            .pipe(Repository.responseMapToList<Province>(Province));
    };
    public singleListSex = (): Observable<Sex[]> => {
        return this.http.post<Sex[]>(kebabCase(nameof(this.singleListSex)), new SexFilter())
            .pipe(Repository.responseMapToList<Sex>(Sex));
    };

    public filterListSex = (): Observable<Sex[]> => {
        return this.http.post<Sex[]>(kebabCase(nameof(this.filterListSex)), new SexFilter())
            .pipe(Repository.responseMapToList<Sex>(Sex));
    };
    public singleListStatus = (): Observable<Status[]> => {
        return this.http.post<Status[]>(kebabCase(nameof(this.singleListStatus)), new StatusFilter())
            .pipe(Repository.responseMapToList<Status>(Status));
    };

    public filterListStatus = (): Observable<Status[]> => {
        return this.http.post<Status[]>(kebabCase(nameof(this.filterListStatus)), new StatusFilter())
            .pipe(Repository.responseMapToList<Status>(Status));
    };
    public singleListWard = (wardFilter: WardFilter): Observable<Ward[]> => {
        return this.http.post<Ward[]>(kebabCase(nameof(this.singleListWard)), wardFilter)
            .pipe(Repository.responseMapToList<Ward>(Ward));
    }
    public filterListWard = (wardFilter: WardFilter): Observable<Ward[]> => {
        return this.http.post<Ward[]>(kebabCase(nameof(this.filterListWard)), wardFilter)
            .pipe(Repository.responseMapToList<Ward>(Ward));
    };
    public singleListWorkerGroup = (workerGroupFilter: WorkerGroupFilter): Observable<WorkerGroup[]> => {
        return this.http.post<WorkerGroup[]>(kebabCase(nameof(this.singleListWorkerGroup)), workerGroupFilter)
            .pipe(Repository.responseMapToList<WorkerGroup>(WorkerGroup));
    }
    public filterListWorkerGroup = (workerGroupFilter: WorkerGroupFilter): Observable<WorkerGroup[]> => {
        return this.http.post<WorkerGroup[]>(kebabCase(nameof(this.filterListWorkerGroup)), workerGroupFilter)
            .pipe(Repository.responseMapToList<WorkerGroup>(WorkerGroup));
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

export const workerRepository = new WorkerRepository();
