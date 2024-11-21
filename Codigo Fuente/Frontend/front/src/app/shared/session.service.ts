import {HttpClient} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { sessionModel, sessionRequest } from '../pages/login/panel/sessionModel';
import {tap} from 'rxjs';
import {environment} from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class ApiSessionService {

  url: string;

  constructor(private httpClient: HttpClient) {
    this.url = environment.apiUrl;
  }

  postSession(data: sessionRequest) {
    return this.httpClient.post<sessionModel>(this.url + '/login', data).pipe(
      tap((session: sessionModel) => {
        localStorage.setItem('user', JSON.stringify(session.user));
        localStorage.setItem('token', JSON.stringify(session.token));
      })
    );
  }

}
