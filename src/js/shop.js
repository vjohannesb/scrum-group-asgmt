$(function() {
    /* Slider start */
    $("#slider-range-price").slider({
        range: true,
        animate: "fast",
        min: 1,
        max: 1000,
        values: [1, 1000],
        slide: function(event, ui) {
            $("#priceRangeAmount").val("$" + ui.values[0] + " - $" + ui.values[1]);
        }
    });
    $("#priceRangeAmount").val("$" + $("#slider-range-price").slider("values", 0) +
        " - $" + $("#slider-range-price").slider("values", 1));

    $(".ui-slider-handle").each((idx, elem) => $(elem).addClass(`ui-slider-handle-${idx}`));
    /* Slider end */

    $(".filter-box-container").each((idx, elem) => $(elem).on("click", function() {
        $(elem).toggleClass("selected");
    }));

    $(".filter-brand-container").each((idx, elem) => $(elem).on("click", function() {
        $(elem).toggleClass("selected");
    }));

    let flex = "none";
    $("#btnToggleFilters").on("click", function() {
        flex = flex === "flex" ? "none" : "flex";
        $(".filter-column").toggle();
        $(".filter-toggle-flex").css("display", flex);

    });
});