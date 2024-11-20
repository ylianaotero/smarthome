import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { sessionModel, sessionRequest } from '../pages/login/panel/sessionModel';
import {userRegistrationModel, userRegistrationInstance, userRetrieveModel} from '../pages/home-owners/create/signUpUserModel';
import {
  deviceUnit,
  home,
  addMemberRequest,
  addMemberToHomeRequest,
  member,
  addDeviceToHomeRequest,
  addDeviceRequest,
  addDeviceToHomeListRequest,
  ChangeMemberNotificationsRequest,
  ChangeMemberRequest,
  patchDeviceRequest,
  patchDeviceAlias, addRoomRequest, AddRoomToHomeRequest
} from '../pages/home-owners/homes/panel/homeModels';
import {createHomeModel, homeRetrieveModel} from '../pages/home-owners/homes/create/createHomeModel';
import {Observable, tap} from 'rxjs';
import {DeviceFilterRequestModel, deviceModel} from '../pages/devices/panel/model-device';
import {ImportDevicesRequest, importer, ImporterPath} from '../pages/company-owners/import-device/importerModels';
import {
  GetDeviceTypesResponse, GetRoomResponse, PostMotionSensorRequest,
  PostSecurityCameraRequest,
  PostSmartLampRequest,
  PostWindowSensorRequest
} from '../interfaces/devices';
import {
  createAdministratorModel,
  createCompanyOwnerModel, GetUsersRequest, GetUsersResponse,
  PostAdministratorRequest,
  PostCompanyOwnerRequest
} from '../interfaces/users';
import {
  CreateCompanyRequest,
  GetCompaniesResponse,
  GetCompanyRequest,
  PostCompaniesResponse
} from '../interfaces/companies';

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

  patchDevice(data: patchDeviceRequest) {
    return this.httpClient.patch(this.url + '/homes/'+ data.HomeId.toString()+ '/devices', new patchDeviceAlias(data.HardwareId, data.Name), {headers: {'Authorization': `${this.currentSession?.token}`}});
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

  postRoomToHome(data: addRoomRequest) {
    return this.httpClient.post(this.url + '/homes/' + data.id.toString() + '/rooms', new AddRoomToHomeRequest(data.name), {headers: {'Authorization': `${this.currentSession?.token}`}});
  }

  postDeviceToHome(data: addDeviceRequest) {
    return this.httpClient.post(this.url + '/homes/' + data.id.toString() + '/devices', new addDeviceToHomeListRequest(new addDeviceToHomeRequest(data.deviceId, data.isConnected, data.roomName)), {headers: {'Authorization': `${this.currentSession?.token}`}});
  }

  getMembersOfHome(data: number) {
    return this.httpClient.get<member[]>(this.url + '/homes/' + data.toString() + '/members', {headers: {'Authorization': `${this.currentSession?.token}`}});
  }


  changeMemberNotifications(data: ChangeMemberNotificationsRequest) {
    return this.httpClient.patch(this.url + '/homes/' + data.IdHome.toString() + '/members', new ChangeMemberRequest(data.MemberEmail, data.ReceivesNotifications), {headers: {'Authorization': `${this.currentSession?.token}`}});
  }

  getSupportedDevices() {
    return this.httpClient.get<GetDeviceTypesResponse>(this.url + '/devices' + '/types', {headers: {'Authorization': `${this.currentSession?.token}`}});
  }

  getRooms(data : string) {
    return this.httpClient.get<GetRoomResponse>(this.url + '/homes/' + data + '/rooms', {headers: {'Authorization': `${this.currentSession?.token}`}});
  }


  getImporters() {
    return this.httpClient.get<importer[]>(this.url + '/imports', {headers: {'Authorization': `${this.currentSession?.token}`}});
  }

  postImporter(data : ImporterPath) {
    return this.httpClient.post(this.url + '/imports/new', data ,{headers: {'Authorization': `${this.currentSession?.token}`}});
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
      headers: { 'Authorization': `${this.currentSession?.token}` }
    });
  }

  getDevicesOfHome(data: number, filter : string) {
    let params = new HttpParams();

    if (filter) {
      params = params.set('RoomName', filter);
    }

    return this.httpClient.get<deviceUnit[]>(this.url + '/homes/' + data.toString() + '/devices', {
      params,
      headers: { 'Authorization': `${this.currentSession?.token}` }
    });
  }

  postWindowSensor(request: PostWindowSensorRequest): Observable<any> {
    return this.httpClient.post(this.url + '/devices/window-sensors', {
      name: request.name,
      model: request.model,
      PhotoUrls: request.photoUrls,
      description: request.description,
      functionalities: request.functionalities,
      company: request.company
    }, {
      headers: { 'Authorization': `${this.currentSession?.token}` }
    });
  }

  postSmartLamp(request: PostSmartLampRequest): Observable<any> {
    return this.httpClient.post(this.url + '/devices/smart-lamps', {
      name: request.name,
      model: request.model,
      PhotoUrls: request.photoUrls,
      description: request.description,
      functionalities: request.functionalities,
      company: request.company
    }, {
      headers: { 'Authorization': `${this.currentSession?.token}` }
    });
  }

  postSecurityCamera(request: PostSecurityCameraRequest): Observable<any> {
    return this.httpClient.post(this.url + '/devices/security-cameras', {
      name: request.name,
      Model: request.Model,
      PhotoUrls: request.PhotoUrls,
      Description: request.Description,
      Functionalities: request.Functionalities,
      LocationType: request.LocationType,
      Company: request.Company
    }, {
      headers: { 'Authorization': `${this.currentSession?.token}` }
    });
  }

  postMotionSensor(request: PostMotionSensorRequest): Observable<any> {
    return this.httpClient.post(this.url + '/devices/motion-sensors', {
      name: request.name,
      model: request.model,
      PhotoUrls: request.photoUrls,
      description: request.description,
      company: request.company,
      functionalities: request.functionalities
    }, {
      headers: { 'Authorization': `${this.currentSession?.token}` }
    });
  }

  postAdministrator(data: createAdministratorModel) {
    return this.httpClient.post<PostAdministratorRequest>(this.url + '/administrators', data, {headers: {'Authorization': `${this.currentSession?.token}`}});
  };

  postCompanyOwner(data: createCompanyOwnerModel) {
    return this.httpClient.post<PostCompanyOwnerRequest>(this.url + '/company-owners', data, {headers: {'Authorization': `${this.currentSession?.token}`}});
  }

  getUsers(request: GetUsersRequest, currentPage?: number, pageSize?: number): Observable<GetUsersResponse> {
    let params = new HttpParams();

    params = request.fullName ? params.append('fullName', request.fullName ) : params;
    params = request.role ? params.append('role', request.role ) : params;

    params = pageSize ? params.append('Page', currentPage!.toString()) : params;
    params = currentPage ? params.append('PageSize', pageSize!.toString()) : params;
    return this.httpClient.get<GetUsersResponse>(this.url + '/users', {params , headers: {'Authorization': `${this.currentSession?.token}`}});
  }

  deleteAdministrator(id: number) {
    return this.httpClient.delete(this.url + `/administrators/${id}`, {headers: {'Authorization': `${this.currentSession?.token}`}});
  }

  getCompanies(request:  GetCompanyRequest, currentPage?: number, pageSize?: number):
    Observable<GetCompaniesResponse> {
    let params = new HttpParams();

    if(request.userEmail){
      params = request ? params.append('OwnerEmail', request.userEmail) : params;
    }

    if(request.name){
      params = request ? params.append('Name', request.name) : params;
    }

    if(request.fullName){
      params = request ? params.append('Owner', request.fullName) : params;
    }

    params = pageSize ? params.append('Page', currentPage!.toString()) : params;
    params = currentPage ? params.append('PageSize', pageSize!.toString()) : params;
    return this.httpClient.get<GetCompaniesResponse>(this.url + '/companies',
      {params, headers: {'Authorization': `${this.currentSession?.token}`}});
  }

  createCompany(request: CreateCompanyRequest): Observable<PostCompaniesResponse> {
    return this.httpClient.post<PostCompaniesResponse>(this.url + '/companies', request,
      {headers: {'Authorization': `${this.currentSession?.token}`}});
  }

  getDeviceTypes(): Observable<GetDeviceTypesResponse> {
    return this.httpClient.get<GetDeviceTypesResponse>(this.url + `/devices/types`);
  }


}
