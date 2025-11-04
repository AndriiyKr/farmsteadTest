using FarmsteadMap.DAL.Data.Models;

namespace FarmsteadMap.DAL.Repositories
{
    public interface IMapRepository
    {
        // Map operations
        Task<Map?> GetByIdAsync(long id);
        Task<List<Map>> GetByUserIdAsync(long userId);
        Task<long> CreateAsync(Map map);
        Task<bool> UpdateAsync(Map map);
        Task<bool> DeleteAsync(long id);
        Task<bool> ExistsAsync(long id);

        // Elements operations
        Task<List<TreeSort>> GetTreeSortsAsync();
        Task<List<VegSort>> GetVegSortsAsync();
        Task<List<Flower>> GetFlowersAsync();
        Task<List<Tree>> GetTreesAsync();
        Task<List<Vegetable>> GetVegetablesAsync();

        // Incompatibility operations
        Task<List<TreeIncompatibility>> GetTreeIncompatibilitiesAsync();
        Task<List<VegIncompatibility>> GetVegIncompatibilitiesAsync();
        Task<List<long>> GetIncompatibleTreeIdsAsync(long treeId);
        Task<List<long>> GetIncompatibleVegIdsAsync(long vegId);
        Task<bool> AreTreesIncompatibleAsync(long tree1Id, long tree2Id);
        Task<bool> AreVegetablesIncompatibleAsync(long veg1Id, long veg2Id);
    }
}