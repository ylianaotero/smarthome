export interface GetDeviceRequest {
    name: string | null;
    model: string | null;
    company: string | null;
    kind: string | null;
}

export interface GetDevicesResponse {
    devices: GetDeviceResponse[];
    totalCount: number;
}

export interface GetDeviceResponse {
    name: string;
    model: string;
    photoUrl: string | null;
    companyName: string | null;
}

export interface GetDeviceTypesResponse {
    deviceTypes: string[];
}