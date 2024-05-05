import { Button, ButtonGroup } from "@mui/material";
import { Link } from "react-router-dom";

export const Nav = ({ isAdmin }: { isAdmin: boolean }) => {
  if (isAdmin) {
    return <>Todo</>;
  }

  return (
    <div>
      <div
        style={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
        }}
      >
        <ButtonGroup variant="contained">
          <Link to="/calories">
            <Button>Calories</Button>
          </Link>
        </ButtonGroup>
      </div>
    </div>
  );
};
