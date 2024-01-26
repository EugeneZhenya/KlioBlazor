using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Repository
{
    public interface IPartitionRepository
    {
        Task CreatePartition(Partition partition);
        Task DeletePartition(int Id);
        Task<List<Partition>> GetAllPartitions();
        Task<DetailsPartitionDTO> GetDetailsPartitionDTO(int Id);
        Task<IndexPartitionsDTO> GetIndexPartitionsDTO();
        Task<Partition> GetPartition(int Id);
        Task UpdatePartition(Partition partition);
    }
}
