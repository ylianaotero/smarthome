import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { createAdministratorModel, createCompanyOwnerModel } from '../../../interfaces/users';
import { AdministratorService } from '../../../shared/administrator.service';

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

  feedback: string = "";

  roleTypes: string[] = ['Administrador', 'Dueño de Empresa'];
  selectedRole: string = '';

  constructor(private api: AdministratorService, private router: Router) {}

  goHome(): void {
    this.router.navigate(['/administrators']);
  }

  register(name: string, surname: string, email: string, password: string, photo: string): void {
    this.feedback = "Cargando...";

    if (!name || !surname || !email || !password || !this.selectedRole || (!photo && this.selectedRole === 'Administrador')) {
      this.feedback = "Por favor, completa todos los campos.";
      return;
    }

    if(this.selectedRole === 'Administrador'){
      this.api.postAdministrator(new createAdministratorModel(name, email, password, surname, photo))
      .subscribe({
        next: res => {
          this.feedback = "El administrador fue creado con éxito!";
          this.router.navigate(['administrator']);
        },
        error: err => {
          this.handleError(err);
          console.error(err);
        }
      });
    }else{
      this.api.postCompanyOwner(new createCompanyOwnerModel(name, email, password, surname))
      .subscribe({
        next: res => {
          this.feedback = "El dueño de empresa fue creado con éxito!";
          this.router.navigate(['administrator']);
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
