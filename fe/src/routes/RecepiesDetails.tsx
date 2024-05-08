import { Controller, useForm } from "react-hook-form";
import { useParams } from "react-router-dom";
import { useContext, useState } from "react";
import { Button, FormControl, IconButton, TextField } from "@mui/material";
import { UserContext } from "../components/Fallback";
import { useRecepieGetAllQuery } from "../features/useRecepieGetAllQuery";
import { useRecepieCommentCreate } from "../features/useRecepieCommentCreate";
import { RecepieComment } from "../types/RecepieComment";
import { useRecepieCommentsGetAllQuery } from "../features/useRecepieCommentsGetAllQuery";
import { ArrowDownward, ArrowUpward, Star } from "@mui/icons-material";
import { useRecepieChangeVote } from "../features/useRecepieChangeVote";

const RecepiesDetails = () => {
  const user = useContext(UserContext);
  const id = useParams().id;
  const filter = id
    ? {
        id,
      }
    : {};
  const items = useRecepieGetAllQuery(filter);
  const item = items.data?.[0];

  const [error, setError] = useState<string>("");

  const comments = useRecepieCommentsGetAllQuery({
    recepieId: id,
  });

  const mutation = useRecepieCommentCreate();

  const voteMut = useRecepieChangeVote();

  const form = useForm<RecepieComment>({
    defaultValues: {
      id: "",
      userId: user.id,
      userName: user.name,
      recepieId: id || "",
      comment: "",
    },
  });

  const handleCreate = form.handleSubmit((data) => {
    setError("");

    if (!data.comment) {
      setError("Comment is required");
      return;
    }

    mutation
      .mutateAsync(data)
      .then(() => {
        comments.refetch();
        form.reset();
      })
      .catch((err) => {
        setError(err.message);
      });
  });

  if (items.isLoading) {
    return <div>Loading...</div>;
  }

  if (!item) {
    return <div>Not found</div>;
  }

  return (
    <>
      <div
        style={{
          paddingInline: 10,
        }}
      >
        <div
          style={{
            display: "flex",
            justifyContent: "space-between",
            flexDirection: "column",
          }}
        >
          <h4
            style={{
              textTransform: "uppercase",
              fontWeight: "bold",
              marginBottom: 10,
              display: "flex",
              alignItems: "center",
            }}
          >
            {item.name} #{item.id} {item.isPremium && <Star color="success" />}
          </h4>

          <div
            style={{
              display: "flex",
              alignItems: "center",
              marginBottom: 15,
            }}
          >
            <div>Votes: {item.votes}</div>
            <div>
              <IconButton
                aria-label="upvote"
                onClick={() => {
                  voteMut.mutateAsync({ id: item.id, type: "up" }).then(() => {
                    items.refetch();
                  });
                }}
              >
                <ArrowUpward />
              </IconButton>

              <IconButton
                aria-label="downvote"
                onClick={() => {
                  voteMut
                    .mutateAsync({ id: item.id, type: "down" })
                    .then(() => {
                      items.refetch();
                    });
                }}
              >
                <ArrowDownward />
              </IconButton>
            </div>
          </div>

          <div>
            <div>Calories: {item.calories}</div>
            <div>Ingredients: {item.ingredients.join(", ")}</div>
            <div>Description: {item.description}</div>
          </div>
        </div>

        <div
          style={{
            color: "red",
            paddingBottom: 10,
          }}
        >
          {error && <>Something went wrong: {error}</>}
        </div>

        <hr />

        <div>
          <h5>Comments: {comments.isLoading && "loading..."}</h5>
          {comments.data?.map((comment) => (
            <div
              key={comment.id}
              style={{
                display: "flex",
                justifyContent: "space-between",
                flexDirection: "column",
                padding: 10,
                border: "1px solid #ccc",
                borderRadius: 5,
                marginBottom: 10,
              }}
            >
              <h5
                style={{
                  marginTop: 5,
                  marginBottom: 5,
                }}
              >
                {comment.userName} says:
              </h5>
              <p>{comment.comment}</p>
            </div>
          ))}
          <form
            style={{
              display: "flex",
              marginTop: 20,
            }}
          >
            <Controller
              name="comment"
              control={form.control}
              render={({ field }) => (
                <FormControl
                  size="small"
                  fullWidth
                  sx={{ m: 1, minWidth: 120, maxWidth: "85%" }}
                >
                  <TextField
                    label="Comment"
                    placeholder="Comment..."
                    onChange={field.onChange}
                    value={field.value}
                    size="small"
                    required
                    multiline
                  />
                </FormControl>
              )}
            />

            <Button
              onClick={handleCreate}
              variant="contained"
              sx={{ m: 1, minWidth: 80 }}
              color="secondary"
            >
              Add comment
            </Button>
          </form>
        </div>
      </div>
    </>
  );
};

export default RecepiesDetails;
