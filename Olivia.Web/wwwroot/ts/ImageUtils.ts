abstract class ImageUtils {
    static button: JQuery<HTMLButtonElement>;
    static input: JQuery<HTMLInputElement>;
    static imageContainer: JQuery<HTMLDivElement>;

    public static init() {
        ImageUtils.button = $('#PostImageButton');
        ImageUtils.input = $('#PostImageInput');
        ImageUtils.imageContainer = $('#PostImagePreviewer')

        ImageUtils.button.click(ImageUtils.invokeFileInput);
        ImageUtils.input.change(ImageUtils.updateSelectedImages)
    }

    static invokeFileInput(args: JQuery.ClickEvent) {
        ImageUtils.input.click();
    }

    static updateSelectedImages(args: JQuery.ChangeEvent) {
        let files = ImageUtils.input.get(0).files;

        ImageUtils.imageContainer.html('');
        for (const file of files) {
            let fileReader = new FileReader();
            fileReader.onload = ImageUtils.loadImageToContainer;
            fileReader.readAsDataURL(file);
        }
    }

    static loadImageToContainer(args: ProgressEvent) {
        let _this = <FileReader>args.target;
        let img = $('<img>').prop('src', _this.result);

        let div = $('<div>').addClass('PreviewImage col-3').append(img);
        ImageUtils.imageContainer.append(div);
    }
}

export { ImageUtils };