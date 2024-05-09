import { Controller, useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { useContext, useMemo, useState } from "react";
import { Button, FormControl, TextField } from "@mui/material";
import { UserContext } from "../components/Fallback";
import { UserType } from "../types/User";
import { useUsersCreate } from "../features/useUsersCreate";
import { useCalorieNoteGetAllQuery } from "../features/useCalorieNoteGetAllQuery";
import { CalorieNote } from "../types/CalorieNote";
import { BarChart, LineChart } from "@mui/x-charts";

const MIDDLE_CCALS_PER_DAY = 2250;

const calcDays = () => {
  const now = new Date();
  const days = new Date(now.getFullYear(), now.getMonth() + 1, 0).getDate();
  const arr = [];

  for (let i = 1; i <= days; i++) {
    arr.push(`0${i}`.slice(-2));
  }

  return arr;
};
const days = calcDays();

const UsersSettings = () => {
  const user = useContext(UserContext);
  const [error, setError] = useState<string>("");

  const mutation = useUsersCreate({
    type: "edit",
    data: {
      id: user.id,
    },
  });

  const calNotesQ = useCalorieNoteGetAllQuery({
    userId: user.id,
  });

  const preparedCalNotes = useMemo(() => {
    if (!calNotesQ.data) {
      return [];
    }

    const c = [...calNotesQ.data]
      .sort((a, b) => +b.createdAt - +a.createdAt)
      .reduce((acc: Record<string, number>, curr: CalorieNote) => {
        if (!acc[curr.createdAt]) {
          acc[curr.createdAt] = 0;
        }

        acc[curr.createdAt] += curr.calorie;

        return acc;
      }, {});

    return Object.values(c).slice(0, 31);
  }, [calNotesQ.data]);

  const [daysData, kilos, newWeight] = useMemo(() => {
    let total = 0;

    const ccals = new Array(days.length).fill(0).map((_, i) => {
      if (i + 1 >= new Date().getDate()) {
        return 0;
      }

      const ccal = preparedCalNotes[i] || 0; // Math.floor(1500 + Math.random() * 3000);

      total += ccal;
      return ccal;
    });

    const extrapolatedTotalCals = (total / new Date().getDate()) * days.length;
    const totalKilos = extrapolatedTotalCals / 7700;
    const kilosPerDay = totalKilos / days.length;
    const ccalsPerDay = extrapolatedTotalCals / days.length;

    // kilos

    let currWeight = user.weight;

    const kilos = ccals.map((ccal) => {
      if (!ccal) {
        const sign = ccalsPerDay > MIDDLE_CCALS_PER_DAY ? 1 : -1;
        currWeight += kilosPerDay * sign;

        return currWeight;
      }

      const sign = ccal > MIDDLE_CCALS_PER_DAY ? 1 : -1;
      currWeight += (ccal / 7700) * sign;

      return currWeight;
    });

    return [ccals, kilos, currWeight];
  }, [preparedCalNotes, user.weight]);

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
      weight: user.weight,
      desiredWeight: user.desiredWeight,
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

        <h4
          style={{
            textTransform: "uppercase",
            fontWeight: "bold",
            marginBlock: 30,
          }}
        >
          Statistics
        </h4>

        <div
          style={{
            display: "flex",
            justifyContent: "space-between",
            flexDirection: "column",

            height: 700,
          }}
        >
          <div
            style={{
              height: 300,
            }}
          >
            <h5
              style={{
                textAlign: "center",
                fontSize: 16,
              }}
            >
              Calories gain per las 30 days
            </h5>

            <BarChart
              xAxis={[
                {
                  id: "barCategories",
                  data: days,
                  scaleType: "band",
                  label: "Day of the month",
                },
              ]}
              series={[
                {
                  label: "Calories gained",
                  data: daysData,
                },
              ]}
            />
          </div>
          <div
            style={{
              height: 300,
            }}
          >
            <h5
              style={{
                textAlign: "center",
                marginBottom: 5,
                fontSize: 16,
              }}
            >
              Weight prediction for the next 30 days
            </h5>
            <h6
              style={{
                textAlign: "center",
                margin: 0,
                fontSize: 14,
              }}
            >
              You will {newWeight > user.weight ? "gain" : "lose"}{" "}
              {Math.abs(newWeight - user.weight).toFixed(2)} kilos
            </h6>

            <LineChart
              xAxis={[
                {
                  id: "barCategories",
                  data: days,
                  scaleType: "band",
                  label: "Day of the month",
                },
              ]}
              series={[
                {
                  label: "Estimated weight",
                  data: kilos,
                },
              ]}
            />
          </div>
        </div>
      </div>
    </>
  );
};

export default UsersSettings;