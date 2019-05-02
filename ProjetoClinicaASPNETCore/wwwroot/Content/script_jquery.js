$(function ($) {
    $('#confirmCadastroAnimal').on('show.bs.modal', function () {
        var nome = $("#animalNome").val();
        $('#animalNomeModal').text(nome);
    });
}(jQuery));

