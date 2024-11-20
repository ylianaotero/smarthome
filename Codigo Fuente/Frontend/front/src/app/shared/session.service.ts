import {HttpClient} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { sessionModel, sessionRequest } from '../pages/login/panel/sessionModel';
import {tap} from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class ApiSessionService {

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

}
