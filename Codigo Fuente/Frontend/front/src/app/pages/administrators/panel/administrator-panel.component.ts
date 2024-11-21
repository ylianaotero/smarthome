import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GetCompanyResponse } from '../../../interfaces/companies';
import { GetUsersRequest, GetUsersResponse, GetUserResponse} from '../../../interfaces/users';
import {GetDeviceTypesResponse} from '../../../interfaces/devices';
import {userRetrieveModel} from '../../home-owners/create/signUpUserModel';
import {ApiUserService} from '../../../shared/user.service';
import {ApiDeviceService} from '../../../shared/devices.service';

@Component({
  selector: 'app-administrator-panel',
  templateUrl: './administrator-panel.component.html',
  styleUrls: ['../../../../styles.css']
})
export class AdministratorPanelComponent implements OnInit {
  userName: string;

  currentPage: number = 1;
  pageSize: number = 1;

  totalUsers: number = 0;

  selectedFullName: string = '';
  selectedRole: string = '';

  users: GetUserResponse[] = [];
  companies: GetCompanyResponse[] = [];


  modalShowDevices: boolean = false;
  modalShowDevicesTypes: boolean = false;

  deviceTypes: string[] = [];

  user : userRetrieveModel | null = null;


  constructor(private router: Router, private deviceApi: ApiDeviceService, private userApi: ApiUserService) {
    const storedUser = localStorage.getItem('user');
    if(storedUser){
      this.user = JSON.parse(storedUser) as userRetrieveModel;
    }
    this.userName = this.user?.name || 'Usuario';
  }

  ngOnInit(): void {
    this.getUsers();
    this.getDevicesTypes();
  }

  logOut() : void {
    localStorage.setItem('user', JSON.stringify(null));
    localStorage.setItem('token', '');
    this.router.navigate(['']);
  }

  getDevicesTypes(): void {
    this.deviceApi.getSupportedDevices().subscribe({
      next: (res: GetDeviceTypesResponse) => {
        console.log(res)
        this.deviceTypes = res.deviceTypes || [];
        console.log(res.deviceTypes);
      }
    });
  }

  goViewNotifications(): void {
    this.router.navigate(['/notifications']);
  }

  goViewDevices(): void {
    this.router.navigate(['devices']);
  }

  getUsers(): void {
    const request: GetUsersRequest = {
      fullName: this.selectedFullName,
      role: this.selectedRole
    };

    this.userApi.getUsers(request).subscribe({
      next: (res: GetUsersResponse) => {
        this.users = res.users || [];
        this.totalUsers = res.users.length;
        console.log(this.users);
      }
    });
  }

  goCreateUser(): void {
    this.router.navigate(['/administrators/create-user']);
  }

  goListUsers(): void {
    this.router.navigate(['/administrators/list-users']);
  }

  goDeleteAdmin(): void {
    this.router.navigate(['/administrators/delete-admin']);
  }

  goListCompanies(): void {
    this.router.navigate(['/administrators/list-companies']);
  }

  openModal(modal: string): void {
    this.changeSelectedModal(modal, true);
    document.body.classList.add('modal-open');
    this.createBackdrop();
  }

  closeModal(modal: string): void {
    this.changeSelectedModal(modal, false);
    document.body.classList.remove('modal-open');
    this.removeBackdrop();
  }

  changeSelectedModal(modal: string, showModal: boolean): void{
    if(modal == "showDevices"){
      this.modalShowDevices = showModal;
    }else if(modal == "showDevicesTypes"){
      this.modalShowDevicesTypes = showModal;
    }
  }

  closeModalBackdrop(event: MouseEvent,modal: string ): void {
    const target = event.target as HTMLElement;
    if (target.id === 'myModalShowDevices') {
      this.closeModal(modal);
    }else if(target.id === 'myModalShowDevicesTypes'){
      this.closeModal(modal);
    }
  }

  private createBackdrop(): void {
    const backdrop = document.createElement('div');
    backdrop.className = 'modal-backdrop fade show';
    document.body.appendChild(backdrop);
  }

  private removeBackdrop(): void {
    const backdrop = document.querySelector('.modal-backdrop');
    if (backdrop) {
      document.body.removeChild(backdrop);
    }
  }

}
