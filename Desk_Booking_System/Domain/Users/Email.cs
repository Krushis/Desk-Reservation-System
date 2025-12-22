using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Users
{
    public record Email
    {
        public string Value { get; init; }

        private Email(string value)
        {
            Value = value;
        }

        public static Email Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be empty", nameof(email));
            }
            // further validation would be implemented later on

            return new Email(email);
        }
    }

    // for validation later on if needed
}
