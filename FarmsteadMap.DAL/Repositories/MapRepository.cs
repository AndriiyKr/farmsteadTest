// <copyright file="MapRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.DAL.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FarmsteadMap.DAL.Data.Models;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Implementation of the map repository for managing map data.
    /// </summary>
    public class MapRepository : IMapRepository
    {
        private readonly AppDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public MapRepository(AppDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public async Task<Map?> GetByIdAsync(long id)
        {
            return await this.context.Maps
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        /// <inheritdoc/>
        public async Task<List<Map>> GetByUserIdAsync(long userId)
        {
            return await this.context.Maps
                .Where(m => m.UserId == userId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<long> CreateAsync(Map map)
        {
            this.context.Maps.Add(map);
            await this.context.SaveChangesAsync();
            return map.Id;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(Map map)
        {
            this.context.Maps.Update(map);
            var affected = await this.context.SaveChangesAsync();
            return affected > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(long id)
        {
            var map = await this.context.Maps.FindAsync(id);
            if (map == null)
            {
                return false;
            }

            this.context.Maps.Remove(map);
            var affected = await this.context.SaveChangesAsync();
            return affected > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(long id)
        {
            return await this.context.Maps.AnyAsync(m => m.Id == id);
        }

        /// <inheritdoc/>
        /// <inheritdoc/>
        public async Task<List<TreeSort>> GetTreeSortsAsync(long treeId) // <--- Додали параметр
        {
            return await this.context.TreeSorts
                .Include(ts => ts.Tree)
                .Where(ts => ts.TreeId == treeId) // <--- Додали фільтр SQL (WHERE tree_id = @treeId)
                .ToListAsync();
        }

        // Аналогічно для овочів:
        public async Task<List<VegSort>> GetVegSortsAsync(long vegId)
        {
            return await this.context.VegSorts
                .Include(vs => vs.Vegetable)
                .Where(vs => vs.VegId == vegId) // Переконайся, що властивість називається VegetableId або VegId
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<Flower>> GetFlowersAsync()
        {
            return await this.context.Flowers.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<Tree>> GetTreesAsync()
        {
            return await this.context.Trees.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<Vegetable>> GetVegetablesAsync()
        {
            return await this.context.Vegetables.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<TreeIncompatibility>> GetTreeIncompatibilitiesAsync()
        {
            return await this.context.TreeIncompatibilities.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<VegIncompatibility>> GetVegIncompatibilitiesAsync()
        {
            return await this.context.VegIncompatibilities.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<long>> GetIncompatibleTreeIdsAsync(long treeId)
        {
            var asTree1 = await this.context.TreeIncompatibilities
                .Where(ti => ti.Tree1Id == treeId)
                .Select(ti => ti.Tree2Id)
                .ToListAsync();

            var asTree2 = await this.context.TreeIncompatibilities
                .Where(ti => ti.Tree2Id == treeId)
                .Select(ti => ti.Tree1Id)
                .ToListAsync();

            return asTree1.Union(asTree2).ToList();
        }

        /// <inheritdoc/>
        public async Task<List<long>> GetIncompatibleVegIdsAsync(long vegId)
        {
            var asVeg1 = await this.context.VegIncompatibilities
                .Where(vi => vi.Veg1Id == vegId)
                .Select(vi => vi.Veg2Id)
                .ToListAsync();

            var asVeg2 = await this.context.VegIncompatibilities
                .Where(vi => vi.Veg2Id == vegId)
                .Select(vi => vi.Veg1Id)
                .ToListAsync();

            return asVeg1.Union(asVeg2).ToList();
        }

        /// <inheritdoc/>
        public async Task<bool> AreTreesIncompatibleAsync(long tree1Id, long tree2Id)
        {
            var (smallerId, largerId) = this.NormalizeIds(tree1Id, tree2Id);

            return await this.context.TreeIncompatibilities
                .AnyAsync(ti => ti.Tree1Id == smallerId && ti.Tree2Id == largerId);
        }

        /// <inheritdoc/>
        public async Task<bool> AreVegetablesIncompatibleAsync(long veg1Id, long veg2Id)
        {
            var (smallerId, largerId) = this.NormalizeIds(veg1Id, veg2Id);

            return await this.context.VegIncompatibilities
                .AnyAsync(vi => vi.Veg1Id == smallerId && vi.Veg2Id == largerId);
        }

        private (long smallerId, long largerId) NormalizeIds(long id1, long id2)
        {
            return id1 < id2 ? (id1, id2) : (id2, id1);
        }
    }
}