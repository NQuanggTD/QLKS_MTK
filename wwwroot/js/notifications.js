// Notifications management
let allNotifications = [];
let unreadCount = 0;

async function loadNotifications() {
    try {
        allNotifications = await apiCall('/notifications');
        updateNotificationBadge();
    } catch (error) {
        console.error('Error loading notifications:', error);
    }
}

async function loadUnreadCount() {
    try {
        const result = await apiCall('/notifications/count');
        unreadCount = result.count;
        updateNotificationBadge();
    } catch (error) {
        console.error('Error loading unread count:', error);
    }
}

function updateNotificationBadge() {
    const badge = document.querySelector('.icon-btn .badge');
    if (badge) {
        if (unreadCount > 0) {
            badge.textContent = unreadCount > 99 ? '99+' : unreadCount;
            badge.style.display = 'block';
        } else {
            badge.style.display = 'none';
        }
    }
}

function toggleNotificationPanel() {
    const panel = document.getElementById('notificationPanel');
    if (!panel) {
        createNotificationPanel();
    } else {
        if (panel.classList.contains('show')) {
            panel.classList.remove('show');
        } else {
            loadNotifications().then(() => {
                displayNotifications();
                panel.classList.add('show');
            });
        }
    }
}

function createNotificationPanel() {
    const panel = document.createElement('div');
    panel.id = 'notificationPanel';
    panel.className = 'notification-panel';
    panel.innerHTML = `
        <div class="notification-header">
            <h3><i class="fas fa-bell"></i> Thông báo</h3>
            <div class="notification-actions">
                <button class="icon-btn-small" onclick="markAllAsRead()" title="Đánh dấu đã đọc tất cả">
                    <i class="fas fa-check-double"></i>
                </button>
                <button class="icon-btn-small" onclick="deleteAllReadNotifications()" title="Xóa đã đọc">
                    <i class="fas fa-trash"></i>
                </button>
            </div>
        </div>
        <div class="notification-body" id="notificationList"></div>
    `;
    document.body.appendChild(panel);
    
    loadNotifications().then(() => {
        displayNotifications();
        panel.classList.add('show');
    });

    // Close when clicking outside
    document.addEventListener('click', function(e) {
        if (!panel.contains(e.target) && !e.target.closest('.icon-btn[title="Thông báo"]')) {
            panel.classList.remove('show');
        }
    });
}

function displayNotifications() {
    const listDiv = document.getElementById('notificationList');
    if (!listDiv) return;

    if (allNotifications.length === 0) {
        listDiv.innerHTML = '<div class="notification-empty"><i class="fas fa-inbox"></i><p>Không có thông báo nào</p></div>';
        return;
    }

    listDiv.innerHTML = allNotifications.map(notif => `
        <div class="notification-item ${notif.isRead ? 'read' : 'unread'}" onclick="handleNotificationClick('${notif.id}', '${notif.link}')">
            <div class="notification-icon ${getNotificationIconClass(notif.type)}">
                <i class="fas ${notif.icon || 'fa-info-circle'}"></i>
            </div>
            <div class="notification-content">
                <div class="notification-title">${notif.title}</div>
                <div class="notification-message">${notif.message}</div>
                <div class="notification-time">${formatTimeAgo(notif.createdDate)}</div>
            </div>
            ${!notif.isRead ? '<div class="notification-dot"></div>' : ''}
            <button class="notification-delete" onclick="deleteNotification(event, '${notif.id}')" title="Xóa">
                <i class="fas fa-times"></i>
            </button>
        </div>
    `).join('');
}

function getNotificationIconClass(type) {
    const classMap = {
        0: 'notif-info',      // Info
        1: 'notif-success',   // Success
        2: 'notif-warning',   // Warning
        3: 'notif-error',     // Error
        4: 'notif-booking',   // Booking
        5: 'notif-payment',   // Payment
        6: 'notif-system'     // System
    };
    return classMap[type] || 'notif-info';
}

function formatTimeAgo(dateString) {
    const date = new Date(dateString);
    const now = new Date();
    const diffMs = now - date;
    const diffMins = Math.floor(diffMs / 60000);
    const diffHours = Math.floor(diffMs / 3600000);
    const diffDays = Math.floor(diffMs / 86400000);

    if (diffMins < 1) return 'Vừa xong';
    if (diffMins < 60) return `${diffMins} phút trước`;
    if (diffHours < 24) return `${diffHours} giờ trước`;
    if (diffDays < 7) return `${diffDays} ngày trước`;
    return formatDate(dateString);
}

async function handleNotificationClick(id, link) {
    try {
        await apiCall(`/notifications/${id}/read`, 'PATCH');
        if (link && link !== '') {
            window.location.href = link;
        }
        loadUnreadCount();
    } catch (error) {
        console.error('Error marking notification as read:', error);
    }
}

async function deleteNotification(event, id) {
    event.stopPropagation();
    
    try {
        await apiCall(`/notifications/${id}`, 'DELETE');
        allNotifications = allNotifications.filter(n => n.id !== id);
        displayNotifications();
        loadUnreadCount();
    } catch (error) {
        console.error('Error deleting notification:', error);
    }
}

async function markAllAsRead() {
    try {
        await apiCall('/notifications/read-all', 'PATCH');
        allNotifications.forEach(n => n.isRead = true);
        displayNotifications();
        loadUnreadCount();
        showNotification('Đã đánh dấu tất cả là đã đọc', 'success');
    } catch (error) {
        showNotification('Có lỗi xảy ra', 'error');
    }
}

async function deleteAllReadNotifications() {
    if (!confirm('Bạn có chắc muốn xóa tất cả thông báo đã đọc?')) return;
    
    try {
        await apiCall('/notifications/read', 'DELETE');
        allNotifications = allNotifications.filter(n => !n.isRead);
        displayNotifications();
        showNotification('Đã xóa thông báo đã đọc', 'success');
    } catch (error) {
        showNotification('Có lỗi xảy ra', 'error');
    }
}

// Initialize notifications
document.addEventListener('DOMContentLoaded', function() {
    loadUnreadCount();
    
    // Refresh notification count every 30 seconds
    setInterval(loadUnreadCount, 30000);
});
