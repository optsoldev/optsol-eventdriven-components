import { Booking } from "./api/ApiDomain";
import { Requests } from "./api/requests";
import "./App.css";
import { useApiRequest } from "./hooks/ApiRequest";

type Props = {};

export const NewApp = (props: Props) => {
  const resp = useApiRequest<{ correlationId: string }>({
    api: Requests.Booking.Create,
    onCompleted: (resp) => console.log("fazer outra coisa", resp),
  });

  const resp1 = useApiRequest<{ correlationId: string }>({
    api: Requests.Booking.Create,
  });

  const resp2 = useApiRequest<{ correlationId: string }>({
    api: Requests.Booking.Create,
    onCompleted: (resp) => console.log("fazer outra coisa II", resp),
  });

  const data = {
    userId: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    from: "string",
    to: "string",
    hotelId: 0,
    departure: "2022-07-11T01:50:51.132Z",
  };

  const handleClick = () => resp.start<Booking.Post>(data);
  const handleClick1 = () => resp1.start<Booking.Post>(data);
  const handleClick2 = () => resp2.start<Booking.Post>(data);

  const handleAll = () => {
    handleClick();
    handleClick1();
    handleClick2();
  };

  console.log("render App");

  return (
    <div className="App">
      <header className="App-header">
        Socket test
        <button onClick={handleClick}>Test</button>
        <span>Status: {resp.status}</span>
        <span>Correlation Id: {resp.correlationId}</span>
        <span>Response: {JSON.stringify(resp.response)}</span>
        <hr />
        <hr />
        Socket test 1<button onClick={handleClick1}>Test1</button>
        <span>Status: {resp1.status}</span>
        <span>Correlation Id: {resp1.correlationId}</span>
        <span>Response: {JSON.stringify(resp1.response)}</span>
        <hr />
        <hr />
        Socket test 1<button onClick={handleClick2}>Test2</button>
        <span>Status: {resp2.status}</span>
        <span>Correlation Id: {resp2.correlationId}</span>
        <span>Response: {JSON.stringify(resp2.response)}</span>
        <hr />
        <hr />
        <hr />
        <button onClick={handleAll}>All</button>
      </header>
    </div>
  );
};
