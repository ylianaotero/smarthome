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
  addRole,
  createAdministratorModel,
  createCompanyOwnerModel, GetUsersRequest, GetUsersResponse,
  PostAdministratorRequest,
  PostCompanyOwnerRequest, PostCompanyOwnerResponse, ResponseAdmin
} from '../interfaces/users';
import {
  CreateCompanyRequest,
  GetCompaniesResponse,
  GetCompanyRequest,
  PostCompaniesResponse
} from '../interfaces/companies';
import {
  GetNotificationResponse,
  NotificationsFilterRequestModel
} from '../pages/notifications/panel/model-notification';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private httpClient: HttpClient) {
  }

  url: string = 'http://localhost:5217/api/v1';



  postSession(data: sessionRequest) {
    return this.httpClient.post<sessionModel>(this.url + '/login', data).pipe(
      tap((session: sessionModel) => {
        localStorage.setItem('user', JSON.stringify(session.user));
        localStorage.setItem('token', JSON.stringify(session.token));
      })
    );
  }

  postHomeOwner(data: userRegistrationInstance) {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.post<userRetrieveModel>(this.url + '/home-owners', data);
  }

  patchDevice(data: patchDeviceRequest) {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.patch(this.url + '/homes/'+ data.HomeId.toString()+ '/devices', new patchDeviceAlias(data.HardwareId, data.Name), {headers: {'Authorization': `${storedUser}`}});
  }

  postHome(data: createHomeModel) {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.post<homeRetrieveModel>(this.url + '/homes', data, {headers: {'Authorization': `${storedUser}`}});
  }

  postRole(data: string) {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.post(this.url + '/users/'+data+'/roles', new addRole("homeowner"), {headers: {'Authorization': `${storedUser}`}});
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

  getSupportedDevices() {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.get<GetDeviceTypesResponse>(this.url + '/devices' + '/types', {headers: {'Authorization': `${storedUser}`}});
  }

  getRooms(data : string) {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.get<GetRoomResponse>(this.url + '/homes/' + data + '/rooms', {headers: {'Authorization': `${storedUser}`}});
  }


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


  getNotifications(modelIn: NotificationsFilterRequestModel): Observable<GetNotificationResponse[]> {
    const storedUser = localStorage.getItem('token');
    const url = this.url + '/notifications';
    let params = new HttpParams();
    if (modelIn.Kind) {
      params = params.set('Kind', modelIn.Kind);
    }

    if (modelIn.Read) {
      params = params.set('Read', modelIn.Read);
    }


    if (modelIn.Date != null) {
      console.log(modelIn.Date.toString());
      params = params.set('Date', modelIn.Date.toString());
    }



    return this.httpClient.get<GetNotificationResponse[]>(url, {
      params,
      headers: { 'Authorization': `${storedUser}` }
    });
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

  postAdministrator(data: createAdministratorModel) {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.post<ResponseAdmin>(this.url + '/administrators', data, {headers: {'Authorization': `${storedUser}`}});
  };

  postCompanyOwner(data: createCompanyOwnerModel) {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.post<PostCompanyOwnerResponse>(this.url + '/company-owners', data, {headers: {'Authorization': `${storedUser}`}});
  }

  getUsers(request: GetUsersRequest, currentPage?: number, pageSize?: number): Observable<GetUsersResponse> {
    let params = new HttpParams();
    const storedUser = localStorage.getItem('token');

    params = request.fullName ? params.append('fullName', request.fullName ) : params;
    params = request.role ? params.append('role', request.role ) : params;

    params = pageSize ? params.append('Page', currentPage!.toString()) : params;
    params = currentPage ? params.append('PageSize', pageSize!.toString()) : params;
    return this.httpClient.get<GetUsersResponse>(this.url + '/users', {params , headers: {'Authorization': `${storedUser}`}});
  }

  deleteAdministrator(id: number) {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.delete(this.url + `/administrators/${id}`, {headers: {'Authorization': `${storedUser}`}});
  }

  getCompanies(request:  GetCompanyRequest, currentPage?: number, pageSize?: number):
    Observable<GetCompaniesResponse> {
    const storedUser = localStorage.getItem('token');
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
      {params, headers: {'Authorization': `${storedUser}`}});
  }

  createCompany(request: CreateCompanyRequest): Observable<PostCompaniesResponse> {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.post<PostCompaniesResponse>(this.url + '/companies', request,
      {headers: {'Authorization': `${storedUser}`}});
  }

  getDeviceTypes(): Observable<GetDeviceTypesResponse> {
    return this.httpClient.get<GetDeviceTypesResponse>(this.url + `/devices/types`);
  }


}
