// import { useMemo, useState } from "react";
// import { getStyledDataGrid } from "../utils/getStyledDataGrid";
// import {
//   Autocomplete,
//   Button,
//   FormControl,
//   FormHelperText,
//   IconButton,
//   TextField,
// } from "@mui/material";
// import { Link } from "react-router-dom";
// import { Delete, Edit } from "@mui/icons-material";
// import { GridColDef } from "@mui/x-data-grid";
// import { useOrderGetAll } from "../features/useOrderGetAll";
// import { useBurryGetAll } from "../features/useBurryGetAll";
// import { useMutationBurryDelete } from "../features/useMutationBurryDelete";

// const StyledDataGrid = getStyledDataGrid();

// export const Bury = () => {
//   const [filter, setFilter] = useState<
//     | {
//         type: "filter_by_order_id";
//         data: { order_id: string };
//       }
//     | undefined
//   >(undefined);
//   const [accSelect, setAccSelect] = useState<number>(0);

//   const { error, isLoading, data, refetch } = useBurryGetAll(filter);
//   const itemDelete = useMutationBurryDelete();
//   const [localError, setLocalError] = useState<string>("");

//   const ordersList = useOrderGetAll();

//   const rows = data || [];

//   const columns = useMemo(() => {
//     return [
//       {
//         field: "burry_place_id",
//         headerName: "ID",
//         type: "number",
//         sortable: false,
//       },
//       {
//         field: "adress",
//         headerName: "Адреса",
//         type: "string",
//         sortable: false,
//       },
//       {
//         field: "number_of_places",
//         headerName: "Кількість місць",
//         type: "string",
//         sortable: false,
//       },
//       {
//         field: "name_of_burried",
//         headerName: "Імʼя похованого",
//         type: "string",
//         sortable: false,
//       },
//       {
//         field: "date_of_burry",
//         headerName: "Дата поховання",
//         type: "string",
//         sortable: false,
//       },
//       {
//         field: "order_id",
//         headerName: "Номер замовлення",
//         type: "string",
//         sortable: false,
//       },
//       {
//         field: "actions",
//         headerName: "Дії",
//         sortable: false,
//         width: 85,
//         renderCell: (cellValues) => {
//           return (
//             <>
//               <Link to={`/bury/add/${cellValues.row.burry_place_id}`}>
//                 <IconButton aria-label="edit">
//                   <Edit />
//                 </IconButton>
//               </Link>
//               <IconButton
//                 aria-label="delete"
//                 onClick={() => {
//                   const confirm = window.confirm(
//                     `Видалити похоронне місце ${cellValues.row.burry_place_id}?`,
//                   );

//                   if (!confirm) {
//                     return;
//                   }

//                   itemDelete
//                     .mutateAsync({ id: cellValues.row.burry_place_id })
//                     .then(() => {
//                       setLocalError("");
//                       refetch();
//                     })
//                     .catch((error) => {
//                       setLocalError(error.message);
//                     });
//                 }}
//               >
//                 <Delete />
//               </IconButton>
//             </>
//           ) as React.JSX.Element;
//         },
//       },
//     ] as GridColDef[];
//   }, [itemDelete, refetch]);

//   return (
//     <>
//       {error && (
//         <div style={{ color: "red" }}>
//           Щось пішло не так: {(error as Error).message}
//         </div>
//       )}
//       {localError && (
//         <div style={{ color: "red" }}>Щось пішло не так: {localError}</div>
//       )}

//       <div
//         style={{
//           marginBottom: ".25rem",
//         }}
//       >
//         Фільтрувати за замовленням
//         <FormControl
//           size="small"
//           fullWidth
//           error={ordersList.isError}
//           sx={{ m: 1, minWidth: 120, maxWidth: "95%" }}
//         >
//           <Autocomplete
//             disablePortal
//             onChange={(_, value) => {
//               setAccSelect(value || 0);

//               if (!value) {
//                 setFilter(undefined);
//                 return;
//               }

//               setFilter({
//                 type: "filter_by_order_id",
//                 data: {
//                   order_id: value || 0,
//                 },
//               });
//             }}
//             value={accSelect}
//             options={[
//               0,
//               ...(ordersList.data || []).map((account) => account.order_id),
//             ]}
//             getOptionLabel={(option) => {
//               const account = (ordersList.data || []).find(
//                 (account) => account.order_id === option,
//               );
//               return account ? `${account.order_id}` : "";
//             }}
//             renderInput={(params) => (
//               <TextField {...params} label="Замовлення" />
//             )}
//             size="small"
//             disabled={
//               ordersList.isLoading ||
//               (!ordersList.isLoading && ordersList.isError)
//             }
//           />

//           <FormHelperText component="span">
//             {ordersList.isError && <div>Щось пішло не так</div>}
//           </FormHelperText>
//         </FormControl>
//       </div>

//       <div>
//         <Button
//           onClick={() => {
//             setFilter(undefined);
//             setAccSelect(0);
//           }}
//           color="error"
//           variant="contained"
//         >
//           Скинути сортування
//         </Button>
//       </div>

//       <div
//         style={{
//           marginBottom: "1rem",
//         }}
//       >
//         <div
//           style={{
//             display: "flex",
//             justifyContent: "flex-end",
//             marginBottom: ".25rem",
//           }}
//         >
//           <Link to="/bury/add">
//             <Button variant="contained" color="success">
//               Додати похоронне місце
//             </Button>
//           </Link>
//         </div>
//       </div>

//       <div
//         style={{
//           height: 550,
//           borderRadius: "5px",
//           backgroundColor: "#f5f5f5",
//         }}
//       >
//         <StyledDataGrid
//           loading={isLoading}
//           rows={rows}
//           getRowId={(row) => row.burry_place_id}
//           columns={columns}
//           columnBuffer={3}
//           initialState={{
//             pagination: {
//               paginationModel: {
//                 page: 0,
//                 pageSize: 25,
//               },
//             },
//           }}
//           pageSizeOptions={[25, 50, 100]}
//           getRowHeight={() => "auto"}
//           columnHeaderHeight={75}
//           rowSelection={false}
//         />
//       </div>
//     </>
//   );
// };

// export default Bury;
