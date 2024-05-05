// import { Controller, useForm } from "react-hook-form";
// import { useNavigate, useParams } from "react-router-dom";
// import { useEffect, useState } from "react";
// import {
//   Autocomplete,
//   Button,
//   FormControl,
//   FormHelperText,
//   TextField,
// } from "@mui/material";
// import { useOrderGetAll } from "../features/useOrderGetAll";
// import { useBurryGetAll } from "../features/useBurryGetAll";
// import { useMutationBurryCreate } from "../features/useMutationBurryCreate";
// import { BurryType } from "../types/Burry";

// const BuryAdd = () => {
//   const buryToEditId = useParams().buryId;
//   const isEdit = buryToEditId !== undefined;
//   const filter = isEdit
//     ? {
//         type: "one" as const,
//         data: {
//           id: buryToEditId,
//         },
//       }
//     : undefined;
//   const items = useBurryGetAll(filter);
//   const [error, setError] = useState<string>("");

//   const ordersList = useOrderGetAll();

//   const ItemMut = useMutationBurryCreate(
//     isEdit
//       ? {
//           type: "edit",
//           data: {
//             id: buryToEditId || "",
//           },
//         }
//       : {
//           type: "create",
//         },
//   );

//   const navigate = useNavigate();

//   const form = useForm<BurryType>({
//     defaultValues: {
//       adress: "",
//       number_of_places: 0,
//       name_of_burried: "",
//       date_of_burry: "",
//       order_id: 0,
//     },
//   });

//   useEffect(() => {
//     if (!items.data || Array.isArray(items.data) || !isEdit) return;

//     const item = items.data;

//     form.setValue("adress", item.adress || "");
//     form.setValue("number_of_places", item.number_of_places || 0);
//     form.setValue("name_of_burried", item.name_of_burried || "");
//     form.setValue("date_of_burry", item.date_of_burry || "");
//     form.setValue("order_id", item.order_id || 0);
//   }, [items.data, isEdit, form]);

//   const handleCreate = form.handleSubmit((data) => {
//     setError("");

//     const nameSurnamePattern = /^[A-Za-zА-Яа-я]+ [A-Za-zА-Яа-я]+$/;
//     const datePattern = /^\d{4}-\d{2}-\d{2}$/;

//     if (
//       !data.adress ||
//       !data.number_of_places ||
//       (data.name_of_burried &&
//         !nameSurnamePattern.test(data.name_of_burried)) ||
//       (data.date_of_burry && !datePattern.test(data.date_of_burry))
//     ) {
//       setError("Заповніть всі обовʼязкові поля");
//       return;
//     }

//     ItemMut.mutateAsync({
//       ...data,
//       name_of_burried: data.name_of_burried || null,
//       date_of_burry: data.date_of_burry || null,
//       order_id: data.order_id || null,
//     })
//       .then(() => {
//         navigate("/bury");
//       })
//       .catch((err) => {
//         setError(err.message);
//       });
//   });

//   const handleReset = () => {
//     form.reset();
//     setError("");
//   };

//   return (
//     <>
//       <div
//         style={{
//           paddingInline: 10,
//         }}
//       >
//         <div
//           style={{
//             display: "flex",
//             justifyContent: "space-between",
//             alignItems: "center",
//           }}
//         >
//           <h4
//             style={{
//               textTransform: "uppercase",
//               fontWeight: "bold",
//               marginBlock: 30,
//             }}
//           >
//             {isEdit ? "Змінити" : "Створити"} поховальне місце
//             {isEdit && ` # ${buryToEditId}`}
//           </h4>
//         </div>

//         <div
//           style={{
//             color: "red",
//             paddingBottom: 10,
//           }}
//         >
//           {error && <>Щось пішло не так: {error}</>}
//         </div>

//         <div
//           style={{
//             marginBottom: 20,
//             padding: 10,
//             border: "1px solid #ccc",
//             borderRadius: 5,
//           }}
//         >
//           <form
//             style={{
//               display: "flex",
//               flexDirection: "column",
//               justifyContent: "space-between",
//               alignItems: "center",
//               flexWrap: "wrap",
//             }}
//           >
//             <Controller
//               name="adress"
//               control={form.control}
//               render={({ field }) => (
//                 <FormControl
//                   size="small"
//                   fullWidth
//                   sx={{ m: 1, minWidth: 120, maxWidth: "95%" }}
//                 >
//                   <TextField
//                     label="Адреса"
//                     placeholder="12d-2"
//                     onChange={field.onChange}
//                     value={field.value}
//                     size="small"
//                     required
//                   />
//                 </FormControl>
//               )}
//             />

//             <Controller
//               name="number_of_places"
//               control={form.control}
//               render={({ field }) => (
//                 <FormControl
//                   size="small"
//                   fullWidth
//                   sx={{ m: 1, minWidth: 120, maxWidth: "95%" }}
//                 >
//                   <TextField
//                     label="Кількість місць"
//                     placeholder="123"
//                     onChange={field.onChange}
//                     value={field.value}
//                     size="small"
//                     required
//                     type="number"
//                   />
//                 </FormControl>
//               )}
//             />

//             <Controller
//               name="name_of_burried"
//               control={form.control}
//               render={({ field }) => (
//                 <FormControl
//                   size="small"
//                   fullWidth
//                   sx={{ m: 1, minWidth: 120, maxWidth: "95%" }}
//                 >
//                   <TextField
//                     label="Імʼя похованого"
//                     placeholder="Василь Грищенко"
//                     onChange={field.onChange}
//                     value={field.value}
//                     size="small"
//                   />
//                 </FormControl>
//               )}
//             />

//             <Controller
//               name="date_of_burry"
//               control={form.control}
//               render={({ field }) => (
//                 <FormControl
//                   size="small"
//                   fullWidth
//                   sx={{ m: 1, minWidth: 120, maxWidth: "95%" }}
//                 >
//                   <TextField
//                     label="Дата поховання"
//                     placeholder="yyyy-mm-dd"
//                     onChange={field.onChange}
//                     value={field.value}
//                     size="small"
//                   />
//                 </FormControl>
//               )}
//             />

//             <Controller
//               name="order_id"
//               control={form.control}
//               render={({ field }) => (
//                 <FormControl
//                   size="small"
//                   fullWidth
//                   error={ordersList.isError}
//                   sx={{ m: 1, minWidth: 120, maxWidth: "95%" }}
//                 >
//                   <Autocomplete
//                     disablePortal
//                     onChange={(_, value) => {
//                       field.onChange(value || 0);
//                     }}
//                     value={field.value}
//                     options={[
//                       0,
//                       ...(ordersList.data || []).map(
//                         (account) => account.order_id,
//                       ),
//                     ]}
//                     getOptionLabel={(option) => {
//                       const account = (ordersList.data || []).find(
//                         (account) => account.order_id === option,
//                       );
//                       return account ? `${account.order_id}` : "";
//                     }}
//                     renderInput={(params) => (
//                       <TextField {...params} label="Замовлення" />
//                     )}
//                     size="small"
//                     disabled={
//                       ordersList.isLoading ||
//                       (!ordersList.isLoading && ordersList.isError)
//                     }
//                   />

//                   <FormHelperText component="span">
//                     {ordersList.isError && <div>Щось пішло не так</div>}
//                   </FormHelperText>
//                 </FormControl>
//               )}
//             />
//           </form>

//           <div
//             style={{ width: "100%", display: "flex", justifyContent: "center" }}
//           >
//             <Button
//               onClick={handleCreate}
//               variant="contained"
//               sx={{ m: 1, minWidth: 80 }}
//             >
//               {isEdit ? "Змінити" : "Створити"}
//             </Button>
//             <Button
//               onClick={handleReset}
//               variant="contained"
//               color="error"
//               sx={{ m: 1, minWidth: 80 }}
//             >
//               Очистити
//             </Button>
//           </div>
//         </div>
//       </div>
//     </>
//   );
// };

// export default BuryAdd;
