import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";
import {
  createContext,
  PropsWithChildren,
  useEffect,
  useRef,
  useState,
} from "react";

export type SocketResponse = {
  correlationId: string;
};

interface Context {
  hubConnection?: HubConnection;
}

export const SignalRContext = createContext<Context>({});

export const SignalRProvider = ({ children }: PropsWithChildren) => {
  const [ready, setReady] = useState(false);
  const connectionRef = useRef<HubConnection>();

  useEffect(() => {
    connectionRef.current = new HubConnectionBuilder()
      .configureLogging(LogLevel.Debug)
      .withUrl("https://localhost:7186/hubs/bookingNotification")
      .withAutomaticReconnect()
      .build();

    connectionRef.current.start().then(() => setReady(true));
  }, []);

  if (!ready) return null;

  return (
    <SignalRContext.Provider value={{ hubConnection: connectionRef.current }}>
      {children}
    </SignalRContext.Provider>
  );
};
