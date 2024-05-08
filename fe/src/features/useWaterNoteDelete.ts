import { useMutation } from "react-query";
import { fetchAbstract } from "../utils/fetchAbstract";

export const useWaterNoteDelete = () => {
  return useMutation((data: { id: number }) => {
    return fetchAbstract("water-notes/" + data.id, "DELETE");
  });
};
