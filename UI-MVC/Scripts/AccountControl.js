﻿function getRotationDegrees(obj) {
  var matrix = obj.css("-webkit-transform") ||
    obj.css("-moz-transform") ||
    obj.css("-ms-transform") ||
    obj.css("-o-transform") ||
    obj.css("transform");
  if (matrix !== 'none') {
    var values = matrix.split('(')[1].split(')')[0].split(',');
    var a = values[0];
    var b = values[1];
    var angle = Math.round(Math.atan2(b, a) * (180 / Math.PI));
  } else {
    angle = 0;
  }
  return angle < 0 ? angle + 360 : angle;
}

var rotation = 0;

$(".arrow-account").click(function() {
  $(this).parent().parent().children("form").toggle(300);
  rotation = getRotationDegrees($(this)) + 180;
  $(this).css({ 'transform': 'rotate(' + rotation + 'deg)' });
});

$("#btn-clear").click(function() {
  $("#inputCurrentPassword").removeAttr('value');
  $("#inputNewPassword").removeAttr('value');
  $("#inputConfirmPassword").removeAttr('value');
});

$(".btn-delete").click(function() {
  $(".modal").show();
});

$(".close").click(function() {
  $(".modal").hide();
});

$(".btn-cancel").click(function() {
  $(".modal").hide();
});