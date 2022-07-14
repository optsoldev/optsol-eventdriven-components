import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import axios from "axios";
import { useEffect, useState } from "react";
import { ApiRequest } from "../api/requests";
const BASE_URL = "https://localhost:7186";

type Props<T> = {
  api: ApiRequest;
  baseURL?: string;
  onError?: (e: Error) => void;
  onCompleted?: (e: T | undefined) => void;
};

type ApiRequestStatus =
  | "not-started"
  | "requesting"
  | "waiting"
  | "completed"
  | "error";

type CorrelationIdApiResponse = {
  correlationId: string;
};

type SocketResponse = {
  correlationId: string;
};

const axiosInstance = axios.create({
  timeout: 30000,
});

const hubConnection = new HubConnectionBuilder()
  .configureLogging(LogLevel.Debug)
  .withUrl("https://localhost:7186/hubs/bookingNotification")
  .withAutomaticReconnect()
  .build();

hubConnection.start();

export function useApiRequest<T>({
  api,
  baseURL,
  onCompleted,
  onError,
}: Props<T>) {
  const [status, setStatus] = useState<ApiRequestStatus>("not-started");
  const [correlationId, setCorrelationId] = useState<string>();
  const [response, setResponse] = useState<T>();
  const [error, setError] = useState<Error>();

  function start<Body = undefined>(body?: Body) {
    setStatus("requesting");

    axiosInstance
      .request<CorrelationIdApiResponse>({
        method: api.method,
        baseURL: !api.external ? baseURL ?? BASE_URL : undefined,
        url: api.url,
        data: body,
      })
      .then((response) => {
        setCorrelationId(response.data.correlationId);
        setStatus("waiting");
      })
      .catch((error) => {
        setResponse(undefined);
        setCorrelationId(undefined);
        setError(error);
        setStatus("error");
      });
  }

  function fireObserverFunctions() {
    switch (status) {
      case "completed":
        onCompleted && onCompleted(response);
        break;
      case "error":
        onError && error && onError(error);
        break;

      default:
        break;
    }
  }

  const handleMessageReceived = (
    message: SocketResponse,
    correlationId: string | undefined
  ) => {
    if (message.correlationId === correlationId) {
      setStatus("completed");
      setResponse(message as any as T);
    }
  };

  useEffect(() => {
    fireObserverFunctions();
  }, [status]);

  useEffect(() => {
    hubConnection.on(api.hubMethod, (message) =>
      handleMessageReceived(message, correlationId)
    );

    return () => {
      hubConnection.off(api.hubMethod);
    };
  }, [correlationId]);

  return { start, status, correlationId, response };
}
