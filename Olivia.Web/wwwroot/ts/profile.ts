import * as $ from 'jquery';

var input = $('#sendImage');
var imgIndex = 0;

function addImage() {
    input.click();
}

function closeFormImg(e: JQuery.ClickEvent) {
    $(e.target).parent().remove();
}

$(() => {
    input.change(() => {
        var container = $('#imgCarousel');
        var files = (<HTMLInputElement>input.get(0)).files;
        var reader = new FileReader();

        reader.onload = function () {
            var div = $('<div>');
            div.addClass('formImg col-3')

            var close = $('<p>X</p>');
            close.click((e) => {
                closeFormImg(e);
            });

            var img = $('<img>');
            img.attr('id', `form-img-${imgIndex++}`);
            img.attr('src', reader.result.toString());

            div.append(img);
            div.append(close);
            container.append(div);
        };

        container.html('');
        $.map(files, (file: File) => {
            reader.readAsDataURL(file);
        });
    });
});