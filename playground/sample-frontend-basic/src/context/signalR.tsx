import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";
import { createContext, PropsWithChildren } from "react";

export type SocketResponse = {
  correlationId: string;
};

export enum HubConnectionName {
  Booking = "booking",
}
interface Context {
  getConnection: () => HubConnection;
}

export const SignalRContext = createContext<Context>({
  getConnection: () => ({} as HubConnection),
});
export const SignalRProvider = ({ children }: PropsWithChildren) => {
  console.log("SignalRProvider");

  const hubConnection = new HubConnectionBuilder()
    .configureLogging(LogLevel.Debug)
    .withUrl("https://localhost:7186/hubs/bookingNotification")
    .withAutomaticReconnect()
    .build();

  hubConnection.start();

  const getConnection = () => {
    console.log("getConnection");
    return hubConnection;
  };

  return (
    <SignalRContext.Provider value={{ getConnection }}>
      {children}
    </SignalRContext.Provider>
  );
};
