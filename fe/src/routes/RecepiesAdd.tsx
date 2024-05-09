import { Controller, useForm } from "react-hook-form";
import { useNavigate, useParams } from "react-router-dom";
import { useContext, useEffect, useState } from "react";
import {
  Button,
  Checkbox,
  FormControl,
  FormControlLabel,
  TextField,
} from "@mui/material";
import { UserContext } from "../components/Fallback";
import { useRecepieGetAllQuery } from "../features/useRecepieGetAllQuery";
import { useRecepieCreate } from "../features/useRecepieCreate";
import { Recepie } from "../types/Recepie";
import { useUserDoneAchievement } from "../features/useUserDoneAchievement";

const RecepiesAdd = () => {
  const user = useContext(UserContext);
  const toEdit = useParams().id;
  const isEdit = toEdit !== undefined;
  const filter = isEdit
    ? {
        id: toEdit,
      }
    : {};
  const items = useRecepieGetAllQuery(filter);
  const item = items.data?.[0];

  const achieve = useUserDoneAchievement();

  const [error, setError] = useState<string>("");

  const mutation = useRecepieCreate(
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

  const form = useForm<Recepie>({
    defaultValues: {
      id: "",
      name: "",
      ingredients: "",
      calories: 0,
      description: "",
      votes: 0,
      isPremium: false,
      isCreatedByUser: user.role === "admin",
    },
  });

  useEffect(() => {
    if (!item || !isEdit) return;

    form.setValue("id", item.id || "");
    form.setValue("name", item.name || "");
    form.setValue("ingredients", item.ingredients.join(", ") || "");
    form.setValue("calories", item.calories || 0);
    form.setValue("description", item.description || "");
    form.setValue("votes", item.votes || 0);
    form.setValue("isPremium", item.isPremium || false);
    form.setValue(
      "isCreatedByUser",
      item.isCreatedByUser || user.role === "admin",
    );
  }, [items.data, isEdit, form, item, user.id, user.role]);

  const handleCreate = form.handleSubmit((data) => {
    setError("");

    if (
      !data.name ||
      !data.ingredients.length ||
      !data.calories ||
      !data.description
    ) {
      setError("Fill all fields");
      return;
    }

    mutation
      .mutateAsync({
        ...data,
        ingredients: data.ingredients.toString().split(", "),
      })
      .then(() => {
        achieve
          .mutateAsync({
            id: user.id,
            achievement: "chef",
          })
          .then(() => {
            navigate("/recepies");
          });
      })
      .catch((err) => {
        setError(err.message);
      });
  });

  const handleReset = () => {
    form.reset();
    setError("");
  };

  if (user.role !== "admin" && item && !item.isCreatedByUser) {
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
            {isEdit ? "Edit" : "Add"} Recipe
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
                    placeholder="Lasagna"
                    onChange={field.onChange}
                    value={field.value}
                    size="small"
                    required
                  />
                </FormControl>
              )}
            />

            <Controller
              name="calories"
              control={form.control}
              render={({ field }) => (
                <FormControl
                  size="small"
                  fullWidth
                  sx={{ m: 1, minWidth: 120, maxWidth: "95%" }}
                >
                  <TextField
                    label="Calories"
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
              name="description"
              control={form.control}
              render={({ field }) => (
                <FormControl
                  size="small"
                  fullWidth
                  sx={{ m: 1, minWidth: 120, maxWidth: "95%" }}
                >
                  <TextField
                    label="Description"
                    placeholder="Description"
                    onChange={field.onChange}
                    value={field.value}
                    size="small"
                    required
                    multiline
                  />
                </FormControl>
              )}
            />

            <Controller
              name="ingredients"
              control={form.control}
              render={({ field }) => (
                <FormControl
                  size="small"
                  fullWidth
                  sx={{ m: 1, minWidth: 120, maxWidth: "95%" }}
                >
                  <TextField
                    label="Ingredients"
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
              name="isPremium"
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
                  label="Premium"
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

export default RecepiesAdd;
