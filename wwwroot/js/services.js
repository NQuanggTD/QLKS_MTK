// Services management
let allServices = [];
let filteredServices = [];

async function loadServices() {
    try {
        allServices = await apiCall('/services');
        filteredServices = [...allServices];
        displayServices();
    } catch (error) {
        console.error('Error loading services:', error);
        showNotification('Không thể tải danh sách dịch vụ', 'error');
    }
}

function displayServices() {
    const grid = document.getElementById('servicesGrid');
    if (!grid) return;

    if (filteredServices.length === 0) {
        grid.innerHTML = '<div class="loading-spinner"><p>Không có dịch vụ nào</p></div>';
        return;
    }

    grid.innerHTML = filteredServices.map(service => `
        <div class="service-card">
            <div class="service-header">
                <div class="service-icon">
                    <i class="fas ${getServiceCategoryIcon(service.category)}"></i>
                </div>
                <div class="service-title">
                    <h3>${service.name}</h3>
                    <p class="service-category">${getServiceCategoryText(service.category)}</p>
                </div>
            </div>
            ${service.isPopular ? '<span class="status-badge status-available" style="margin-bottom: 10px;"><i class="fas fa-star"></i> Phổ biến</span>' : ''}
            <p class="service-description">${service.description || 'Không có mô tả'}</p>
            <div style="display: flex; gap: 10px; margin-bottom: 15px; font-size: 13px; color: var(--text-gray);">
                <span><i class="fas fa-clock"></i> ${service.estimatedDuration} phút</span>
                <span><i class="fas fa-box"></i> ${service.unit}</span>
            </div>
            <div class="service-footer">
                <div class="service-price">${formatCurrency(service.price)}</div>
                <div class="service-actions">
                    <button class="action-btn action-btn-edit" onclick="editService('${service.id}')" title="Sửa">
                        <i class="fas fa-edit"></i>
                    </button>
                    <button class="action-btn action-btn-delete" onclick="deleteService('${service.id}')" title="Xóa">
                        <i class="fas fa-trash"></i>
                    </button>
                </div>
            </div>
        </div>
    `).join('');
}

function filterServices() {
    const categoryFilter = document.getElementById('categoryFilter').value;
    const statusFilter = document.getElementById('statusFilter').value;

    filteredServices = allServices.filter(service => {
        if (categoryFilter && service.category.toString() !== categoryFilter) return false;
        if (statusFilter && service.status.toString() !== statusFilter) return false;
        return true;
    });

    displayServices();
}

function openAddServiceModal() {
    document.getElementById('serviceModalTitle').textContent = 'Thêm dịch vụ mới';
    document.getElementById('serviceForm').reset();
    document.getElementById('serviceId').value = '';
    document.getElementById('serviceModal').classList.add('show');
}

function editService(serviceId) {
    const service = allServices.find(s => s.id === serviceId);
    if (!service) return;

    document.getElementById('serviceModalTitle').textContent = 'Chỉnh sửa dịch vụ';
    document.getElementById('serviceId').value = service.id;
    document.getElementById('serviceName').value = service.name;
    document.getElementById('serviceCategory').value = service.category;
    document.getElementById('serviceStatus').value = service.status;
    document.getElementById('servicePrice').value = service.price;
    document.getElementById('serviceUnit').value = service.unit || '';
    document.getElementById('estimatedDuration').value = service.estimatedDuration || '';
    document.getElementById('serviceDescription').value = service.description || '';
    document.getElementById('isPopular').checked = service.isPopular;

    document.getElementById('serviceModal').classList.add('show');
}

async function deleteService(serviceId) {
    if (!confirm('Bạn có chắc chắn muốn xóa dịch vụ này?')) return;

    try {
        await apiCall(`/services/${serviceId}`, 'DELETE');
        showNotification('Xóa dịch vụ thành công', 'success');
        loadServices();
    } catch (error) {
        showNotification(error.message, 'error');
    }
}

function closeServiceModal() {
    document.getElementById('serviceModal').classList.remove('show');
}

// Form submission
document.addEventListener('DOMContentLoaded', function() {
    if (window.location.pathname.includes('Services')) {
        loadServices();

        const serviceForm = document.getElementById('serviceForm');
        if (serviceForm) {
            serviceForm.addEventListener('submit', async function(e) {
                e.preventDefault();

                const serviceId = document.getElementById('serviceId').value;
                const serviceData = {
                    name: document.getElementById('serviceName').value,
                    category: parseInt(document.getElementById('serviceCategory').value),
                    status: parseInt(document.getElementById('serviceStatus').value),
                    price: parseFloat(document.getElementById('servicePrice').value),
                    unit: document.getElementById('serviceUnit').value,
                    estimatedDuration: parseInt(document.getElementById('estimatedDuration').value) || 0,
                    description: document.getElementById('serviceDescription').value,
                    isPopular: document.getElementById('isPopular').checked,
                    imageUrl: '/images/services/default.jpg'
                };

                try {
                    if (serviceId) {
                        await apiCall(`/services/${serviceId}`, 'PUT', serviceData);
                        showNotification('Cập nhật dịch vụ thành công', 'success');
                    } else {
                        await apiCall('/services', 'POST', serviceData);
                        showNotification('Thêm dịch vụ mới thành công', 'success');
                    }
                    
                    closeServiceModal();
                    loadServices();
                } catch (error) {
                    showNotification(error.message, 'error');
                }
            });
        }

        // Close modal when clicking outside
        document.getElementById('serviceModal')?.addEventListener('click', function(e) {
            if (e.target === this) {
                closeServiceModal();
            }
        });
    }
});
