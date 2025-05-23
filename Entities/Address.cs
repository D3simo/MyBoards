﻿namespace MyBoards.Entities
{
    public class Address
    {
        // Address properties
        public Guid Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }

        // Reference do User entity
        public User User { get; set; }

        // Foreign key to User
         public Guid UserId { get; set; }
    }
}
