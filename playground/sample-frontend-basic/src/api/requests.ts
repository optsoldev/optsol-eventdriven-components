import { HubMethod } from "../shared/HubMethods";

interface ApiModel {
  [key: string]: {
    [key: string]: ApiRequest;
  };
}

export type ApiRequest = {
  url: string;
  method: "POST" | "GET" | "PUT" | "DELETE";
  hubMethod: HubMethod;
  external?: boolean;
};

export const Requests = {
  Booking: {
    Create: {
      method: "POST",
      url: "BookingService",
      hubMethod: "TravelBooked",
    } as ApiRequest,
  },
};
