using System.ComponentModel.DataAnnotations;

namespace CardManagement.Core.Models.Users
{
    internal class ValidGuidAttribute : ValidationAttribute
    {
        private readonly bool _skipNullCheck;
        public ValidGuidAttribute(bool skipNullCheck = false)
        {
            _skipNullCheck = skipNullCheck;
        }
        public override bool IsValid(object value)
        {
            if (_skipNullCheck && value==null)
            {
                return true;
            }                
            if (value == null || !(value is string))
            {
                return false;
            }
            string guidString = (string)value;
            return Guid.TryParse(guidString, out _);
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be a valid GUID.";
        }


    }
}