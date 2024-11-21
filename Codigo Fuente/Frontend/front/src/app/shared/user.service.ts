import {HttpClient, HttpParams} from '@angular/common/http';
import { Injectable } from '@angular/core';
import {Observable} from 'rxjs';
import {
  addRole,
  createAdministratorModel,
  createCompanyOwnerModel, GetUsersRequest, GetUsersResponse,
  PostCompanyOwnerResponse,
  ResponseAdmin
} from '../interfaces/users';
import {userRegistrationInstance, userRetrieveModel} from '../pages/home-owners/create/signUpUserModel';
import {environment} from '../../enviroments/environment';
@Injectable({
  providedIn: 'root'
})
export class ApiUserService {

  url: string;

  constructor(private httpClient: HttpClient) {
    this.url = environment.apiUrl;
  }

  postHomeOwner(data: userRegistrationInstance) {
    return this.httpClient.post<userRetrieveModel>(this.url + '/home-owners', data);
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

  postRole(data: string) {
    const storedUser = localStorage.getItem('token');
    return this.httpClient.post(this.url + '/users/'+data+'/roles', new addRole("homeowner"), {headers: {'Authorization': `${storedUser}`}});
  }


}
