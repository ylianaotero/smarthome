export interface deviceModel {
  name: string;
  model: string;
  photoUrl?: string;
  companyName?: string;
}

export class DeviceFilterRequestModel {
  Name: string | undefined = undefined;
  Model: string | undefined = undefined;
  Company: string | undefined = undefined;
  Kind: string | undefined = undefined;

  constructor(
    name: string | undefined,
    model: string | undefined,
    company: string | undefined,
    kind: string | undefined
  ) {
    this.Name = name;
    this.Model = model;
    this.Company = company;
    this.Kind = kind;
  }
}
