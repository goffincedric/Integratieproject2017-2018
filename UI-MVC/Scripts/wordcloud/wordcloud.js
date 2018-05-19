
function makeAjaxCall(url, methodType) {
  var promiseObj = new Promise(function(resolve, reject) {
    var xhr = new XMLHttpRequest();

    xhr.open(methodType, url, true);
    xhr.setRequestHeader("x-api-key", "303d22a4-402b-4d3c-b279-9e81c0480711");
    xhr.send();
    xhr.onreadystatechange = function() {
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

var URL = "https://localhost:44342/api/item/GetThemasTop/" + 6;
makeAjaxCall(URL, "GET").then(process, errorHandler);

function process(output) {
  var keys = [];
  keys = Object.keys(output);
  var label = [];
  var values = [];


  var obj = "[";
  var obj2 = [];
  for (var i = 0; i < keys.length; i++) {
    var result = new Object();
    result.text = keys[i];
    result.count = output[keys[i]];
    var objKeysRegex = /({|,)(?:\s*)(?:')?([A-Za-z_$\.][A-Za-z0-9_ \-\.$]*)(?:')?(?:\s*):/g;
    var newQuotedKeysString = JSON.stringify(result).replace(objKeysRegex, "$1\"$2\":");
    var newObject = JSON.parse(newQuotedKeysString);
    obj2.push(newObject);

  }



  var myConfig = {
    "graphset": [
      {
        "type": "wordcloud",
        "options": {
          "style": {
            "tooltip": {
              visible: true,
              text: '%text: %hits'
            }
          },
          "words": obj2


        }
      }
    ]
  };

  $("#loader-8").hide();
  zingchart.render({
    id: 'myChart',
    data: myConfig,
    height: '100%',
    width: '100%',

  });


}

function errorHandler(statusCode) {
  console.log("failed with status", status);
}