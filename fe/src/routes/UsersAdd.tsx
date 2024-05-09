import { Controller, useForm } from "react-hook-form";
import { useNavigate, useParams } from "react-router-dom";
import { useContext, useEffect, useState } from "react";
import {
  Button,
  Checkbox,
  FormControl,
  FormControlLabel,
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
      bannedIngredients: "" as unknown as [],
      dailyCalories: 0,
      weight: 0,
      desiredWeight: 0,
      hydrated: false,
      exercised: false,
      ateHealthy: false,
      chef: false,
      critic: false,
      criticTwoPointO: false,
      social: false,
    },
  });

  useEffect(() => {
    if (!item || !isEdit) return;

    form.setValue("id", item.id || "");
    form.setValue("name", item.name || "");
    form.setValue("role", item.role || "user");
    form.setValue("subscription", item.subscription || "t-1");
    form.setValue("email", item.email || "");
    form.setValue(
      "bannedIngredients",
      (item.bannedIngredients.join(", ") || "") as unknown as [],
    );
    form.setValue("dailyCalories", item.dailyCalories || 0);
    form.setValue("weight", item.weight || 0);
    form.setValue("desiredWeight", item.desiredWeight || 0);
    form.setValue("hydrated", item.hydrated || false);
    form.setValue("exercised", item.exercised || false);
    form.setValue("ateHealthy", item.ateHealthy || false);
    form.setValue("chef", item.chef || false);
    form.setValue("critic", item.critic || false);
    form.setValue("criticTwoPointO", item.criticTwoPointO || false);
    form.setValue("social", item.social || false);
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

            <Controller
              name="weight"
              control={form.control}
              render={({ field }) => (
                <FormControl
                  size="small"
                  fullWidth
                  sx={{ m: 1, minWidth: 120, maxWidth: "95%" }}
                >
                  <TextField
                    label="Weight"
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

            <Controller
              name="desiredWeight"
              control={form.control}
              render={({ field }) => (
                <FormControl
                  size="small"
                  fullWidth
                  sx={{ m: 1, minWidth: 120, maxWidth: "95%" }}
                >
                  <TextField
                    label="Desired Weight"
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

            <Controller
              name="hydrated"
              control={form.control}
              render={({ field }) => (
                <FormControlLabel
                  control={
                    <Checkbox
                      onChange={field.onChange}
                      value={field.value}
                      size="small"
                    />
                  }
                  label="Hydrated"
                />
              )}
            />

            <Controller
              name="exercised"
              control={form.control}
              render={({ field }) => (
                <FormControlLabel
                  control={
                    <Checkbox
                      onChange={field.onChange}
                      value={field.value}
                      size="small"
                    />
                  }
                  label="Exercised"
                />
              )}
            />

            <Controller
              name="ateHealthy"
              control={form.control}
              render={({ field }) => (
                <FormControlLabel
                  control={
                    <Checkbox
                      onChange={field.onChange}
                      value={field.value}
                      size="small"
                    />
                  }
                  label="Ate Healthy"
                />
              )}
            />

            <Controller
              name="chef"
              control={form.control}
              render={({ field }) => (
                <FormControlLabel
                  control={
                    <Checkbox
                      onChange={field.onChange}
                      value={field.value}
                      size="small"
                    />
                  }
                  label="Chef"
                />
              )}
            />

            <Controller
              name="critic"
              control={form.control}
              render={({ field }) => (
                <FormControlLabel
                  control={
                    <Checkbox
                      onChange={field.onChange}
                      value={field.value}
                      size="small"
                    />
                  }
                  label="Critic"
                />
              )}
            />

            <Controller
              name="criticTwoPointO"
              control={form.control}
              render={({ field }) => (
                <FormControlLabel
                  control={
                    <Checkbox
                      onChange={field.onChange}
                      value={field.value}
                      size="small"
                    />
                  }
                  label="Critic 2.0"
                />
              )}
            />

            <Controller
              name="social"
              control={form.control}
              render={({ field }) => (
                <FormControlLabel
                  control={
                    <Checkbox
                      onChange={field.onChange}
                      value={field.value}
                      size="small"
                    />
                  }
                  label="Social"
                />
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
