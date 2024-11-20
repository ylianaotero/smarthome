import {HttpClient, HttpParams} from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  GetNotificationResponse,
  NotificationsFilterRequestModel
} from '../pages/notifications/panel/model-notification';
import {Observable} from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class ApiNotificationService {

  constructor(private httpClient: HttpClient) {
  }

  url: string = 'http://localhost:5217/api/v1';


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

}
