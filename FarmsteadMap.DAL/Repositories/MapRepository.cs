using FarmsteadMap.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmsteadMap.DAL.Repositories
{
    public class MapRepository : IMapRepository
    {
        private readonly AppDbContext _context;

        public MapRepository(AppDbContext context)
        {
            _context = context;
        }

        // Map operations
        public async Task<Map?> GetByIdAsync(long id)
        {
            return await _context.Maps
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Map>> GetByUserIdAsync(long userId)
        {
            return await _context.Maps
                .Where(m => m.UserId == userId)
                .ToListAsync();
        }

        public async Task<long> CreateAsync(Map map)
        {
            _context.Maps.Add(map);
            await _context.SaveChangesAsync();
            return map.Id;
        }

        public async Task<bool> UpdateAsync(Map map)
        {
            _context.Maps.Update(map);
            var affected = await _context.SaveChangesAsync();
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var map = await _context.Maps.FindAsync(id);
            if (map == null) return false;

            _context.Maps.Remove(map);
            var affected = await _context.SaveChangesAsync();
            return affected > 0;
        }

        public async Task<bool> ExistsAsync(long id)
        {
            return await _context.Maps.AnyAsync(m => m.Id == id);
        }

        // Elements operations
        public async Task<List<TreeSort>> GetTreeSortsAsync()
        {
            return await _context.TreeSorts
                .Include(ts => ts.Tree)
                .ToListAsync();
        }

        public async Task<List<VegSort>> GetVegSortsAsync()
        {
            return await _context.VegSorts
                .Include(vs => vs.Vegetable)
                .ToListAsync();
        }

        public async Task<List<Flower>> GetFlowersAsync()
        {
            return await _context.Flowers.ToListAsync();
        }

        public async Task<List<Tree>> GetTreesAsync()
        {
            return await _context.Trees.ToListAsync();
        }

        public async Task<List<Vegetable>> GetVegetablesAsync()
        {
            return await _context.Vegetables.ToListAsync();
        }

        // Incompatibility operations (respecting check constraints)
        public async Task<List<TreeIncompatibility>> GetTreeIncompatibilitiesAsync()
        {
            return await _context.TreeIncompatibilities.ToListAsync();
        }

        public async Task<List<VegIncompatibility>> GetVegIncompatibilitiesAsync()
        {
            return await _context.VegIncompatibilities.ToListAsync();
        }

        public async Task<List<long>> GetIncompatibleTreeIdsAsync(long treeId)
        {
            // Шукаємо в обох колонках через окремі запити
            var asTree1 = await _context.TreeIncompatibilities
                .Where(ti => ti.Tree1Id == treeId)
                .Select(ti => ti.Tree2Id)
                .ToListAsync();

            var asTree2 = await _context.TreeIncompatibilities
                .Where(ti => ti.Tree2Id == treeId)
                .Select(ti => ti.Tree1Id)
                .ToListAsync();

            return asTree1.Union(asTree2).ToList();
        }

        public async Task<List<long>> GetIncompatibleVegIdsAsync(long vegId)
        {
            // Шукаємо в обох колонках через окремі запити
            var asVeg1 = await _context.VegIncompatibilities
                .Where(vi => vi.Veg1Id == vegId)
                .Select(vi => vi.Veg2Id)
                .ToListAsync();

            var asVeg2 = await _context.VegIncompatibilities
                .Where(vi => vi.Veg2Id == vegId)
                .Select(vi => vi.Veg1Id)
                .ToListAsync();

            return asVeg1.Union(asVeg2).ToList();
        }

        public async Task<bool> AreTreesIncompatibleAsync(long tree1Id, long tree2Id)
        {
            // Нормалізуємо ID відповідно до check constraint (tree1_id < tree2_id)
            var (smallerId, largerId) = NormalizeIds(tree1Id, tree2Id);

            return await _context.TreeIncompatibilities
                .AnyAsync(ti => ti.Tree1Id == smallerId && ti.Tree2Id == largerId);
        }

        public async Task<bool> AreVegetablesIncompatibleAsync(long veg1Id, long veg2Id)
        {
            // Нормалізуємо ID відповідно до check constraint (veg1_id < veg2_id)
            var (smallerId, largerId) = NormalizeIds(veg1Id, veg2Id);

            return await _context.VegIncompatibilities
                .AnyAsync(vi => vi.Veg1Id == smallerId && vi.Veg2Id == largerId);
        }

        // Допоміжний метод для нормалізації ID відповідно до check constraints
        private (long smallerId, long largerId) NormalizeIds(long id1, long id2)
        {
            return id1 < id2 ? (id1, id2) : (id2, id1);
        }
    }
}