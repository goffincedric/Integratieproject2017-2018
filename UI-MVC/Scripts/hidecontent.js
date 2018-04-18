$(".arrow-password-hide").click(function () {
    $(".cpwform").hide(500);
    $(".arrow-password-hide").toggleClass('arrow-password-hide arrow-password-show');
}),
$(".arrow-password-show").click(function () {
    $(".cpwform").show(500);
    $(".arrow-password-show").toggleClass('arrow-password-show arrow-password-hide');
});