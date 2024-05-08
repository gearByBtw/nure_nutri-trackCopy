export type Recepie = {
  id: string;
  name: string;
  ingredients: string[];
  calories: number;
  description: string;
  votes: number;
  isPremium: boolean;
  isCreatedByUser: boolean;
};
