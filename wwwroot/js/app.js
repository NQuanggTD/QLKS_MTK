// Global utility functions
const API_BASE = '/api';

// Set active navigation item
function setActiveNav() {
    const path = window.location.pathname;
    const navItems = document.querySelectorAll('.nav-item');
    
    navItems.forEach(item => {
        item.classList.remove('active');
        if (item.getAttribute('href') === path) {
            item.classList.add('active');
        }
    });
}

// Format currency
function formatCurrency(amount) {
    return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    }).format(amount);
}

// Format date
function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('vi-VN', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit'
    });
}

// Format datetime
function formatDateTime(dateString) {
    const date = new Date(dateString);
    return date.toLocaleString('vi-VN', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit'
    });
}

// Show notification
function showNotification(message, type = 'success') {
    // Create toast container if not exists
    let container = document.querySelector('.toast-container');
    if (!container) {
        container = document.createElement('div');
        container.className = 'toast-container';
        document.body.appendChild(container);
    }

    // Create toast element
    const toast = document.createElement('div');
    toast.className = `toast toast-${type}`;
    
    const iconMap = {
        success: 'fa-check-circle',
        error: 'fa-exclamation-circle',
        warning: 'fa-exclamation-triangle',
        info: 'fa-info-circle'
    };
    
    toast.innerHTML = `
        <i class="fas ${iconMap[type] || iconMap.info}"></i>
        <div class="toast-message">${message}</div>
    `;
    
    container.appendChild(toast);
    
    // Auto remove after 3 seconds
    setTimeout(() => {
        toast.style.opacity = '0';
        toast.style.transform = 'translateX(400px)';
        setTimeout(() => {
            container.removeChild(toast);
            if (container.children.length === 0) {
                document.body.removeChild(container);
            }
        }, 300);
    }, 3000);
}

// API call helper
async function apiCall(endpoint, method = 'GET', data = null) {
    const options = {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        }
    };

    if (data && (method === 'POST' || method === 'PUT' || method === 'PATCH')) {
        options.body = JSON.stringify(data);
    }

    try {
        const response = await fetch(API_BASE + endpoint, options);
        
        if (!response.ok) {
            const error = await response.json();
            throw new Error(error.message || 'Có lỗi xảy ra');
        }

        // Handle DELETE requests with no content
        if (response.status === 204) {
            return null;
        }

        return await response.json();
    } catch (error) {
        console.error('API Error:', error);
        throw error;
    }
}

// Global search functionality
document.addEventListener('DOMContentLoaded', function() {
    setActiveNav();
    
    const globalSearch = document.getElementById('globalSearch');
    if (globalSearch) {
        globalSearch.addEventListener('keyup', function(e) {
            if (e.key === 'Enter') {
                const keyword = this.value.trim();
                if (keyword) {
                    // Implement global search logic here
                    console.log('Searching for:', keyword);
                }
            }
        });
    }
});

// Room status helpers
function getRoomStatusText(status) {
    const statusMap = {
        0: 'Trống',
        1: 'Đang sử dụng',
        2: 'Bảo trì',
        3: 'Đã đặt'
    };
    return statusMap[status] || 'Không xác định';
}

function getRoomStatusClass(status) {
    const classMap = {
        0: 'status-available',
        1: 'status-occupied',
        2: 'status-maintenance',
        3: 'status-reserved'
    };
    return classMap[status] || '';
}

// Room type helpers
function getRoomTypeText(type) {
    const typeMap = {
        0: 'Phòng đơn',
        1: 'Phòng đôi',
        2: 'Suite',
        3: 'Deluxe',
        4: 'Presidential'
    };
    return typeMap[type] || 'Không xác định';
}

// Booking status helpers
function getBookingStatusText(status) {
    const statusMap = {
        0: 'Chờ xác nhận',
        1: 'Đã xác nhận',
        2: 'Đã nhận phòng',
        3: 'Đã trả phòng',
        4: 'Đã hủy'
    };
    return statusMap[status] || 'Không xác định';
}

function getBookingStatusClass(status) {
    const classMap = {
        0: 'status-pending',
        1: 'status-confirmed',
        2: 'status-checkedin',
        3: 'status-available',
        4: 'status-cancelled'
    };
    return classMap[status] || '';
}

// Service category helpers
function getServiceCategoryText(category) {
    const categoryMap = {
        0: 'Ăn uống',
        1: 'Đưa đón',
        2: 'Spa',
        3: 'Giặt ủi',
        4: 'Dịch vụ phòng',
        5: 'Giải trí',
        6: 'Hội nghị'
    };
    return categoryMap[category] || 'Khác';
}

function getServiceCategoryIcon(category) {
    const iconMap = {
        0: 'fa-utensils',
        1: 'fa-car',
        2: 'fa-spa',
        3: 'fa-tshirt',
        4: 'fa-concierge-bell',
        5: 'fa-gamepad',
        6: 'fa-briefcase'
    };
    return iconMap[category] || 'fa-concierge-bell';
}
