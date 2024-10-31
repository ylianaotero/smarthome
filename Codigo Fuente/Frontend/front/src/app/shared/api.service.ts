import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { sessionModel, sessionRequest } from '../logIn/sessionModel';
import {userRegistrationModel, userRegistrationInstance, userRetrieveModel} from '../signUpHomeOwner/signUpUserModel';
import {
  deviceUnit,
  home,
  addMemberRequest,
  addMemberToHomeRequest,
  member,
  addDeviceToHomeRequest,
  addDeviceRequest,
  addDeviceToHomeListRequest, ChangeMemberNotificationsRequest, ChangeMemberRequest
} from '../homesOfHomeOwner/homeModels';
import {createHomeModel, homeRetrieveModel} from '../createHome/createHomeModel';
import {Observable, tap} from 'rxjs';
import {DeviceFilterRequestModel, deviceModel} from '../account/deviceModels';
import {ImportDevicesRequest, importer} from '../import/importerModels';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private httpClient: HttpClient) {
    this.updateSession();
  }

  url: string = 'http://localhost:5217/api/v1';

  currentSession: sessionModel | undefined = undefined;

  updateSession() {
    if (!this.currentSession) {
      const userSession = localStorage.getItem('user');
      if (userSession) {
        this.currentSession = JSON.parse(userSession) as sessionModel;
      }
    }
  }


  postSession(data: sessionRequest) {
    return this.httpClient.post<sessionModel>(this.url + '/login', data).pipe(
      tap((session: sessionModel) => {
        this.currentSession = session;
        localStorage.setItem('user', JSON.stringify(session));
      })
    );
  }

  postHomeOwner(data: userRegistrationInstance) {
    return this.httpClient.post<userRetrieveModel>(this.url + '/home-owners', data);
  }

  postHome(data: createHomeModel) {
    return this.httpClient.post<homeRetrieveModel>(this.url + '/homes', data, {headers: {'Authorization': `${this.currentSession?.token}`}});
  }

  getHomesOfHomeOwner() {
    return this.httpClient.get<home[]>(this.url + '/homes' + '?HomeOwnerId=' + this.currentSession?.user.id.toString(), {headers: {'Authorization': `${this.currentSession?.token}`}});
  }

  postMemberToHome(data: addMemberRequest) {
    return this.httpClient.post(this.url + '/homes/' + data.id.toString() + '/members', new addMemberToHomeRequest(data.email, data.hasPermissionToListDevices, data.hasPermissionToAddDevice, data.recivesNotifications), {headers: {'Authorization': `${this.currentSession?.token}`}});
  }

  postDeviceToHome(data: addDeviceRequest) {
    return this.httpClient.post(this.url + '/homes/' + data.id.toString() + '/devices', new addDeviceToHomeListRequest(new addDeviceToHomeRequest(data.deviceId, data.isConnected)), {headers: {'Authorization': `${this.currentSession?.token}`}});
  }

  getMembersOfHome(data: number) {
    return this.httpClient.get<member[]>(this.url + '/homes/' + data.toString() + '/members', {headers: {'Authorization': `${this.currentSession?.token}`}});
  }

  getDevicesOfHome(data: number) {
    return this.httpClient.get<deviceUnit[]>(this.url + '/homes/' + data.toString() + '/devices', {headers: {'Authorization': `${this.currentSession?.token}`}});
  }

  changeMemberNotifications(data: ChangeMemberNotificationsRequest) {
    return this.httpClient.patch(this.url + '/homes/' + data.IdHome.toString() + '/members', new ChangeMemberRequest(data.MemberEmail, data.ReceivesNotifications), {headers: {'Authorization': `${this.currentSession?.token}`}});
  }

  getSupportedDevices() {
    return this.httpClient.get(this.url + '/devices' + '/types', {headers: {'Authorization': `${this.currentSession?.token}`}});
  }

  getImporters(directoryPath: string ) {
    return this.httpClient.get<importer[]>(this.url + '/imports?dllPath=' + directoryPath, {headers: {'Authorization': `${this.currentSession?.token}`}});
  }

  importDevices(data: ImportDevicesRequest) {
    return this.httpClient.post(this.url + '/imports', data ,{headers: {'Authorization': `${this.currentSession?.token}`}});
  }

  getDevices(modelIn: DeviceFilterRequestModel, page: number, pageSize: number): Observable<deviceModel[]> {
    const url = this.url + '/devices';
    let params = new HttpParams();
    if (modelIn.Name) {
      params = params.set('Name', modelIn.Name);
    }
    if (modelIn.Model !== undefined && modelIn.Model !== null && !isNaN(modelIn.Model)) {
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
      headers: { 'Authorization': `${this.currentSession?.token}` }
    });
  }
}
