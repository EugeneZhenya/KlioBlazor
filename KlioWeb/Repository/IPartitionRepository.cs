using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;

namespace KlioWeb.Repository
{
    public interface IPartitionRepository
    {
        Task<List<Partition>> GetAllPartitions();
        Task<DetailsPartitionDTO> GetDetailsPartitionDTO(int Id);
        Task<IndexPartitionsDTO> GetIndexPartitionsDTO();
        Task<Partition> GetPartition(int Id);
    }
}
