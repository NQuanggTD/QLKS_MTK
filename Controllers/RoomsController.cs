using Microsoft.AspNetCore.Mvc;
using QLKS.Models;
using QLKS.Services;

namespace QLKS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public IActionResult GetAllRooms()
        {
            var rooms = _roomService.GetAllRooms();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public IActionResult GetRoomById(string id)
        {
            var room = _roomService.GetRoomById(id);
            if (room == null)
                return NotFound(new { message = "Phòng không tồn tại" });

            return Ok(room);
        }

        [HttpGet("status/{status}")]
        public IActionResult GetRoomsByStatus(RoomStatus status)
        {
            var rooms = _roomService.GetRoomsByStatus(status);
            return Ok(rooms);
        }

        [HttpGet("type/{type}")]
        public IActionResult GetRoomsByType(RoomType type)
        {
            var rooms = _roomService.GetRoomsByType(type);
            return Ok(rooms);
        }

        [HttpGet("available")]
        public IActionResult GetAvailableRooms([FromQuery] DateTime checkIn, [FromQuery] DateTime checkOut)
        {
            var rooms = _roomService.GetAvailableRooms(checkIn, checkOut);
            return Ok(rooms);
        }

        [HttpPost]
        public IActionResult AddRoom([FromBody] Room room)
        {
            try
            {
                var newRoom = _roomService.AddRoom(room);
                return CreatedAtAction(nameof(GetRoomById), new { id = newRoom.Id }, newRoom);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRoom(string id, [FromBody] Room room)
        {
            try
            {
                room.Id = id;
                var updatedRoom = _roomService.UpdateRoom(room);
                
                if (updatedRoom == null)
                    return NotFound(new { message = "Phòng không tồn tại" });

                return Ok(updatedRoom);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRoom(string id)
        {
            var result = _roomService.DeleteRoom(id);
            if (!result)
                return NotFound(new { message = "Phòng không tồn tại" });

            return Ok(new { message = "Xóa phòng thành công" });
        }

        [HttpPatch("{id}/status")]
        public IActionResult UpdateRoomStatus(string id, [FromBody] RoomStatusUpdate statusUpdate)
        {
            var result = _roomService.UpdateRoomStatus(id, statusUpdate.Status);
            if (!result)
                return NotFound(new { message = "Phòng không tồn tại" });

            return Ok(new { message = "Cập nhật trạng thái thành công" });
        }
    }

    public class RoomStatusUpdate
    {
        public RoomStatus Status { get; set; }
    }
}
