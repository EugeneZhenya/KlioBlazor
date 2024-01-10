using KlioBlazor.Helpers;
using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Components;
using System.Collections.Concurrent;

namespace KlioBlazor.Repository
{
    public class PartitionRepository : IPartitionRepository
    {
        private NavigationManager navigationManager { get; set; }
        private readonly IHttpService httpService;
        private string url = "api/partitions";

        public PartitionRepository(IHttpService httpService, NavigationManager navigationManager)
        {
            this.httpService = httpService;
            this.navigationManager = navigationManager;
            this.url = this.navigationManager.BaseUri + this.url;
        }

        public async Task<List<Partition>> GetAllPartitions()
        {
            return await Get<List<Partition>>($"{url}/all");
        }

        public async Task<IndexPartitionsDTO> GetIndexPartitionsDTO()
        {
            return await Get<IndexPartitionsDTO>(url);
        }

        public async Task<Partition> GetPartition(int Id)
        {
            return await Get<Partition>($"{url}/{Id}");
        }

        private async Task<T> Get<T>(string url)
        {
            var response = await httpService.Get<T>(url);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task CreatePartition(Partition partition)
        {
            var response = await httpService.Post(url, partition);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task UpdatePartition(Partition partition)
        {
            var response = await httpService.Put(url, partition);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
