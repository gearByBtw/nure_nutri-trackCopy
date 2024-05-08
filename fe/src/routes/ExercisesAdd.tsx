import { Controller, useForm } from "react-hook-form";
import { useNavigate, useParams } from "react-router-dom";
import { useContext, useEffect, useState } from "react";
import { Button, FormControl, TextField } from "@mui/material";
import { UserContext } from "../components/Fallback";
import { useExerciseGetAllQuery } from "../features/useExerciseGetAllQuery";
import { Exercise } from "../types/Exercise";
import { useExercisesCreate } from "../features/useExercisesCreate";

const ExercisesAdd = () => {
  const user = useContext(UserContext);
  const toEdit = useParams().id;
  const isEdit = toEdit !== undefined;
  const filter = isEdit
    ? {
        id: toEdit,
      }
    : {};
  const items = useExerciseGetAllQuery(filter);
  const item = items.data?.[0];
  const [error, setError] = useState<string>("");

  const mutation = useExercisesCreate(
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

  const form = useForm<Exercise>({
    defaultValues: {
      id: "",
      name: "",
      calories: 0,
      description: "",
    },
  });

  useEffect(() => {
    if (!item || !isEdit) return;

    form.setValue("id", item.id || "");
    form.setValue("name", item.name || "");
    form.setValue("calories", item.calories || 0);
    form.setValue("description", item.description || "");
  }, [items.data, isEdit, form, item, user.id]);

  const handleCreate = form.handleSubmit((data) => {
    setError("");

    if (!data.name || !data.calories || !data.description) {
      setError("Fill all fields");
      return;
    }

    mutation
      .mutateAsync(data)
      .then(() => {
        navigate("/exercises");
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
            {isEdit ? "Edit" : "Add"} Exercises
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
                    placeholder="Push-ups"
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

export default ExercisesAdd;
