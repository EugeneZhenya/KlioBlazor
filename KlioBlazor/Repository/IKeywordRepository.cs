using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Repository
{
    public interface IKeywordRepository
    {
        Task CreateKeyword(Keyword keyword);
    }
}
