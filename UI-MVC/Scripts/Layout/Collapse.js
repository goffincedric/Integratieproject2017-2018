$(document).ready(function () {
    if ($(document).width() < 1440) {
        $(".app").removeClass("is-collapsed");
    }
});

$(window).resize(function () {
    if ($(document).width() < 1436) {
        $(".app").removeClass("is-collapsed");
    } else {
        $(".app").addClass("is-collapsed");
    }
});