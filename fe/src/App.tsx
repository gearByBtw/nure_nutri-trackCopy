import { Outlet } from "react-router-dom";
import { Nav } from "./components/Nav";

const App = () => {
  return (
    <>
      <Nav isAdmin={false} />
      <hr />
      <Outlet />
    </>
  );
};

export default App;
