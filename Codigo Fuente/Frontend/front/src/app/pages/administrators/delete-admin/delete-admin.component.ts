import {Component, OnInit} from '@angular/core';
import { Router } from '@angular/router';
import { GetUsersRequest, GetUsersResponse, GetUserResponse } from '../../../interfaces/users';
import {userRetrieveModel} from '../../home-owners/create/signUpUserModel';
import {ApiUserService} from '../../../shared/user.service';

@Component({
  selector: 'app-delete-admin',
  templateUrl: './delete-admin.component.html',
  styleUrls: ['../../../../styles.css'],
})
export class DeleteAdminComponent implements OnInit{

  users: GetUserResponse[] = [];
  totalUsers: number = 0;
  selectedName: string = "";

  feedback: string = "";

  sessionUserEmail: string = "";

  modalUser: string | null = '';
  modalImage: string | null = null;
  modalUserId: number = -1
  isPhotoModalOpen = false;
  isConfirmationModalOpen = false;

  user : userRetrieveModel | null = null;

  constructor(private router: Router, private userApi : ApiUserService) {}

  ngOnInit(): void {
    this.getUsers();
    const storedUser = localStorage.getItem('user');
    if(storedUser){
      this.user = JSON.parse(storedUser) as userRetrieveModel;
    }
    this.sessionUserEmail = this.user?.email|| 'Usuario';
  }

  getUsers(): void {
    const request: GetUsersRequest = {
      fullName: this.selectedName,
      role: "administrator"
    };

    this.userApi.getUsers(request).subscribe({
      next: (res: GetUsersResponse) => {
        this.users = res.users || [];
        this.totalUsers = res.users.length;
        console.log(this.users);
      }
    });

    this.closeModal();
  }



  deleteAdmin(id:number): void {
    this.feedback = "Cargando...";
    this.userApi.deleteAdministrator(id).subscribe({
      next: res => {
        this.feedback = "El administrador fue eliminado con éxito!";
        this.getUsers();
      },
      error: err => {
        this.handleError(err);
        console.error(err);
      }
    });
  }

  formatDate(date: string): string {
    let splittedDate = date.split('T');
    let time = splittedDate[1].split('.');
    return splittedDate[0] + ' ' + time[0];
  }

  userPhotoUrl(user: GetUserResponse): string {
    return user.photoUrl ? user.photoUrl : 'https://i.postimg.cc/hP6nR6FY/smarthomeuser.webp'
  }

  openPhotoModal(imageUrl: string | null, user: string | null): void {
    this.modalUser = user;
    this.modalImage = imageUrl;
    this.isPhotoModalOpen = true;
  }

  openConfirmationModal(userId: number, userName: string | null): void {
    this.modalUserId = userId;
    this.modalUser = userName;
    this.isConfirmationModalOpen = true;
  }

  closeModal(): void {
    this.isPhotoModalOpen = false;
    this.isConfirmationModalOpen = false
  }

  private handleError(err: any): void {
    switch (err.status) {
      case 400:
        this.feedback = "Entrada inválida. Por favor, revisa tus datos.";
        break;
      case 409:
        this.feedback = "El email ya está en uso.";
        break;
      case 412:
        this.feedback = "Precondition failed: " + err.error;
        break;
      case 0:
        this.feedback = "No se ha podido conectar con el servidro. Por favor, inténtalo de nuevo.";
        break;
      default:
        this.feedback = "A ocurrido un error inesperado. Por favor, inténtalo de nuevo.";
    }
  }
}
