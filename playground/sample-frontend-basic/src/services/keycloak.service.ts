import Keycloak from "keycloak-js";
import { BASE_CONFIG } from "../shared/baseConfig";

const _kc = new Keycloak({
  url: BASE_CONFIG.Keycloak.URL,
  realm: BASE_CONFIG.Keycloak.Realm ?? "",
  clientId: BASE_CONFIG.Keycloak.ClientID ?? "",
});

/**
 * Initializes Keycloak instance and calls the provided callback function if successfully authenticated.
 *
 * @param onAuthenticatedCallback
 */
const initKeycloak = (onAuthenticatedCallback: any) => {
  console.log("keycloak init");
  _kc
    .init({
      onLoad: "login-required",
      pkceMethod: "S256",
      checkLoginIframe: false,
    })
    .then((authenticated) => {
      if (!authenticated) {
        console.log("user is not authenticated..!");
      }
      onAuthenticatedCallback();
    })
    .catch(console.error);
};

const doLogin = _kc.login;

const doLogout = _kc.logout;

const getToken = () => _kc.token;

const isLoggedIn = () => !!_kc.token;

const updateToken = (successCallback: any) =>
  _kc.updateToken(5).then(successCallback).catch(doLogin);

const getUsername = () => _kc.tokenParsed?.preferred_username;

const hasRole = (roles: any) =>
  roles.some((role: any) => _kc.hasRealmRole(role));

const AuthService = {
  initKeycloak,
  doLogin,
  doLogout,
  isLoggedIn,
  getToken,
  updateToken,
  getUsername,
  hasRole,
};

export default AuthService;
