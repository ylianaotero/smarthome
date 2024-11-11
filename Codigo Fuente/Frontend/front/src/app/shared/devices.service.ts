import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import {GetDeviceRequest, GetDevicesResponse, GetDeviceTypesResponse } from '../interfaces/devices';


@Injectable({
    providedIn: 'root'
})
export class DevicesService {

  constructor(private httpClient: HttpClient) {
  }

  url: string = 'http://localhost:5217/api/v1';

  getDevices(request: GetDeviceRequest, currentPage?: number, pageSize?: number): Observable<GetDevicesResponse> {
    let params = new HttpParams();
    params = request.name ? params.append('name', request.name ) : params;
    params = request.model ? params.append('model', request.model ) : params;
    params = request.company ? params.append('company', request.company ) : params;
    params = request.kind ? params.append('kind', request.kind ) : params;
    params = pageSize ? params.append('currentPage', currentPage!.toString()) : params;
    params = currentPage ? params.append('pageSize', pageSize!.toString()) : params;

    return this.httpClient.get<GetDevicesResponse>(this.url + '/devices', {params});
  }

  getDeviceTypes(): Observable<GetDeviceTypesResponse> {
    return this.httpClient.get<GetDeviceTypesResponse>(this.url + `/devices/types`);
  }

}