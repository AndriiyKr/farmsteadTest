<<<<<<< HEAD
﻿// <copyright file="MapRepository.cs" company="PlaceholderCompany">
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
=======
﻿using FarmsteadMap.DAL.Data.Models;
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
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                .Where(m => m.UserId == userId)
                .ToListAsync();
        }

<<<<<<< HEAD
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
        public async Task<List<TreeSort>> GetTreeSortsAsync()
        {
            return await this.context.TreeSorts
=======
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
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                .Include(ts => ts.Tree)
                .ToListAsync();
        }

<<<<<<< HEAD
        /// <inheritdoc/>
        public async Task<List<VegSort>> GetVegSortsAsync()
        {
            return await this.context.VegSorts
=======
        public async Task<List<VegSort>> GetVegSortsAsync()
        {
            return await _context.VegSorts
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                .Include(vs => vs.Vegetable)
                .ToListAsync();
        }

<<<<<<< HEAD
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
=======
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
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                .Where(ti => ti.Tree1Id == treeId)
                .Select(ti => ti.Tree2Id)
                .ToListAsync();

<<<<<<< HEAD
            var asTree2 = await this.context.TreeIncompatibilities
=======
            var asTree2 = await _context.TreeIncompatibilities
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                .Where(ti => ti.Tree2Id == treeId)
                .Select(ti => ti.Tree1Id)
                .ToListAsync();

            return asTree1.Union(asTree2).ToList();
        }

<<<<<<< HEAD
        /// <inheritdoc/>
        public async Task<List<long>> GetIncompatibleVegIdsAsync(long vegId)
        {
            var asVeg1 = await this.context.VegIncompatibilities
=======
        public async Task<List<long>> GetIncompatibleVegIdsAsync(long vegId)
        {
            // Шукаємо в обох колонках через окремі запити
            var asVeg1 = await _context.VegIncompatibilities
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                .Where(vi => vi.Veg1Id == vegId)
                .Select(vi => vi.Veg2Id)
                .ToListAsync();

<<<<<<< HEAD
            var asVeg2 = await this.context.VegIncompatibilities
=======
            var asVeg2 = await _context.VegIncompatibilities
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                .Where(vi => vi.Veg2Id == vegId)
                .Select(vi => vi.Veg1Id)
                .ToListAsync();

            return asVeg1.Union(asVeg2).ToList();
        }

<<<<<<< HEAD
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

=======
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
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        private (long smallerId, long largerId) NormalizeIds(long id1, long id2)
        {
            return id1 < id2 ? (id1, id2) : (id2, id1);
        }
    }
}