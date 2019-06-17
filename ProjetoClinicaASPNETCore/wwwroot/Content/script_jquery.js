/* TODO: MODAL CONFIGURATION */

$(function ($) {
    $('#confirmCadastroAnimal').on('show.bs.modal', function () {
        var nome = $("#animalNome").val();
        $('#animalNomeModal').text(nome);
    });
}(jQuery));

/* TOASTR CONFIGURATION */

$(function () {
     var displayMessage = function (message, msgType) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "500",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
    toastr[msgType](message);
     };
 
     if ($('#success').val()) {
    displayMessage($('#success').val(), 'success');
     }
     if ($('#info').val()) {
    displayMessage($('#info').val(), 'info');
     }
    if ($('#warning').val()) {
    displayMessage($('#warning').val(), 'warning');
    }
    if ($('#error').val()) {
    displayMessage($('#error').val(), 'error');
    }
});

$('.multi-field-wrapper').each(function() {
    var $wrapper = $('.multi-fields', this);
    $(".add-field", $(this)).click(function(e) {
        $('.multi-field:first-child', $wrapper).clone(true).appendTo($wrapper).find('input').val('').focus();
    });
    $('.multi-field .remove-field', $wrapper).click(function() {
        if ($('.multi-field', $wrapper).length > 1)
            $(this).parent('.multi-field').remove();
    });
});