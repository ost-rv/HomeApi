using AutoMapper;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HomeApi.Controllers
{
    /// <summary>
    /// Контроллер комнат
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private IRoomRepository _roomRepository;
        private IMapper _mapper;

        public RoomsController(IRoomRepository repository, IMapper mapper)
        {
            _roomRepository = repository;
            _mapper = mapper;
        }

        //TODO: Задание - добавить метод на получение всех существующих комнат

        /// <summary>
        /// Добавление комнаты
        /// </summary>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
        {
            var existingRoom = await _roomRepository.GetRoomByName(request.Name);
            if (existingRoom == null)
            {
                var newRoom = _mapper.Map<AddRoomRequest, Room>(request);
                await _roomRepository.AddRoom(newRoom);
                return StatusCode(201, $"Комната {request.Name} добавлена!");
            }

            return StatusCode(409, $"Ошибка: Комната {request.Name} уже существует.");
        }

        /// <summary>
        /// Обновление существующей комнаты
        /// </summary>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Edit(
            [FromRoute] Guid id,
            [FromBody] EditRoomRequest request)
        {
            
            var withSameName = await _roomRepository.GetRoomByName(request.NewName);
            if (withSameName != null)
                return StatusCode(400, $"Ошибка: Комната с именем {request.NewName} уже присутствует. Выберите другое имя!");

            var room = await _roomRepository.GetRoomById(id);
            if (room == null)
                return StatusCode(400, $"Ошибка: Устройство с идентификатором {id} не существует.");


            var updateRoomQuery = _mapper.Map<EditRoomRequest, UpdateRoomQuery>(request);
            await _roomRepository.UpdateRoom(room, updateRoomQuery);

            return StatusCode(200, $"Комната обновлена!  Имя — {room.Name}, Площадь — {room.Area},  Напряжение  —  {room.Voltage}, Газ — {room.GasConnected }, Дата добавления — {room.AddDate}");
        
        }

        /// <summary>
        /// Удаление существующей комнаты
        /// </summary>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(
            [FromRoute] Guid id)
        {
            var room = await _roomRepository.GetRoomById(id);
            if (room == null)
                return StatusCode(400, $"Ошибка: Комнаты с идентификатором {id} не существует.");

            await _roomRepository.DeleteRoom(room);

            return StatusCode(200, $"Комната  Имя — {room.Name}, id — {room.Id}  удалена!");
        }
    }
}
