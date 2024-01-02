using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Repository
{
    public interface IPersonRepository
    {
        Task CreatePerson(Person person);
    }
}
