import { useMutation } from "react-query";
import { fetchAbstract } from "../utils/fetchAbstract";

export const useExerciseNoteDelete = () => {
  return useMutation((data: { id: number }) => {
    return fetchAbstract("exercises-notes/" + data.id, "DELETE");
  });
};
