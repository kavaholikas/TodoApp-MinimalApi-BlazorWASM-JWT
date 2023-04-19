namespace TodoJWT.Blazor.Utils
{
    public static class InputValidation
    {
        private const string _toShort = "{0} is to short. Must be at least 5 characters long.";
        private const string _toLong = "{0} is to long. {0} cannot be longer then 20 characters.";

        public static string ToShort(string input) => string.Format(_toShort, input);
        public static string ToLong(string input) => string.Format(_toLong, input);

        public static bool IsInputToShort(string input) => input.Length < 5;
        public static bool IsInputToLong(string input) => input.Length > 20;

        public static bool IsInputValid(string input) => input.Length >= 5 && input.Length <= 20;

        public static bool IsInputToShort(string input, string inputName, out string error)
        {
            if (input.Length < 5)
            {
                error = string.Format(_toShort, inputName);
                return true;
            }

            error = string.Empty;
            return false;
        }

        public static bool IsInputToLong(string input, string inputName, out string error)
        {
            if (input.Length > 20)
            {
                error = string.Format(_toLong, inputName);
                return true;
            }

            error = string.Empty;
            return false;
        }

        public static bool IsInputValid(string input, string inputName, out string error)
        {
            if (IsInputValid(input))
            {
                error = string.Empty;
                return true;
            }

            if (IsInputToShort(input, inputName, out error))
            {
                return false;
            }

            if (IsInputToLong(input, inputName, out error))
            {
                return false;
            }

            return false;
        }
    }
}
