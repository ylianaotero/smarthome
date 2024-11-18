import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import { sessionModel, sessionRequest } from '../pages/login/panel/sessionModel';
import { createAdministratorModel, GetUsersRequest, GetUsersResponse, PostAdministratorRequest, createCompanyOwnerModel, PostCompanyOwnerRequest} from '../interfaces/users';
import { Injectable } from '@angular/core';
import {Observable, tap} from 'rxjs';


@Injectable({
    providedIn: 'root'
})
export class AdministratorService {

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
            localStorage.setItem('user', JSON.stringify(session)); // Guarda la sesión en localStorage
        })
        );
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
        params = pageSize ? params.append('currentPage', currentPage!.toString()) : params;
        params = currentPage ? params.append('pageSize', pageSize!.toString()) : params;
        return this.httpClient.get<GetUsersResponse>(this.url + '/users', {params , headers: {'Authorization': `${this.currentSession?.token}`}});
    }

    deleteAdministrator(id: number) {
        return this.httpClient.delete(this.url + `/administrators/${id}`, {headers: {'Authorization': `${this.currentSession?.token}`}});
    }

}
