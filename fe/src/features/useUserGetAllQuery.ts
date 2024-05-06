import { useQuery } from "react-query";
import { fetchAbstract } from "../utils/fetchAbstract";
import { UserType } from "../types/User";
import { urlBuilder } from "../utils/urlBuilder";

export const useUserGetAllQuery = (filter: { id?: string }) => {
  return useQuery({
    queryKey: ["users", filter],
    queryFn: async () => {
      return (await fetchAbstract(
        urlBuilder("users", filter),
        "GET",
      )) as UserType[];
    },
  });
};
