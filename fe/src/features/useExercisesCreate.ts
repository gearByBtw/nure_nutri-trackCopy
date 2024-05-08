import { useMutation } from "react-query";
import { fetchAbstract } from "../utils/fetchAbstract";
import { Exercise } from "../types/Exercise";

export const useExercisesCreate = (
  filter:
    | {
        type: "edit";
        data: { id: string };
      }
    | {
        type: "create";
      },
) => {
  return useMutation((data: Exercise) => {
    if (filter.type === "edit") {
      return fetchAbstract("exercises/" + filter.data.id, "PATCH", data);
    }

    return fetchAbstract("exercises/", "POST", data);
  });
};
