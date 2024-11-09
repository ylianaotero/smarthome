import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { createAdministratorModel, createCompanyOwnerModel } from '../../../interfaces/users';
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

  isNewAdmin: boolean = false;

  constructor(private api: AdministratorService, private router: Router) {}

  ngOnInit(): void {
    console.log("Create User Component");
    let url = this.router.url;
    if (url == '/administrator/new-admin') {
      this.isNewAdmin = true;
    }
  }

  goHome(): void {
    this.router.navigate(['/administrator']);
  }

  register(name: string, surname: string, email: string, password: string, photo: string): void {
    this.feedback = "Cargando...";

    if (!name || !surname || !email || !password || (!photo && this.isNewAdmin)) {
      this.feedback = "Por favor, completa todos los campos obligatorios.";
      return;
    }

    console.log(email);
    console.log(password);

    if(this.isNewAdmin){
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
        this.feedback = "No se ha podido conectar con el servidro. Por favor, inténtalo de nuevo.";
        break;
      default:
        this.feedback = "A ocurrido un error inesperado. Por favor, inténtalo de nuevo.";
    }
  }
}
