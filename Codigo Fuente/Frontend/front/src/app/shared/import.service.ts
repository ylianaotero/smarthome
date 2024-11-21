import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import { Injectable } from '@angular/core';
import {ImportDevicesRequest, importer, ImporterPath} from '../pages/company-owners/import-device/importerModels';

@Injectable({
  providedIn: 'root'
})
export class ApiImportService {

  constructor(private httpClient: HttpClient) {
  }

  url: string = 'http://localhost:5217/api/v1';

  getImporters() {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.get<importer[]>(this.url + '/imports', {headers: {'Authorization': `${storedUser}`}});
  }

  postImporter(data : ImporterPath) {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.post(this.url + '/imports/new', data ,{headers: {'Authorization': `${storedUser}`}});
  }

  importDevices(data: ImportDevicesRequest) {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.post(this.url + '/imports', data ,{headers: {'Authorization': `${storedUser}`}});
  }

}
