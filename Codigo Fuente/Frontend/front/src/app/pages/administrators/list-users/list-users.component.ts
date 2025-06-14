import {Component, OnInit} from '@angular/core';
import { Router } from '@angular/router';
import {GetUsersRequest, GetUsersResponse, GetUserResponse, Role} from '../../../interfaces/users';
import {ApiUserService} from '../../../shared/user.service';

@Component({
  selector: 'app-list-users',
  templateUrl: './list-users.component.html',
  styleUrls: ['../../../../styles.css'],
})
export class ListUsersComponent implements OnInit{
  users: GetUserResponse[] = [];
  totalUsers: number = 0;
  selectedName: string = "";
  selectedRole: string = "";
  roles: string[] = ['administrator', 'homeowner', 'companyowner'];

  feedback: string = "";

  modalUser: string | null = '';
  modalImage: string | null = null;
  isPhotoModalOpen = false;

  currentPage: number = 1;
  pageSize: number = 6;


  constructor(private router: Router, private userApi : ApiUserService) {}

  changePage(page: number): void {
    this.currentPage = page;
    this.getUsers();
  }
  get totalPages(): number {
    return Math.ceil(this.totalUsers / this.pageSize);
  }

  ngOnInit(): void {
    this.getUsers();
  }
  getUsers(): void {
    const request: GetUsersRequest = {
      fullName: this.selectedName,
      role: this.selectedRole
    };

    this.userApi.getUsers(request,this.currentPage, this.pageSize).subscribe({
      next: (res: GetUsersResponse) => {
        this.users = res.users || [];
        console.log(res.totalCount);
        this.totalUsers = res.totalCount;
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
    return roles.map(role => role.kind).join(', ');
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
