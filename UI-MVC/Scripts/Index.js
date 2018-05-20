$("#download1").on('click',
  function () {
    var image = document.getElementById("evol").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download1.setAttribute("href", image);

  });

$("#download2").on('click',
  function () {
    console.log("donut");
    var image = document.getElementById("doughnut-chart").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download2.setAttribute("href", image);

  });
$("#download3").on('click',
  function () {
    var image = document.getElementById("pie-chart").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download3.setAttribute("href", image);

  });
$("#download4").on('click',
  function () {
    var image = document.getElementById("bar-chart").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download4.setAttribute("href", image);

  });


$("#download5").on('click',
  function () {
    var image = document.getElementById("line-chart").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download5.setAttribute("href", image);

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

function showIncrease() {
  var URL = "https://localhost:44342/api/item/GetPersonIncrease";
  makeAjaxCall(URL, "GET").then(process, errorHandler);

  function process(output) {
    var keys = [];
    keys = Object.keys(output);
    var label = [];
    var values = [];


    for (var i = 0; i < keys.length; i++) {
      label.push(keys[i]);
      $("#pol" + i).html(keys[i]);
      values.push(output[keys[i]]);
      $("#poli" + i).html(output[keys[i]]);
    }
    console.log(label);
    console.log(values);
  }

  function errorHandler(statusCode) {
    console.log("failed with status", status);
  }
}

showIncrease();


function drawBarChart() {

  var URL = "https://localhost:44342/api/item/getpersonstop/5";
  makeAjaxCall(URL, "GET").then(process, errorHandler);
  var can = $('#bar-chart');

  function process(output) {
    console.log(output);

    var keys = [];
    keys = Object.keys(output);
    var label = [];
    var values = [];

    for (var i = 0; i < keys.length; i++) {
      label.push(keys[i]);

      values.push(output[keys[i]]);
    }
    new Chart(can,
      {
        type: 'bar',
        data: {
          labels: label,
          datasets: [
            {
              label: "Aantal tweets",
              backgroundColor: "#03a9f4",
              borderColor: "rgba(3, 169, 244, 0.5)",
              data: values
            }
          ]
        },
        options: {
          legend: { display: true },
          responsive: true
        }
      });
    $("#loader-4").hide();
    can.show();
  }


  function errorHandler(statusCode) {
    console.log("failed with status", status);
  }
}


drawBarChart();

function drawLineChart() {
  var URL = "https://localhost:44342/api/item/GetMostPopularPersons/3";
  makeAjaxCall(URL, "GET").then(process, errorHandler);

  function process(output) {
    var can = $('#evol');
    var datagrafiek = {
      labels: ["Dag 1", "Dag 2", "Dag 3", "Dag 4", "Dag 5", "Dag 6", "Dag 7", "Dag 8", "Dag 9", "Dag 10"],
      datasets: []
    };

    var myLineChart = new Chart(can,
      {
        type: 'line',
        data: datagrafiek,
        options: {
          responsive: true,
          scales: {
            yAxes: [
              {
                display: true,
                ticks: {
                  // minimum will be 0, unless there is a lower value.
                  // OR //
                  beginAtZero: false
                },
                  scaleLabel: {
                    display: true,
                    labelString: 'Sentiment evolutie'
                  }


                  // minimum value will be 0.
                }
              
            ]
          }
        }
      });
    $("#loader-1").hide();
    can.show();

    var counter = 0;
    $.each(output,
      function (key, value) {
        var URL = "https://localhost:44342/api/item/GetPersonEvolution/" + key;
        makeAjaxCall(URL, "GET").then(process, errorHandler);

        function process(output2) {
          var keys = [];
          keys = Object.keys(output2);

          var label = [];
          var values = [];

          var backgroundcolors = ["rgba(237, 231, 246, 0.5)", "rgba(232, 245, 233, 0.5)", "rgba(3, 169, 244, 0.5)"];
          var borderColors = ["#673ab7", "#2196f3", "#4caf50"]
          var points = ["#512da8", "#1976d2", "#388e3c"];

          for (var i = 0; i < keys.length; i++) {
            label.push(keys[i]);
            values.push(output2[keys[i]]);
          }

          var myNewDataSet = {
            label: value,
            data: values,
            borderWidth: 2,
           // backgroundColor: backgroundcolors[counter],
            borderColor: borderColors[counter],
            pointBackgroundColor: points[counter],
            fill:false

          }

          datagrafiek.datasets.push(myNewDataSet);

          myLineChart.update();
          counter++;
        }
      });


    function errorHandler(statusCode) {
      console.log("failed with status", status);
    }


  }


  function errorHandler(statusCode) {
    console.log("failed with status", status);
  }

}

drawLineChart();

function TrendingPersonGraphs() {
  var URL = "https://localhost:44342/api/item/getMostPopularPerson";
  makeAjaxCall(URL, "GET").then(process, errorHandler);

  function process(id) {

    function getHashtags(id) {
      var URL = "https://localhost:44342/api/item/GetTrendingHashtagsCount/" + id;
      makeAjaxCall(URL, "GET").then(process, errorHandler);

      function process(output) {

        var keys = [];
        keys = Object.keys(output);
        var label = [];
        var values = [];

        for (var i = 0; i < keys.length; i++) {
          label.push(keys[i]);
          values.push(output[keys[i]]);
        }

        var can = $('#doughnut-chart');


        new Chart(can,
          {
            type: 'doughnut',
            data: {
              labels: label,
              datasets: [
                {
                  backgroundColor: ["#ff6384", "#36a2eb", "#cc65fe", "#ffce56", "#7DDF64"],
                  data: values
                }
              ]
            },
            options: {
              responsive: true
            }
          });

        $("#loader-2").hide();
        can.show();
      }

      function errorHandler(statusCode) {
        console.log("failed with status", status);
      }
    }

    getHashtags(id);

    function drawPie(id) {
      var URL = "https://localhost:44342/api/item/GetTrendingMentionsCount/" + id;
      makeAjaxCall(URL, "GET").then(process, errorHandler);

      function process(output) {

        var keys = [];
        keys = Object.keys(output);
        var label = [];
        var values = [];

        for (var i = 0; i < keys.length; i++) {
          label.push(keys[i]);
          values.push(output[keys[i]]);
        }

        var can = $('#pie-chart');
        // can.parent().attr('Style', 'width: 400px; height: 250');


        new Chart(can,
          {
            type: 'pie',
            data: {
              labels: label,
              datasets: [
                {
                  backgroundColor: ["#ff6384", "#36a2eb", "#cc65fe", "#ffce56", "#7DDF64"],
                  data: values
                }
              ]
            },
            options: {
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

    drawPie(id);
  }

  function errorHandler(statusCode) {
    console.log("failed with status", status);
  }
}

TrendingPersonGraphs();

function map() {
    var URL = "https://localhost:44342/api/item/GetTweetsByDistrict";
    makeAjaxCall(URL, "GET").then(process);

    var mapData;

    function process(output) {
        console.log(output);

        var keys = [];
        keys = Object.keys(output);
        var label = [];
        var values = [];

        for (var i = 0; i < keys.length; i++) {
            label.push(keys[i]);

            values.push(output[keys[i]]);
        }

        mapData = {
            "ANT": values[1],
            "BRU": values[3],
            "LIM": values[4],
            "OVL": values[2],
            "VBR": values[5],
            "WVL": values[0]
        };

        $('#test-map').vectorMap({
            map: "be_mill",
            series: {
                regions: [{
                    values: mapData,
                    scale: ["#C8EEFF", "#0071A4"],
                    normalizeFunction: 'polynomial'
                }]
            },
            backgroundColor: "transparent",
            regionStyle: {
                initial: {
                    fill: "#e6e6e6"
                }
            },
            normalizeFunction: "linear",
            onRegionTipShow: function (e, el, code) {
                el.html(el.html() + ' (GDP - ' + mapData[code] + ')');
            }
        });
    }
}

map();

function drawOrganisations() {
  var URL = "https://localhost:44342/api/item/GetMostPopularOrganisations/3";
  makeAjaxCall(URL, "GET").then(process, errorHandler);

  function process(output) {
    var can = $('#line-chart');
    var datagrafiek = {
      labels: ["Dag 1", "Dag 2", "Dag 3", "Dag 4", "Dag 5", "Dag 6", "Dag 7", "Dag 8", "Dag 9", "Dag 10"],
      datasets: []
    };

    var myLineChart = new Chart(can,
      {
        type: 'line',
        data: datagrafiek,
        options: {
          responsive: true,
          scales: {
            yAxes: [
              {
                display: true,
                ticks: {
                  // minimum will be 0, unless there is a lower value.
                  // OR //
                  beginAtZero: false
                },
                  scaleLabel: {
                    display: true,
                    labelString: 'Tweet aantaal'
                  }

                  // minimum value will be 0.
                
              }
            ]
          }
        }
      });
    $("#loader-7").hide();
    can.show();

    var counter = 0;
    $.each(output,
      function (key, value) {
        var URL = "https://localhost:44342/api/item/GetItemTweet/" + key;
        makeAjaxCall(URL, "GET").then(process, errorHandler);

        function process(output2) {
          var keys = [];
          keys = Object.keys(output2);

          var label = [];
          var values = [];

          var backgroundcolors = ["rgba(237, 231, 246, 0.5)", "rgba(232, 245, 233, 0.5)", "rgba(3, 169, 244, 0.5)"];
          var borderColors = ["#673ab7", "#2196f3", "#4caf50"]
          var points = ["#512da8", "#1976d2", "#388e3c"];

          for (var i = 0; i < keys.length; i++) {
            label.push(keys[i]);
            values.push(output2[keys[i]]);
          }

          var myNewDataSet = {
            label: value,
            data: values,
            borderWidth: 2,
            borderColor: borderColors[counter],
            pointBackgroundColor: points[counter],
            fill: false

          }

          datagrafiek.datasets.push(myNewDataSet);

          myLineChart.update();
          counter++;
        }
      });


    function errorHandler(statusCode) {
      console.log("failed with status", status);
    }


  }


  function errorHandler(statusCode) {
    console.log("failed with status", status);
  }

}

drawOrganisations();




function drawMap() {

  var URL = "https://localhost:44342/api/item/GetTweetsByDistrict";
  makeAjaxCall(URL, "GET").then(process);


  function process(output) {
    console.log(output);

    var keys = [];
    keys = Object.keys(output);
    var label = [];
    var values = [];

    for (var i = 0; i < keys.length; i++) {
      label.push(keys[i]);

      values.push(output[keys[i]]);
    }
    
  }

}


function showTweets() {

  var URL = "https://localhost:44342/api/item/GetTweetName";
  makeAjaxCall(URL, "GET").then(process);

  console.log("TEEEEEEEEEEEEEEST");
  function process(output) {

    var html = "https://twitter.com/" + output + "?ref_src=twsrc%5Etfw";

    $("#twitterfeed").append('<a class="twitter-timeline" tweet-limit="5" height="450" href="' + html + '"></a>');
    
  }
  
}

showTweets();