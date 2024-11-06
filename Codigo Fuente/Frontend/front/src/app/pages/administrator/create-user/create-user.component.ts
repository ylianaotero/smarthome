import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { createAdministratorModel } from '../../../interfaces/administrator';
import { AdministratorService } from '../../../shared/administrator.service';
import { connect } from 'rxjs';


@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html',
  styleUrls: ['./create-user.component.css'],
})

export class CreateUserComponent {

  nameValue: string = '';
  surnameValue: string = '';
  emailValue: string = '';
  passwordValue: string = '';
  photoValue: string = '';

  feedback: string = "";

  constructor(private api: AdministratorService, private router: Router) {}

  goHome(): void {
    this.router.navigate(['/administrator']);
  }

  registerAdmin(name: string, surname: string, email: string, password: string, photo: string): void {
    this.feedback = "Cargando...";

    if (!name || !surname || !email || !password || !photo) {
      this.feedback = "Por favor, completa todos los campos obligatorios.";
      return;
    }

    console.log(email);
    console.log(password);

    this.api.postAdministrator(new createAdministratorModel(name, surname, email, password, photo))
      .subscribe({
        next: res => {
          this.feedback = "El administrador fue creado con éxito!";
          this.router.navigate(['amindistrator-panel']);
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
