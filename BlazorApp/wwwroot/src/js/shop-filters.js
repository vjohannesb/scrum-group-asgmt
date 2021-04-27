
export function loadFilterUI() {
    /* Slider */
    $("#slider-range-price").slider({
        range: true,
        animate: "fast",
        min: 1,
        max: 1000,
        values: [1, 1000],
        slide: function (event, ui) {
            $("#priceRangeAmount").val("$" + ui.values[0] + " - $" + ui.values[1]);
        }
    });
    $("#priceRangeAmount").val("$" + $("#slider-range-price").slider("values", 0) +
        " - $" + $("#slider-range-price").slider("values", 1));

    $(".ui-slider-handle").each((idx, elem) => $(elem).addClass(`ui-slider-handle-${idx}`));

    $(".filter-box-container, .filter-brand-container").each((idx, elem) => {
        $(elem).click((e) => $(e.currentTarget).toggleClass("selected"));
    });

    $(".shop-product-n-link").each((idx, elem) => {
        $(elem).click((e) => {
            $(".shop-product-n-link.selected").removeClass("selected");
            $(e.currentTarget).addClass("selected");
        });
    });

    $(".list-box-large-box i").each((idx, elem) => {
        $(elem).click((e) => {
            $(".list-box-large-box i.selected").removeClass("selected");
            $(e.currentTarget).addClass("selected");
        });
    });
}