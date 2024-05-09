import { Controller, useForm } from "react-hook-form";
import { useNavigate, useParams } from "react-router-dom";
import { useContext, useEffect, useState } from "react";
import {
  Button,
  FormControl,
  MenuItem,
  Select,
  TextField,
} from "@mui/material";
import { UserContext } from "../components/Fallback";
import { UserType } from "../types/User";
import { useUserGetAllQuery } from "../features/useUserGetAllQuery";
import { useUsersCreate } from "../features/useUsersCreate";

const UsersAdd = () => {
  const user = useContext(UserContext);
  const toEdit = useParams().userId;
  const isEdit = toEdit !== undefined;
  const filter = isEdit
    ? {
        id: toEdit,
      }
    : {};
  const items = useUserGetAllQuery(filter);
  const item = items.data?.[0];
  const [error, setError] = useState<string>("");

  const mutation = useUsersCreate(
    isEdit
      ? {
          type: "edit",
          data: {
            id: toEdit || "",
          },
        }
      : {
          type: "create",
        },
  );

  const navigate = useNavigate();

  const form = useForm<UserType>({
    defaultValues: {
      id: "",
      name: "",
      role: "user",
      subscription: "t-1",
      email: "",
      bannedIngredients: "",
      dailyCalories: 0,
    },
  });

  useEffect(() => {
    if (!item || !isEdit) return;

    form.setValue("id", item.id || "");
    form.setValue("name", item.name || "");
    form.setValue("role", item.role || "user");
    form.setValue("subscription", item.subscription || "t-1");
    form.setValue("email", item.email || "");
    form.setValue("bannedIngredients", item.bannedIngredients.join(", ") || "");
    form.setValue("dailyCalories", item.dailyCalories || 0);
  }, [items.data, isEdit, form, item, user.id]);

  const handleCreate = form.handleSubmit((data) => {
    setError("");

    if (!data.name || !data.role || !data.subscription || !data.email) {
      setError("Fill all fields");
      return;
    }

    mutation
      .mutateAsync({
        ...data,
        bannedIngredients: data.bannedIngredients.toString().split(", "),
      })
      .then(() => {
        navigate("/users");
      })
      .catch((err) => {
        setError(err.message);
      });
  });

  const handleReset = () => {
    form.reset();
    setError("");
  };

  if (user.role !== "admin") {
    return <div>Access denied</div>;
  }

  return (
    <>
      <div
        style={{
          paddingInline: 10,
        }}
      >
        <div
          style={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
          }}
        >
          <h4
            style={{
              textTransform: "uppercase",
              fontWeight: "bold",
              marginBlock: 30,
            }}
          >
            {isEdit ? "Edit" : "Add"} Users
            {isEdit && ` # ${toEdit}`}
          </h4>
        </div>

        <div
          style={{
            color: "red",
            paddingBottom: 10,
          }}
        >
          {error && <>Something went wrong: {error}</>}
        </div>

        <div
          style={{
            marginBottom: 20,
            padding: 10,
            border: "1px solid #ccc",
            borderRadius: 5,
          }}
        >
          <form
            style={{
              display: "flex",
              flexDirection: "column",
              justifyContent: "space-between",
              alignItems: "center",
              flexWrap: "wrap",
            }}
          >
            <Controller
              name="name"
              control={form.control}
              render={({ field }) => (
                <FormControl
                  size="small"
                  fullWidth
                  sx={{ m: 1, minWidth: 120, maxWidth: "95%" }}
                >
                  <TextField
                    label="Name"
                    placeholder="John Doe"
                    onChange={field.onChange}
                    value={field.value}
                    size="small"
                    required
                  />
                </FormControl>
              )}
            />

            <Controller
              name="role"
              control={form.control}
              render={({ field }) => (
                <FormControl
                  size="small"
                  fullWidth
                  sx={{ m: 1, minWidth: 120, maxWidth: "95%" }}
                >
                  <Select
                    label="Role"
                    placeholder="Role"
                    onChange={field.onChange}
                    value={field.value}
                    size="small"
                    required
                  >
                    <MenuItem value="admin">Admin</MenuItem>
                    <MenuItem value="user">User</MenuItem>
                  </Select>
                </FormControl>
              )}
            />

            <Controller
              name="subscription"
              control={form.control}
              render={({ field }) => (
                <FormControl
                  size="small"
                  fullWidth
                  sx={{ m: 1, minWidth: 120, maxWidth: "95%" }}
                >
                  <Select
                    label="Subscription"
                    placeholder="Subscription"
                    onChange={field.onChange}
                    value={field.value}
                    size="small"
                    required
                  >
                    <MenuItem value="t-1">T-1</MenuItem>
                    <MenuItem value="t-2">T-2</MenuItem>
                    <MenuItem value="t-3">T-3</MenuItem>
                  </Select>
                </FormControl>
              )}
            />

            <Controller
              name="email"
              control={form.control}
              render={({ field }) => (
                <FormControl
                  size="small"
                  fullWidth
                  sx={{ m: 1, minWidth: 120, maxWidth: "95%" }}
                >
                  <TextField
                    label="Email"
                    placeholder="expample@mail.com"
                    onChange={field.onChange}
                    value={field.value}
                    size="small"
                    required
                  />
                </FormControl>
              )}
            />

            <Controller
              name="bannedIngredients"
              control={form.control}
              render={({ field }) => (
                <FormControl
                  size="small"
                  fullWidth
                  sx={{ m: 1, minWidth: 120, maxWidth: "95%" }}
                >
                  <TextField
                    label="Banned Ingredients"
                    placeholder="ing1, ing2, ing3"
                    onChange={field.onChange}
                    value={field.value}
                    size="small"
                    required
                  />
                </FormControl>
              )}
            />

            <Controller
              name="dailyCalories"
              control={form.control}
              render={({ field }) => (
                <FormControl
                  size="small"
                  fullWidth
                  sx={{ m: 1, minWidth: 120, maxWidth: "95%" }}
                >
                  <TextField
                    label="Daily Calories"
                    placeholder="0"
                    onChange={field.onChange}
                    value={field.value}
                    size="small"
                    required
                    inputProps={{ type: "number" }}
                  />
                </FormControl>
              )}
            />
          </form>

          <div
            style={{ width: "100%", display: "flex", justifyContent: "center" }}
          >
            <Button
              onClick={handleCreate}
              variant="contained"
              sx={{ m: 1, minWidth: 80 }}
            >
              {isEdit ? "Edit" : "Add"}
            </Button>
            <Button
              onClick={handleReset}
              variant="contained"
              color="error"
              sx={{ m: 1, minWidth: 80 }}
            >
              Clear
            </Button>
          </div>
        </div>
      </div>
    </>
  );
};

export default UsersAdd;
