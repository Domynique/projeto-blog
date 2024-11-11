$(document).ready(function () {

    // Função para submissão de edição do post
    $('#edit-post-form').on('submit', 'form', function (event) {

        event.preventDefault();

        $.ajax({
            url: $(this).attr('action'),
            type: $(this).attr('method'),
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    // Exibe mensagem de sucesso
                    $('#messagesPost').html('<div class="alert alert-success"><i class="fas fa-exclamation-triangle"></i> ' + result.mensagem + '</div>');
                } else {

                    $('#edit-post-form').html(result.html);
                    $('#comentarios').html(result.comentariosHtml);

                    // Exibe mensagens de erro
                    var errorMessages = result.mensagens;
                    var errorList = '<ul>';
                    errorMessages.forEach(function (message) {
                        errorList += '<li>' + message + '</li>';
                    });
                    errorList += '</ul>';
                    $('#messagesPost').html('<div class="alert alert-danger"><i class="fas fa-exclamation-triangle"></i> ' + errorList + '</div>');
                }
            },
            error: function (xhr, status, error) {
                console.error("Erro na requisição AJAX: ", status, error);
            }

        });
    });


    if ($('#comentario-form-edit').length) {

        $('#comentario-form-edit').on('submit', function (event) {

            event.preventDefault();

            if ($(this).valid()) {

                const formData = $(this).serialize();
                const postId = $(this).find('input[name="PostId"]').val();
                const comentarioId = $(this).find('input[name="ComentarioId"]').val();

                // Adiciona feedback visual (spinner)
                $('#loadingSpinner').show();

                $.post(`/posts/${postId}/comentarios/editar/${comentarioId}`, formData)
                    .done(function (response) {
                        $('#loadingSpinner').hide();
                        const tempDiv = $('<div>').html(response);
                        if (tempDiv.find('#comentario-lista').length) {
                            $('#comentario-form-edit').find('#Conteudo').val('');
                            $('#comentario-lista').html(response);
                            $('#messagesEditComentario').html('<div class="alert alert-success"><i class="fas fa-exclamation-triangle"></i> Comentário alterado com sucesso!</div>');
                        } else {
                            $('#comentario-form-edit').html(response);
                        }
                    }).fail(function (xhr, status, error) {
                        $('#loadingSpinner').hide();
                        handleError(xhr, status, error);
                        $('#messagesEditComentario').html('<div class="alert alert-danger"><i class="fas fa-exclamation-triangle"></i> Ocorreu um erro ao adicionar o comentário. Por favor, tente novamente.</div>');
                    });
            }

        });
    }

    const form = $('#comentario-form');

    if (form.length) {
        form.on('submit', function (event) {

            event.preventDefault();

            if ($(this).valid()) {

                const formData = form.serialize();
                const postId = form.find('input[name="PostId"]').val();

                // Adiciona feedback visual (spinner)
                $('#loadingSpinner').show();

                $.post(`/posts/${postId}/comentarios/adicionar`, formData)
                    .done(function (response) {
                        $('#loadingSpinner').hide();
                        const tempDiv = $('<div>').html(response);
                        if (tempDiv.find('#comentario-lista').length) {
                            form.find('#Conteudo').val('');
                            $('#comentario-lista').html(response);
                            $('#messagesComentario').html('<div class="alert alert-success"><i class="fas fa-exclamation-triangle"></i> Comentário adicionado com sucesso!</div>');
                        } else {
                            form.html(response);
                        }
                    }).fail(function (xhr, status, error) {
                        $('#loadingSpinner').hide();
                        handleError(xhr, status, error);
                        $('#messagesComentario').html('<div class="alert alert-danger"><i class="fas fa-exclamation-triangle"></i> Ocorreu um erro ao adicionar o comentário. Por favor, tente novamente.</div>');
                    });
            }

        });
    }

    function handleError(xhr, status, error) {
        console.error("Erro na requisição AJAX: ", status, error);
    }



    
});
