export const urlBuilder = (
  urlString: string,
  queryObject: Record<string, unknown>,
) => {
  const queryParams = new URLSearchParams(
    Object.entries(queryObject)
      .map(([key, value]) => {
        if (value) {
          return [key, `${value}`];
        }
      })
      .filter((x) => x) as string[][],
  ).toString();

  return `${urlString}${queryParams ? `?${queryParams}` : ""}/`;
};
