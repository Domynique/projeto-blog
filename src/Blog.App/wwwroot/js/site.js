$(document).ready(function () {

    if (@User.Identity.IsAuthenticated.ToString().ToLower()) {
        $('#registerPartial').hide();
    }

    $('#showLogin').click(function () {
        $('#registerPartial').appendTo('#renderBody').hide();
        $('#loginForm').show();
    });

    $('#showRegister').click(function (e) {
        e.preventDefault();
        $('#registerPartial').appendTo('#renderBody').show();
        $('#loginForm').hide();
    });

    if (window.location.href.indexOf("Account/Login") > -1) {
        $('#registerPartial').appendTo('#renderBody').hide();
    }
});