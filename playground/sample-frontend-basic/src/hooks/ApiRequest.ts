import { HubConnection } from "@microsoft/signalr";
import { useEffect, useMemo } from "react";
import { ApiRequest } from "../api/requests";
import { SocketResponse } from "../context/signalR";
import { useAxios } from "./axios";
const BASE_URL = "https://localhost:7186";

type Props = {
  api: ApiRequest;
  hubConnection: HubConnection;
  baseURL?: string;
};

type HubRequest<T> = {
  data: any;
  onError?: (e: Error) => void;
  onCompleted: (e: T) => void;
};

export interface CorrelationIdApiResponse {
  correlationId: string;
}

export function useApiRequest<T extends SocketResponse>({
  api,
  hubConnection,
}: Props) {
  const axiosInstance = useAxios(BASE_URL);
  const requestsMap = useMemo(() => new Map<string, (e: T) => void>(), []);

  async function start<Body = undefined>(request: HubRequest<T>) {
    const response = await axiosInstance.request<
      Body,
      CorrelationIdApiResponse
    >({
      method: api.method,
      url: api.url,
      data: request.data,
    });
    requestsMap.set(response.correlationId, request.onCompleted);
  }

  useEffect(() => {
    hubConnection.on(api.hubMethod, (message: T) => {
      const cb = requestsMap.get(message.correlationId);
      cb && cb(message);
      requestsMap.delete(message.correlationId);
    });

    return () => hubConnection.off(api.hubMethod);
  }, [api.hubMethod, hubConnection, requestsMap]);

  return { start };
}
