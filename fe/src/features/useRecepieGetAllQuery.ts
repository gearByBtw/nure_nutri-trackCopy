import { useQuery } from "react-query";
import { fetchAbstract } from "../utils/fetchAbstract";
import { Recepie } from "../types/Recepie";

export const useRecepieGetAllQuery = (filter: { id?: string }) => {
  return useQuery({
    queryKey: ["recepie", filter],
    queryFn: async () => {
      return (await fetchAbstract(
        `recepies/?id=${filter.id}`,
        "GET",
      )) as Recepie[];
    },
  });
};
