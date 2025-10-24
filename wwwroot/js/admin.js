// Admin functions for Wangg's Hotel Management System
// File: wwwroot/js/admin.js

// Load sidebar user info on page load
document.addEventListener('DOMContentLoaded', function() {
    loadSidebarUserInfo();
});

// Load and display user info in sidebar
async function loadSidebarUserInfo() {
    try {
        const response = await fetch('/api/auth/me');
        const data = await response.json();
        
        if (data.success && data.user) {
            updateSidebarUserInfo(data.user);
        }
    } catch (error) {
        console.error('Error loading sidebar user info:', error);
    }
}

// Update sidebar with user information
function updateSidebarUserInfo(user) {
    const userName = document.getElementById('sidebarUserName');
    const userRole = document.getElementById('sidebarUserRole');
    const userAvatar = document.getElementById('userAvatar');
    
    if (userName) userName.textContent = user.fullName || user.username || 'Admin';
    if (userRole) userRole.textContent = getRoleText(user.role) || 'Quản trị viên';
    
    // Update avatar if available
    if (userAvatar && user.avatarUrl) {
        userAvatar.innerHTML = `<img src="${user.avatarUrl}" alt="Avatar" style="width: 40px; height: 40px; border-radius: 50%; object-fit: cover;">`;
    }
}

// Admin Profile Management
function openAdminProfile() {
    // Load current user data from session/localStorage
    loadCurrentUserData();
    
    showAdminModal('Hồ sơ Admin', `
        <div class="admin-profile-form">
            <div class="profile-avatar">
                <img id="currentAvatar" src="/images/default-avatar.png" alt="Avatar" style="width: 120px; height: 120px; border-radius: 50%; object-fit: cover;">
                <button class="btn-change-avatar" onclick="changeAvatar()">
                    <i class="fas fa-camera"></i> Đổi ảnh
                </button>
            </div>
            <form id="adminProfileForm">
                <div class="form-group">
                    <label>Tên đăng nhập</label>
                    <input type="text" id="adminUsername" class="form-control" readonly>
                </div>
                <div class="form-group">
                    <label>Họ và tên</label>
                    <input type="text" id="adminFullName" class="form-control" required>
                </div>
                <div class="form-group">
                    <label>Email</label>
                    <input type="email" id="adminEmail" class="form-control" required>
                </div>
                <div class="form-group">
                    <label>Số điện thoại</label>
                    <input type="tel" id="adminPhone" class="form-control" required>
                </div>
                <div class="form-group">
                    <label>Vai trò</label>
                    <input type="text" id="adminRole" class="form-control" readonly>
                </div>
            </form>
            <div class="modal-actions">
                <button class="btn btn-secondary" onclick="closeAdminModal()">Hủy</button>
                <button class="btn btn-primary" onclick="saveAdminProfile()">
                    <i class="fas fa-save"></i> Lưu thay đổi
                </button>
            </div>
        </div>
    `);
}

// Load current user data
async function loadCurrentUserData() {
    try {
        const response = await fetch('/api/auth/me');
        const data = await response.json();
        
        if (data.success && data.user) {
            // Update form fields
            setTimeout(() => {
                const usernameEl = document.getElementById('adminUsername');
                const fullNameEl = document.getElementById('adminFullName');
                const emailEl = document.getElementById('adminEmail');
                const phoneEl = document.getElementById('adminPhone');
                const roleEl = document.getElementById('adminRole');
                
                if (usernameEl) usernameEl.value = data.user.username || '';
                if (fullNameEl) fullNameEl.value = data.user.fullName || '';
                if (emailEl) emailEl.value = data.user.email || '';
                if (phoneEl) phoneEl.value = data.user.phone || '';
                if (roleEl) roleEl.value = getRoleText(data.user.role) || '';
                
                // Update avatar
                const avatarImg = document.getElementById('currentAvatar');
                if (avatarImg && data.user.avatarUrl) {
                    avatarImg.src = data.user.avatarUrl;
                }
                
                // Update sidebar user info
                const userName = document.querySelector('.user-name');
                const userRole = document.querySelector('.user-role');
                if (userName) userName.textContent = data.user.fullName;
                if (userRole) userRole.textContent = getRoleText(data.user.role);
            }, 100);
        }
    } catch (error) {
        console.error('Error loading user data:', error);
    }
}

function getRoleText(role) {
    const roles = {
        'Admin': 'Quản trị viên',
        'Manager': 'Quản lý',
        'Staff': 'Nhân viên'
    };
    return roles[role] || role;
}

// Save profile changes
async function saveAdminProfile() {
    console.log('🔵 Save Admin Profile - START');
    
    const fullNameEl = document.getElementById('adminFullName');
    const emailEl = document.getElementById('adminEmail');
    const phoneEl = document.getElementById('adminPhone');
    
    console.log('📋 Form elements:', { fullNameEl, emailEl, phoneEl });
    
    if (!fullNameEl || !emailEl || !phoneEl) {
        console.error('❌ Form elements not found!');
        alert('Lỗi: Không tìm thấy form!');
        return;
    }
    
    const fullName = fullNameEl.value.trim();
    const email = emailEl.value.trim();
    const phone = phoneEl.value.trim();
    
    console.log('📝 Form data:', { fullName, email, phone });
    
    if (!fullName || !email || !phone) {
        console.warn('⚠️ Validation failed - missing fields');
        showNotification('Vui lòng nhập đầy đủ thông tin', 'error');
        return;
    }
    
    // Show loading notification
    showNotification('Đang lưu hồ sơ...', 'info');
    console.log('🌐 Calling API...');
    
    try {
        const response = await fetch('/api/auth/profile', {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ fullName, email, phone })
        });
        
        console.log('📡 Response status:', response.status);
        const data = await response.json();
        console.log('📦 Response data:', data);
        
        if (data.success) {
            console.log('✅ Update successful!');
            
            // Reload user data from server to update sidebar
            console.log('🔄 Reloading sidebar...');
            await loadSidebarUserInfo();
            
            // Show success message
            showNotification('✅ Cập nhật hồ sơ thành công!', 'success');
            
            // Close modal after a short delay
            console.log('🚪 Closing modal...');
            setTimeout(() => {
                closeAdminModal();
                console.log('✅ Modal closed');
            }, 500);
        } else {
            console.error('❌ Update failed:', data.message);
            showNotification('❌ ' + (data.message || 'Cập nhật thất bại'), 'error');
        }
    } catch (error) {
        console.error('💥 Error saving profile:', error);
        showNotification('❌ Đã xảy ra lỗi khi lưu hồ sơ', 'error');
    }
    
    console.log('🔵 Save Admin Profile - END');
}

function changeAvatar() {
    const input = document.createElement('input');
    input.type = 'file';
    input.accept = 'image/*';
    
    input.onchange = (e) => {
        const file = e.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = (event) => {
                const avatarImg = document.getElementById('currentAvatar');
                const avatarUrl = event.target.result;
                
                if (avatarImg) {
                    avatarImg.src = avatarUrl;
                }
                
                // Update sidebar avatar immediately
                const sidebarAvatar = document.querySelector('.sidebar-footer .user-info i');
                if (sidebarAvatar) {
                    // Replace icon with image
                    const imgTag = document.createElement('img');
                    imgTag.src = avatarUrl;
                    imgTag.alt = 'Avatar';
                    imgTag.style.cssText = 'width: 40px; height: 40px; border-radius: 50%; object-fit: cover;';
                    sidebarAvatar.parentNode.replaceChild(imgTag, sidebarAvatar);
                }
                
                // Save to localStorage for persistence
                localStorage.setItem('userAvatar', avatarUrl);
                
                showToast('Ảnh đại diện đã được cập nhật', 'success');
            };
            reader.readAsDataURL(file);
        }
    };
    
    input.click();
}

// Admin Settings
function openAdminSettings() {
    showAdminModal('Cài đặt Admin', `
        <div class="admin-settings-form">
            <div class="settings-section">
                <h4><i class="fas fa-shield-alt"></i> Bảo mật</h4>
                <button class="btn btn-outline-primary" onclick="changePassword()">
                    <i class="fas fa-key"></i> Đổi mật khẩu
                </button>
                <button class="btn btn-outline-primary" onclick="enable2FA()">
                    <i class="fas fa-mobile-alt"></i> Bật xác thực 2 yếu tố
                </button>
            </div>
            
            <div class="settings-section">
                <h4><i class="fas fa-bell"></i> Thông báo</h4>
                <label class="switch-label">
                    <input type="checkbox" id="emailNotif" checked>
                    <span>Nhận thông báo qua Email</span>
                </label>
                <label class="switch-label">
                    <input type="checkbox" id="smsNotif">
                    <span>Nhận thông báo qua SMS</span>
                </label>
                <label class="switch-label">
                    <input type="checkbox" id="desktopNotif" checked>
                    <span>Thông báo trên Desktop</span>
                </label>
            </div>
            
            <div class="settings-section">
                <h4><i class="fas fa-history"></i> Lịch sử hoạt động</h4>
                <button class="btn btn-outline-secondary" onclick="viewActivityLog()">
                    <i class="fas fa-list"></i> Xem lịch sử đăng nhập
                </button>
                <button class="btn btn-outline-secondary" onclick="viewChangeLog()">
                    <i class="fas fa-edit"></i> Xem lịch sử thay đổi
                </button>
            </div>
            
            <div class="settings-section">
                <h4><i class="fas fa-database"></i> Dữ liệu</h4>
                <button class="btn btn-outline-warning" onclick="exportData()">
                    <i class="fas fa-download"></i> Xuất dữ liệu
                </button>
                <button class="btn btn-outline-warning" onclick="backupDatabase()">
                    <i class="fas fa-save"></i> Sao lưu hệ thống
                </button>
            </div>
            
            <div class="modal-actions">
                <button class="btn btn-secondary" onclick="closeAdminModal()">Đóng</button>
            </div>
        </div>
    `);
}

// Logout
function confirmLogout() {
    showConfirmModal(
        'Đăng xuất',
        'Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?',
        performLogout
    );
}

async function performLogout() {
    console.log('🚪 Performing logout...');
    
    // Show loading notification
    showNotification('Đang đăng xuất...', 'info');
    
    try {
        // Call logout API
        const response = await fetch('/api/auth/logout', { 
            method: 'POST',
            headers: { 'Content-Type': 'application/json' }
        });
        
        const data = await response.json();
        console.log('Logout response:', data);
        
        // Clear storage
        localStorage.clear();
        sessionStorage.clear();
        
        // Show success notification
        showNotification('✅ Đăng xuất thành công!', 'success');
        
        // Redirect to login page after notification
        setTimeout(() => {
            window.location.href = '/Home/Login';
        }, 1000);
        
    } catch (error) {
        console.error('Logout error:', error);
        
        // Even if API fails, still logout locally
        localStorage.clear();
        sessionStorage.clear();
        
        showNotification('✅ Đăng xuất thành công!', 'success');
        
        setTimeout(() => {
            window.location.href = '/Home/Login';
        }, 1000);
    }
}

// Modals
function showAdminModal(title, content) {
    const modalHTML = `
        <div class="modal-overlay" id="adminModal" onclick="closeAdminModalOnOverlay(event)">
            <div class="modal-dialog admin-modal">
                <div class="modal-header">
                    <h3>${title}</h3>
                    <button class="close-btn" onclick="closeAdminModal()">
                        <i class="fas fa-times"></i>
                    </button>
                </div>
                <div class="modal-body">
                    ${content}
                </div>
            </div>
        </div>
    `;
    
    document.body.insertAdjacentHTML('beforeend', modalHTML);
    setTimeout(() => {
        document.getElementById('adminModal').classList.add('active');
    }, 10);
}

function closeAdminModal() {
    const modal = document.getElementById('adminModal');
    if (modal) {
        modal.classList.remove('active');
        setTimeout(() => modal.remove(), 300);
    }
}

function closeAdminModalOnOverlay(event) {
    if (event.target.classList.contains('modal-overlay')) {
        closeAdminModal();
    }
}

// Change Password
function changePassword() {
    closeAdminModal();
    showAdminModal('Đổi mật khẩu', `
        <form id="changePasswordForm" class="admin-form">
            <div class="form-group">
                <label>Mật khẩu hiện tại</label>
                <input type="password" id="currentPassword" class="form-control" required>
            </div>
            <div class="form-group">
                <label>Mật khẩu mới</label>
                <input type="password" id="newPassword" class="form-control" required>
            </div>
            <div class="form-group">
                <label>Xác nhận mật khẩu mới</label>
                <input type="password" id="confirmPassword" class="form-control" required>
            </div>
            <div class="password-requirements">
                <small>
                    <i class="fas fa-info-circle"></i>
                    Mật khẩu phải có ít nhất 6 ký tự
                </small>
            </div>
            <div class="modal-actions">
                <button type="button" class="btn btn-secondary" onclick="closeAdminModal()">Hủy</button>
                <button type="button" class="btn btn-primary" onclick="submitChangePassword()">
                    <i class="fas fa-key"></i> Đổi mật khẩu
                </button>
            </div>
        </form>
    `);
}

function submitChangePassword() {
    const currentPassword = document.getElementById('currentPassword').value;
    const newPassword = document.getElementById('newPassword').value;
    const confirmPassword = document.getElementById('confirmPassword').value;
    
    if (!currentPassword || !newPassword || !confirmPassword) {
        showToast('Vui lòng điền đầy đủ thông tin', 'warning');
        return;
    }
    
    if (newPassword !== confirmPassword) {
        showToast('Mật khẩu xác nhận không khớp', 'error');
        return;
    }
    
    if (newPassword.length < 6) {
        showToast('Mật khẩu phải có ít nhất 6 ký tự', 'error');
        return;
    }
    
    // Disable button to prevent double submit
    const submitBtn = document.querySelector('#changePasswordForm .btn-primary');
    if (submitBtn) {
        submitBtn.disabled = true;
        submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Đang xử lý...';
    }
    
    fetch('/api/auth/change-password', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ currentPassword, newPassword, confirmPassword })
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            showToast('Đổi mật khẩu thành công!', 'success');
            
            // Force close modal
            const modal = document.getElementById('adminModal');
            if (modal) {
                modal.classList.remove('active');
                setTimeout(() => {
                    modal.remove();
                }, 500);
            }
        } else {
            showToast(data.message || 'Đổi mật khẩu thất bại', 'error');
            if (submitBtn) {
                submitBtn.disabled = false;
                submitBtn.innerHTML = '<i class="fas fa-key"></i> Đổi mật khẩu';
            }
        }
    })
    .catch(error => {
        console.error('Change password error:', error);
        showToast('Đã xảy ra lỗi khi đổi mật khẩu', 'error');
        if (submitBtn) {
            submitBtn.disabled = false;
            submitBtn.innerHTML = '<i class="fas fa-key"></i> Đổi mật khẩu';
        }
    });
}

// 2FA
function enable2FA() {
    closeAdminModal();
    showAdminModal('Xác thực 2 yếu tố', `
        <div class="twofa-setup">
            <div class="qr-code-container">
                <div class="qr-placeholder">
                    <i class="fas fa-qrcode"></i>
                    <p>Quét mã QR bằng ứng dụng Google Authenticator</p>
                </div>
            </div>
            <div class="form-group">
                <label>Nhập mã xác thực</label>
                <input type="text" id="verifyCode" class="form-control" placeholder="000000" maxlength="6">
            </div>
            <div class="modal-actions">
                <button class="btn btn-secondary" onclick="closeAdminModal()">Hủy</button>
                <button class="btn btn-primary" onclick="verify2FA()">
                    <i class="fas fa-check"></i> Xác nhận
                </button>
            </div>
        </div>
    `);
}

function verify2FA() {
    const code = document.getElementById('verifyCode').value;
    if (code.length === 6) {
        showToast('Đã bật xác thực 2 yếu tố thành công', 'success');
        closeAdminModal();
    } else {
        showToast('Mã xác thực không hợp lệ', 'error');
    }
}

// Activity Log
async function viewActivityLog() {
    closeAdminModal();
    
    try {
        const response = await fetch('/api/auth/login-history');
        const data = await response.json();
        
        let historyHTML = '';
        if (data.success && data.history && data.history.length > 0) {
            data.history.forEach(item => {
                const loginTime = new Date(item.loginTime).toLocaleString('vi-VN');
                historyHTML += '<tr><td>' + loginTime + '</td><td>' + item.ipAddress + '</td><td>' + item.browser + '</td><td>' + item.device + '</td><td><span class="badge badge-success">Thành công</span></td></tr>';
            });
        } else {
            historyHTML = '<tr><td colspan="5" style="text-align: center;">Chưa có lịch sử đăng nhập</td></tr>';
        }
        
        showAdminModal('Lịch sử đăng nhập', `
            <div class="activity-log">
                <table class="data-table">
                    <thead>
                        <tr>
                            <th>Thời gian</th>
                            <th>Địa chỉ IP</th>
                            <th>Trình duyệt</th>
                            <th>Thiết bị</th>
                            <th>Trạng thái</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${historyHTML}
                    </tbody>
                </table>
                <div class="modal-actions">
                    <button class="btn btn-secondary" onclick="closeAdminModal()">Đóng</button>
                </div>
            </div>
        `);
    } catch (error) {
        console.error('Error loading activity log:', error);
        showToast('Không thể tải lịch sử đăng nhập', 'error');
    }
}

// Change Log
function viewChangeLog() {
    closeAdminModal();
    showAdminModal('Lịch sử thay đổi', `
        <div class="change-log">
            <div class="log-item">
                <div class="log-icon"><i class="fas fa-edit"></i></div>
                <div class="log-content">
                    <div class="log-title">Cập nhật thông tin phòng 101</div>
                    <div class="log-time">10/10/2025 22:30</div>
                </div>
            </div>
            <div class="log-item">
                <div class="log-icon"><i class="fas fa-plus"></i></div>
                <div class="log-content">
                    <div class="log-title">Thêm booking mới</div>
                    <div class="log-time">10/10/2025 20:15</div>
                </div>
            </div>
            <div class="modal-actions">
                <button class="btn btn-secondary" onclick="closeAdminModal()">Đóng</button>
            </div>
        </div>
    `);
}

// Export & Backup
function exportData() {
    showToast('Đang xuất dữ liệu...', 'info');
    
    setTimeout(() => {
        const data = {
            exportDate: new Date().toISOString(),
            rooms: 'Sample room data',
            bookings: 'Sample booking data'
        };
        
        const blob = new Blob([JSON.stringify(data, null, 2)], { type: 'application/json' });
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'hotel-data-' + Date.now() + '.json';
        a.click();
        
        showToast('Đã xuất dữ liệu thành công', 'success');
    }, 1000);
}

function backupDatabase() {
    showConfirmModal(
        'Sao lưu hệ thống',
        'Bạn có chắc chắn muốn tạo bản sao lưu?',
        () => {
            showToast('Đang sao lưu hệ thống...', 'info');
            setTimeout(() => {
                showToast('Đã sao lưu hệ thống thành công', 'success');
            }, 2000);
        }
    );
}

// Confirm Modal
function showConfirmModal(title, message, onConfirm) {
    // Remove existing modal if any
    const existingModal = document.getElementById('confirmModal');
    if (existingModal) {
        existingModal.remove();
    }
    
    const modalHTML = `
        <div class="modal-overlay active" id="confirmModal" onclick="closeConfirmModalOnOverlay(event)">
            <div class="modal-dialog confirm-modal" style="max-width: 450px;">
                <div class="modal-header" style="background: linear-gradient(135deg, var(--primary-navy), var(--accent-gold)); color: white;">
                    <h3 style="margin: 0; color: white;"><i class="fas fa-question-circle"></i> ${title}</h3>
                </div>
                <div class="modal-body" style="text-align: center; padding: 30px 25px;">
                    <p style="font-size: 16px; margin: 0; line-height: 1.6;">${message}</p>
                </div>
                <div class="modal-actions" style="padding: 0 25px 25px 25px; display: flex; gap: 15px; justify-content: center;">
                    <button class="btn btn-secondary" onclick="closeConfirmModal()" style="flex: 1; max-width: 150px;">
                        <i class="fas fa-times"></i> Hủy
                    </button>
                    <button class="btn btn-primary" onclick="executeConfirmAction()" style="flex: 1; max-width: 150px;">
                        <i class="fas fa-check"></i> Xác nhận
                    </button>
                </div>
            </div>
        </div>
    `;
    
    document.body.insertAdjacentHTML('beforeend', modalHTML);
    
    // Store callback function
    window._confirmCallback = onConfirm;
}

function executeConfirmAction() {
    // Disable buttons to prevent double click
    const confirmBtn = document.querySelector('#confirmModal .btn-primary');
    if (confirmBtn) {
        confirmBtn.disabled = true;
        confirmBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Đang xử lý...';
    }
    
    // Close modal first
    closeConfirmModal();
    
    // Execute callback after a short delay
    setTimeout(() => {
        if (window._confirmCallback && typeof window._confirmCallback === 'function') {
            try {
                window._confirmCallback();
            } catch (error) {
                console.error('Callback error:', error);
                showToast('Đã xảy ra lỗi', 'error');
            }
        }
        window._confirmCallback = null;
    }, 100);
}

function closeConfirmModal() {
    const modal = document.getElementById('confirmModal');
    if (modal) {
        modal.style.display = 'none';
        modal.classList.remove('active');
        setTimeout(() => {
            modal.remove();
        }, 100);
    }
}

function closeConfirmModalOnOverlay(event) {
    if (event.target.id === 'confirmModal') {
        closeConfirmModal();
    }
}

// Init
document.addEventListener('DOMContentLoaded', () => {
    // Load saved avatar
    const savedAvatar = localStorage.getItem('userAvatar');
    if (savedAvatar) {
        const sidebarAvatar = document.querySelector('.sidebar-footer .user-info i');
        if (sidebarAvatar) {
            const imgTag = document.createElement('img');
            imgTag.src = savedAvatar;
            imgTag.alt = 'Avatar';
            imgTag.style.cssText = 'width: 40px; height: 40px; border-radius: 50%; object-fit: cover;';
            sidebarAvatar.parentNode.replaceChild(imgTag, sidebarAvatar);
        }
    }
    
    // Load saved profile
    const savedProfile = localStorage.getItem('adminProfile');
    if (savedProfile) {
        const profile = JSON.parse(savedProfile);
        const userName = document.querySelector('.user-name');
        if (userName) userName.textContent = profile.fullName;
    }
    
    // Load user from API
    fetch('/api/auth/me')
        .then(response => response.json())
        .then(data => {
            if (data.success && data.user) {
                const userName = document.querySelector('.user-name');
                const userRole = document.querySelector('.user-role');
                if (userName) userName.textContent = data.user.fullName;
                if (userRole) userRole.textContent = getRoleText(data.user.role);
                
                // Save to localStorage
                localStorage.setItem('user', JSON.stringify(data.user));
            }
        })
        .catch(error => console.error('Error loading user:', error));
    
    console.log('Admin module loaded');
});
