import { Component } from '@angular/core';
import { AdministratorService } from '../../../shared/administrator.service';
import { Router } from '@angular/router';
import { GetUsersRequest, GetUsersResponse, GetUserResponse } from '../../../interfaces/users';

@Component({
  selector: 'app-delete-admin',
  templateUrl: './delete-admin.component.html',
  styleUrl: './delete-admin.component.css'
})
export class DeleteAdminComponent {

  users: GetUserResponse[] = [];
  totalUsers: number = 0;

  feedback: string = "";

  constructor(private api: AdministratorService, private router: Router) {}

  ngOnInit(): void {
    this.getUsers();
    console.log(this.users);
  }

  goHome(): void {
    this.router.navigate(['/administrator']);
  }

  getUsers(): void {
    const request: GetUsersRequest = {
      fullName: "",
      role: "administrator"
    };
  
    this.api.getUsers(request).subscribe({
      next: (res: GetUsersResponse) => {
        this.users = res.users || [];
        this.totalUsers = res.users.length;
        console.log(this.users);
      }
    });
  }

  deleteAdmin(id:number): void {
    this.feedback = "Cargando...";
    this.api.deleteAdministrator(id).subscribe({
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
