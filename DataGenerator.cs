using Bogus;
using MyBoards.Entities;

namespace MyBoards
{
    public class DataGenerator
    {
        public void Seed(MyBoardsContext context)
        {
            var addressGenerator = new Faker<Address>()
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.Country, f => f.Address.Country())
                .RuleFor(a => a.Street, f => f.Address.StreetName())
                .RuleFor(a => a.PostalCode, f => f.Address.ZipCode());

            Address address = addressGenerator.Generate();

            var userGenerator = new Faker<User>()
                //.RuleFor(u => u.Id, f => Guid.NewGuid())
                .RuleFor(u => u.FullName, f => f.Person.FullName)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.Address, f => addressGenerator.Generate());

            User user = userGenerator.Generate();

            var users = userGenerator.Generate(100);
        }
    }
}
