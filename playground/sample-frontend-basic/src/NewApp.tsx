import { useState } from "react";
import "./App.css";
import { useBookingService } from "./services/booking.service";

type Props = {};

const data = {
  userId: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  from: "string",
  to: "string",
  hotelId: 0,
  departure: "2022-07-11T01:50:51.132Z",
};
export const NewApp = (props: Props) => {
  const [response, setResponse] = useState<any>();
  const [response2, setResponse2] = useState<any>();
  const [response3, setResponse3] = useState<any>();

  const { criar } = useBookingService();

  const handleClick = () => {
    setResponse({});
    return criar(data).then(setResponse);
  };

  const handleClick2 = () => {
    setResponse2({});
    criar(data).then(setResponse2);
  };

  const handleClick3 = () => {
    setResponse3({});
    return criar(data).then(setResponse3);
  };

  const handleAll = () => {
    handleClick();
    handleClick2();
    handleClick3();
  };

  console.log("render App");

  return (
    <div className="App">
      <header className="App-header">
        <span>Response: {JSON.stringify(response)}</span>
        <hr />
        <hr />
        <hr />
        Socket test
        <button onClick={handleClick}>Test</button>
        <span>{JSON.stringify(response)}</span>
        <hr />
        <hr />
        Socket test 1<button onClick={handleClick2}>Test1</button>
        <span>{JSON.stringify(response2)}</span>
        <hr />
        <hr />
        Socket test 1<button onClick={handleClick3}>Test2</button>
        <span>{JSON.stringify(response3)}</span>
        <hr />
        <hr />
        <hr />
        <button onClick={handleAll}>All</button>
        <hr />
      </header>
    </div>
  );
};
