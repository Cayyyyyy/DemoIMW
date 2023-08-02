import { Repository } from "react3l-common";
import { kebabCase } from "lodash";
import { httpConfig } from 'core/config/http';
import { BASE_API_URL } from "core/config/consts";
import { Observable } from "rxjs";
import { AxiosResponse } from "axios";

import nameof from "ts-nameof.macro";


import { AppUser, AppUserFilter } from 'models/AppUser';
import { Organization, OrganizationFilter } from 'models/Organization';
import { Sex, SexFilter } from 'models/Sex';
import { Status, StatusFilter } from 'models/Status';

export type KeyType = string | number;

export const API_APP_USER_PREFIX: string = 'rpc/iwm/app-user';


export class AppUserRepository extends Repository {
    constructor() {
        super(httpConfig);
        this.baseURL = new URL(API_APP_USER_PREFIX, BASE_API_URL).href;      
    }

    public count = (appUserFilter?: AppUserFilter): Observable<number> => {
        return this.http.post<number>(kebabCase(nameof(this.count)), appUserFilter)
          .pipe(Repository.responseDataMapper<number>());
    };

    public list = (appUserFilter?: AppUserFilter): Observable<AppUser[]> => {
        return this.http.post<AppUser[]>(kebabCase(nameof(this.list)), appUserFilter)
            .pipe(Repository.responseMapToList<AppUser>(AppUser));
    };

    public get = (id: number | string): Observable<AppUser> => {
        return this.http.post<AppUser>
            (kebabCase(nameof(this.get)), { id })
            .pipe(Repository.responseMapToModel<AppUser>(AppUser));
    };

    public create = (appUser: AppUser): Observable<AppUser> => {
        return this.http.post<AppUser>(kebabCase(nameof(this.create)), appUser)
            .pipe(Repository.responseMapToModel<AppUser>(AppUser));
    };

    public update = (appUser: AppUser): Observable<AppUser> => {
        return this.http.post<AppUser>(kebabCase(nameof(this.update)), appUser)
            .pipe(Repository.responseMapToModel<AppUser>(AppUser));
    };

    public delete = (appUser: AppUser): Observable<AppUser> => {
        return this.http.post<AppUser>(kebabCase(nameof(this.delete)), appUser)
            .pipe(Repository.responseMapToModel<AppUser>(AppUser));
    };

    public bulkDelete = (idList: KeyType[]): Observable<void> => {
        return this.http.post(kebabCase(nameof(this.bulkDelete)), idList)
            .pipe(Repository.responseDataMapper());
    };

    public save = (appUser: AppUser): Observable<AppUser> => {
        return appUser.id ? this.update(appUser) : this.create(appUser);
    };

    public singleListOrganization = (organizationFilter: OrganizationFilter): Observable<Organization[]> => {
        return this.http.post<Organization[]>(kebabCase(nameof(this.singleListOrganization)), organizationFilter)
            .pipe(Repository.responseMapToList<Organization>(Organization));
    }
    public filterListOrganization = (organizationFilter: OrganizationFilter): Observable<Organization[]> => {
        return this.http.post<Organization[]>(kebabCase(nameof(this.filterListOrganization)), organizationFilter)
            .pipe(Repository.responseMapToList<Organization>(Organization));
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

export const appUserRepository = new AppUserRepository();
