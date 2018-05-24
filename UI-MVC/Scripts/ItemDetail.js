("#download1").on('click',
  function () {
    var image = document.getElementById("line-chart2").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download1.setAttribute("href", image);

  });

("#download2").on('click',
  function () {
    var image = document.getElementById("sentiment").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download1.setAttribute("href", image);

  });

("#download3").on('click',
  function () {
    var image = document.getElementById("age-chart").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download1.setAttribute("href", image);

  });

("#download4").on('click',
  function () {
    var image = document.getElementById("gender-chart").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download1.setAttribute("href", image);

  });

("#download5").on('click',
  function () {
    var image = document.getElementById("hashtag-chart").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download1.setAttribute("href", image);

  });


("#download6").on('click',
  function () {
    var image = document.getElementById("mentions-chart").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download1.setAttribute("href", image);

  });

("#download7").on('click',
  function () {
    var image = document.getElementById("mycanvas").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download1.setAttribute("href", image);

  });


function makeAjaxCall(url, methodType) {
  var promiseObj = new Promise(function (resolve, reject) {
    var xhr = new XMLHttpRequest();
    xhr.open(methodType, url, true);
    xhr.setRequestHeader("x-api-key", "303d22a4-402b-4d3c-b279-9e81c0480711");
    xhr.send();
    xhr.onreadystatechange = function () {
      if (xhr.readyState === 4) {
        if (xhr.status === 200) {
          console.log("xhr done successfully");
          var resp = "";
          resp = xhr.response;
          resolve(JSON.parse(resp));

        } else {
          reject(xhr.status);
          console.log("xhr failed");
        }
      } else {
        console.log("xhr processing going on");
      }
    }
    console.log("request sent succesfully");

  });
  return promiseObj;
}


function showDetails(id) {
  var URL = "https://localhost:44342/api/item/GetItemDetails/" + id;
  makeAjaxCall(URL, "GET").then(process, errorHandler);

  function process(output) {

    var keys = [];
    keys = Object.keys(output);
    for (var i = 1; i < keys.length + 1; i++) {

      $("#output" + i).html(output[keys[i - 1]]);
    }
  }

  function errorHandler(statusCode) {
    console.log("failed with status", status);
  }
}