export class createHomeModel {
  OwnerId: number;
  Street: string;
  DoorNumber: number;
  Latitude: number;
  Longitude: number;
  MaximumMembers: number;
  Alias: string;

  constructor(
    OwnerId: number,
    Street: string,
    DoorNumber: number,
    Latitude: number,
    Longitude: number,
    MaximumMembers: number,
    Alias: string
) {
    this.OwnerId = OwnerId;
    this.Street = Street;
    this.DoorNumber = DoorNumber;
    this.Latitude = Latitude;
    this.Longitude = Longitude;
    this.MaximumMembers = MaximumMembers;
    this.Alias = Alias;
  }
}

export interface homeRetrieveModel{
  Street: string;
  DoorNumber: number;
  Latitude: number;
  Longitude: number;
  Alias: string;
}

