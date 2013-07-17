using System;

namespace OrangeJuice.Server.Data
{
    public interface IUserRepository
    {
        Guid Register(string email);
    }

    public class UserRepositoryStub : IUserRepository
    {
        public Guid Register(string email)
        {
            return Guid.NewGuid();
        }
    }
}