
//agregar nombre desp
export interface home {
  id: number;
  street: string;
  doorNumber: number;
  latitude: number;
  longitude: number;
}

export class addMemberRequest{
  id: number;
  email: string;
  hasPermissionToListDevices: boolean;
  hasPermissionToAddDevice: boolean;
  recivesNotifications: boolean;
  constructor(
    id:number,
    email: string,
    hasPermissionToListDevices: boolean,
    hasPermissionToAddDevice: boolean,
    recivesNotifications: boolean
  ) {
    this.id = id;
    this.email = email;
    this.hasPermissionToListDevices = hasPermissionToListDevices;
    this.hasPermissionToAddDevice = hasPermissionToAddDevice;
    this.recivesNotifications = recivesNotifications;
  }
}


export class addMemberToHomeRequest {
  userEmail: string;
  hasPermissionToListDevices: boolean;
  hasPermissionToAddDevice: boolean;
  recivesNotifications: boolean;

  constructor(
    userEmail: string,
    hasPermissionToListDevices: boolean,
    hasPermissionToAddDevice: boolean,
    recivesNotifications: boolean
  ) {
    this.userEmail = userEmail;
    this.hasPermissionToListDevices = hasPermissionToListDevices;
    this.hasPermissionToAddDevice = hasPermissionToAddDevice;
    this.recivesNotifications = recivesNotifications;
  }
}

export interface member {
  fullName: string;
  email: string;
  photo: string;
  hasPermissionToListDevices: boolean,
  hasPermissionToAddDevice: boolean,
  recivesNotifications: boolean
}




