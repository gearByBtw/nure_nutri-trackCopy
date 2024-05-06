import { BrowserRouter, Routes, Route } from "react-router-dom";
import App from "./App";

import { QueryClient, QueryClientProvider } from "react-query";
import { Users } from "./routes/Users";
import UsersAdd from "./routes/UsersAdd";

const queryClient = new QueryClient();

const Router = () => {
  return (
    <QueryClientProvider client={queryClient}>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<App />}>
            <Route index element={<>Welcome to NutriTrack</>} />
            <Route path="*" element={<>404</>} />

            <Route path="users" element={<Users />} />
            <Route path="users/add" element={<UsersAdd />} />
            <Route path="users/add/:userId" element={<UsersAdd />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </QueryClientProvider>
  );
};

export default Router;
