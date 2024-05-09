import { ReactNode, createContext } from "react";
import { UserType } from "../types/User";

export const UserContext = createContext<UserType>({} as UserType);

export const AuthFallback = ({ children }: { children: ReactNode }) => {
  const { isLoading, data } = {
    isLoading: false,
    data: {
      id: "1",
      name: "John Doe",
      role: "admin",
      subscription: "t-1",
      email: "123@gmail.com",
      bannedIngredients: ["1", "2", "3"],
      dailyCalories: 2000,
      weight: 80,
      desiredWeight: 70,

      hydrated: true,
      exercised: true,
      ateHealthy: false,
      chef: true,
      critic: false,
      criticTwoPointO: true,
      social: true,
    } as UserType,
  }; // get authenticated user

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
