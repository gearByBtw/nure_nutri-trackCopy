import { ReactNode, createContext, useContext } from "react";
import { useUserGetQuery } from "../features/useUserGetQuery";
import { UserType } from "../types/User";

export const UserContext = createContext<UserType>({} as UserType);

export const RoleFallback = ({
  type,
  children,
}: {
  type: UserType["role"];
  children: ReactNode;
}) => {
  const user = useContext(UserContext);

  if (user.role !== type) {
    return <div>Access denied</div>;
  }

  return children;
};

export const AuthFallback = ({ children }: { children: ReactNode }) => {
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

  return <UserContext.Provider value={data}>{children}</UserContext.Provider>;
};
