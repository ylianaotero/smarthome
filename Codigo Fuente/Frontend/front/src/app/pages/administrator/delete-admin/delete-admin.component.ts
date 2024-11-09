import { Component } from '@angular/core';
import { AdministratorService } from '../../../shared/administrator.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-delete-admin',
  templateUrl: './delete-admin.component.html',
  styleUrl: './delete-admin.component.css'
})
export class DeleteAdminComponent {

  feedback: string = "";

  constructor(private api: AdministratorService, private router: Router) {}

  goHome(): void {
    this.router.navigate(['/administrator']);
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
