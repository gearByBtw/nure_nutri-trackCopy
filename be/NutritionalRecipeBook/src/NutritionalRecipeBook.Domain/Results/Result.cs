namespace NutritionalRecipeBook.Domain.Results
{
    public class Result
    {
        protected Result(bool isSuccess, Error error, ResultState? statusCode = null)
        {
            IsSuccess = isSuccess;
            Error = error;
            State = statusCode;
        }

        public ResultState? State { get; }

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public Error Error { get; }

        public static Result Success() => new(true, Error.None, ResultState.Ok);

        private static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

        public static Result UnprocessableEntity(Error error) => new(false, error, ResultState.UnprocessableEntity);

        public static Result Failure(Error error) => new(false, error, ResultState.BadRequest);

        private static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

        protected static Result<TValue> Create<TValue>(TValue value) => value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
    }
}
