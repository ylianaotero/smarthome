
export interface GetNotificationResponse {
  hardwareId: string;
  deviceKind: string;
  event: string;
  read: boolean;
  readAt: string;
}

export class NotificationsFilterRequestModel {
  Read: boolean | null = null;
  Kind: string | null = null;
  Date: Date | null = null;
  constructor(
    read: boolean | null,
    kind: string | null,
    date: Date | null
  ) {
    this.Read = read;
    this.Kind = kind;
    this.Date = date
  }
}

