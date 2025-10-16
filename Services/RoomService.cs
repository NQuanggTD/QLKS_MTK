using QLKS.Models;
using QLKS.Utils;

namespace QLKS.Services
{
    public interface IRoomService
    {
        List<Room> GetAllRooms();
        Room? GetRoomById(string id);
        Room? GetRoomByNumber(string roomNumber);
        List<Room> GetRoomsByStatus(RoomStatus status);
        List<Room> GetRoomsByType(RoomType type);
        List<Room> GetAvailableRooms(DateTime checkIn, DateTime checkOut);
        Room AddRoom(Room room);
        Room? UpdateRoom(Room room);
        bool DeleteRoom(string id);
        bool UpdateRoomStatus(string id, RoomStatus status);
    }

    public class RoomService : IRoomService
    {
        private readonly IDataStore _dataStore;
        private const string DataKey = "rooms";

        public RoomService(IDataStore dataStore)
        {
            _dataStore = dataStore;
            InitializeSampleData();
        }

        private void InitializeSampleData()
        {
            var rooms = _dataStore.Load<List<Room>>(DataKey);
            if (rooms == null || !rooms.Any())
            {
                rooms = new List<Room>
                {
                    new Room
                    {
                        RoomNumber = "101",
                        Type = RoomType.Standard,
                        Status = RoomStatus.Available,
                        PricePerNight = 500000,
                        Floor = 1,
                        MaxGuests = 2,
                        ImageUrl = "/images/rooms/standard.jpg",
                        Description = "Phòng tiêu chuẩn tiện nghi, phù hợp cho khách đi công tác",
                        Amenities = new List<string> { "WiFi", "TV", "Điều hòa", "Minibar" }
                    },
                    new Room
                    {
                        RoomNumber = "102",
                        Type = RoomType.Standard,
                        Status = RoomStatus.Available,
                        PricePerNight = 500000,
                        Floor = 1,
                        MaxGuests = 2,
                        ImageUrl = "/images/rooms/standard.jpg",
                        Description = "Phòng tiêu chuẩn rộng rãi",
                        Amenities = new List<string> { "WiFi", "TV", "Điều hòa", "Minibar", "Ban công" }
                    },
                    new Room
                    {
                        RoomNumber = "201",
                        Type = RoomType.Suite,
                        Status = RoomStatus.Occupied,
                        PricePerNight = 1500000,
                        Floor = 2,
                        MaxGuests = 4,
                        ImageUrl = "/images/rooms/suite.jpg",
                        Description = "Suite sang trọng với phòng khách riêng",
                        Amenities = new List<string> { "WiFi", "Smart TV", "Điều hòa", "Minibar", "Ban công", "Bồn tắm" }
                    },
                    new Room
                    {
                        RoomNumber = "202",
                        Type = RoomType.Deluxe,
                        Status = RoomStatus.Available,
                        PricePerNight = 800000,
                        Floor = 2,
                        MaxGuests = 3,
                        ImageUrl = "/images/rooms/deluxe.jpg",
                        Description = "Phòng Deluxe cao cấp với view đẹp",
                        Amenities = new List<string> { "WiFi", "Smart TV", "Điều hòa", "Minibar", "Ban công", "Bồn tắm", "Bếp nhỏ" }
                    },
                    new Room
                    {
                        RoomNumber = "301",
                        Type = RoomType.Presidential,
                        Status = RoomStatus.Available,
                        PricePerNight = 5000000,
                        Floor = 3,
                        MaxGuests = 6,
                        ImageUrl = "/images/rooms/presidential.jpg",
                        Description = "Phòng Tổng thống với đầy đủ tiện nghi 5 sao",
                        Amenities = new List<string> { "WiFi", "Smart TV", "Điều hòa", "Minibar", "Ban công lớn", "Bồn tắm Jacuzzi", "Phòng khách", "Phòng ăn" }
                    },
                    new Room
                    {
                        RoomNumber = "103",
                        Type = RoomType.Standard,
                        Status = RoomStatus.Maintenance,
                        PricePerNight = 500000,
                        Floor = 1,
                        MaxGuests = 2,
                        ImageUrl = "/images/rooms/standard.jpg",
                        Description = "Phòng tiêu chuẩn đang bảo trì",
                        Amenities = new List<string> { "WiFi", "TV", "Điều hòa" }
                    }
                };

                _dataStore.Save(DataKey, rooms);
                Logger.Info("Initialized sample room data");
            }
        }

        public List<Room> GetAllRooms()
        {
            return _dataStore.Load<List<Room>>(DataKey) ?? new List<Room>();
        }

        public Room? GetRoomById(string id)
        {
            var rooms = GetAllRooms();
            return rooms.FirstOrDefault(r => r.Id == id);
        }

        public Room? GetRoomByNumber(string roomNumber)
        {
            var rooms = GetAllRooms();
            return rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
        }

        public List<Room> GetRoomsByStatus(RoomStatus status)
        {
            var rooms = GetAllRooms();
            return rooms.Where(r => r.Status == status).ToList();
        }

        public List<Room> GetRoomsByType(RoomType type)
        {
            var rooms = GetAllRooms();
            return rooms.Where(r => r.Type == type).ToList();
        }

        public List<Room> GetAvailableRooms(DateTime checkIn, DateTime checkOut)
        {
            return GetRoomsByStatus(RoomStatus.Available);
        }

        public Room AddRoom(Room room)
        {
            var rooms = GetAllRooms();
            
            // Check if room number already exists
            if (rooms.Any(r => r.RoomNumber == room.RoomNumber))
            {
                throw new InvalidOperationException($"Số phòng {room.RoomNumber} đã tồn tại");
            }

            room.Id = Guid.NewGuid().ToString();
            rooms.Add(room);
            _dataStore.Save(DataKey, rooms);
            
            Logger.Info($"Added new room: {room.RoomNumber}");
            return room;
        }

        public Room? UpdateRoom(Room room)
        {
            var rooms = GetAllRooms();
            var existingRoom = rooms.FirstOrDefault(r => r.Id == room.Id);
            
            if (existingRoom == null)
                return null;

            // Update properties
            existingRoom.RoomNumber = room.RoomNumber;
            existingRoom.Type = room.Type;
            existingRoom.Status = room.Status;
            existingRoom.PricePerNight = room.PricePerNight;
            existingRoom.Floor = room.Floor;
            existingRoom.MaxGuests = room.MaxGuests;
            existingRoom.ImageUrl = room.ImageUrl;
            existingRoom.Description = room.Description;
            existingRoom.Amenities = room.Amenities;
            existingRoom.BedType = room.BedType;
            existingRoom.Size = room.Size;

            _dataStore.Save(DataKey, rooms);
            Logger.Info($"Updated room: {room.RoomNumber}");
            
            return existingRoom;
        }

        public bool DeleteRoom(string id)
        {
            var rooms = GetAllRooms();
            var room = rooms.FirstOrDefault(r => r.Id == id);
            
            if (room == null)
                return false;

            rooms.Remove(room);
            _dataStore.Save(DataKey, rooms);
            
            Logger.Info($"Deleted room: {room.RoomNumber}");
            return true;
        }

        public bool UpdateRoomStatus(string id, RoomStatus status)
        {
            var rooms = GetAllRooms();
            var room = rooms.FirstOrDefault(r => r.Id == id);
            
            if (room == null)
                return false;

            room.Status = status;
            _dataStore.Save(DataKey, rooms);
            
            Logger.Info($"Updated room {room.RoomNumber} status to {status}");
            return true;
        }
    }
}
