type HubMethodsModule = typeof import("./constants");
export type HubMethod = HubMethodsModule[keyof HubMethodsModule];
