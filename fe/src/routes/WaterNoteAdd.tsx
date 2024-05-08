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
import { UserContext } from "../components/Fallback";
import { formatDateToYYYYMMDD } from "../utils/parseDate";
import { useUserGetAllQuery } from "../features/useUserGetAllQuery";
import { useWaterNoteGetAllQuery } from "../features/useWaterNoteGetAllQuery";
import { useWaterNoteCreate } from "../features/useWaterNoteCreate";
import { WaterNote } from "../types/WaterNote";

const WaterNoteAdd = () => {
  const user = useContext(UserContext);
  const toEdit = useParams().id;
  const isEdit = toEdit !== undefined;
  const filter = isEdit
    ? {
        id: toEdit,
      }
    : {};
  const items = useWaterNoteGetAllQuery(filter);
  const item = items.data?.[0];
  const [error, setError] = useState<string>("");

  const users = useUserGetAllQuery({});

  const mutation = useWaterNoteCreate(
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

  const form = useForm<WaterNote>({
    defaultValues: {
      id: "",
      userId: user.id,
      createdAt: formatDateToYYYYMMDD(new Date()),
      ml: 0,
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
    form.setValue("ml", item.ml || 0);
  }, [items.data, isEdit, form, item, user.id]);

  const handleCreate = form.handleSubmit((data) => {
    setError("");

    const datePattern = /^\d{4}-\d{2}-\d{2}$/;

    if (
      !data.ml ||
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
        navigate("/water-notes");
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
            {isEdit ? "Edit" : "Add"} Water Note
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
              name="ml"
              control={form.control}
              render={({ field }) => (
                <FormControl
                  size="small"
                  fullWidth
                  sx={{ m: 1, minWidth: 120, maxWidth: "95%" }}
                >
                  <TextField
                    label="Water (ml)"
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

export default WaterNoteAdd;
