import { Controller, useForm } from "react-hook-form";
import { useNavigate, useParams } from "react-router-dom";
import { useContext, useEffect, useState } from "react";
import {
  Autocomplete,
  Button,
  FormControl,
  FormHelperText,
  TextField,
} from "@mui/material";
import { useCalorieNoteGetAllQuery } from "../features/useCalorieNoteGetAllQuery";
import { useRecepieGetAllQuery } from "../features/useRecepieGetAllQuery";
import { useCalorieNoteCreate } from "../features/useCalorieNoteCreate";
import { CalorieNote } from "../types/CalorieNote";
import { UserContext } from "../components/Fallback";
import { formatDateToYYYYMMDD } from "../utils/parseDate";
import { useUserGetAllQuery } from "../features/useUserGetAllQuery";
import { useUserDoneAchievement } from "../features/useUserDoneAchievement";

const CalorieNoteAdd = () => {
  const user = useContext(UserContext);
  const toEdit = useParams().calorieId;
  const isEdit = toEdit !== undefined;
  const filter = isEdit
    ? {
        id: toEdit,
      }
    : {};
  const items = useCalorieNoteGetAllQuery(filter);
  const item = items.data?.[0];
  const [error, setError] = useState<string>("");

  const recepies = useRecepieGetAllQuery({});
  const users = useUserGetAllQuery({});

  const achieve = useUserDoneAchievement();

  const mutation = useCalorieNoteCreate(
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

  const form = useForm<CalorieNote>({
    defaultValues: {
      id: "",
      userId: user.id,
      createdAt: formatDateToYYYYMMDD(new Date()),
      calorie: 0,
      recepieId: "",
      recepieName: "",
    },
  });

  useEffect(() => {
    if (!item || !isEdit) return;

    form.setValue("id", item.id || "");
    form.setValue("userId", item.userId || user.id);
    form.setValue(
      "createdAt",
      item.createdAt || formatDateToYYYYMMDD(new Date()),
    );
    form.setValue("calorie", item.calorie || 0);
    form.setValue("recepieId", item.recepieId || "");
    form.setValue("recepieName", item.recepieName || "");
  }, [items.data, isEdit, form, item, user.id]);

  const handleCreate = form.handleSubmit((data) => {
    setError("");

    const datePattern = /^\d{4}-\d{2}-\d{2}$/;

    if (
      !data.recepieId ||
      !data.createdAt ||
      !datePattern.test(data.createdAt) ||
      !data.userId
    ) {
      setError("Fill all fields");
      return;
    }

    mutation
      .mutateAsync(data)
      .then(() => {
        achieve
          .mutateAsync({
            id: user.id,
            achievement: "ateHealthy",
          })
          .then(() => {
            navigate("/calories");
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
            {isEdit ? "Edit" : "Add"} Calorie Note
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
            {user.role === "admin" && (
              <Controller
                name="userId"
                control={form.control}
                render={({ field }) => (
                  <FormControl
                    size="small"
                    fullWidth
                    error={users.isError}
                    sx={{ m: 1, minWidth: 120, maxWidth: "95%" }}
                  >
                    <Autocomplete
                      disablePortal
                      onChange={(_, value) => {
                        field.onChange(value || "");
                      }}
                      value={field.value}
                      options={[
                        "",
                        ...(users.data || []).map((account) => account.id),
                      ]}
                      getOptionLabel={(option) => {
                        const account = (users.data || []).find(
                          (account) => account.id === option,
                        );
                        return account ? `${account.name}` : "";
                      }}
                      renderInput={(params) => (
                        <TextField {...params} label="User" />
                      )}
                      size="small"
                      disabled={
                        users.isLoading || (!users.isLoading && users.isError)
                      }
                    />

                    <FormHelperText component="span">
                      {users.isError && <div>Something went wrong</div>}
                    </FormHelperText>
                  </FormControl>
                )}
              />
            )}

            <Controller
              name="createdAt"
              control={form.control}
              render={({ field }) => (
                <FormControl
                  size="small"
                  fullWidth
                  sx={{ m: 1, minWidth: 120, maxWidth: "95%" }}
                >
                  <TextField
                    label="Date"
                    placeholder="yyyy-mm-dd"
                    onChange={field.onChange}
                    value={field.value}
                    size="small"
                  />
                </FormControl>
              )}
            />

            <Controller
              name="recepieId"
              control={form.control}
              render={({ field }) => (
                <FormControl
                  size="small"
                  fullWidth
                  error={recepies.isError}
                  sx={{ m: 1, minWidth: 120, maxWidth: "95%" }}
                >
                  <Autocomplete
                    disablePortal
                    onChange={(_, value) => {
                      field.onChange(value || "");
                    }}
                    value={field.value}
                    options={[
                      0,
                      ...(recepies.data || [])
                        .filter((account) => {
                          if (user.role === "admin") {
                            return true;
                          }

                          if (
                            account.isPremium &&
                            user.subscription === "t-1"
                          ) {
                            return false;
                          }

                          if (
                            user.bannedIngredients.some((i) =>
                              account.ingredients.includes(i),
                            )
                          ) {
                            return false;
                          }

                          return true;
                        })
                        .map((account) => account.id),
                    ]}
                    getOptionLabel={(option) => {
                      const account = (recepies.data || []).find(
                        (account) => account.id === option,
                      );
                      return account ? `${account.name}` : "";
                    }}
                    renderInput={(params) => (
                      <TextField {...params} label="Recipe" />
                    )}
                    size="small"
                    disabled={
                      recepies.isLoading ||
                      (!recepies.isLoading && recepies.isError)
                    }
                  />

                  <FormHelperText component="span">
                    {recepies.isError && <div>Something went wrong</div>}
                  </FormHelperText>
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

export default CalorieNoteAdd;
