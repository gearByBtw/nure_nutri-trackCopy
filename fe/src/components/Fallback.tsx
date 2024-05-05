import { ReactNode } from "react";
import { useUserGetQuery } from "../features/useUserGetQuery";

export const Fallback = ({
  type,
  children,
}: {
  type: "admin" | "user";
  children: ReactNode;
}) => {
  const { isLoading, data } = useUserGetQuery({ token: "1" });

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (!data) {
    setTimeout(() => {
      window.location.assign("/login");
    }, 1000);

    return <div>Not authorized. Redirecting...</div>;
  }

  if (data.role !== type) {
    return <div>Access denied</div>;
  }

  return children;
};
