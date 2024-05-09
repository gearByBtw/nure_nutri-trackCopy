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

  // achievements
  hydrated: boolean; // add water note
  exercised: boolean; // add exercise note
  ateHealthy: boolean; // add healthy note
  chef: boolean; // add recipe
  critic: boolean; // up/down vote
  criticTwoPointO: boolean; // leave a comment
  social: boolean; // share progress
};

export const ACHIEVEMENTS: {
  name: string;
  description: string;
  icon: string;
  key:
    | "hydrated"
    | "exercised"
    | "ateHealthy"
    | "chef"
    | "critic"
    | "criticTwoPointO"
    | "social";
}[] = [
  {
    name: "Hydrated",
    description: "Add water note",
    icon: "💧",
    key: "hydrated",
  },
  {
    name: "Exercised",
    description: "Add exercise note",
    icon: "🏋️",
    key: "exercised",
  },
  {
    name: "Ate Healthy",
    description: "Add calorie note",
    icon: "🥗",
    key: "ateHealthy",
  },
  {
    name: "Chef",
    description: "Add recipe",
    icon: "👨‍🍳",
    key: "chef",
  },
  {
    name: "Critic",
    description: "Vote for a recipe",
    icon: "👍",
    key: "critic",
  },
  {
    name: "Critic 2.0",
    description: "Leave a comment on a recipe",
    icon: "💬",
    key: "criticTwoPointO",
  },
  {
    name: "Social",
    description: "Share progress",
    icon: "📈",
    key: "social",
  },
];
