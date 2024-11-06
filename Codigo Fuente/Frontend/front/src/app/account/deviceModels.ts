export interface deviceModel {
  name: string;
  model: number;
  photoUrl?: string;
  companyName?: string;
}

export class DeviceFilterRequestModel {
  Name: string | undefined = undefined;
  Model: number | undefined = undefined;
  Company: string | undefined = undefined;
  Kind: string | undefined = undefined;

  constructor(
    name: string | undefined,
    model: number | undefined,
    company: string | undefined,
    kind: string | undefined
  ) {
    this.Name = name;
    this.Model = model;
    this.Company = company;
    this.Kind = kind;
  }
}