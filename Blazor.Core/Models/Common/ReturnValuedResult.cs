namespace CardManagement.Core.Models.Common
{
    public class ReturnValuedResult<T> where T : class
    {
        public ReturnValuedResult()
        {
            Value = default(T);
            Errors = new List<string>();
        }
        public bool IsValid => !Errors.Any();
        public T Value { get; set; }
        public List<string> Errors { get; set; }
    }
}
