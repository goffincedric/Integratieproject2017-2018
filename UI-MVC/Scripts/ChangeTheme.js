﻿$(document).ready(function () {
    $("#dark-mode").click(function () {
        $("#ThemeLink").attr("href", "/Content/Theme/DarkMode.css")
    })
}),
    $(document).ready(function () {
        $("#light-mode").click(function () {
            $("#ThemeLink").attr("href", "/Content/Theme/LightMode.css")
        })
    }), $(document).ready(function () {
        $("#future-mode").click(function () {
            $("#ThemeLink").attr("href", "/Content/Theme/FutureMode.css")
        })
    });