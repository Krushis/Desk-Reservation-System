using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions
{
    public record Error(string Code, string Name)
    {
        public static readonly Error None = new(string.Empty, string.Empty);

        public static readonly Error NullValue = new("Error.NullValue", "Null value was provided");

        public static readonly Error NotFound = new("Error.NotFound", "Value not found");

        public static readonly Error UserIdNotMatching = new("Error.UserIdNotMatching", "User ID for cancelling of a reservation does " +
            "not match");

        public static readonly Error DeskNotAvailable = new("Error.DeskNotAvailable", "Desk not available for reservation");
    }
}
