import { Repository } from "react3l-common";
import { kebabCase } from "lodash";
import { httpConfig } from 'core/config/http';
import { BASE_API_URL } from "core/config/consts";
import { Observable } from "rxjs";
import { AxiosResponse } from "axios";

import nameof from "ts-nameof.macro";


import { Image, ImageFilter } from 'models/Image';

export type KeyType = string | number;

export const API_IMAGE_PREFIX: string = 'rpc/iwm/image';


export class ImageRepository extends Repository {
    constructor() {
        super(httpConfig);
        this.baseURL = new URL(API_IMAGE_PREFIX, BASE_API_URL).href;      
    }

    public count = (imageFilter?: ImageFilter): Observable<number> => {
        return this.http.post<number>(kebabCase(nameof(this.count)), imageFilter)
          .pipe(Repository.responseDataMapper<number>());
    };

    public list = (imageFilter?: ImageFilter): Observable<Image[]> => {
        return this.http.post<Image[]>(kebabCase(nameof(this.list)), imageFilter)
            .pipe(Repository.responseMapToList<Image>(Image));
    };

    public get = (id: number | string): Observable<Image> => {
        return this.http.post<Image>
            (kebabCase(nameof(this.get)), { id })
            .pipe(Repository.responseMapToModel<Image>(Image));
    };

    public create = (image: Image): Observable<Image> => {
        return this.http.post<Image>(kebabCase(nameof(this.create)), image)
            .pipe(Repository.responseMapToModel<Image>(Image));
    };

    public update = (image: Image): Observable<Image> => {
        return this.http.post<Image>(kebabCase(nameof(this.update)), image)
            .pipe(Repository.responseMapToModel<Image>(Image));
    };

    public delete = (image: Image): Observable<Image> => {
        return this.http.post<Image>(kebabCase(nameof(this.delete)), image)
            .pipe(Repository.responseMapToModel<Image>(Image));
    };

    public bulkDelete = (idList: KeyType[]): Observable<void> => {
        return this.http.post(kebabCase(nameof(this.bulkDelete)), idList)
            .pipe(Repository.responseDataMapper());
    };

    public save = (image: Image): Observable<Image> => {
        return image.id ? this.update(image) : this.create(image);
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

export const imageRepository = new ImageRepository();
