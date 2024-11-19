import {Component, OnInit, ViewChild} from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from '../../../shared/api.service';
import {ImportDevicesRequest, importer, ImporterPath} from './importerModels';
import {NgForm} from '@angular/forms';

@Component({
  selector: 'app-import',
  templateUrl: './import-device.component.html',
  styleUrls: ['../../../../styles.css']
})
export class ImportDeviceComponent implements OnInit {

  @ViewChild('importForm') importForm1!: NgForm;

  @ViewChild('addImporterForm') importForm2!: NgForm;

  importers!: importer[];
  importRequest: ImportDevicesRequest = new ImportDevicesRequest('', '', '');
  feedback: string = '';
  isLoading: boolean = false;

  dllPath: string = '';
  statusMessage: string = '';

  constructor(private api: ApiService, private router: Router) {}

  AddImporter() {
    if (this.dllPath === '') {
      this.statusMessage = 'Por favor, ingresa una ruta de DLL válida.';
      return;
    }

    this.dllPath = this.dllPath.replace(/\\/g, '\\\\');

    this.api.postImporter(new ImporterPath(this.dllPath)).subscribe({
      next: (res: any) => {
        this.getImporters();
        this.statusMessage = 'Importador agregado exitosamente!';
        this.importForm2.reset();

        this.dllPath = '';
      },
      error: (err: any) => {
        this.statusMessage = 'Ocurrió un error al agregar el importador.';
        this.importForm2.reset();
      }
    });
  }


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
      this.importForm1.reset();
    } else if (err.status === 400) {
      this.feedback = 'Datos inválidos. Verifica la información e intenta nuevamente.';
      this.importForm1.reset();
    } else if (err.status === 404) {
      this.feedback = 'Datos no encontrados.';
      this.importForm1.reset();
    } else if (err.status === 401) {
      this.feedback = 'No autorizado. Por favor, verifica tu sesión o credenciales.';
      this.importForm1.reset();
    }else if (err.status === 200) {
      this.feedback = 'Importación realizada exitosamente.';
      this.resetForm();
      this.importForm1.reset();
    }
    else {
      this.feedback = 'Ocurrió un error inesperado. Por favor, intenta más tarde.';
      this.importForm1.reset();
    }
  }

  resetForm() {
    this.importRequest = new ImportDevicesRequest('', '', '');
  }
}
