﻿$("#download1").on('click',
  function () {
    var image = document.getElementById("evol").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download1.setAttribute("href", image);

  });

$("#download2").on('click',
  function () {
    var image = document.getElementById("#doughnut-chart").toDataURL("image/jpg")
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
    var image = document.getElementById("bar2-chart").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download4.setAttribute("href", image);

  });
$("#download6").on('click',
  function () {
    var image = document.getElementById("radar2-chart").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download4.setAttribute("href", image);

  });
$("#download7").on('click',
  function () {
    var image = document.getElementById("myChart").toDataURL("image/jpg")
      .replace("image/jpg", "image/octet-stream");
    download4.setAttribute("href", image);

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

function drawRadarChart() {
  var can2 = $('#radar2-chart');

  new Chart(can2,
    {
      type: 'radar',
      data: {
        labels: ["Test 1", "Test 2", "Test 3", "Test 4", "Test 5"],
        datasets: [
          {
            label: "Data 1",
            fill: true,
            backgroundColor: "rgba(237, 231, 246, 0.5)",
            borderColor: "#673ab7",
            pointBorderColor: "#fff",
            pointBackgroundColor: "#512da8",
            data: [8.77, 55.61, 21.69, 6.62, 60.82]
          }, {
            label: "Data 2",
            fill: true,
            backgroundColor: "rgba(232, 245, 233, 0.5)",
            borderColor: "#2196f3",
            pointBorderColor: "#fff",
            pointBackgroundColor: "#1976d2",
            data: [19.48, 54.16, 7.61, 8.06, 4.45]
          }
        ]
      },
      options: {
        responive: true
      }
    });

}

drawRadarChart();

function drawBarChart() {

  var URL = "https://localhost:44342/api/item/getpersonstop/5";
  makeAjaxCall(URL, "GET").then(process, errorHandler);
  var can = $('#bar2-chart');

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
                  beginAtZero: false,


                  // minimum value will be 0.
                }
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
            backgroundColor: backgroundcolors[counter],
            borderColor: borderColors[counter],
            pointBackgroundColor: points[counter]

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