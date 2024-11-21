import {HttpClient, HttpParams} from '@angular/common/http';
import { Injectable } from '@angular/core';
import {createHomeModel, homeRetrieveModel} from '../pages/home-owners/homes/create/createHomeModel';
import {userRetrieveModel} from '../pages/home-owners/create/signUpUserModel';
import {
  addDeviceRequest, addDeviceToHomeListRequest, addDeviceToHomeRequest,
  addMemberRequest,
  addMemberToHomeRequest,
  addRoomRequest, AddRoomToHomeRequest, ChangeMemberNotificationsRequest, ChangeMemberRequest, deviceUnit,
  home, member
} from '../pages/home-owners/homes/panel/homeModels';
import {GetRoomResponse} from '../interfaces/devices';
import {environment} from '../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class ApiHomeService {

  url: string;

  constructor(private httpClient: HttpClient) {
    this.url = environment.apiUrl;
  }

  postHome(data: createHomeModel) {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.post<homeRetrieveModel>(this.url + '/homes', data, {headers: {'Authorization': `${storedUser}`}});
  }

  getHomesOfHomeOwner() {
    const storedUserToken = localStorage.getItem('token');
    const storedUser = localStorage.getItem('user');
    let res: userRetrieveModel | null = null;
    if(storedUser){
      res = JSON.parse(storedUser) as userRetrieveModel;
    }
    return this.httpClient.get<home[]>(this.url + '/homes' + '?HomeOwnerId=' + res?.id, {headers: {'Authorization': `${storedUserToken}`}});
  }

  postMemberToHome(data: addMemberRequest) {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.post(this.url + '/homes/' + data.id.toString() + '/members', new addMemberToHomeRequest(data.email, data.hasPermissionToListDevices, data.hasPermissionToAddDevice, data.recivesNotifications), {headers: {'Authorization': `${storedUser}`}});
  }

  postRoomToHome(data: addRoomRequest) {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.post(this.url + '/homes/' + data.id.toString() + '/rooms', new AddRoomToHomeRequest(data.name), {headers: {'Authorization': `${storedUser}`}});
  }

  postDeviceToHome(data: addDeviceRequest) {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.post(this.url + '/homes/' + data.id.toString() + '/devices', new addDeviceToHomeListRequest(new addDeviceToHomeRequest(data.deviceId, data.isConnected, data.roomName)), {headers: {'Authorization': `${storedUser}`}});
  }

  getMembersOfHome(data: number) {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.get<member[]>(this.url + '/homes/' + data.toString() + '/members', {headers: {'Authorization': `${storedUser}`}});
  }

  changeMemberNotifications(data: ChangeMemberNotificationsRequest) {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.patch(this.url + '/homes/' + data.IdHome.toString() + '/members', new ChangeMemberRequest(data.MemberEmail, data.ReceivesNotifications), {headers: {'Authorization': `${storedUser}`}});
  }


  getRooms(data : string) {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.get<GetRoomResponse>(this.url + '/homes/' + data + '/rooms', {headers: {'Authorization': `${storedUser}`}});
  }

  getDevicesOfHome(data: number, filter : string) {
    const storedUser = localStorage.getItem('token');
    let params = new HttpParams();

    if (filter) {
      params = params.set('RoomName', filter);
    }

    return this.httpClient.get<deviceUnit[]>(this.url + '/homes/' + data.toString() + '/devices', {
      params,
      headers: { 'Authorization': `${storedUser}` }
    });
  }





}
