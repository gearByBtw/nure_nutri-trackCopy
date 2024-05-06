import { useQuery } from "react-query";
import { fetchAbstract } from "../utils/fetchAbstract";
import { UserType } from "../types/User";

export const useUserGetAllQuery = (filter: { id?: string }) => {
  return useQuery({
    queryKey: ["users", filter],
    queryFn: async () => {
      return (await fetchAbstract(
        `users/?id=${filter.id}`,
        "GET",
      )) as UserType[];
    },
  });
};
