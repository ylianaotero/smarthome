import {HttpClient, HttpParams} from '@angular/common/http';
import { Injectable } from '@angular/core';
import {Observable} from 'rxjs';
import {patchDeviceAlias, patchDeviceRequest} from '../pages/home-owners/homes/panel/homeModels';
import {
  GetDeviceTypesResponse, PostMotionSensorRequest,
  PostSecurityCameraRequest,
  PostSmartLampRequest,
  PostWindowSensorRequest
} from '../interfaces/devices';
import {DeviceFilterRequestModel, deviceModel} from '../pages/devices/panel/model-device';

@Injectable({
  providedIn: 'root'
})
export class ApiDeviceService {

  constructor(private httpClient: HttpClient) {
  }

  url: string = 'http://localhost:5217/api/v1';

  patchDevice(data: patchDeviceRequest) {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.patch(this.url + '/homes/'+ data.HomeId.toString()+ '/devices', new patchDeviceAlias(data.HardwareId, data.Name), {headers: {'Authorization': `${storedUser}`}});
  }

  getSupportedDevices() {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.get<GetDeviceTypesResponse>(this.url + '/devices' + '/types', {headers: {'Authorization': `${storedUser}`}});
  }


  getDevices(modelIn: DeviceFilterRequestModel, page: number, pageSize: number): Observable<deviceModel[]> {
    const storedUser = localStorage.getItem('token');
    const url = this.url + '/devices';
    let params = new HttpParams();
    if (modelIn.Name) {
      params = params.set('Name', modelIn.Name);
    }
    if (modelIn.Model) {
      params = params.set('Model', modelIn.Model);
    }

    if (modelIn.Company) {
      params = params.set('Company', modelIn.Company);
    }
    if (modelIn.Kind) {
      params = params.set('Kind', modelIn.Kind);
    }

    params = params.set('Page', page.toString());
    params = params.set('PageSize', pageSize.toString());


    return this.httpClient.get<deviceModel[]>(url, {
      params,
      headers: { 'Authorization': `${storedUser}` }
    });

  }

  postWindowSensor(request: PostWindowSensorRequest): Observable<any> {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.post(this.url + '/devices/window-sensors', {
      name: request.name,
      model: request.model,
      PhotoUrls: request.photoUrls,
      description: request.description,
      functionalities: request.functionalities,
      company: request.company
    }, {
      headers: { 'Authorization': `${storedUser}` }
    });
  }

  postSmartLamp(request: PostSmartLampRequest): Observable<any> {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.post(this.url + '/devices/smart-lamps', {
      name: request.name,
      model: request.model,
      PhotoUrls: request.photoUrls,
      description: request.description,
      functionalities: request.functionalities,
      company: request.company
    }, {
      headers: { 'Authorization': `${storedUser}` }
    });
  }

  postSecurityCamera(request: PostSecurityCameraRequest): Observable<any> {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.post(this.url + '/devices/security-cameras', {
      name: request.name,
      Model: request.Model,
      PhotoUrls: request.PhotoUrls,
      Description: request.Description,
      Functionalities: request.Functionalities,
      LocationType: request.LocationType,
      Company: request.Company
    }, {
      headers: { 'Authorization': `${storedUser}` }
    });
  }

  postMotionSensor(request: PostMotionSensorRequest): Observable<any> {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.post(this.url + '/devices/motion-sensors', {
      name: request.name,
      model: request.model,
      PhotoUrls: request.photoUrls,
      description: request.description,
      company: request.company,
      functionalities: request.functionalities
    }, {
      headers: { 'Authorization': `${storedUser}` }
    });
  }





}
