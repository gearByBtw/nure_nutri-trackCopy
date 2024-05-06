import { getBaseUrl } from "./getBaseUrl";

export const fetchAbstract = async (
  urlPart: string,
  method: string,
  body?: object,
) => {
  const response = await fetch(getBaseUrl() + urlPart, {
    method: method,
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(body),
  });

  if (!response.ok) {
    throw new Error(await response.text());
  }

  try {
    return await response.json();
  } catch (error) {
    console.log(error);
    return {};
  }
};
