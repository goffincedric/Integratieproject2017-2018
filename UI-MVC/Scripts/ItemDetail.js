$("#download1").on('click',
  function () {
    var image = document.getElementById("line2-chart").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download1.setAttribute("href", image);

  });

$("#download2").on('click',
  function () {
    var image = document.getElementById("sentiment").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download2.setAttribute("href", image);

  });

$("#download3").on('click',
  function () {
    var image = document.getElementById("age-chart").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download3.setAttribute("href", image);

  });

$("#download4").on('click',
  function () {
    var image = document.getElementById("gender-chart").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download4.setAttribute("href", image);

  });

$("#download5").on('click',
  function () {
    var image = document.getElementById("hashtag-chart").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download5.setAttribute("href", image);

  });


$("#download6").on('click',
  function () {
    var image = document.getElementById("mentions-chart").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download6.setAttribute("href", image);

  });

$("#download7").on('click',
  function () {
    var image = document.getElementById("mycanvas").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download7.setAttribute("href", image);

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
  var URL = "http://10.134.216.25:8010/api/item/GetItemDetails/" + id;
  makeAjaxCall(URL, "GET").then(process);

  function process(output) {

    var keys = [];
    keys = Object.keys(output);
    for (var i = 1; i < keys.length + 1; i++) {

      $("#output" + i).html(output[keys[i - 1]]);
    }
  }

  
}


function Age(id) {
  var URL = "http://10.134.216.25:8010/api/item/GetGender/" + id;
  makeAjaxCall(URL, "GET").then(process);

  function process(output) {
    var keys = [];
    keys = Object.keys(output);
    var label = [];
    var values = [];

    for (var i = 0; i < keys.length; i++) {
      label.push(keys[i].substring(0, 10));
      values.push(output[keys[i]]);
    }
    var can = $('#age-chart');

    new Chart(can,
      {
        type: 'doughnut',
        data:
          {
            labels: label,
            datasets: [
              {
                backgroundColor: ["#36a2eb", "#ffce56", "#7DDF64"],
                data: values
              }
            ]
          },
        options:
          {
            responsive: true
          }
      });
    $("#loader-2").hide();
    can.show();
  }

}

function Geslacht(id) {
  var URL = "http://10.134.216.25:8010/api/item/GetAges/" + id;
  makeAjaxCall(URL, "GET").then(process, errorHandler);

  function process(output) {
    var keys = [];
    keys = Object.keys(output);
    var label = [];
    var values = [];

    for (var i = 0; i < keys.length; i++) {
      label.push(keys[i].substring(0, 10));
      values.push(output[keys[i]]);
    }

    var can = $('#gender-chart');
    new Chart(can,
      {
        type: 'pie',
        data:
          {
            labels: label,
            datasets: [
              {
                backgroundColor: ["#36a2eb", "#ffce56", "#7DDF64"],
                data: values
              }
            ]
          },
        options:
          {
            responsive: true
          }
      });
    $("#loader-3").hide();
    can.show();
  }

  function errorHandler(statusCode) {
    console.log("failed with status", status);
  }
}


function urls(id) {


  var URL = "http://10.134.216.25:8010/api/item/GetTrendingUrl/" + id;
  makeAjaxCall(URL, "GET").then(process);

  function process(output) {
    console.log(output);

    $.each(output,
      function (data, realdata) {
        $("#urls").append('<i style="margin-right:10px" class="fas fa-link"></i>' +
          '<a target="_blank" href=' +
          realdata +
          ">" +
          realdata +
          "</a><br/>");
      });
  }

}

function drawLineChart(id) {

  var URL = "http://10.134.216.25:8010/api/item/GetItemTweet/" + id;
  makeAjaxCall(URL, "GET").then(process);

  function process(output) {
    var keys = [];
    keys = Object.keys(output);
    var label = [];
    var values = [];

    for (var i = 0; i < keys.length; i++) {
      label.push(keys[i].substring(0, 10));
      values.push(output[keys[i]]);
    }
    var can = $('#line-chart2');
    new Chart(can,
      {
        type: 'line',
        data:
          {
            labels: label,
            datasets: [
              {
                label: "Tweets",
                // backgroundColor: "rgba(3, 169, 244, 0.5)",
                borderColor: "#0277bd",
                data: values,
                fill: false
              }
            ]
          },
        options:
          {
            legend: { display: true },
            responsive: true,
            scales: {
              yAxes: [{
                scaleLabel: {
                  display: true,
                  labelString: 'Tweet aantaal'
                }
              }]
            }
          }
      });
    $("#loader-1").hide();
    can.show();
  }

}

function drawSentiment(id) {

  var URL = "http://10.134.216.25:8010/api/item/GetPersonEvolution/" + id;
  makeAjaxCall(URL, "GET").then(process);

  function process(output) {
    var keys = [];
    keys = Object.keys(output);
    var label = [];
    var values = [];

    for (var i = 0; i < keys.length; i++) {
      label.push(keys[i].substring(0, 10));
      values.push(output[keys[i]]);
    }
    var can = $('#sentiment');
    new Chart(can,
      {
        type: 'line',
        data:
          {
            labels: label,
            datasets: [
              {
                label: "Sentiment evolutie",

                borderColor: "#0277bd",
                data: values,
                fill: false
              }
            ]
          },
        options:
          {
            legend: { display: true },
            responsive: true,
            scales: {
              yAxes: [{
                scaleLabel: {
                  display: true,
                  labelString: 'Sentiment waarde'
                }
              }]
            }
          }
      });
    $("#loader-8").hide();
    can.show();
  }

}

//ORGANISATION AND THEME ONLY FUNCTION


function showTopPersons(id) {
  var URL = "http://10.134.216.25:8010/api/item/GetPopularTweetName/" + id;
  makeAjaxCall(URL, "GET").then(process);

  function process(output) {

    var keys = [];
    keys = Object.keys(output);
    for (var i = 1; i < keys.length + 1; i++) {

      $("#pers" + i).html(keys[i - 1]);
      var image = "https://twitter.com/" + output[keys[i - 1]] + "/profile_image?size=original";
      $("#pic" + i).append('<img style="width: 50px; height:50px; border-radius:50%;" src="' + image + '" alt="logo" />')
    }
  }


}

function Hashtags(id) {
  var URL = "http://10.134.216.25:8010/api/item/GetTrendingHashtagsCount/" + id;
  makeAjaxCall(URL, "GET").then(process);

  function process(output) {
    var keys = [];
    keys = Object.keys(output);
    var label = [];
    var values = [];

    for (var i = 0; i < keys.length; i++) {
      label.push(keys[i].substring(0, 10));
      values.push(output[keys[i]]);
    }

    var can = $('#hashtag-chart');
    new Chart(can,
      {
        type: 'bar',
        data:
          {
            labels: label,
            datasets: [
              {
                backgroundColor: ["#ff6384", "#36a2eb", "#cc65fe", "#ffce56", "#7DDF64"],
                data: values
              }
            ]
          },
        options:
          {
            responsive: true,
            legend: {
              display: false
            }
          }
      });
    $("#loader-4").hide();
    can.show();
  }
}


function Mentions(id) {
  var URL = "http://10.134.216.25:8010/api/item/GetTrendingMentionsCount/" + id;
  makeAjaxCall(URL, "GET").then(process);

  function process(output) {
    var keys = [];
    keys = Object.keys(output);
    var label = [];
    var values = [];

    for (var i = 0; i < keys.length; i++) {
      label.push(keys[i].substring(0, 10));
      values.push(output[keys[i]]);
    }
    var can = $('#mentions-chart');

    new Chart(can,
      {
        type: 'horizontalBar',
        data:
          {
            labels: label,
            datasets: [
              {
                backgroundColor: ["#ff6384", "#36a2eb", "#cc65fe", "#ffce56", "#7DDF64"],
                data: values
              }
            ]
          },
        options:
          {
            responsive: true,
            legend: {
              display: false
            }
          }
      });
    $("#loader-5").hide();
    can.show();
  }

}

//PERSON ONLY CODE///

function HashtagsPerson(id) {
  var URL = "http://10.134.216.25:8010/api/item/GetTrendingHashtags/" + id;
  makeAjaxCall(URL, "GET").then(process);

  function process(output) {
    var counter = 0;

    $.each(output,
      function (data, realdata) {
        counter++;
        var name = 'https://twitter.com/search?q=%23' + realdata + '&src=tyah&lang=nl';
        if (counter <= 4) {
          $("#hashtag1").append('<i style="margin-right:10px" class="fas fa-hashtag"></i>' +
            '<a  target="_blank" href=' +
            name +
            ">" +
            realdata +
            '</a><br/>');
        } else if (counter <= 8) {
          $("#hashtag2").append('<i style="margin-right:10px" class="fas fa-hashtag"></i>' +
            '<a  target="_blank" href=' +
            name +
            ">" +
            realdata +
            '</a><br/>');
        } else {
          $("#hashtag3").append('<i style="margin-right:10px" class="fas fa-hashtag"></i>' +
            '<a  target="_blank" href=' +
            name +
            ">" +
            realdata +
            '</a><br/>');
        }
      });
    $("#loader-7").hide();
    $("#hashtags").show();
  }
}

function mentionlist(id) {
  var URL = "http://10.134.216.25:8010/api/item/GetTrendingMentions/" + id;
  makeAjaxCall(URL, "GET").then(process);

  function process(output) {
    var counter2 = 0;
    $.each(output,
      function (data, realdata) {
        var name = 'https://twitter.com/' + realdata;
        counter2++;
        if (counter2 <= 4) {
          $("#mention1").append('<i style="margin-right:10px" class="fas fa-at"></i>' +
            '<a  target="_blank" href=' +
            name +
            ">" +
            realdata +
            '</a><br/>');
        } else if (counter2 <= 8) {
          $("#mention2").append('<i style="margin-right:10px" class="fas fa-at"></i>' +
            '<a  target="_blank" href=' +
            name +
            ">" +
            realdata +
            '</a><br/>');
        } else {
          $("#mention3").append('<i style="margin-right:10px" class="fas fa-at"></i>' +
            '<a  target="_blank" href=' +
            name +
            ">" +
            realdata +
            '</a><br/>');
        }
      });
    $("#loader-6").hide();
    $("#mentions").show();
  }



}


function drawNodeBox(id) {
  var URL = "http://10.134.216.25:8010/api/item/getrecordsfromperson/" +id;
  makeAjaxCall(URL, "GET").then(process);

  function process(output) {
    var itemid = id;


    var options = {
      userId: 'ThomasVerhoeven',
      projectId: 'sparkline',
      functionId: 'main',
      canvasId: 'mycanvas'
    };


    ndbx.embed(options,
      function (err, player) {
        if (err) {
          console.log('Load error:', err);
        } else {
          window.player = player;

          player.setValue('datareader', 'v', output);
          player.start();
          player.stop(); //anders infinite loop

        }
      });

    $("#loader-9").hide();
    $("#mycanvas").show();
  }

}