import { useContext, useMemo, useState } from "react";
import { Button, IconButton } from "@mui/material";
import { Link } from "react-router-dom";
import { Delete, Edit } from "@mui/icons-material";
import { GridColDef } from "@mui/x-data-grid";
import { UserContext } from "../components/Fallback";
import { getStyledDataGrid } from "../utils/getStyledDataGrid";
import { useExerciseGetAllQuery } from "../features/useExerciseGetAllQuery";
import { useExerciseDelete } from "../features/useExerciseDelete";

const StyledDataGrid = getStyledDataGrid();

export const Exercises = () => {
  const user = useContext(UserContext);
  const isAdmin = user.role === "admin";

  const { error, isLoading, data, refetch } = useExerciseGetAllQuery({});
  const deleteU = useExerciseDelete();
  const [localError, setLocalError] = useState<string>("");

  const rows = data || [];

  const columns = useMemo(() => {
    return [
      {
        field: "id",
        headerName: "ID",
        type: "number",
        sortable: false,
        width: 85,
      },
      {
        field: "name",
        headerName: "Name",
        type: "string",
        sortable: false,
      },
      {
        field: "calories",
        headerName: "Calories",
        type: "number",
        sortable: false,
      },
      {
        field: "description",
        headerName: "Description",
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
              <Link to={`/exercises/edit/${cellValues.row.id}`}>
                <IconButton aria-label="edit">
                  <Edit />
                </IconButton>
              </Link>
              <IconButton
                aria-label="delete"
                onClick={() => {
                  const confirm = window.confirm(
                    `Confirm deletion of exercise ${cellValues.row.id}?`,
                  );

                  if (!confirm) {
                    return;
                  }

                  deleteU
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
  }, [deleteU, refetch]);

  if (!isAdmin) {
    return <div>Access denied</div>;
  }

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
          marginBottom: "1rem",
        }}
      >
        <div
          style={{
            display: "flex",
            justifyContent: "flex-end",
            marginBottom: ".25rem",
          }}
        >
          <Link to="/exercises/add">
            <Button variant="contained" color="success">
              Add exercise
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

export default Exercises;
