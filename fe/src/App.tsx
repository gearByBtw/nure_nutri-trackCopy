import { Outlet } from "react-router-dom";
import { Nav } from "./components/Nav";
import { AuthFallback } from "./components/Fallback";

const App = () => {
  return (
    <AuthFallback>
      <Nav isAdmin={false} />
      <hr />
      <Outlet />
    </AuthFallback>
  );
};

export default App;
