import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {ApiService} from '../../../shared/api.service';


@Component({
  selector: 'app-notifications-list',
  templateUrl: './notifications-panel.component.html',
  styleUrls: [ '../../../../styles.css']
})
export class NotificationsPanelComponent implements OnInit {
  constructor(private router: Router, private sharedApi: ApiService) {}

  ngOnInit(): void {
  }
}
