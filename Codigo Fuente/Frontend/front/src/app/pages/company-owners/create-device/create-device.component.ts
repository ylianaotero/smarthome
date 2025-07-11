import {Component, OnInit} from '@angular/core';
import { Router } from '@angular/router';
import { ApiDeviceService } from '../../../shared/devices.service';
import {
  GetDeviceTypesResponse, PostMotionSensorRequest, PostSecurityCameraRequest,
  PostSmartLampRequest,
  PostWindowSensorRequest
} from '../../../interfaces/devices';
import {GetCompaniesResponse, GetCompanyRequest} from '../../../interfaces/companies';
import {userRetrieveModel} from '../../home-owners/create/signUpUserModel';
import {ApiCompanyService} from '../../../shared/company.service';

@Component({
  selector: 'app-create-device',
  templateUrl: './create-device.component.html',
  styleUrls: ['create-device.component.css','../../../../styles.css']
})
export class CreateDeviceComponent implements OnInit  {

  feedback: string = "";

  companyOwnerName: string = '';
  companyOwnerEmail: string = '';

  deviceName: string = '';
  deviceType: string = '';
  deviceModel: string = '';
  deviceDescription: string = '';
  deviceCompanyId: number = -1;
  deviceFunctionalities: string[] = [];
  deviceLocationType: string = '';
  devicePhotoUrls: string[] = [];

  functionalitiesByType: { [key: string]: string[] } = {
    WindowSensor: ['OpenClosed'],
    SecurityCamera: ['MotionDetection', 'HumanDetection'],
    SmartLamp: ['OnOff'],
    MotionSensor: ['MotionDetection']
  };

  possibleDeviceTypes: string[];
  possibleLocationTypes: string[] = ['Indoor', 'Outdoor'];

  user : userRetrieveModel | null = null;

  constructor(private companyApi: ApiCompanyService, private router: Router, private deviceApi : ApiDeviceService) {
    const storedUser = localStorage.getItem('user');
    if(storedUser){
      this.user = JSON.parse(storedUser) as userRetrieveModel;
    }
    this.companyOwnerName= this.user?.name || 'Usuario';
    this.companyOwnerEmail = this.user?.email || '';
    this.deviceCompanyId = -1;
    this.possibleDeviceTypes = [];
  }

  ngOnInit(): void {
    this.getDevicesTypes();
    this.getCompanyId();
  }

  getCompanyId(): void {

    const data: GetCompanyRequest = {
      userEmail: this.companyOwnerEmail,
      name: null,
      fullName: null,
    };

    this.companyApi.getCompanies(data).subscribe({
      next: (res: GetCompaniesResponse) => {
        console.log(res)
        let company = res.companies || [];
        this.deviceCompanyId = company[0].id;
      }
    });
  }

  goToLogin(): void {
    this.router.navigate(['/login']);
  }
  goBack(): void {
    this.router.navigate(['/home']);
  }

  getDevicesTypes(): void {
    this.deviceApi.getSupportedDevices().subscribe({
      next: (res: GetDeviceTypesResponse) => {
        this.possibleDeviceTypes = res.deviceTypes || [];
        console.log(res.deviceTypes);
      }
    });
  }

  currentPhotoUrl: string = '';

  addPhotoUrl(): void {
    if (this.currentPhotoUrl.trim()) {
      this.devicePhotoUrls.push(this.currentPhotoUrl.trim());
      this.currentPhotoUrl = '';
    }
  }

  removePhotoUrl(index: number): void {
    this.devicePhotoUrls.splice(index, 1);
  }

  createDevice(name : string, type : string, model : string, description : string, functionalities : string[], locationType : string, photoUrls : string[]): void {
    if (name === '' || type === '' || model === '' || description === '' || functionalities.length === 0 || photoUrls.length === 0) {
      this.feedback = 'Por favor, rellene todos los campos.';
      return;
    }

    if (type === 'SecurityCamera' && locationType === '') {
      this.feedback = 'Por favor, rellene todos los campos.';
      return;
    }

    if (type === 'SecurityCamera' && locationType !== 'Indoor' && locationType !== 'Outdoor') {
      this.feedback = 'Por favor, seleccione un tipo de ubicación válido.';
      return;
    }

    this.getCompanyId();

    if (this.deviceCompanyId == -1) {
      this.feedback = 'No se ha podido obtener el ID de la compañía. Si aun no ha creado una compañía, por favor, hágalo primero.';
      return;
    }

    if (type === 'WindowSensor') {
      if (!functionalities.every(f => this.functionalitiesByType['WindowSensor'].includes(f)) || functionalities.length === 0) {
        this.feedback = 'Por favor, seleccione funcionalidades válidas.';
        return;
      }

      this.deviceApi.postWindowSensor(
        new PostWindowSensorRequest(name, model, description, functionalities, photoUrls, this.deviceCompanyId)
      ).subscribe({
        next: res => {
          this.feedback = 'Dispositivo creado correctamente.';
        },
        error: err => {
          this.feedback = 'Error al crear el dispositivo.';
          console.error(err);
        }
      });

    } else if (type === 'SmartLamp') {
      if (!functionalities.every(f => this.functionalitiesByType['SmartLamp'].includes(f)) || functionalities.length === 0) {
        this.feedback = 'Por favor, seleccione funcionalidades válidas.';
        return;
      }

      this.deviceApi.postSmartLamp(
        new PostSmartLampRequest(name, model, description, functionalities, photoUrls, this.deviceCompanyId)
      ).subscribe({
        next: res => {
          this.feedback = 'Dispositivo creado correctamente.';
        },
        error: err => {
          this.feedback = 'Error al crear el dispositivo.';
          console.error(err);
        }
      });

    } else if (type === 'SecurityCamera') {
      if (!functionalities.every(f => this.functionalitiesByType['SecurityCamera'].includes(f)) || functionalities.length === 0) {
        this.feedback = 'Por favor, seleccione funcionalidades válidas.';
        return;
      }

      this.deviceApi.postSecurityCamera(
        new PostSecurityCameraRequest(name, model, description, functionalities, photoUrls, locationType, this.deviceCompanyId)
      ).subscribe({
        next: res => {
          this.feedback = 'Dispositivo creado correctamente.';
        },
        error: err => {
          this.feedback = 'Error al crear el dispositivo.';
          console.error(err);
        }
      });

    } else if (type === 'MotionSensor') {
      if (!functionalities.every(f => this.functionalitiesByType['MotionSensor'].includes(f)) || functionalities.length === 0) {
        this.feedback = 'Por favor, seleccione funcionalidades válidas.';
        return;
      }

      this.deviceApi.postMotionSensor(
        new PostMotionSensorRequest(name, model, description, functionalities, photoUrls, this.deviceCompanyId)
      ).subscribe({
        next: res => {
          this.feedback = 'Dispositivo creado correctamente.';
        },
        error: err => {
          this.feedback = 'Error al crear el dispositivo.';
          console.error(err);
        }
      });
    }
    }

}

