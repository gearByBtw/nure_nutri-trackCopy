import { useMutation } from "react-query";
import { fetchAbstract } from "../utils/fetchAbstract";
import { ExercisesNote } from "../types/ExercisesNote";

export const useExercisesNoteCreate = (
  filter:
    | {
        type: "edit";
        data: { id: string };
      }
    | {
        type: "create";
      },
) => {
  return useMutation((data: ExercisesNote) => {
    if (filter.type === "edit") {
      return fetchAbstract("exercises-notes/" + filter.data.id, "PATCH", data);
    }

    return fetchAbstract("exercises-notes/", "POST", data);
  });
};
