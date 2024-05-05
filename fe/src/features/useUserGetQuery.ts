import { useQuery } from "react-query";
import { fetchAbstract } from "../utils/fetchAbstract";
import { UserType } from "../types/User";

export const useUserGetQuery = (filter: { token: string }) => {
  return useQuery({
    queryKey: ["user", filter],
    queryFn: async () => {
      return (await fetchAbstract(`users/${filter.token}`, "GET")) as UserType;
    },
  });
};
