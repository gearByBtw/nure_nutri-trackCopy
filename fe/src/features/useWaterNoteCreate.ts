import { useMutation } from "react-query";
import { fetchAbstract } from "../utils/fetchAbstract";
import { WaterNote } from "../types/WaterNote";

export const useWaterNoteCreate = (
  filter:
    | {
        type: "edit";
        data: { id: string };
      }
    | {
        type: "create";
      },
) => {
  return useMutation((data: WaterNote) => {
    if (filter.type === "edit") {
      return fetchAbstract("water-notes/" + filter.data.id, "PATCH", data);
    }

    return fetchAbstract("water-notes/", "POST", data);
  });
};
