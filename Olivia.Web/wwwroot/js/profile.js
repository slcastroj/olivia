function addImage() {
    var input = $('#sendImage');
    input.change(() => {
        var container = $('#imgCarousel');
        var file = input[0].files[0];
        var reader = new FileReader();

        reader.onloadend = function () {
            var div = $('<div>');
            div.addClass('formImg col-12 col-md-3')
            var img = $('<img>');
            img.attr('src', reader.result);

            div.append(img);
            container.append(div);
        }

        reader.readAsDataURL(file);
    });

    input.click();
}