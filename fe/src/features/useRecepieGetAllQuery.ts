import { useQuery } from "react-query";
import { fetchAbstract } from "../utils/fetchAbstract";
import { Recepie } from "../types/Recepie";
import { urlBuilder } from "../utils/urlBuilder";

export const useRecepieGetAllQuery = (filter: { id?: string }) => {
  return useQuery({
    queryKey: ["recepies", filter],
    queryFn: async () => {
      return (await fetchAbstract(
        urlBuilder("recepies", filter),
        "GET",
      )) as Recepie[];
    },
  });
};
