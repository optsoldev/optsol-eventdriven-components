import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";
import {
  createContext,
  PropsWithChildren,
  useCallback,
  useContext,
  useMemo,
} from "react";
import AuthService from "../services/keycloak.service";
import { BASE_CONFIG } from "../shared/baseConfig";

export type SocketResponse = {
  correlationId: string;
};

export enum HubConnectionName {
  Booking = "booking",
}
export enum HubNamesEnum {
  BookingHub = "/hubs/bookingNotification",
}

export type HubInfoDTO = {
  name: HubNamesEnum;
  url: string;
};

export interface ISignalRContext {
  getConnection: (name: HubNamesEnum) => HubConnection;
}

export type SignalRProviderProps = {
  hubs: HubInfoDTO[];
};

const hubs: HubInfoDTO[] = [
  { name: HubNamesEnum.BookingHub, url: BASE_CONFIG.HubURL ?? "" },
];

export const SignalRContext = createContext<ISignalRContext>(
  {} as ISignalRContext
);

function SignalRProvider({ children }: PropsWithChildren) {
  const conexoes = useMemo(() => new Map<string, HubConnection>(), []);
  const accessTokenFactory = useCallback(
    (): string => AuthService.getToken() ?? "",
    []
  );

  const getConnection = useCallback(
    (name: HubNamesEnum) => {
      console.log("get connection", name, conexoes, conexoes.get(name));
      const hub = hubs.find((hub) => hub.name === name);
      if (!hub) throw new Error("URL de conexao do Hub nao foi encontrada!");

      const conn = conexoes.get(name);
      if (conn) return conn;

      console.log("create connection");
      const hubConnection = new HubConnectionBuilder()
        .configureLogging(LogLevel.Debug)
        .withUrl(hub.url + "/hubs/bookingNotification", {
          //skipNegotiation: true,
          //transport: HttpTransportType.WebSockets,
          //accessTokenFactory: accessTokenFactory,
        })
        .withAutomaticReconnect()
        .build();
      hubConnection.start();

      conexoes.set(name, hubConnection);

      return hubConnection;
    },
    [conexoes]
  );

  return (
    <SignalRContext.Provider
      value={{
        getConnection,
      }}
    >
      {children}
    </SignalRContext.Provider>
  );
}

function useSignalRContext() {
  const { getConnection } = useContext(SignalRContext);

  if (getConnection === undefined) {
    throw new Error(
      "useSignalRContext deve ser utilizando dentro de um SignalRProvider"
    );
  }

  return {
    getConnection,
  };
}

export { SignalRProvider, useSignalRContext };
