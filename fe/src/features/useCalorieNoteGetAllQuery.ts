import { useQuery } from "react-query";
import { fetchAbstract } from "../utils/fetchAbstract";

import { CalorieNote } from "../types/CalorieNote";

export const useCalorieNoteGetAllQuery = (filter: {
  userId?: string;
  createdAt?: string;
  id?: string;
}) => {
  return useQuery({
    queryKey: ["calories-note", filter],
    queryFn: async () => {
      return (await fetchAbstract(
        `calories?userId=${filter.userId}&createdAt=${filter.createdAt}&id=${filter.id}`,
        "GET",
      )) as CalorieNote[];
    },
  });
};
