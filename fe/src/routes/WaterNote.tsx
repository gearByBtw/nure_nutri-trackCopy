import { useContext, useMemo, useState } from "react";
import { Button, IconButton } from "@mui/material";
import { Link } from "react-router-dom";
import { Delete, Edit } from "@mui/icons-material";
import { GridColDef } from "@mui/x-data-grid";
import { UserContext } from "../components/Fallback";
import { formatDateToYYYYMMDD } from "../utils/parseDate";
import { getStyledDataGrid } from "../utils/getStyledDataGrid";
import { useWaterNoteGetAllQuery } from "../features/useWaterNoteGetAllQuery";
import { useWaterNoteDelete } from "../features/useWaterNoteDelete";

const StyledDataGrid = getStyledDataGrid();

export const WaterNote = () => {
  const user = useContext(UserContext);
  const isAdmin = user.role === "admin";

  const { error, isLoading, data, refetch } = useWaterNoteGetAllQuery({
    userId: isAdmin ? undefined : user.id,
    createdAt: isAdmin ? undefined : formatDateToYYYYMMDD(new Date()),
  });
  const caloryNoteDelete = useWaterNoteDelete();
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
        field: "ml",
        headerName: "Water (ml)",
        type: "number",
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
              <Link to={`/water-notes/edit/${cellValues.row.id}`}>
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
          <Link to="/water-notes/add">
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

export default WaterNote;
