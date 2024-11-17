import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from '../../../shared/api.service';
import { ImportDevicesRequest, importer } from './importerModels';

@Component({
  selector: 'app-import',
  templateUrl: './imports.component.html',
  styleUrls: ['./imports.component.css', '../../../../styles.css']
})
export class ImportsComponent implements OnInit {
  importers!: importer[];
  importRequest: ImportDevicesRequest = new ImportDevicesRequest('', '', '');
  feedback: string = '';
  isLoading: boolean = false;

  constructor(private api: ApiService, private router: Router) {}

  ngOnInit() {
    this.getImporters();
  }

  getImporters() {
    this.api.getImporters().subscribe({
      next: (res: any) => {
        console.log(res);
        this.importers = res || [];
      }
    });
  }

  onImportFormSubmit() {
    this.feedback = '';
    this.isLoading = true;
    this.importRequest.DllPath = this.importRequest.DllPath.replace(/\\/g, '\\\\');
    this.importRequest.FilePath = this.importRequest.FilePath.replace(/\\/g, '\\\\');

    this.api.importDevices(this.importRequest).subscribe({
      error: (err) => this.handleError(err),
      complete: () => this.isLoading = false
    });
  }

  handleError(err: any): void {
    this.isLoading = false;

    if (err.status === 0) {
      this.feedback = 'No se pudo conectar con el servidor. Inténtalo de nuevo más tarde.';
    } else if (err.status === 400) {
      this.feedback = 'Datos inválidos. Verifica la información e intenta nuevamente.';
    } else if (err.status === 404) {
      this.feedback = 'Datos no encontrados.';
    } else if (err.status === 401) {
      this.feedback = 'No autorizado. Por favor, verifica tu sesión o credenciales.';
    }else if (err.status === 200) {
      this.feedback = 'Importación realizada exitosamente.';
      this.resetForm();
    }
    else {
      this.feedback = 'Ocurrió un error inesperado. Por favor, intenta más tarde.';
    }
  }

  resetForm() {
    this.importRequest = new ImportDevicesRequest('', '', '');
  }
}
