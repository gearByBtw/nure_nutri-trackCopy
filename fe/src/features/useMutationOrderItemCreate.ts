// import { useMutation } from "react-query";
// import { fetchAbstract } from "../utils/fetchAbstract";
// import { OrderDocumentType } from "../types/OrderDocument";

// ! example

// export const useMutationOrderItemCreate = (
//   filter:
//     | {
//         type: "edit";
//         data: { id: string };
//       }
//     | {
//         type: "create";
//       },
// ) => {
//   return useMutation((data: OrderDocumentType) => {
//     if (filter.type === "edit") {
//       return fetchAbstract("order_items/" + filter.data.id, "PATCH", data);
//     }

//     return fetchAbstract("order_items/", "POST", data);
//   });
// };
