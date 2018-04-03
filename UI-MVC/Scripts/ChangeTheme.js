﻿$(document).ready(function () {
  $("#dark-mode").hover(function () {
    $(".dropdown-menu").css("background-color", "#333333");
    $(".dropdown-menu").css("color", "white");
    $(".c-grey-900").css("color", "white !important");
  }, function () {
    $(".dropdown-menu").css("background-color", "");
    $(".dropdown-menu").css("color", "");
    $(".c-grey-900").css("color", "");
  });
}),
  $(document).ready(function () {
    $("#light-mode").hover(function () {
      $(".dropdown-menu").css("background-color", "white");
    }, function () {
      $(".dropdown-menu").css("background-color", "");
    });
  }),
  $(document).ready(function () {
    $("#future-mode").hover(function () {
      $(".dropdown-menu").css("background", "linear-gradient(to top left, rgba(0,0,0,0.125), rgba(0,0,0,0.25))");
    }, function () {
      $(".dropdown-menu").css("background", "");
    });
  });