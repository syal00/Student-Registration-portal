// Delete confirmation modal handler
document.addEventListener('DOMContentLoaded', function () {
    const deleteModal = document.getElementById('deleteConfirmModal');
    if (!deleteModal) return;

    deleteModal.addEventListener('show.bs.modal', function (event) {
        const button = event.relatedTarget;
        if (!button) return;

        const form = document.getElementById('deleteConfirmForm');
        const message = document.getElementById('deleteConfirmMessage');
        const hiddenFields = document.getElementById('deleteConfirmHiddenFields');
        const title = button.getAttribute('data-delete-title') || 'Confirm Delete';

        deleteModal.querySelector('.modal-title').innerHTML =
            '<i class="bi bi-exclamation-triangle-fill text-danger me-2"></i>' + title;
        message.textContent = button.getAttribute('data-delete-message') || 'Are you sure you want to delete this item?';
        form.action = button.getAttribute('data-delete-action') || '#';

        hiddenFields.innerHTML = '';
        const fieldsJson = button.getAttribute('data-delete-fields');
        if (fieldsJson) {
            try {
                const fields = JSON.parse(fieldsJson);
                Object.entries(fields).forEach(([name, value]) => {
                    const input = document.createElement('input');
                    input.type = 'hidden';
                    input.name = name;
                    input.value = value;
                    hiddenFields.appendChild(input);
                });
            } catch (e) {
                console.error('Invalid delete fields JSON', e);
            }
        }
    });
});
