import { Component } from '@angular/core';
import { ApiService } from '../../../shared/api.service';
import { Router } from '@angular/router';
import { DevicesService } from '../../../shared/devices.service';
import {
  GetDeviceResponse,
  GetDeviceTypesResponse, PostMotionSensorRequest, PostSecurityCameraRequest,
  PostSmartLampRequest,
  PostWindowSensorRequest
} from '../../../interfaces/devices';

@Component({
  selector: 'app-create-device',
  templateUrl: './create-device.component.html',
  styleUrls: ['../../../../styles.css']
})
export class CreateDeviceComponent {

  feedback: string = "";

  deviceName: string = '';
  deviceType: string = '';
  deviceModel: string = '';
  deviceDescription: string = '';
  deviceCompanyId: number = -1;
  deviceFunctionalities: string[] = [];
  deviceLocationType: string = '';
  devicePhotoUrls: string[] = [];

  possibleFunctionalitiesWindowSensor: string[] = ['OpenClosed'];
  possibleFunctionalitiesSmartLamp: string[] = ['OnOff'];
  possibleFunctionalitiesSecurityCamera: string[] = ['MotionDetection', 'HumanDetection'];
  possibleFunctionalitiesMotionSensor: string[] = ['MotionDetection'];

  possibleDeviceTypes: string[];

  constructor(private api: ApiService, private router: Router, private apiDevices: DevicesService) {}

  goToLogin(): void {
    this.router.navigate(['/login']);
  }
  goBack(): void {
    this.router.navigate(['/home']);
  }

  getDevicesTypes(): void {
    this.apiDevices.getDeviceTypes().subscribe({
      next: (res: GetDeviceTypesResponse) => {
        this.possibleDeviceTypes = res.deviceTypes || [];
        console.log(res.deviceTypes);
      }
    });
  }

  createDevice(name : string, type : string, model : string, description : string, functionalities : string[], locationType : string, photoUrls : string[]): void {
    if (name === '' || type === '' || model === '' || description === '' || functionalities === []) {
      this.feedback = 'Por favor, rellene todos los campos.';
      return;
    }

    if (type === 'SecurityCamera' && locationType === '') {
      this.feedback = 'Por favor, rellene todos los campos.';
      return;
    }

    if (type === 'WindowSensor') {
      if (!functionalities.every(f => this.possibleFunctionalitiesWindowSensor.includes(f)) || functionalities.length === 0) {
        this.feedback = 'Por favor, seleccione funcionalidades válidas.';
        return;
      }

      this.apiDevices.postWindowSensor(
        new PostWindowSensorRequest(name, model, description, functionalities, photoUrls)
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
      if (!functionalities.every(f => this.possibleFunctionalitiesSmartLamp.includes(f)) || functionalities.length === 0) {
        this.feedback = 'Por favor, seleccione funcionalidades válidas.';
        return;
      }

      this.apiDevices.postSmartLamp(
        new PostSmartLampRequest(name, model, description, functionalities, photoUrls)
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
      if (!functionalities.every(f => this.possibleFunctionalitiesSecurityCamera.includes(f)) || functionalities.length === 0) {
        this.feedback = 'Por favor, seleccione funcionalidades válidas.';
        return;
      }

      this.apiDevices.postSecurityCamera(
        new PostSecurityCameraRequest(name, model, description, functionalities, photoUrls, locationType)
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
      if (!functionalities.every(f => this.possibleFunctionalitiesMotionSensor.includes(f)) || functionalities.length === 0) {
        this.feedback = 'Por favor, seleccione funcionalidades válidas.';
        return;
      }

      this.apiDevices.postMotionSensor(
        new PostMotionSensorRequest(name, model, description, functionalities, photoUrls)
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

