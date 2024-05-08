namespace NutritionalRecipeBook.Domain.Results
{
    public class Result<T> : Result
    {
        private readonly T _value;
        public T Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("The value of a failure result can not be accessed.");


        protected internal Result(T value, bool isSuccess, Error error, ResultState? state = null)
            : base(isSuccess, error, state)
        {
            _value = value;
        }

        public static Result<T> Success(T value) => new(value, true, null, ResultState.Ok);

        public static Result<T> NotFound(Error error) => new(default, false, error, ResultState.NotFound);

        public static Result<T> Unauthorized(Error error) => new(default, false, error, ResultState.Unauthorized);

        public new static Result<T> Failure(Error error) => new(default, false, error, ResultState.BadRequest);

        public static implicit operator Result<T>(T value) => Create(value);
    }
}
