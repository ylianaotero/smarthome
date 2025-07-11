import { Component } from '@angular/core';
import { Router } from '@angular/router';
import {createAdministratorModel, createCompanyOwnerModel} from '../../../interfaces/users';
import {ApiUserService} from '../../../shared/user.service';

@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html',
  styleUrls: ['../../../../styles.css'],
})

export class CreateUserComponent {

  nameValue: string = '';
  surnameValue: string = '';
  emailValue: string = '';
  passwordValue: string = '';
  photoValue: string = '';

  homeOwner : boolean = false;

  feedback: string = "";

  roleTypes: string[] = ['Administrador', 'Dueño de Empresa'];
  selectedRole: string = '';

  userId : number = -1;

  constructor(private userApi: ApiUserService, private router: Router) {}

  goHome(): void {
    this.router.navigate(['/administrators']);
  }

  addRole(): void {
    this.userApi.postRole(this.userId.toString())
      .subscribe({
        next: res => {
          this.router.navigate(['/home/user-panel']);
        }
      });

  }

  register(name: string, surname: string, email: string, password: string, photo: string): void {
    this.feedback = "Cargando...";

    if (!name || !surname || !email || !password || !this.selectedRole || (!photo && this.selectedRole === 'Administrador')) {
      this.feedback = "Por favor, completa todos los campos.";
      return;
    }

    if(this.selectedRole === 'Administrador'){
      this.userApi.postAdministrator(new createAdministratorModel(name, email, password, surname, photo))
      .subscribe({
        next: res => {
          this.feedback = "El administrador fue creado con éxito!";
          if(this.homeOwner){
            this.userId = res.id
            this.addRole()
          }else{
            this.router.navigate(['/administrator']);
          }
        },
        error: err => {
          this.handleError(err);
          console.error(err);
        }
      });
    }else{
      this.userApi.postCompanyOwner(new createCompanyOwnerModel(name, email, password, surname))
      .subscribe({
        next: res => {
          if(this.homeOwner){
            this.userId = res.id
            this.addRole();
          }else{
            this.router.navigate(['/company-owners']);
          }
        },
        error: err => {
          this.handleError(err);
          console.error(err);
        }
      });
    }
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
        this.feedback = "No se ha podido conectar con el servidor. Por favor, inténtalo de nuevo.";
        break;
      default:
        this.feedback = "Ha ocurrido un error inesperado. Por favor, inténtalo de nuevo.";
    }
  }
}
