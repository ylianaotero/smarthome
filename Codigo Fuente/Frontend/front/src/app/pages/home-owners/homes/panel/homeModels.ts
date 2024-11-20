
export interface home {
  id: number;
  street: string;
  doorNumber: number;
  latitude: number;
  longitude: number;
}

export interface home {
  id: number;


}

export interface patchDeviceRequest {
  Name: string;
  HardwareId: string;
  HomeId: number;
}

export interface deviceUnit {
  name: string;
  hardwareId: string;
  isConnected: boolean;
  model: number;
  photo: string;
  roomName: string;
  status: string;
}

export class patchDeviceAlias {
  HardwareId: string;
  Name: string;

  constructor(HardwareId: string, Name: string) {
    this.HardwareId = HardwareId;
    this.Name = Name;
  }
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

export class addRoomRequest {
  id: number;
  name: string;

  constructor(id: number, name: string) {
    this.id = id;
    this.name = name;
  }
}


export class addDeviceRequest{
  id: number;
  deviceId : number;
  isConnected: boolean;
  roomName : string;
  constructor(
    id: number,
    deviceId : number,
    isConnected: boolean,
    roomName : string
  ) {
    this.id = id;
    this.deviceId = deviceId;
    this.isConnected = isConnected;
    this.roomName = roomName
  }
}

export class addDeviceToHomeRequest {
  deviceId : number;
  isConnected: boolean;
  roomName : string;
  constructor(
    deviceId : number,
    isConnected: boolean,
    roomName : string
  ) {
    this.deviceId = deviceId;
    this.isConnected = isConnected;
    this.roomName = roomName
  }
}

export class addDeviceToHomeListRequest {
  deviceUnits : addDeviceToHomeRequest[] = [];

  constructor(element?: addDeviceToHomeRequest) {
    if (element) {
      this.addElement(element);
    }
  }

  addElement(element: addDeviceToHomeRequest) {
    this.deviceUnits.push(element);
  }
}



export class addMemberToHomeRequest {
  UserEmail: string = "";
  HasPermissionToListDevices: boolean = false;
  HasPermissionToAddADevice: boolean = false;
  ReceivesNotifications: boolean = false;

  constructor(
    userEmail: string,
    hasPermissionToListDevices: boolean,
    hasPermissionToAddDevice: boolean,
    receivesNotifications: boolean
  ) {
    this.UserEmail = userEmail;
    this.HasPermissionToListDevices = hasPermissionToListDevices;
    this.HasPermissionToAddADevice = hasPermissionToAddDevice;
    this.ReceivesNotifications = receivesNotifications;
  }
}

export class AddRoomToHomeRequest {
  Name: string = "";

  constructor(name: string) {
    this.Name = name;
  }
}


export class ChangeMemberNotificationsRequest {
  IdHome: number;
  MemberEmail: string;
  ReceivesNotifications: boolean;

  constructor(idHome: number, memberEmail: string, receivesNotifications: boolean) {
    this.IdHome = idHome;
    this.MemberEmail = memberEmail;
    this.ReceivesNotifications = receivesNotifications;
  }
}

export class ChangeMemberRequest {
  MemberEmail: string;
  ReceivesNotifications: boolean;

  constructor(memberEmail: string, receivesNotifications: boolean) {
    this.MemberEmail = memberEmail;
    this.ReceivesNotifications = receivesNotifications;
  }
}



export interface member {
  fullName: string;
  email: string;
  photo: string;
  hasPermissionToListDevices: boolean,
  hasPermissionToAddDevice: boolean,
  receivesNotifications: boolean
}




