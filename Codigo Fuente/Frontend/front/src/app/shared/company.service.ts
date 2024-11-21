import {HttpClient, HttpParams} from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  CreateCompanyRequest,
  GetCompaniesResponse,
  GetCompanyRequest,
  PostCompaniesResponse
} from '../interfaces/companies';
import {Observable} from 'rxjs';
import {environment} from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class ApiCompanyService {

  url: string;

  constructor(private httpClient: HttpClient) {
    this.url = environment.apiUrl;
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



}
