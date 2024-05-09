import { Button } from "@mui/material";
import { Link } from "react-router-dom";
import { UserContext } from "./Fallback";
import { useContext } from "react";

export const Nav = () => {
  const user = useContext(UserContext);
  const isAdmin = user.role === "admin";

  return (
    <div>
      <div
        style={{
          display: "flex",
          flexDirection: "row",
          alignItems: "center",
          flexWrap: "wrap",
          justifyContent: "center",
          gap: 10,
        }}
      >
        <Link to="/">
          <Button color="success">Home</Button>
        </Link>

        <Link to="/calories">
          <Button color="success">Calories</Button>
        </Link>

        <Link to="/exercises-notes">
          <Button color="success">Exercises Notes</Button>
        </Link>

        <Link to="/water-notes">
          <Button color="success">Water</Button>
        </Link>

        <Link to="/recepies">
          <Button color="success">Recipes</Button>
        </Link>

        <Link to="/settings">
          <Button color="success">User Details</Button>
        </Link>

        {isAdmin && (
          <Link to="/users">
            <Button color="success">Users</Button>
          </Link>
        )}

        {isAdmin && (
          <Link to="/exercises">
            <Button color="success">Exercises</Button>
          </Link>
        )}
      </div>
    </div>
  );
};
