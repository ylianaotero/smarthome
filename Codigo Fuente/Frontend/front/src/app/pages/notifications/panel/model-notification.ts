
export interface GetNotificationResponse {
  hardwareId: string;
  deviceKind: string;
  event: string;
  read: boolean;
  readAt: string;
  createdAt: string;
}

export class NotificationsFilterRequestModel {
  Read: boolean | null = null;
  Kind: string | null = null;
  ReadDate: Date | null = null;
  CreatedDate: Date | null = null;

  constructor(
    read: boolean | null,
    kind: string | null,
    readDate: Date | null,
    createdDate: Date | null
  ) {
    this.Read = read;
    this.Kind = kind;
    this.ReadDate = readDate;
    createdDate = createdDate;
  }
}

