import { BrowserRouter, Routes, Route } from "react-router-dom";
import App from "./App";

import { QueryClient, QueryClientProvider } from "react-query";
import { Users } from "./routes/Users";
import UsersAdd from "./routes/UsersAdd";
import { CalorieNote } from "./routes/CalorieNote";
import CalorieNoteAdd from "./routes/CalorieNoteAdd";
import { Exercises } from "./routes/Exercises";
import ExercisesAdd from "./routes/ExercisesAdd";
import { ExerciseNote } from "./routes/ExerciseNote";
import ExerciseNoteAdd from "./routes/ExerciseNoteAdd";
import { WaterNote } from "./routes/WaterNote";
import WaterNoteAdd from "./routes/WaterNoteAdd";
import Recepies from "./routes/Recepies";
import RecepiesAdd from "./routes/RecepiesAdd";

const queryClient = new QueryClient();

const Router = () => {
  return (
    <QueryClientProvider client={queryClient}>
      <BrowserRouter basename="/nure_nutri-track">
        <Routes>
          <Route path="/" element={<App />}>
            <Route index element={<>Welcome to NutriTrack</>} />
            <Route path="*" element={<>404</>} />

            <Route path="users" element={<Users />} />
            <Route path="users/add" element={<UsersAdd />} />
            <Route path="users/edit/:userId" element={<UsersAdd />} />

            <Route path="calories" element={<CalorieNote />} />
            <Route path="calories/add" element={<CalorieNoteAdd />} />
            <Route
              path="calories/edit/:calorieId"
              element={<CalorieNoteAdd />}
            />

            <Route path="exercises" element={<Exercises />} />
            <Route path="exercises/add" element={<ExercisesAdd />} />
            <Route path="exercises/edit/:id" element={<ExercisesAdd />} />

            <Route path="exercises-notes" element={<ExerciseNote />} />
            <Route path="exercises-notes/add" element={<ExerciseNoteAdd />} />
            <Route
              path="exercises-notes/edit/:id"
              element={<ExerciseNoteAdd />}
            />

            <Route path="water-notes" element={<WaterNote />} />
            <Route path="water-notes/add" element={<WaterNoteAdd />} />
            <Route path="water-notes/edit/:id" element={<WaterNoteAdd />} />

            <Route path="recepies" element={<Recepies />} />
            <Route path="recepies/add" element={<RecepiesAdd />} />
            <Route path="recepies/edit/:id" element={<RecepiesAdd />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </QueryClientProvider>
  );
};

export default Router;
