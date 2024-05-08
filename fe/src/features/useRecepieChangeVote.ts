import { useMutation } from "react-query";
import { fetchAbstract } from "../utils/fetchAbstract";

export const useRecepieChangeVote = () => {
  return useMutation((data: { id: string; type: "up" | "down" }) => {
    return fetchAbstract("recepies/" + data.id + "?type=" + data.type, "PUT");
  });
};
