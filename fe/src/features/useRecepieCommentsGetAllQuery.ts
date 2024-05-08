import { useQuery } from "react-query";
import { fetchAbstract } from "../utils/fetchAbstract";

import { urlBuilder } from "../utils/urlBuilder";
import { RecepieComment } from "../types/RecepieComment";

export const useRecepieCommentsGetAllQuery = (filter: {
  recepieId?: string;
}) => {
  return useQuery({
    queryKey: ["recepie-comments", filter],
    queryFn: async () => {
      return (await fetchAbstract(
        urlBuilder("recepie-comments", filter),
        "GET",
      )) as RecepieComment[];
    },
  });
};
