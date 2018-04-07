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
     }),
     $(document).ready(function () {
         $("#dark-mode").hover(function () {
             $("#ThemeLink").attr("href", "/Content/PreviewTheme/PDarkMode.css")
         }, function () {
             $("#ThemeLink").attr("href", "")
         })
     }),
     $(document).ready(function () {
         $("#light-mode").hover(function () {
             $("#ThemeLink").attr("href", "/Content/PreviewTheme/PLightMode.css")
         }, function () {
             $("#ThemeLink").attr("href", "")
         })
     }), $(document).ready(function () {
         $("#future-mode").hover(function () {
             $("#ThemeLink").attr("href", "/Content/PreviewTheme/PFutureMode.css")
         }, function () {
             $("#ThemeLink").attr("href", "")
         })
     });