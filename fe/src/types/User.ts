export type UserType = {
  id: string;
  role: "admin" | "user";
  subscription: "t-1" | "t-2" | "t-3";
  email: string;
  bannedIngredients: string[];
};
