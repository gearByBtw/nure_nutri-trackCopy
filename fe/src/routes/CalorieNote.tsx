import { useContext, useMemo, useState } from "react";
import { Box, Button, IconButton, LinearProgress } from "@mui/material";
import { Link } from "react-router-dom";
import { Delete, Edit } from "@mui/icons-material";
import { GridColDef } from "@mui/x-data-grid";
import { useCalorieNoteGetAllQuery } from "../features/useCalorieNoteGetAllQuery";
import { UserContext } from "../components/Fallback";
import { formatDateToYYYYMMDD } from "../utils/parseDate";
import { useCalorieNoteDelete } from "../features/useCalorieNoteDelete";
import { getStyledDataGrid } from "../utils/getStyledDataGrid";

const StyledDataGrid = getStyledDataGrid();

export const CalorieNote = () => {
  const user = useContext(UserContext);
  const isAdmin = user.role === "admin";

  const { error, isLoading, data, refetch } = useCalorieNoteGetAllQuery({
    userId: isAdmin ? undefined : user.id,
    createdAt: isAdmin ? undefined : formatDateToYYYYMMDD(new Date()),
  });
  const caloryNoteDelete = useCalorieNoteDelete();
  const [localError, setLocalError] = useState<string>("");

  const rows = data || [];

  const columns = useMemo(() => {
    const c = [
      {
        field: "id",
        headerName: "ID",
        type: "number",
        sortable: false,
      },
      {
        field: "createdAt",
        headerName: "Date",
        type: "string",
        sortable: false,
      },
      {
        field: "calorie",
        headerName: "Calorie",
        type: "number",
        sortable: false,
      },
      {
        field: "recepieName",
        headerName: "Recipe",
        type: "string",
        sortable: false,
      },
      {
        field: "actions",
        headerName: "Actions",
        sortable: false,
        width: 85,
        renderCell: (cellValues) => {
          return (
            <>
              <Link to={`/calories/edit/${cellValues.row.id}`}>
                <IconButton aria-label="edit">
                  <Edit />
                </IconButton>
              </Link>
              <IconButton
                aria-label="delete"
                onClick={() => {
                  const confirm = window.confirm(
                    `Confirm deletion of note ${cellValues.row.id}?`,
                  );

                  if (!confirm) {
                    return;
                  }

                  caloryNoteDelete
                    .mutateAsync({ id: cellValues.row.id })
                    .then(() => {
                      setLocalError("");
                      refetch();
                    })
                    .catch((error) => {
                      setLocalError(error.message);
                    });
                }}
              >
                <Delete />
              </IconButton>
            </>
          ) as React.JSX.Element;
        },
      },
    ] as GridColDef[];

    if (isAdmin) {
      c.push({
        field: "userId",
        headerName: "User ID",
        type: "string",
        sortable: false,
      });
    }

    return c;
  }, [caloryNoteDelete, refetch, isAdmin]);

  const callories = useMemo(() => {
    return data?.reduce((acc, row) => acc + row.calorie, 0) || 0;
  }, [data]);

  return (
    <>
      {error && (
        <div style={{ color: "red" }}>
          Something went wrong: {(error as Error).message}
        </div>
      )}
      {localError && (
        <div style={{ color: "red" }}>Something went wrong: {localError}</div>
      )}

      <div
        style={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          marginBottom: "1rem",
        }}
      >
        <Box sx={{ width: "300px" }}>
          <div
            style={{
              display: "flex",
              justifyContent: "center",
            }}
          >
            <b>Daily calories:</b>
          </div>
          <div
            style={{
              display: "flex",
              justifyContent: "space-between",
              marginBottom: ".25rem",
              marginTop: ".5rem",
            }}
          >
            <span>{callories}</span>
            <span>{(callories / user.dailyCalories) * 100} %</span>
            <span>{user.dailyCalories}</span>
          </div>

          <LinearProgress
            variant="determinate"
            value={Math.min((callories / user.dailyCalories) * 100, 100)}
            color={callories > user.dailyCalories ? "error" : "success"}
          />
        </Box>
        <div
          style={{
            display: "flex",
            justifyContent: "flex-end",
            marginBottom: ".25rem",
          }}
        >
          <Link to="/calories/add">
            <Button variant="contained" color="success">
              Add note
            </Button>
          </Link>
        </div>
      </div>

      <div
        style={{
          height: 550,
          borderRadius: "5px",
          backgroundColor: "#f5f5f5",
        }}
      >
        <StyledDataGrid
          loading={isLoading}
          rows={rows}
          getRowId={(row) => row.id}
          columns={columns}
          columnBuffer={3}
          initialState={{
            pagination: {
              paginationModel: {
                page: 0,
                pageSize: 25,
              },
            },
          }}
          pageSizeOptions={[25, 50, 100]}
          getRowHeight={() => "auto"}
          columnHeaderHeight={75}
          rowSelection={false}
        />
      </div>
    </>
  );
};

export default CalorieNote;
