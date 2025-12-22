using Domain.Abstractions;
using Domain.Users.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Users
{
    public sealed class User : Entity
    {
        private User(Guid id, string firstName, string lastName, Email email) : base(id) 
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        private User() { }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Email Email { get; private set; }

        public static User Create(string firstName, string lastName, Email email)
        {
            User user = new User(Guid.NewGuid(), firstName, lastName, email);

            user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id)); // optional but implemented for further extensibility
            // didnt implement it into other entities though

            return user;
        }


    }
}
