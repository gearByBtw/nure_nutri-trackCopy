import { useContext, useMemo } from "react";
import { UserContext } from "../components/Fallback";

import { BarChart, PieChart } from "@mui/x-charts";
import { useUserGetAllQuery } from "../features/useUserGetAllQuery";
import { useExerciseGetAllQuery } from "../features/useExerciseGetAllQuery";
import { useWaterNoteGetAllQuery } from "../features/useWaterNoteGetAllQuery";
import { useRecepieCommentsGetAllQuery } from "../features/useRecepieCommentsGetAllQuery";
import { useCalorieNoteGetAllQuery } from "../features/useCalorieNoteGetAllQuery";

const tierTypes = ["Tier-1", "Tier-2", "Tier-3"];

const CompanyStats = () => {
  const user = useContext(UserContext);

  const usersQ = useUserGetAllQuery({});
  const exQ = useExerciseGetAllQuery({});
  const waterQ = useWaterNoteGetAllQuery({});
  const caloQ = useCalorieNoteGetAllQuery({});
  const recepiesQ = useRecepieCommentsGetAllQuery({});

  const tierData = useMemo(() => {
    const data = [0, 0, 0];

    usersQ.data?.forEach((user) => {
      if (user.subscription === "t-1") {
        data[0]++;
      } else if (user.subscription === "t-2") {
        data[1]++;
      } else if (user.subscription === "t-3") {
        data[2]++;
      }
    });

    return data;
  }, [usersQ.data]);

  const ussageData = useMemo(() => {
    return [
      waterQ.data?.length || 0,
      caloQ.data?.length || 0,
      exQ.data?.length || 0,
      recepiesQ.data?.length || 0,
    ];
  }, [
    waterQ.data?.length,
    caloQ.data?.length,
    exQ.data?.length,
    recepiesQ.data?.length,
  ]);

  if (user.subscription !== "t-3") {
    return <div>Access denied</div>;
  }

  const isLoading =
    usersQ.isLoading ||
    exQ.isLoading ||
    waterQ.isLoading ||
    caloQ.isLoading ||
    recepiesQ.isLoading;

  if (isLoading) {
    return <div>Loading...</div>;
  }

  return (
    <>
      <div
        style={{
          paddingInline: 10,
        }}
      >
        <div
          style={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
          }}
        >
          <h4
            style={{
              textTransform: "uppercase",
              fontWeight: "bold",
              marginBlock: 30,
            }}
          >
            Company Statistics
          </h4>
        </div>

        <div
          style={{
            fontSize: 18,
          }}
        >
          Total number of users: {usersQ.data?.length}
        </div>

        <hr />

        <div
          style={{
            height: 300,
          }}
        >
          <h5
            style={{
              textAlign: "center",
              fontSize: 16,
            }}
          >
            Users per subscription tier
          </h5>

          <BarChart
            xAxis={[
              {
                id: "barCategories",
                data: tierTypes,
                scaleType: "band",
                label: "Subscription tier",
              },
            ]}
            series={[
              {
                label: "Users",
                data: tierData,
              },
            ]}
          />
        </div>

        <div
          style={{
            height: 300,
            paddingTop: 70,
          }}
        >
          <hr />
          <h5
            style={{
              textAlign: "center",
              fontSize: 16,
            }}
          >
            App usage
          </h5>

          <div
            style={{
              display: "flex",
              justifyContent: "center",
            }}
          >
            <PieChart
              width={500}
              height={300}
              series={[
                {
                  data: [
                    {
                      label: "Water intake",
                      value: ussageData[0],
                    },
                    {
                      label: "Calories",
                      value: ussageData[1],
                    },
                    {
                      label: "Exercises",
                      value: ussageData[2],
                    },
                    {
                      label: "Other",
                      value: ussageData[3],
                    },
                  ],
                },
              ]}
            />
          </div>
        </div>
      </div>
    </>
  );
};

export default CompanyStats;
