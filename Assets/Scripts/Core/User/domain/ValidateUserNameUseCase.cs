using System.Text.RegularExpressions;
using ModestTree;

namespace Core.User.domain
{
    public class ValidateUserNameUseCase
    {
        private const int MINLength = 8;
        private const int MAXLength = 24;
        private readonly Regex validCharsRegex = new("^[a-zA-Z0-9]+([._]?[a-zA-Z0-9]+)+$");
        public UserNameValidState Validate(string name)
        {
            if (name.IsEmpty())
                return UserNameValidState.IsEmpty;

            switch (name.Length)
            {
                case < MINLength:
                    return UserNameValidState.TooShort;
                case > MAXLength:
                    return UserNameValidState.TooLong;
            }

            if (!validCharsRegex.IsMatch(name))
                return UserNameValidState.ContainsInvalidCharacters;

            return UserNameValidState.Valid;
        }
        
        public enum UserNameValidState
        {
            Valid,
            TooShort,
            TooLong,
            IsEmpty,
            ContainsInvalidCharacters
        }
    }
}