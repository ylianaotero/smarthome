import { Component } from '@angular/core';
import { AdministratorService } from '../../../shared/administrator.service';
import { Router } from '@angular/router';
import {GetUsersRequest, GetUsersResponse, GetUserResponse, Role} from '../../../interfaces/users';
import {ApiService} from '../../../shared/api.service';

@Component({
  selector: 'app-list-users',
  templateUrl: './list-users.component.html',
  styleUrls: ['../../../../styles.css'],
})
export class ListUsersComponent {
  users: GetUserResponse[] = [];
  totalUsers: number = 0;
  selectedName: string = "";
  selectedRole: string = "";
  roles: string[] = ['administrator', 'homeowner', 'companyowner'];

  feedback: string = "";

  modalUser: string | null = '';
  modalImage: string | null = null;
  isPhotoModalOpen = false;

  constructor(private api: ApiService, private router: Router) {}

  ngOnInit(): void {
    this.getUsers();
  }
  getUsers(): void {
    const request: GetUsersRequest = {
      fullName: this.selectedName,
      role: this.selectedRole
    };

    this.api.getUsers(request).subscribe({
      next: (res: GetUsersResponse) => {
        this.users = res.users || [];
        this.totalUsers = res.users.length;
        console.log(this.users);
      }
    });
  }

  formatDate(date: string): string {
    let splittedDate = date.split('T');
    let time = splittedDate[1].split('.');
    return splittedDate[0] + ' ' + time[0];
  }

  formatRoles(roles: Role[]): string {
    return roles.join(', ');
  }

  userPhotoUrl(user: GetUserResponse): string {
    return user.photoUrl ? user.photoUrl : 'https://i.postimg.cc/hP6nR6FY/smarthomeuser.webp'
  }

  openPhotoModal(imageUrl: string | null, user: string | null): void {
    this.modalUser = user;
    this.modalImage = imageUrl;
    this.isPhotoModalOpen = true;
  }

  closeModal(): void {
    this.isPhotoModalOpen = false;
  }
}
