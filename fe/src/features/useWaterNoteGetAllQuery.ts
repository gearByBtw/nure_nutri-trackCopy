import { useQuery } from "react-query";
import { fetchAbstract } from "../utils/fetchAbstract";

import { urlBuilder } from "../utils/urlBuilder";
import { WaterNote } from "../types/WaterNote";

export const useWaterNoteGetAllQuery = (filter: {
  userId?: string;
  createdAt?: string;
  id?: string;
}) => {
  return useQuery({
    queryKey: ["water-note", filter],
    queryFn: async () => {
      return (await fetchAbstract(
        urlBuilder("water-notes", filter),
        "GET",
      )) as WaterNote[];
    },
  });
};
