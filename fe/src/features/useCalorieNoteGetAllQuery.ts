import { useQuery } from "react-query";
import { fetchAbstract } from "../utils/fetchAbstract";

import { CalorieNote } from "../types/CalorieNote";
import { urlBuilder } from "../utils/urlBuilder";

export const useCalorieNoteGetAllQuery = (filter: {
  userId?: string;
  createdAt?: string;
  id?: string;
}) => {
  return useQuery({
    queryKey: ["calories-note", filter],
    queryFn: async () => {
      return (await fetchAbstract(
        urlBuilder("calories", filter),
        "GET",
      )) as CalorieNote[];
    },
  });
};
