import { useMutation } from "react-query";
import { fetchAbstract } from "../utils/fetchAbstract";
import { RecepieComment } from "../types/RecepieComment";

export const useRecepieCommentCreate = () => {
  return useMutation((data: RecepieComment) => {
    return fetchAbstract("recepie-comments/", "POST", data);
  });
};
