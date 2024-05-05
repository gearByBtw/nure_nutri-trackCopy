import { styled } from "@mui/material";
import { DataGrid } from "@mui/x-data-grid";

export const getStyledDataGrid = () => {
  return styled(DataGrid)({
    borderColor: "#242424",
    "& .MuiDataGrid-columnHeaderTitleContainer, .MuiDataGrid-cell": {
      display: "flex",
      alignItems: "center",
      justifyContent: "center",
    },
    "& .MuiDataGrid-cellContent, .MuiDataGrid-columnHeaderTitle": {
      wordWrap: "break-word",
      whiteSpace: "normal",
      lineHeight: "1.5",
      textAlign: "center",
    },
    "& .MuiDataGrid-cell": {
      width: "300px",
    },
    "& .row-1": {
      backgroundColor: "#f5f5f5",
    },
    "& .row-2": {
      backgroundColor: "#e5e5e5",
      marginLeft: "10px",
    },
    "& .row-3": {
      backgroundColor: "#d5d5d5",
      marginLeft: "20px",
    },
    "& .row-4": {
      backgroundColor: "#c5c5c5",
      marginLeft: "30px",
    },
    "& .row-5": {
      backgroundColor: "#b5b5b5",
      marginLeft: "40px",
    },
    "& .row-6": {
      backgroundColor: "#a5a5a5",
      marginLeft: "50px",
    },
    "& .row-7": {
      backgroundColor: "#959595",
      marginLeft: "60px",
    },
  });
};
