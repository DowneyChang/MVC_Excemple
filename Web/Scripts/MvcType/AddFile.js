$(function () {
    $('#AddFile').click(function () {
        var newFileInput = $("<p>").append(
            $("<input>").attr('type', 'file').attr('name', 'file')
        );
        $('.uploadFile').append(newFileInput);
    });

    $('#DelFile').click(function () {
        if ($('.uploadFile input').length > 1) {
            $('.uploadFile input').last().remove();
        }
    });
});