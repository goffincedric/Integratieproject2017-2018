var div = document.getElementById("myDropdown");
var a = div.getElementsByTagName("a");
for (i = 0; i < a.length; i++) {
  a[i].style.display = "none";
}

function myFunction() {
  document.getElementById("myDropdown").classList.toggle("show");
}

function filterFunction() {
  var ul, li, i;
  var input = document.getElementById("myInput");

  var filter = input.value.toUpperCase();
  div = document.getElementById("myDropdown");
  a = div.getElementsByTagName("a");
  for (i = 0; i < a.length; i++) {
    if (a[i].innerHTML.toUpperCase().indexOf(filter) > -1) {
      a[i].style.display = "";

    } else {
      a[i].style.display = "none";
    }
  }
  if (!filter) {
    for (i = 0; i < a.length; i++) {
      a[i].style.display = "none";
    }
  }
}