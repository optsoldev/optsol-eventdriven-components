import React from "react";
import "./App.css";
import { useApiRequest } from "./hooks/ApiRequest";
import { Requests } from "./api/requests";
import { Booking } from "./api/ApiDomain";

type Props = {};

export const NewApp = (props: Props) => {
  const { start, status, correlationId, response } = useApiRequest<{}>({
    api: Requests.Booking.Create,
  });

  return (
    <div className="App">
      <header className="App-header">
        Socket test
        <button
          onClick={() =>
            start<Booking.Post>({
              userId: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
              from: "string",
              to: "string",
              hotelId: 0,
              departure: "2022-07-11T01:50:51.132Z",
            })
          }
        >
          Test
        </button>
        <span>Status: {status}</span>
        <span>Correlation Id: {correlationId}</span>
        <span>Response: {JSON.stringify(response)}</span>
      </header>
    </div>
  );
};
