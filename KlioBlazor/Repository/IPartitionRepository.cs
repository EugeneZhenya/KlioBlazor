using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Repository
{
    public interface IPartitionRepository
    {
        Task CreatePartition(Partition partition);
        Task<List<Partition>> GetPartitions();
    }
}
