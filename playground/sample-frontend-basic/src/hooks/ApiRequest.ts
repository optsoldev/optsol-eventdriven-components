import { HubConnection } from "@microsoft/signalr";
import { useCallback, useEffect, useRef } from "react";
import {
  HubNamesEnum,
  SocketResponse,
  useSignalRContext,
} from "../context/signalR";
import { Command } from "../models/Command";
import { useAxios } from "./axios";
const BASE_URL = "https://localhost:7186";

type CallbackSuccess = (value: Command.Success) => void;
type CallbackFailed = (value: Command.Failed) => void;
type MapCallback = {
  callbackSuccess: CallbackSuccess;
  callbackFailed: CallbackFailed;
};

type Props = {
  nomeDoHub: HubNamesEnum;
  eventoDeSucesso: string;
  eventoDeFalha: string;
};

type HubRequest = {
  data: any;
  url: string;
  method: "POST" | "PUT" | "DELETE" | "PATCH";
  onCompleted: CallbackSuccess;
  onFailed: CallbackFailed;
};
export function useApiRequest<T extends SocketResponse>({
  nomeDoHub,
  eventoDeFalha,
  eventoDeSucesso,
}: Props) {
  const axios = useAxios(BASE_URL);
  const { getConnection } = useSignalRContext();
  const requestsMap = useRef(new Map<string, MapCallback>());

  const conexao = useRef<HubConnection | undefined>(getConnection(nomeDoHub));

  console.log("command");

  const start = useCallback(
    (request: HubRequest) => {
      // const contentType = equest.formData ? 'multipart/form-data' : 'application/json';
      const contentType = "application/json";

      axios
        .request<Command.Accepted, Command.Accepted>({
          method: request.method,
          url: request.url,
          data: request.data,
          headers: {
            "Content-Type": contentType,
          },
        })
        .then((resp) => {
          console.log(resp);
          requestsMap.current.set(resp.correlationId, {
            callbackSuccess: request.onCompleted,
            callbackFailed: request.onFailed,
          });
        });
    },
    [axios]
  );

  const listen = useCallback(async () => {
    console.log("listen");

    console.log(
      "listen",
      conexao.current?.state,
      requestsMap,
      eventoDeSucesso,
      eventoDeFalha
    );
    conexao.current?.on(eventoDeSucesso, (mensagem: Command.Success) => {
      console.log("on success", requestsMap);

      const mapCallback = requestsMap.current.get(mensagem.id);
      if (mapCallback) {
        mapCallback.callbackSuccess(mensagem);
        requestsMap.current.delete(mensagem.id);
      }
    });

    conexao.current?.on(eventoDeFalha, (mensagem: Command.Failed) => {
      console.log("on fail", requestsMap);

      const mapCallback = requestsMap.current.get(mensagem.id);
      if (mapCallback) {
        mapCallback.callbackFailed(mensagem);
        requestsMap.current.delete(mensagem.id);
      }
    });
  }, [eventoDeFalha, eventoDeSucesso]);

  const unsub = useCallback(() => {
    console.log("unsub");
    if (!conexao.current) return;
    console.log("unsub after");
    conexao.current.off(eventoDeSucesso);
    conexao.current.off(eventoDeFalha);
  }, [eventoDeFalha, eventoDeSucesso]);

  useEffect(() => {
    console.log("effect 1");
    if (!conexao.current) return;
    console.log("effect 1 after");

    conexao.current?.onreconnecting(() => {
      console.log("reconnecting");
      unsub();
    });
    conexao.current?.onreconnected(() => {
      console.log("reconnected");
      listen();
    });
  }, [listen, unsub]);

  useEffect(() => {
    console.log("effect 2");
    listen();
    console.log("effect 2 after");
    return () => {
      console.log("unsub effect");
      unsub();
    };
  }, [listen, unsub]);

  return { start };
}
