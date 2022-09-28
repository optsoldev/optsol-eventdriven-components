export const BASE_CONFIG = {
  Keycloak: {
    URL: process.env.REACT_APP_KEYCLOAK_URL,
    Realm: process.env.REACT_APP_KEYCLOAK_REALM,
    ClientID: process.env.REACT_APP_KEYCLOAK_CLIENT_ID,
  },
  HubURL: process.env.REACT_APP_BASE_HUB_URL,
} as const;
