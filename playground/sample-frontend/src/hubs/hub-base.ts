import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";

export abstract class HubBase {
  private hubConnection: HubConnection | null = null;
  private methodGroupName: string;
  private connected: boolean = false;

  constructor(baseUrl: string, hubName: string, methodGroupName: string) {
    this.hubBuilder(baseUrl, hubName);
    this.methodGroupName = methodGroupName;
  }

  private hubBuilder(baseUrl: string, hubName: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${baseUrl}${hubName}`)
      .configureLogging(LogLevel.Information)
      .withAutomaticReconnect()
      .build();
    //this._hubConnection.serverTimeoutInMilliseconds = 1500000;
  }

  private Init(keyUserRegitered: string): void {
    this.start()
      .then((_) => {
        this.addToGroup(keyUserRegitered);
        this.connected = true;
        console.log(`Hub State :: ${this.connected}`);
      })
      .catch((_) => {
        this.connected = false;
        console.log(`Hub State :: ${this.connected}`);
        console.log("Error :: " + _);
      });
  }

  public on(
    method: string,
    groupKey: string,
    callBack: (...args: any[]) => void
  ): void {
    this.Init(groupKey);
    (this.hubConnection as HubConnection).on(method, callBack);
  }

  private start(): Promise<void> {
    return (this.hubConnection as HubConnection).start();
  }

  private addToGroup(groupKey: string) {
    this.invoke(this.methodGroupName, groupKey)
      .then((_) => {
        console.log(`Join Group :: ${groupKey}`);
      })
      .catch((_) => console.log(_));
  }

  private invoke(methodName: string, ...args: any[]): Promise<any> {
    return (this.hubConnection as HubConnection).invoke(methodName, ...args);
  }
}
