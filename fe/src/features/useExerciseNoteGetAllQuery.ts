import { useQuery } from "react-query";
import { fetchAbstract } from "../utils/fetchAbstract";

import { urlBuilder } from "../utils/urlBuilder";
import { ExercisesNote } from "../types/ExercisesNote";

export const useExerciseNoteGetAllQuery = (filter: {
  userId?: string;
  createdAt?: string;
  id?: string;
}) => {
  return useQuery({
    queryKey: ["exercises-notes", filter],
    queryFn: async () => {
      return (await fetchAbstract(
        urlBuilder("exercises-notes/", filter),
        "GET",
      )) as ExercisesNote[];
    },
  });
};
