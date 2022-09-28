export namespace Command {
  export type Accepted = {
    correlationId: string;
  };

  export type Success = {
    id: string;
    version: number;
    userId: string;
  };

  export type Failed = {
    id: string;
    messages: string[];
    userId: string;
  };
}
