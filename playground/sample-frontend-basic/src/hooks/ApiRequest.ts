import { useContext, useEffect, useRef, useState } from "react";
import { ApiRequest } from "../api/requests";
import { SignalRContext, SocketResponse } from "../context/signalR";
import { useAxios } from "./axios";
const BASE_URL = "https://localhost:7186";

type Props<T> = {
  api: ApiRequest;
  baseURL?: string;
  onError?: (e: Error) => void;
  onCompleted?: (e: T) => void;
};

type ApiRequestStatus =
  | "not-started"
  | "requesting"
  | "waiting"
  | "completed"
  | "error";

interface CorrelationIdApiResponse {
  correlationId: string;
}

export function useApiRequest<T extends SocketResponse>({
  api,
  baseURL,
  onCompleted,
  onError,
}: Props<T>) {
  const [status, setStatus] = useState<ApiRequestStatus>("not-started");
  const [error, setError] = useState<Error>();
  const [response, setResponse] = useState<T>();

  const comp = useRef(onCompleted);
  const correlationId = useRef<string>("");

  const axiosInstance = useAxios(BASE_URL);
  const { hubConnection } = useContext(SignalRContext);

  function start<Body = undefined>(body?: Body) {
    setStatus("requesting");

    axiosInstance
      .request<Body, CorrelationIdApiResponse>({
        method: api.method,
        url: api.url,
        data: body,
      })
      .then((response) => {
        console.log({ response });

        correlationId.current = response.correlationId;
        setStatus("waiting");
      })
      .catch((error) => {
        correlationId.current = "";
        setResponse(undefined);
        setStatus("error");
        setError(error);
      });
  }

  useEffect(() => {
    comp.current = onCompleted;
  }, [onCompleted]);

  useEffect(() => {
    console.log("effect II", correlationId.current);

    onError && error && onError(error);
  }, [error, onError, status]);

  useEffect(() => {
    console.log("effect", api.hubMethod, correlationId.current);

    hubConnection?.on(api.hubMethod, (message: T) => {
      console.log("on Called", message.correlationId, correlationId.current);
      if (message.correlationId === correlationId.current) {
        comp.current && comp.current(message);
        setStatus("completed");
        setResponse(message);
      }
    });
  }, [api.hubMethod, hubConnection]);

  return { start, status, correlationId: correlationId.current, response };
}
