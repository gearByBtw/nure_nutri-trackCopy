import { useQuery } from "react-query";
import { fetchAbstract } from "../utils/fetchAbstract";
import { Recepie } from "../types/Recepie";

export const useRecepieGetAllQuery = (filter: {}) => {
  return useQuery({
    queryKey: ["recepie", filter],
    queryFn: async () => {
      return (await fetchAbstract(`recepies`, "GET")) as Recepie[];
    },
  });
};
