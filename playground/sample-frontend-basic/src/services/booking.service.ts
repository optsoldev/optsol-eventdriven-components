import { Requests } from "../api/requests";
import { HubNamesEnum } from "../context/signalR";
import { useApiRequest } from "../hooks/ApiRequest";

export const useBookingService = () => {
  const resp = useApiRequest<{ correlationId: string }>({
    nomeDoHub: HubNamesEnum.BookingHub,
    eventoDeSucesso: Requests.Booking.Create.hubMethod,
    eventoDeFalha: "",
  });

  const test = console.log;

  const criar = (data: any) =>
    new Promise((resolve, reject) =>
      resp.start({
        data,
        method: "POST",
        url: Requests.Booking.Create.url,
        onCompleted: resolve,
        onFailed: reject,
      })
    );

  return { criar };
};
