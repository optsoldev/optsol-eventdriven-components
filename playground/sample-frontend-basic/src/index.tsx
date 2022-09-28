import React from "react";
import ReactDOM from "react-dom/client";
import { SignalRProvider } from "./context/signalR";
import "./index.css";
import { NewApp } from "./NewApp";
import reportWebVitals from "./reportWebVitals";
import AuthService from "./services/keycloak.service";

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);

AuthService.initKeycloak(() =>
  root.render(
    <React.StrictMode>
      <SignalRProvider>
        <NewApp />
      </SignalRProvider>
    </React.StrictMode>
  )
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
