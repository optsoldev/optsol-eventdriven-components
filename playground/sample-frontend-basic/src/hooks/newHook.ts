import { useContext } from "react";
import { SignalRContext, SocketResponse } from "../context/signalR";
import { CorrelationIdApiResponse } from "./ApiRequest";
import { useAxios } from "./axios";

const BASE_URL = "https://localhost:7186";

export function useRequestAPI() {
  const axiosInstance = useAxios(BASE_URL);
  const { getConnection } = useContext(SignalRContext);
  const hubConnection = getConnection();

  return async function newHook<T extends SocketResponse>(
    data: any,
    responseBackMethod: string,
    timeout: number = 20000
  ) {
    return new Promise(async (resolve, reject) => {
      let timer: NodeJS.Timeout;

      function start<Body = undefined>() {
        return axiosInstance
          .request<Body, CorrelationIdApiResponse>({
            method: "POST",
            url: "BookingService",
            data,
          })
          .then((response) => response.correlationId);
      }

      const id = await start();

      function responseHandler(message: T) {
        console.log("handler");

        if (message.correlationId === id) {
          resolve(message);
          clearTimeout(timer);
        }
      }

      hubConnection?.on(responseBackMethod, responseHandler);

      timer = setTimeout(() => {
        console.log("timeout");
        hubConnection?.off(responseBackMethod);
        reject(new Error("Timedout waiting for msg"));
      }, timeout);
    });
  };
}
