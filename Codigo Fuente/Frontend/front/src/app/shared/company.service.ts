import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import { sessionModel, sessionRequest } from '../logIn/sessionModel';
import { Injectable } from '@angular/core';
import {Observable, tap} from 'rxjs';
import {
  GetCompaniesRequest,
  GetCompaniesResponse,
  PostCompaniesRequest,
  PostCompaniesResponse
} from '../interfaces/companies';

@Injectable({
    providedIn: 'root'
})
export class CompanyService {

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

     getCompanies(request: GetCompaniesRequest, currentPage?: number, pageSize?: number):
       Observable<GetCompaniesResponse> {
          let params = new HttpParams();

          params = request.name ? params.append('name', request.name ) : params;
          params = request.owner ? params.append('owner', request.owner ) : params;
          params = request.ownerEmail ? params.append('ownerEmail', request.ownerEmail ) : params;
          params = pageSize ? params.append('currentPage', currentPage!.toString()) : params;
          params = currentPage ? params.append('pageSize', pageSize!.toString()) : params;
          return this.httpClient.get<GetCompaniesResponse>(this.url + '/companies',
            {params, headers: {'Authorization': `${this.currentSession?.token}`}});
     }

     createCompany(request: PostCompaniesRequest): Observable<PostCompaniesResponse> {
       let params = new HttpParams();
        params = request.name ? params.append('name', request.name ) : params;
        params = request.rut ? params.append('rut', request.rut ) : params;
        params = request.logoUrl ? params.append('logoUrl', request.logoUrl ) : params;
        params = request.ownerId ? params.append('ownerId', request.ownerId.toString() ) : params;
        return this.httpClient.post<PostCompaniesResponse>(this.url + '/companies', params,
          {headers: {'Authorization': `${this.currentSession?.token}`}});
     }


}
