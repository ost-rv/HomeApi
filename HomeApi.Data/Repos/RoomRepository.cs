using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeApi.Data.Repos
{
    /// <summary>
    /// Репозиторий для операций с объектами типа "Room" в базе
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly HomeApiContext _context;

        public RoomRepository(HomeApiContext context)
        {
            _context = context;
        }

        /// <summary>
        ///  Найти комнату по имени
        /// </summary>
        public async Task<Room> GetRoomByName(string name)
        {
            return await _context.Rooms.Where(r => r.Name == name).FirstOrDefaultAsync();
        }

        /// <summary>
        ///  Добавить новую комнату
        /// </summary>
        public async Task AddRoom(Room room)
        {
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                await _context.Rooms.AddAsync(room);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Обновить существующую комнату
        /// </summary>
        public async Task UpdateRoom(Room room, UpdateRoomQuery query)
        {

            // Если в запрос переданы параметры для обновления — проверяем их на null
            // И если нужно — обновляем свойство комнаты
            if (query.NewAddDate.HasValue)
            { 
                room.AddDate = query.NewAddDate.Value;
            }
            if (!string.IsNullOrEmpty(query.NewName))
            {
                room.Name = query.NewName;
            }
            if (query.NewArea.HasValue)
            {
                room.Area = query.NewArea.Value;
            }
            if (query.NewGasConnected.HasValue)
            {
                room.GasConnected = query.NewGasConnected.Value;
            }
            if (query.NewVoltage.HasValue)
            {
                room.Voltage = query.NewVoltage.Value;
            }

            // Добавляем в базу
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                _context.Rooms.Update(room);

            // Сохраняем изменения в базе
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Найти комнату по идентификатору
        /// </summary>
        public async Task<Room> GetRoomById(Guid id)
        {
            return await _context.Rooms
                .Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Удалить комнату
        /// </summary>
        public async Task DeleteRoom(Room room)
        {
            // Удаление из базы
            var entry = _context.Entry(room);
            if (entry.State != EntityState.Detached)
                _context.Rooms.Remove(room);

            // Сохранение изменений
            await _context.SaveChangesAsync();
        }
    }
}