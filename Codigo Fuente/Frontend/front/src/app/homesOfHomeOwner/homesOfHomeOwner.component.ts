import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from '../shared/api.service';
import {addMemberRequest, home, member} from './homeModels';

@Component({
  selector: 'app-homes-home-owner',
  templateUrl: './homesOfHomeOwner.component.html',
  styleUrls: ['./homesOfHomeOwner.component.css']
})
export class HomesOfHomeOwnerComponent implements OnInit {
  homes!: home[];
  members!: member[];
  isLoading: boolean = true;
  selectedHome: home | null = null;
  feedback: string = "";

  newMember = {
    email: '',
    canViewDevices: false,
    canAddDevices: false,
    receiveNotifications: false
  };

  isModalOfMembersOpen: boolean = false;

  isModalOfListOfMembersOpen: boolean = false;

  constructor(private api: ApiService, private router: Router) {}

  ngOnInit(): void {
    this.getHomes();
  }

  getHomes(): void {
    this.api.getHomesOfHomeOwner().subscribe({
      next: (res: any) => {
        this.homes = res.homes || [];
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  getMembers(id?: number): void {
    if(!id){
      return;
    }
    this.api.getMembersOfHome(id).subscribe({
      next: (res: any) => {
        this.members = res.members || [];
      }
    });
  }

  openModal(home: home, modal: string): void {
    this.selectedHome = home;
    this.changeSelectedModal(modal, true);
    document.body.classList.add('modal-open');
    this.createBackdrop();
  }

  closeModal(modal: string): void {
    this.changeSelectedModal(modal, false);
    this.selectedHome = null;
    this.resetMemberForm();
    document.body.classList.remove('modal-open');
    this.removeBackdrop();
  }

  changeSelectedModal(modal: string, bool: boolean): void{
    if(modal == "addMembers"){
      this.isModalOfMembersOpen = bool;
    }else{
      if(modal == "showMembers"){
        this.getMembers(this.selectedHome?.id);
        this.isModalOfListOfMembersOpen = bool;
      }
    }
  }

  closeModalBackdrop(event: MouseEvent,modal: string ): void {
    const target = event.target as HTMLElement;
    if (target.id === 'myModalMembers' || target.id === 'myModalShowMembers') {
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

  saveMember(): void {
    if (this.selectedHome && this.isValidEmail(this.newMember.email)) {

      this.postMember();

    } else {
      this.feedback = 'Datos inválidos. Verifica la información e intenta nuevamente.';
      return;
    }
  }

  isValidEmail(email: string): boolean {
    const emailRegex = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i;
    return emailRegex.test(email);
  }

  postMember(): void {

    if (!this.selectedHome || !this.selectedHome.id) {
      this.feedback = "Error al identificar la casa seleccionada, reeintente.";
      return;
    }

    const request = new addMemberRequest(
      this.selectedHome.id,
      this.newMember.email,
      this.newMember.canViewDevices,
      this.newMember.canAddDevices,
      this.newMember.receiveNotifications
    );

    this.api.postMemberToHome(request).subscribe({
      error: (err) => {
        if(err.status == 200){
          this.feedback = 'Miembro agregado exitosamente.';
          this.closeModal('addMembers');
          return;
        }
        this.handleError(err);
        return;
      }
    });
  }

  handleError(err: any): void {
    if (err.status === 0) {
      this.feedback = 'No se pudo conectar con el servidor. Inténtalo de nuevo más tarde.';
    } else if (err.status === 400) {
      this.feedback = 'Datos inválidos. Verifica la información e intenta nuevamente.';
    } else if (err.status === 409) {
      this.feedback = 'El miembro ya existe en la casa.';
    } else if (err.status === 404) {
      this.feedback = 'La casa o miembro seleccionados no fueron encontrados.';
    } else if (err.status === 412) {
      this.feedback = 'No se pudo agregar el miembro debido a que la casa esta llena.';
    } else {
      this.feedback = 'Ocurrió un error inesperado. Por favor, intenta más tarde.';
    }
  }


  private resetMemberForm(): void {
    this.newMember = {
      email: '',
      canViewDevices: false,
      canAddDevices: false,
      receiveNotifications: false
    };
    this.feedback = "";
  }
}
