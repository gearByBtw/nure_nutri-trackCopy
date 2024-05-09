import { Controller, useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { useContext, useState } from "react";
import { Button, FormControl, TextField } from "@mui/material";
import { UserContext } from "../components/Fallback";
import { UserType } from "../types/User";
import { useUsersCreate } from "../features/useUsersCreate";

const UsersSettings = () => {
  const user = useContext(UserContext);
  const [error, setError] = useState<string>("");

  const mutation = useUsersCreate({
    type: "edit",
    data: {
      id: user.id,
    },
  });

  const navigate = useNavigate();

  const form = useForm<UserType>({
    defaultValues: {
      id: user.id,
      name: user.name,
      role: user.role,
      subscription: user.subscription,
      email: user.email,
      bannedIngredients: user.bannedIngredients.join(", "),
      dailyCalories: user.dailyCalories,
    },
  });

  const handleCreate = form.handleSubmit((data) => {
    setError("");

    mutation
      .mutateAsync({
        ...data,
        bannedIngredients: data.bannedIngredients.toString().split(", "),
      })
      .then(() => {
        navigate("/");
      })
      .catch((err) => {
        setError(err.message);
      });
  });

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
            User Settings
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
              Save
            </Button>
          </div>
        </div>
      </div>
    </>
  );
};

export default UsersSettings;
