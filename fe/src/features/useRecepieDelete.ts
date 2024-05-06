import { useMutation } from "react-query";
import { fetchAbstract } from "../utils/fetchAbstract";

export const useRecepieDelete = () => {
  return useMutation((data: { id: number }) => {
    return fetchAbstract("recepies/" + data.id, "DELETE");
  });
};
