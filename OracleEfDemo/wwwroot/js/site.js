$(document).on('click', '.js-orders', function () {
    const customerId = $(this).data('id');

    $('#ordersModalBody').html('<div class="text-center py-4">Yükleniyor...</div>');

    const modal = bootstrap.Modal.getOrCreateInstance(document.getElementById('ordersModal'));
    modal.show();

    $.ajax({
        url: '/Order/CustomerOrders',
        type: 'GET',
        data: { customerId: customerId },
        success: function (html) {
            $('#ordersModalBody').html(html);
        },
        error: function (xhr) {
            console.error(xhr);
            $('#ordersModalBody').html('<div class="alert alert-danger mb-0">Siparişler yüklenemedi.</div>');
        }
    });
});