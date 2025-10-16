// Rooms management
let allRooms = [];
let filteredRooms = [];

async function loadRooms() {
    try {
        allRooms = await apiCall('/rooms');
        filteredRooms = [...allRooms];
        displayRooms();
    } catch (error) {
        console.error('Error loading rooms:', error);
        showNotification('Không thể tải danh sách phòng', 'error');
    }
}

function displayRooms() {
    const grid = document.getElementById('roomsGrid');
    if (!grid) return;

    if (filteredRooms.length === 0) {
        grid.innerHTML = '<div class="loading-spinner"><p>Không có phòng nào</p></div>';
        return;
    }

    grid.innerHTML = filteredRooms.map(room => `
        <div class="room-card">
            <div class="room-image">
                <i class="fas fa-bed"></i>
            </div>
            <div class="room-content">
                <div class="room-header">
                    <div class="room-number">${room.roomNumber}</div>
                    <span class="status-badge ${getRoomStatusClass(room.status)}">
                        ${getRoomStatusText(room.status)}
                    </span>
                </div>
                <div class="room-info">
                    <div class="room-info-item">
                        <i class="fas fa-door-open"></i>
                        <span>${getRoomTypeText(room.type)}</span>
                    </div>
                    <div class="room-info-item">
                        <i class="fas fa-users"></i>
                        <span>Tối đa ${room.maxOccupancy} người</span>
                    </div>
                    <div class="room-info-item">
                        <i class="fas fa-building"></i>
                        <span>Tầng ${room.floor}</span>
                    </div>
                </div>
                <div class="room-price">${formatCurrency(room.pricePerNight)}/đêm</div>
                <div class="room-actions">
                    <button class="btn btn-primary" onclick="editRoom('${room.id}')">
                        <i class="fas fa-edit"></i> Sửa
                    </button>
                    <button class="btn btn-danger" onclick="deleteRoom('${room.id}')">
                        <i class="fas fa-trash"></i> Xóa
                    </button>
                </div>
            </div>
        </div>
    `).join('');
}

function filterRooms() {
    const statusFilter = document.getElementById('statusFilter').value;
    const typeFilter = document.getElementById('typeFilter').value;
    const floorFilter = document.getElementById('floorFilter').value;

    filteredRooms = allRooms.filter(room => {
        if (statusFilter && room.status.toString() !== statusFilter) return false;
        if (typeFilter && room.type.toString() !== typeFilter) return false;
        if (floorFilter && room.floor.toString() !== floorFilter) return false;
        return true;
    });

    displayRooms();
}

function openAddRoomModal() {
    document.getElementById('modalTitle').textContent = 'Thêm phòng mới';
    document.getElementById('roomForm').reset();
    document.getElementById('roomId').value = '';
    document.getElementById('roomModal').classList.add('show');
}

function editRoom(roomId) {
    const room = allRooms.find(r => r.id === roomId);
    if (!room) return;

    document.getElementById('modalTitle').textContent = 'Chỉnh sửa phòng';
    document.getElementById('roomId').value = room.id;
    document.getElementById('roomNumber').value = room.roomNumber;
    document.getElementById('roomType').value = room.type;
    document.getElementById('pricePerNight').value = room.pricePerNight;
    document.getElementById('floor').value = room.floor;
    document.getElementById('maxOccupancy').value = room.maxOccupancy;
    document.getElementById('roomStatus').value = room.status;
    document.getElementById('description').value = room.description || '';
    document.getElementById('amenities').value = room.amenities ? room.amenities.join(', ') : '';

    document.getElementById('roomModal').classList.add('show');
}

async function deleteRoom(roomId) {
    if (!confirm('Bạn có chắc chắn muốn xóa phòng này?')) return;

    try {
        await apiCall(`/rooms/${roomId}`, 'DELETE');
        showNotification('Xóa phòng thành công', 'success');
        loadRooms();
    } catch (error) {
        showNotification(error.message, 'error');
    }
}

function closeRoomModal() {
    document.getElementById('roomModal').classList.remove('show');
}

// Form submission
document.addEventListener('DOMContentLoaded', function() {
    if (window.location.pathname.includes('Rooms')) {
        loadRooms();

        const roomForm = document.getElementById('roomForm');
        if (roomForm) {
            roomForm.addEventListener('submit', async function(e) {
                e.preventDefault();

                const roomId = document.getElementById('roomId').value;
                const amenitiesText = document.getElementById('amenities').value;
                
                const roomData = {
                    roomNumber: document.getElementById('roomNumber').value,
                    type: parseInt(document.getElementById('roomType').value),
                    status: parseInt(document.getElementById('roomStatus').value),
                    pricePerNight: parseFloat(document.getElementById('pricePerNight').value),
                    floor: parseInt(document.getElementById('floor').value),
                    maxOccupancy: parseInt(document.getElementById('maxOccupancy').value),
                    description: document.getElementById('description').value,
                    amenities: amenitiesText ? amenitiesText.split(',').map(a => a.trim()) : [],
                    imageUrl: '/images/rooms/default.jpg'
                };

                try {
                    if (roomId) {
                        // Update existing room
                        await apiCall(`/rooms/${roomId}`, 'PUT', roomData);
                        showNotification('Cập nhật phòng thành công', 'success');
                    } else {
                        // Add new room
                        await apiCall('/rooms', 'POST', roomData);
                        showNotification('Thêm phòng mới thành công', 'success');
                    }
                    
                    closeRoomModal();
                    loadRooms();
                } catch (error) {
                    showNotification(error.message, 'error');
                }
            });
        }

        // Close modal when clicking outside
        document.getElementById('roomModal')?.addEventListener('click', function(e) {
            if (e.target === this) {
                closeRoomModal();
            }
        });
    }
});
