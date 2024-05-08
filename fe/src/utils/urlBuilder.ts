export const urlBuilder = (
  urlString: string,
  queryObject: Record<string, unknown>,
) => {
  const queryParams = new URLSearchParams([
    ...(Object.entries(queryObject)
      .map(([key, value]) => {
        if (value && key !== "id") {
          return [key, `${value}`];
        }
      })
      .filter((x) => x) as string[][]),
    ["pageSize", "1000"],
    ["pageNumber", "1"],
  ]).toString();

  const idParam = queryObject.id ? `/${queryObject.id}` : "";

  return `${urlString}${idParam}${queryParams ? `?${queryParams}` : ""}/`;
};
