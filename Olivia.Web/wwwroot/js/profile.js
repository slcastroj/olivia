var input = $('#sendImage');

$(() => {
    input.change((changeArgs) => {
        var container = $('#imgCarousel');
        var file = input[0].files[0];
        var reader = new FileReader();

        reader.onload = function (loadArgs) {
            var div = $('<div>');
            div.addClass('formImg col-3')
            var img = $('<img>');
            img.attr('src', loadArgs.target.result);

            div.append(img);
            container.append(div);
        };

        reader.readAsDataURL(file);
    });
});

function addImage() {
    input.click();
}