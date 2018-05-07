
$(document).ready(function () {
  

 let headers = {
      "x-api-key": "303d22a4-402b-4d3c-b279-9e81c0480711"
  }
  var tmp = "";
  tmp = $.ajax({
      async: false,
    type: 'GET',
    Headers: headers,
    dataType: 'json',
      url: "https://localhost:44342/api/item/getpersonstop/" + 10,
      headers: {'x-api-key': '303d22a4-402b-4d3c-b279-9e81c0480711' }
  }).responseJSON

  var keys = [];
  keys = Object.keys(tmp);
  var label = [];
  var values = [];


  var obj = "[";
  var obj2 = [];
  for (var i    = 0; i < keys.length; i++) {
    var result  = new Object();
    result.text = keys[i];
    result.count = tmp[keys[i]];
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

  zingchart.render({
      id: 'myChart',
    data: myConfig,
    height: '100%',
    width: '100%',
   
  });


});