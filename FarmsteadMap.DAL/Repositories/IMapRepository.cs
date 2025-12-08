// <copyright file="IMapRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.DAL.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FarmsteadMap.DAL.Data.Models;

    /// <summary>
    /// Defines the contract for repository operations related to maps and their elements.
    /// </summary>
    public interface IMapRepository
    {
        /// <summary>
        /// Retrieves a map by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the map.</param>
        /// <returns>The <see cref="Map"/> entity if found; otherwise, null.</returns>
        Task<Map?> GetByIdAsync(long id);

        /// <summary>
        /// Retrieves all maps associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of <see cref="Map"/> entities.</returns>
        Task<List<Map>> GetByUserIdAsync(long userId);

        /// <summary>
        /// Creates a new map in the database.
        /// </summary>
        /// <param name="map">The map entity to create.</param>
        /// <returns>The identifier of the created map.</returns>
        Task<long> CreateAsync(Map map);

        /// <summary>
        /// Updates an existing map in the database.
        /// </summary>
        /// <param name="map">The map entity with updated values.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        Task<bool> UpdateAsync(Map map);

        /// <summary>
        /// Deletes a map from the database.
        /// </summary>
        /// <param name="id">The unique identifier of the map to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        Task<bool> DeleteAsync(long id);

        /// <summary>
        /// Checks if a map exists in the database.
        /// </summary>
        /// <param name="id">The unique identifier of the map.</param>
        /// <returns>True if the map exists; otherwise, false.</returns>
        Task<bool> ExistsAsync(long id);

        /// <summary>
        /// Retrieves all available tree sorts.
        /// </summary>
        /// <returns>A list of <see cref="TreeSort"/> entities.</returns>
        Task<List<TreeSort>> GetTreeSortsAsync(long treeId);

        /// <summary>
        /// Retrieves all available vegetable sorts.
        /// </summary>
        /// <returns>A list of <see cref="VegSort"/> entities.</returns>
        Task<List<VegSort>> GetVegSortsAsync(long vegId);

        /// <summary>
        /// Retrieves all available flowers.
        /// </summary>
        /// <returns>A list of <see cref="Flower"/> entities.</returns>
        Task<List<Flower>> GetFlowersAsync();

        /// <summary>
        /// Retrieves all available tree species.
        /// </summary>
        /// <returns>A list of <see cref="Tree"/> entities.</returns>
        Task<List<Tree>> GetTreesAsync();

        /// <summary>
        /// Retrieves all available vegetable types.
        /// </summary>
        /// <returns>A list of <see cref="Vegetable"/> entities.</returns>
        Task<List<Vegetable>> GetVegetablesAsync();

        /// <summary>
        /// Retrieves all tree incompatibility rules.
        /// </summary>
        /// <returns>A list of <see cref="TreeIncompatibility"/> entities.</returns>
        Task<List<TreeIncompatibility>> GetTreeIncompatibilitiesAsync();

        /// <summary>
        /// Retrieves all vegetable incompatibility rules.
        /// </summary>
        /// <returns>A list of <see cref="VegIncompatibility"/> entities.</returns>
        Task<List<VegIncompatibility>> GetVegIncompatibilitiesAsync();

        /// <summary>
        /// Retrieves a list of tree IDs that are incompatible with the specified tree.
        /// </summary>
        /// <param name="treeId">The identifier of the tree.</param>
        /// <returns>A list of incompatible tree identifiers.</returns>
        Task<List<long>> GetIncompatibleTreeIdsAsync(long treeId);

        /// <summary>
        /// Retrieves a list of vegetable IDs that are incompatible with the specified vegetable.
        /// </summary>
        /// <param name="vegId">The identifier of the vegetable.</param>
        /// <returns>A list of incompatible vegetable identifiers.</returns>
        Task<List<long>> GetIncompatibleVegIdsAsync(long vegId);

        /// <summary>
        /// Checks if two specific trees are incompatible.
        /// </summary>
        /// <param name="tree1Id">The identifier of the first tree.</param>
        /// <param name="tree2Id">The identifier of the second tree.</param>
        /// <returns>True if the trees are incompatible; otherwise, false.</returns>
        Task<bool> AreTreesIncompatibleAsync(long tree1Id, long tree2Id);

        /// <summary>
        /// Checks if two specific vegetables are incompatible.
        /// </summary>
        /// <param name="veg1Id">The identifier of the first vegetable.</param>
        /// <param name="veg2Id">The identifier of the second vegetable.</param>
        /// <returns>True if the vegetables are incompatible; otherwise, false.</returns>
        Task<bool> AreVegetablesIncompatibleAsync(long veg1Id, long veg2Id);
    }
}