import { HubBase } from "./hub-base";

export default class BookingNotificationHub extends HubBase {
  constructor() {
    super("https://localhost:7186/hubs/", "bookingNotification", "");
  }
}
