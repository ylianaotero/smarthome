export class createHomeModel {
  OwnerId: number;
  Street: string;
  DoorNumber: number;
  Latitude: number;
  Longitude: number;
  MaximumMembers: number;

  constructor(
    OwnerId: number,
    Street: string,
    DoorNumber: number,
    Latitude: number,
    Longitude: number,
    MaximumMembers: number
  ) {
    this.OwnerId = OwnerId;
    this.Street = Street;
    this.DoorNumber = DoorNumber;
    this.Latitude = Latitude;
    this.Longitude = Longitude;
    this.MaximumMembers = MaximumMembers;
  }
}

export interface homeRetrieveModel{
  Street: string;
  DoorNumber: number;
  Latitude: number;
  Longitude: number;
}

