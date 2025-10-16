// Settings management
let currentSettings = null;

async function loadSettings() {
    try {
        currentSettings = await apiCall('/settings');
        populateSettingsForm();
    } catch (error) {
        console.error('Error loading settings:', error);
        showNotification('Không thể tải cài đặt', 'error');
    }
}

function populateSettingsForm() {
    if (!currentSettings) return;

    // Hotel Information
    document.getElementById('hotelName').value = currentSettings.hotelName || '';
    document.getElementById('hotelAddress').value = currentSettings.hotelAddress || '';
    document.getElementById('hotelPhone').value = currentSettings.hotelPhone || '';
    document.getElementById('hotelEmail').value = currentSettings.hotelEmail || '';

    // Booking Settings
    document.getElementById('checkInTime').value = currentSettings.checkInTime || 14;
    document.getElementById('checkOutTime').value = currentSettings.checkOutTime || 12;
    document.getElementById('bookingExpiryHours').value = currentSettings.bookingExpiryHours || 24;
    document.getElementById('depositPercentage').value = (currentSettings.depositPercentage * 100) || 30;

    // Pricing Settings
    document.getElementById('currency').value = currentSettings.currency || 'VNĐ';
    document.getElementById('vipDiscount').value = (currentSettings.vipDiscount * 100) || 10;

    // Display Settings
    document.getElementById('theme').value = currentSettings.theme || 'default';
    document.getElementById('language').value = currentSettings.language || 'vi';
    document.getElementById('autoRefreshDashboard').value = currentSettings.autoRefreshDashboard || 30;
    document.getElementById('showWelcomeScreen').checked = currentSettings.showWelcomeScreen !== false;

    // Notification Settings
    document.getElementById('enableEmailNotifications').checked = currentSettings.enableEmailNotifications || false;
    document.getElementById('enableSMSNotifications').checked = currentSettings.enableSMSNotifications || false;
}

async function saveSettings() {
    const settingsData = {
        hotelName: document.getElementById('hotelName').value,
        hotelAddress: document.getElementById('hotelAddress').value,
        hotelPhone: document.getElementById('hotelPhone').value,
        hotelEmail: document.getElementById('hotelEmail').value,
        checkInTime: parseInt(document.getElementById('checkInTime').value),
        checkOutTime: parseInt(document.getElementById('checkOutTime').value),
        bookingExpiryHours: parseInt(document.getElementById('bookingExpiryHours').value),
        depositPercentage: parseFloat(document.getElementById('depositPercentage').value) / 100,
        currency: document.getElementById('currency').value,
        vipDiscount: parseFloat(document.getElementById('vipDiscount').value) / 100,
        theme: document.getElementById('theme').value,
        language: document.getElementById('language').value,
        autoRefreshDashboard: parseInt(document.getElementById('autoRefreshDashboard').value),
        showWelcomeScreen: document.getElementById('showWelcomeScreen').checked,
        enableEmailNotifications: document.getElementById('enableEmailNotifications').checked,
        enableSMSNotifications: document.getElementById('enableSMSNotifications').checked
    };

    try {
        await apiCall('/settings', 'PUT', settingsData);
        showNotification('Đã lưu cài đặt thành công', 'success');
        
        // Apply theme if changed
        if (settingsData.theme !== currentSettings.theme) {
            applyTheme(settingsData.theme);
        }
        
        currentSettings = settingsData;
    } catch (error) {
        showNotification('Có lỗi khi lưu cài đặt: ' + error.message, 'error');
    }
}

async function resetSettings() {
    if (!confirm('Bạn có chắc muốn khôi phục tất cả cài đặt về mặc định?')) return;

    try {
        await apiCall('/settings/reset', 'POST');
        showNotification('Đã khôi phục cài đặt mặc định', 'success');
        await loadSettings();
        applyTheme('default');
    } catch (error) {
        showNotification('Có lỗi khi khôi phục cài đặt', 'error');
    }
}

function applyTheme(theme) {
    const root = document.documentElement;
    
    if (theme === 'dark') {
        root.style.setProperty('--primary-navy', '#0d1b2a');
        root.style.setProperty('--secondary-navy', '#1b263b');
        root.style.setProperty('--bg-light', '#1a1a1a');
        root.style.setProperty('--bg-white', '#2a2a2a');
        root.style.setProperty('--text-dark', '#e0e0e0');
        root.style.setProperty('--border-color', '#404040');
        showNotification('Đã chuyển sang giao diện tối', 'success');
    } else {
        root.style.setProperty('--primary-navy', '#1a2f4a');
        root.style.setProperty('--secondary-navy', '#2d4b6e');
        root.style.setProperty('--bg-light', '#f5f7fa');
        root.style.setProperty('--bg-white', '#ffffff');
        root.style.setProperty('--text-dark', '#2c3e50');
        root.style.setProperty('--border-color', '#e1e8ed');
        showNotification('Đã chuyển sang giao diện mặc định', 'success');
    }
    
    // Save theme preference to localStorage
    localStorage.setItem('theme', theme);
}

// Load saved theme on page load
function loadSavedTheme() {
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme) {
        applyTheme(savedTheme);
    }
}

// Initialize settings page
document.addEventListener('DOMContentLoaded', function() {
    if (window.location.pathname.includes('Settings')) {
        loadSettings();
    }
    
    // Load saved theme on all pages
    loadSavedTheme();
});

// Export settings as JSON
function exportSettings() {
    if (!currentSettings) return;
    
    const dataStr = JSON.stringify(currentSettings, null, 2);
    const dataUri = 'data:application/json;charset=utf-8,'+ encodeURIComponent(dataStr);
    
    const exportFileDefaultName = 'hotel-settings.json';
    
    const linkElement = document.createElement('a');
    linkElement.setAttribute('href', dataUri);
    linkElement.setAttribute('download', exportFileDefaultName);
    linkElement.click();
    
    showNotification('Đã xuất cài đặt', 'success');
}

// Import settings from JSON
function importSettings() {
    const input = document.createElement('input');
    input.type = 'file';
    input.accept = 'application/json';
    
    input.onchange = function(e) {
        const file = e.target.files[0];
        const reader = new FileReader();
        
        reader.onload = function(event) {
            try {
                const importedSettings = JSON.parse(event.target.result);
                currentSettings = importedSettings;
                populateSettingsForm();
                showNotification('Đã nhập cài đặt. Nhấn Lưu để áp dụng.', 'success');
            } catch (error) {
                showNotification('File cài đặt không hợp lệ', 'error');
            }
        };
        
        reader.readAsText(file);
    };
    
    input.click();
}
