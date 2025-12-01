// <copyright file="MockMapService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.WPF
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Імітує сервіс BLL для роботи з мапами.
    /// </summary>
    public static class MockMapService
    {
        // Імітація бази даних
        // Виправлено IDE0028/IDE0305
        private static readonly List<Map> Maps =
        [
            new () { Id = 1, Name = "Моя перша мапа", ImageUrl = "/Images/map_placeholder_1.jpg" },
            new () { Id = 2, Name = "Сад біля будинку", ImageUrl = "/Images/map_placeholder_2.jpg" },
        ];

        private static int nextId = 3;

        /// <summary>
        /// Імітує отримання мап для користувача.
        /// </summary>
        /// <returns>Список мап.</returns>
        public static async Task<List<Map>> GetUserMapsAsync()
        {
            await Task.Delay(200);
            return Maps.ToList();
        }

        /// <summary>
        /// Імітує видалення мапи.
        /// </summary>
        /// <param name="mapId">ID мапи для видалення.</param>
        /// <returns>Task.</returns>
        public static async Task DeleteMapAsync(int mapId)
        {
            await Task.Delay(100);
            var map = Maps.FirstOrDefault(m => m.Id == mapId);
            if (map != null)
            {
                Maps.Remove(map);
            }
        }

        /// <summary>
        /// Імітує створення нової мапи.
        /// </summary>
        /// <param name="name">Назва нової мапи.</param>
        /// <returns>Нова створена мапа.</returns>
        public static async Task<Map> CreateMapAsync(string name)
        {
            await Task.Delay(100);
            var newMap = new Map
            {
                Id = nextId++,
                Name = name,
                ImageUrl = "/Images/map_placeholder_1.jpg",
            };
            Maps.Add(newMap);
            return newMap;
        }

        /// <summary>
        /// Імітує перейменування мапи.
        /// </summary>
        /// <param name="mapId">ID мапи.</param>
        /// <param name="newName">Нова назва.</param>
        /// <returns>Task.</returns>
        public static async Task RenameMapAsync(int mapId, string newName)
        {
            await Task.Delay(100);
            var map = Maps.FirstOrDefault(m => m.Id == mapId);
            if (map != null)
            {
                map.Name = newName;
            }
        }
    }
}