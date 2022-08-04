import axios, { AxiosError, AxiosInstance } from "axios";
import { useCallback, useRef } from "react";

export const useAxios = (url: string) => {
  const ref = useRef(0);
  const token = "";

  const initAxios = useCallback(() => {
    ref.current++;
    console.log("axios", token, url, ref.current);

    const api = axios.create({
      timeout: 30000,
      baseURL: url,
      headers: {
        "Access-Control-Allow-Origin": "*",
        Authorization: `Bearer ${token}`,
      },
    });

    api.interceptors.request.use(
      async (config) => config,
      (error) => {
        if (error.response.status === 403) console.log(error);
        return Promise.reject(error);
      }
    );

    api.interceptors.response.use(
      (axiosResponse) => {
        const responseInvalida = !axiosResponse.data;
        if (responseInvalida)
          console.info("Controller não retornou IActionResult!");

        return axiosResponse.data;
      },
      (error: AxiosError<any>) => {
        return Promise.reject(error.response?.data);
      }
    );

    return api;
  }, [token, url]);

  const axiosInstance = useRef<AxiosInstance>(initAxios());
  return axiosInstance.current;
};
