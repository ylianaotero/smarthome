import {HttpClient, HttpParams} from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  GetNotificationResponse,
  NotificationsFilterRequestModel
} from '../pages/notifications/panel/model-notification';
import {Observable} from 'rxjs';
import {environment} from '../../enviroments/environment';
@Injectable({
  providedIn: 'root'
})
export class ApiNotificationService {

  url: string;

  constructor(private httpClient: HttpClient) {
    this.url = environment.apiUrl;
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


    if (modelIn.ReadDate != null) {
      console.log(modelIn.ReadDate.toString());
      params = params.set('ReadDate', modelIn.ReadDate.toString());
    }

    if (modelIn.CreatedDate != null) {
      console.log(modelIn.CreatedDate.toString());
      params = params.set('CreatedDate', modelIn.CreatedDate.toString());
    }



    return this.httpClient.get<GetNotificationResponse[]>(url, {
      params,
      headers: { 'Authorization': `${storedUser}` }
    });
  }

}
