import { useQuery } from "react-query";
import { fetchAbstract } from "../utils/fetchAbstract";
import { urlBuilder } from "../utils/urlBuilder";
import { Exercise } from "../types/Exercise";

export const useExerciseGetAllQuery = (filter: { id?: string }) => {
  return useQuery({
    queryKey: ["exercises", filter],
    queryFn: async () => {
      return (await fetchAbstract(
        urlBuilder("exercises", filter),
        "GET",
      )) as Exercise[];
    },
  });
};
