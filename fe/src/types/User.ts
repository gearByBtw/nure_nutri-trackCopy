export type UserType = {
  id: string;
  name: string;
  role: "admin" | "user";
  subscription: "t-1" | "t-2" | "t-3";
  email: string;
  bannedIngredients: string[];
  dailyCalories: number;
  weight: number;
  desiredWeight: number;
};
