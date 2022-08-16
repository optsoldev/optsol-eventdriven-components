import { useContext } from "react";
import { Requests } from "../api/requests";
import { SignalRContext } from "../context/signalR";
import { useApiRequest } from "../hooks/ApiRequest";

export const useBookingService = () => {
  const { getConnection } = useContext(SignalRContext);
  const hubConnection = getConnection();

  const resp = useApiRequest<{ correlationId: string }>({
    api: Requests.Booking.Create,
    hubConnection,
  });

  const test = console.log;

  const criar = (data: any) =>
    new Promise((resolve, reject) =>
      resp.start({
        data,
        onCompleted: resolve,
        onError: reject,
      })
    );

  return { criar };
};
