import { BrowserRouter, Routes, Route } from "react-router-dom";
import App from "./App";

import { QueryClient, QueryClientProvider } from "react-query";

const queryClient = new QueryClient();

const Router = () => {
  return (
    <QueryClientProvider client={queryClient}>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<App />}>
            <Route index element={<>Welcome to NutriTrack</>} />
            <Route path="*" element={<>404</>} />
          </Route>
        </Routes>
      </BrowserRouter>
    </QueryClientProvider>
  );
};

export default Router;
