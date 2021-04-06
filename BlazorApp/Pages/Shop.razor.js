/* Saker att lägga till i C#/Blazor */

$(function () {
    $(".filter-box-container").each((idx, elem) => $(elem).on("click", function () {
        $(elem).toggleClass("selected");
    }));

    $(".filter-brand-container").each((idx, elem) => $(elem).on("click", function () {
        $(elem).toggleClass("selected");
    }));

    let flex = "none";
    $("#btnToggleFilters").on("click", function () {
        flex = flex === "flex" ? "none" : "flex";
        $(".filter-column").toggle();
        $(".filter-toggle-flex").css("display", flex);
        $("#btnToggleFilters").html(`${flex !== "flex" ? 'Show' : 'Hide'} filters`);
    });
});