var DashBoardId = $('main').attr('id');
DashBoardId = DashBoardId.substring(DashBoardId.indexOf('-') + 1, DashBoardId.length);

let Headers = { "x-api-key": "303d22a4-402b-4d3c-b279-9e81c0480711" };

//api call working
var DashData = "";

DashData = $.ajax({
    async: false,
    type: 'GET',
    dataType: 'json',
    headers: Headers,
    url: "https://localhost:44342/api/dashboard/getdashboardzones/" + DashBoardId
}).responseJSON;

$(function () {
    var itemIds = [];

    var addingElement = false;

    var options = {
        width: 12,
        animate: true,
        //float: true,
        removable: '.trash',
        removeTimeout: 100,
        acceptWidgets: '.grid-stack-item'
    };
    $('.grid-stack').gridstack(options);

    new function () {
        function getRotationDegrees(obj) {
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
            } else { angle = 0; }
            return angle < 0 ? angle + 360 : angle;
        }

        //done
        findElements = function (id) {
            var ZoneData = $.ajax({
                async: false,
                type: 'GET',
                dataType: 'json',
                headers: Headers,
                url: "https://localhost:44342/api/dashboard/getzoneelements/" + id
            }).responseJSON;

            return ZoneData;
        };

        deleteElement = function (element, grid) {
            var Element = element.parent();
            var ElementId = Element.attr('id');
            ElementId = ElementId.substring(ElementId.indexOf('-') + 1, ElementId.length);
            Element = Element.parent();

            $.ajax({
                async: false,
                type: 'Delete',
                headers: Headers,
                url: "https://localhost:44342/api/dashboard/deleteelement/" + ElementId
            })

            grid.data('gridstack').removeWidget(Element);
        };

        //done
        addElement = function (grid) {
            addingElement = true;

            var addZone = $('.add-zone');
            var ZoneId = grid.attr('id');

            ZoneId = ZoneId.substring(ZoneId.indexOf('-') + 1, ZoneId.length);

            var removeElement = grid.children()[grid.children().length - 1];

            grid.data('gridstack').removeWidget(removeElement);

            if (ZoneId === 'x') {
                var Zone = JSON.parse('{"Title" : "New Zone", "DashboardId" : "' + DashBoardId + '"}');

                ZoneId = $.ajax({
                    async: false,
                    type: 'POST',
                    data: Zone,
                    dataType: 'json',
                    headers: Headers,
                    url: "https://localhost:44342/api/dashboard/postzone/" + DashBoardId
                }).responseJSON.ZoneId

                $('.add-zone').remove();

                //zorgen dat pagina niet herladen moet worden
                var $newdiv = $('<div class="p-10 mB-10 zone-' + ZoneId + '"></div>');
                $newdiv.append($('<h4 class="bb-2"></h4>'));
                $newdiv.children('h4').append($('<span class="title mR-15">New Zone</span>'));
                $newdiv.children('h4').append($('<span class="edit-zone"></span>'));
                $newdiv.children('h4').children('.edit-zone').append($('<i class="ti-pencil"></i>'));
                $newdiv.children('h4').append($('<span class="arrow-dashboard"></span>'));
                $newdiv.children('h4').children('.arrow-dashboard').append($('<i class="ti-angle-up"></i>'));
                $newdiv.children('h4').append($('<span class="delete-zone"></span>'));
                $newdiv.children('h4').children('.delete-zone').append($('<i class="ti-trash"></i>'));
                $newdiv.append($('<div class="DashZone"></div>'));
                $newdiv.append($('<div class="grid-stack grid-stack-12" id="zone-' + ZoneId + '"></div >'));
                var newZone = $('#mainContent').append($newdiv);

                addZone = $('#mainContent').append(addZone);

                $('.grid-stack').gridstack(options);

                grid = newZone.children('.zone-' + ZoneId).children('#zone-' + ZoneId);

                var deletezone = grid.parent().children('h4').children('.delete-zone');

                var editzone = grid.parent().children('h4').children('.edit-zone');

                deletezone.on('click', function () {
                    removeZone(grid.parent());
                });

                editzone.on('click', function () {
                    editZone($(this));
                });

                addZone = addZone.children('.add-zone').children('div').children('#zone-x');
                var plusElement = addZone.data('gridstack').addWidget($('<div><div class="grid-stack-item-content bgc-white bd" id ="Element-+"><div><img class="w-3r bdrs-50p alert-img add-element" src="/Content/Images/plus-icon.png"><div/><div/><div/>'), 0, 0, 3, 3, true);

                plusElement.children('.grid-stack-item-content').children('div').children('img').on("click", function () {
                    addElement(addZone);
                });
            };

            var newElement = grid.data('gridstack').addWidget($('<div><div class="grid-stack-item-content bgc-white bd" id="Element-x"><i class="ti-trash float-right mR-15 mT-15 delete-element"></i><canvas></canvas><div/><div/>'), 0, 0, 3, 3, true);
            var X = newElement.data().gsX;
            var Y = newElement.data().gsY;

            var Element = JSON.parse('{"X" : "' + X + '", "Y" : "' + Y + '", "Width": "3", "Height": "3", "IsDraggable": "true", "ZoneId": "' + ZoneId + '"}');

            var ElementId = $.ajax({
                async: false,
                type: 'POST',
                data: Element,
                dataType: 'json',
                headers: Headers,
                url: "https://localhost:44342/api/dashboard/postelement/" + ZoneId
            }).responseJSON.ElementId

            newElement.children('#Element-x').attr('id', 'Element-' + ElementId);

            plusElement = grid.data('gridstack').addWidget($('<div><div class="grid-stack-item-content bgc-white bd" id ="Element-+"><div><img class="w-3r bdrs-50p alert-img add-element" src="/Content/Images/plus-icon.png"><div/><div/><div/>'), 0, 0, 3, 3, true);

            plusElement.children('.grid-stack-item-content').children('div').children('img').on("click", function () {
                addElement(grid);
            });

            newElement.children('#Element-' + ElementId).children('.delete-element').on('click', function () {
                deleteElement($(this), grid);
            });

            var Wizard = $('#Wizard');
            Wizard.show();
            Wizard.children().attr('id', 'Wizard-' + ElementId);
            $('.btn-cancel').off('click');
            $('.btn-cancel').on('click', function () {
                deleteElement(newElement.children('#Element-' + ElementId).children(), grid);
                Wizard.children().attr('id', '');
                Wizard.hide();
            });

            var data = null;
            var onderwerp = null;
            var output = null;

            $('.btn-next').on('click', function () {
                switch ($('input:checked').attr('id')) {
                    case 'person':
                        //datasoort = null;
                        data = $.ajax({
                            async: false,
                            type: 'GET',
                            headers: Headers,
                            url: "https://localhost:44342/api/item/getperson"
                        }).responseJSON;
                        onderwerp = 'politieker';
                        break;
                    case 'organisation':
                        //datasoort = null;
                        data = $.ajax({
                            async: false,
                            type: 'GET',
                            headers: Headers,
                            url: "https://localhost:44342/api/item/getorganisation"
                        }).responseJSON;
                        onderwerp = 'organisatie';
                        break;
                    case 'theme':
                        //datasoort = null;
                        data = $.ajax({
                            async: false,
                            type: 'GET',
                            headers: Headers,
                            url: "https://localhost:44342/api/item/gettheme"
                        }).responseJSON;
                        onderwerp = 'thema';
                        break;
                }

                $('#soort').text(onderwerp + 's');

                let dropdown = $('#locality-dropdown');

                itemData = {
                    items: []
                };

                var cardnumber = 1;
                $('#locality-dropdown').off('change');
                $('#locality-dropdown').on('change', function () {
                    if (cardnumber <= 5) {
                        $('#cardholder').append($('<div id="card' + cardnumber + '" class="col-sm-2-5 pX-5">' +
                            '<h5 class= "info-text" > Voeg hier toe!</h5>' +
                            '<div class="card">' +
                            '<img src="/Content/Images/plus-icon.png" alt="Avatar" style="width:100%">' +
                            '<div class="container">' +
                            '<h4><b class="itemId">#</b></h4>' +
                            '<p class="itemName">Architect & Engineer</p>' +
                            '</div>' +
                            '</div>' +
                            '</div>'));
                        $('#card' + cardnumber).children('.card').children('img').on('click', function () {
                            var item = ($.ajax({
                                async: false,
                                type: 'GET',
                                headers: Headers,
                                url: "https://localhost:44342/api/item/getitem/" + $("#locality-dropdown option:selected").attr('value')
                            }).responseJSON);
                            $(this).attr('src', (item.IconURL).substring(1, item.IconURL.length));
                            $(this).siblings('.container').children('h4').children('.itemId').text(item.ItemId);
                            $(this).siblings('.container').children('.itemName').text(item.Name);
                        })
                        cardnumber++;
                    }
                })
                
                $('#output').off('change');
                $('#output').on('change', function () {
                    output = $('#output option:selected').text();
                    console.log(output);
                });
                //$('#add-soort').off('click');
                //$('#add-soort').on('click', function () {
                //    itemData.items.push($.ajax({
                //        async: false,
                //        type: 'GET',
                //        headers: Headers,
                //        url: "https://localhost:44342/api/item/getitem/" + $("#locality-dropdown option:selected").attr('value')
                //    }).responseJSON);
                //    $('#itemInfo').text(itemData.items);
                //});

                dropdown.empty();

                dropdown.append('<option selected="true" disabled>selecteer een '+onderwerp+'</option>');
                dropdown.prop('selectedIndex', 0);

                // Populate dropdown with list of data

                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.ItemId).text(entry.Name));
                });
            })

            $('.btn-finish').off('click');
            $('.btn-finish').on('click', function () {
                var GraphType = 0;
                $('input:checked').attr('id');
                switch ($('input:checked').attr('id')) {
                    case 'bar': GraphType = 0;
                        break;
                    case 'pie': GraphType = 2;
                        break;
                }
                Wizard.hide();

                //edit later

                var items = getItems($('#cardholder'));

                var itemsJSON = JSON.stringify(items);

                console.log(itemsJSON);

                var Element = JSON.parse('{"ElementId":"' + ElementId + '", "X" : "' + newElement.data().gsX + '", "Y" : "' + newElement.data().gsY + '", "Width": "' + newElement.data().gsWidth + '", "Height": "' + newElement.data().gsHeight + '", "IsDraggable": "' + true + '", "ZoneId": "' + ZoneId + '", "GraphType" : "' + GraphType + '", "Items": '+itemsJSON+'}');

                $.ajax({
                    async: false,
                    type: 'PUT',
                    data: Element,
                    dataType: 'json',
                    headers: Headers,
                    url: "https://localhost:44342/api/dashboard/putelement/" + ElementId
                })

                chooseChart(GraphType, newElement.children('#Element-' + ElementId).children('canvas'), 25);
            });
            addingElement = false;
        };

        editZone = function (grid) {
            console.log(grid);

            grid.off('click');

            grid.children('.ti-pencil').attr('class', 'ti-check');

            var ZoneId = grid.parent().parent().attr('class');

            ZoneId = ZoneId.substring(ZoneId.indexOf('zone-') + 5, ZoneId.length);

            titleTag = grid.siblings('.title');
            oldTitle = titleTag.html();

            titleWidth = titleTag.width();
            titleHeight = titleTag.height();

            titleTag.replaceWith($('<input type="text" name="title" style="width: ' + titleWidth + 'px; height:' + titleHeight + 'px;font-weight: bold; margin-right: 15px; max-width: 600px;">'));

            grid.siblings('input').focus();

            grid.siblings('input').val(oldTitle);

            grid.on('click', function () {
                var input = grid.siblings('input');
                newTitle = input.val();

                Zone = JSON.parse('{ "Title": "' + newTitle + '", "ZoneId": "' + ZoneId + '"}');

                $.ajax({
                    async: false,
                    type: 'PUT',
                    data: Zone,
                    dataType: 'json',
                    headers: Headers,
                    url: "https://localhost:44342/api/dashboard/putzone/" + ZoneId
                })

                titleTag.html(newTitle);

                input.replaceWith(titleTag);

                grid.children('.ti-check').attr('class', 'ti-pencil');

                grid.off('click');
                grid.on('click', function () {
                    editZone(grid);
                })
            })

            grid.siblings('input').keypress(function (e) {
                var input = $(this);

                titleTag.html($(this).val());

                grid.parent().append(titleTag);

                var inputwidth = titleTag.width();

                titleTag.remove();

                input.width(inputwidth + 10);

                if (e.which === 13) {
                    newTitle = $(this).val();

                    Zone = JSON.parse('{ "Title": "' + newTitle + '", "ZoneId": "' + ZoneId + '"}');

                    $.ajax({
                        async: false,
                        type: 'PUT',
                        data: Zone,
                        dataType: 'json',
                        headers: Headers,
                        url: "https://localhost:44342/api/dashboard/putzone/" + ZoneId
                    })

                    titleTag.html(newTitle);

                    $(this).replaceWith(titleTag);

                    grid.children('.ti-check').attr('class', 'ti-pencil');

                    grid.off('click');
                    grid.on('click', function () {
                        editZone(grid);
                    })
                }
            });
        };

        removeZone = function (grid) {
            var ZoneId = grid.attr('class');

            ZoneId = ZoneId.substring(ZoneId.indexOf('zone-') + 5, ZoneId.length);

            $.ajax({
                async: false,
                type: 'DELETE',
                headers: Headers,
                url: "https://localhost:44342/api/dashboard/deletezone/" + ZoneId
            })

            $(grid).remove();
        };

        changeElements = function (elements, grid) {
            if (!addingElement) {
                var ZoneId = grid.attr('id');
                ZoneId = ZoneId.substring(ZoneId.indexOf('-') + 1, ZoneId.length);
                var count = 0;

                for (var e in elements) {

                    var element = elements[e];

                    var ElementId = element.el.children('div').attr('id');
                    ElementId = ElementId.substring(ElementId.indexOf('-') + 1, ElementId.length);

                    if (ElementId !== '+') {
                        if (ZoneId === 'x') {

                            var Zone = JSON.parse('{"Title" : "New Zone", "DashboardId" : "' + DashBoardId + '"}');

                            ZoneId = $.ajax({
                                async: false,
                                type: 'POST',
                                data: Zone,
                                dataType: 'json',
                                headers: Headers,
                                url: "https://localhost:44342/api/dashboard/postzone/" + DashBoardId
                            }).responseJSON.ZoneId

                            var Element = $.ajax({
                                async: false,
                                type: 'GET',
                                headers: Headers,
                                url: "https://localhost:44342/api/dashboard/getelement/" + ElementId
                            }).responseJSON

                            Element.ZoneId = ZoneId;

                            $.ajax({
                                async: false,
                                type: 'PUT',
                                data: Element,
                                dataType: 'json',
                                headers: Headers,
                                url: "https://localhost:44342/api/dashboard/putelement/" + ElementId
                            })

                            var addZone = $('.add-zone');

                            $('.add-zone').remove();

                            var $newdiv = $('<div class="p-10 mB-10 zone-' + ZoneId + '"></div>');
                            $newdiv.append($('<h4 class="bb-2"></h4>'));
                            $newdiv.children('h4').append($('<span class="title mR-15">New Zone</span>'));
                            $newdiv.children('h4').append($('<span class="edit-zone"></span>'));
                            $newdiv.children('h4').children('.edit-zone').append($('<i class="ti-pencil"></i>'));
                            $newdiv.children('h4').append($('<span class="arrow-dashboard"></span>'));
                            $newdiv.children('h4').children('.arrow-dashboard').append($('<i class="ti-angle-up"></i>'));
                            $newdiv.children('h4').append($('<span class="delete-zone"></span>'));
                            $newdiv.children('h4').children('.delete-zone').append($('<i class="ti-trash"></i>'));
                            $newdiv.append($('<div class="DashZone"></div>'));
                            $newdiv.children('.DashZone').append($('<div class="grid-stack grid-stack-12" id="zone-' + ZoneId + '"></div >'));
                            var newZone = $('#mainContent').append($newdiv);

                            addZone = $('#mainContent').append(addZone);

                            $('.grid-stack').gridstack(options);

                            grid.on('change', function (e, items) {
                                changeElements(items, $(this));
                            });

                            addZone = addZone.children('.add-zone').children('div').children('#zone-x');

                            grid = newZone.children('.zone-' + ZoneId).children('.DashZone').children('#zone-' + ZoneId);

                            addZone.data('gridstack').removeWidget($('#Element-' + ElementId).parent());

                            console.log(grid);

                            var deletezone = grid.parent().parent().children('h4').children('.delete-zone');

                            var editzone = grid.parent().parent().children('h4').children('.edit-zone');

                            var hidezone = grid.parent().parent().children('h4').children('.arrow-dashboard');

                            console.log(hidezone);

                            hidezone.on('click', function () {
                                console.log($(this).parent().parent());
                                $(this).parent().parent().children(".DashZone").toggle(300);
                                rotation = getRotationDegrees($(this)) + 180;
                                $(this).css({ 'transform': 'rotate(' + rotation + 'deg)' });
                            });

                            deletezone.on('click', function () {
                                removeZone(grid.parent().parent());
                            });

                            editzone.on('click', function () {
                                editZone($(this));
                            });

                            grid.on('change', function (e, items) {
                                changeElements(items, $(this));
                            });

                            addZone.data('gridstack').move($('.add-zone').children('div').children('div').children('div'), 0, 0);

                            grid.data('gridstack').addWidget($('<div><div class="grid-stack-item-content bgc-white bd" id="Element-' + ElementId + '"><i class="ti-trash float-right mR-15 mT-15 delete-element"></i><canvas></canvas><div/><div/>'), Element.X, Element.Y, Element.Width, Element.Height, true);

                            grid.data('gridstack').addWidget($('<div><div class="grid-stack-item-content bgc-white bd" id ="Element-+"><div><img class="w-3r bdrs-50p alert-img add-element" src="/Content/Images/plus-icon.png"><div/><div/><div/>'), 0, 0, 3, 3, true);

                            chooseChart(Element.GraphType, $('#Element-' + ElementId).children('canvas'), 25);
                        }
                        var oldElement = $.ajax({
                            async: false,
                            type: 'GET',
                            headers: Headers,
                            url: "https://localhost:44342/api/dashboard/getelement/" + ElementId
                        }).responseJSON

                        console.log(oldElement);

                        var oldZoneId = oldElement.ZoneId;

                        Element = JSON.parse('{"ElementId":"' + ElementId + '", "X" : "' + element.x + '", "Y" : "' + element.y + '", "Width": "' + element.width + '", "Height": "' + element.height + '", "IsDraggable": "' + !element.noMove + '", "ZoneId": "' + ZoneId + '", "GraphType": "' + oldElement.GraphType + '"}');

                        $.ajax({
                            async: false,
                            type: 'PUT',
                            data: Element,
                            dataType: 'json',
                            headers: Headers,
                            url: "https://localhost:44342/api/dashboard/putelement/" + ElementId
                        })

                        if (oldZoneId !== ZoneId) {
                            console.log($('#Element-' + ElementId).children('canvas'));

                            chooseChart(oldElement.GraphType, $('#Element-' + ElementId).children('canvas'), 25);
                        }
                    }
                }
            }
        };

        getItems = function (cards) {
            var items = [];

            $.each(cards.children(), function () {
                itemId = $(this).children('.card').children('.container').children('h4').children('.itemId').text();

                items.push($.ajax({
                        async: false,
                        type: 'GET',
                        headers: Headers,
                        url: "https://localhost:44342/api/item/getitem/" + itemId
                    }).responseJSON);
            })
            return items;
        }

        //done
        clearGrid = function (grid) {
            grid.removeAll();
            return false;
        }.bind(this);

        //almost done
        loadGrid = function (grid) {
            clearGrid(grid.data('gridstack'));
            var ZoneId = grid.attr('id');
            ZoneId = ZoneId.substring(ZoneId.indexOf('-') + 1, ZoneId.length);
            if (ZoneId === 'x') {
                var plusElement = grid.data('gridstack').addWidget($('<div><div class="grid-stack-item-content bgc-white bd" id ="Element-+"><div><img class="w-3r bdrs-50p alert-img add-element" src="/Content/Images/plus-icon.png"><div/><div/><div/>'), 0, 0, 3, 3, true);

                plusElement.resizable(false);

                plusElement.children('.grid-stack-item-content').children('div').children('img').on("click", function () {
                    addElement(grid);
                });
            } else {
                var elements = findElements(ZoneId);

                grid.each(function () {

                    var griddata = $(this).data('gridstack');

                    //this for now(change later), Gets correct data from database
                    _.each(elements, function (node) {
                        var newElement = griddata.addWidget($('<div><div class="grid-stack-item-content bgc-white bd" id="Element-' + node.ElementId + '"><i class="ti-trash float-right mR-15 mT-15 delete-element"></i><div/><div/>'),
                            node.X, node.Y, node.Width, node.Height);
                        newElement.children('#Element-' + node.ElementId).children('.delete-element').on('click', function () {
                            deleteElement($(this), grid);
                        });

                        newElement.children('#Element-' + node.ElementId).append($('<canvas></canvas>')).children('canvas').attr('id', 'pie-chart');

                        chooseChart(node.GraphType, newElement.children('#Element-' + node.ElementId).children('canvas'), 25);
                    }, this);

                    var plusElement = griddata.addWidget($('<div><div class="grid-stack-item-content bgc-white bd" id ="Element-+"><div><img class="w-3r bdrs-50p alert-img add-element" src="/Content/Images/plus-icon.png"><div/><div/><div/>'), 0, 0, 3, 3, true);

                    plusElement.children('.grid-stack-item-content').children('div').children('img').on("click", function () {
                        addElement(grid);
                    });
                });
            }
        };

        chooseChart = function (type, can, id) {
            switch (type) {
                case 0: drawBarChart(can, id)
                    break;
                case 1: drawLineChart();
                    break;
                case 2: drawPie(id, can);
                    break;
                case 3: console.log('Scattre');
                    break;
                case 4: drawDonut();
                    break;
                case 5: console.log('WordCloud');
                    break;
                case 6: console.log('Radar');
                    break;
                case 7: console.log('Area');
                    break;
                case 8: console.log('Node');
                    break;
                case 9: console.log('Map');
                    break;
            }
        };

        getTopTrending = function () {
            var tmp = "";
            tmp = $.ajax({
                async: false,
                type: 'GET',
                dataType: 'json',
                url: "https://localhost:44342/api/item/getMostPopularPerson/",
                headers: { 'x-api-key': '303d22a4-402b-4d3c-b279-9e81c0480711' }
            }).responseJSON

            return tmp;
        };

        drawLineChart = function (can) {
            var persons = "";
            persons = $.ajax({
                async: false,
                type: 'GET',
                dataType: 'json',
                url: "https://localhost:44342/api/item/GetMostPopularPersons",
                headers: { 'x-api-key': '303d22a4-402b-4d3c-b279-9e81c0480711' }
            }).responseJSON
            //can.attr('height', '300px');

            var datagrafiek = {
                labels: ["Dag 1", "Dag 2", "Dag 3", "Dag 4", "Dag 5", "Dag 6", "Dag 7", "Dag 8", "Dag 9", "Dag 10"],
                datasets: []
            };

            // var labelslist; 

            var myLineChart = new Chart(can, {
                type: 'line',
                data: datagrafiek,

            });

            var counter = 0;
            $.each(persons, function (key, value) {
                var tweet = "";
                tweet = $.ajax({
                    async: false,
                    type: 'GET',
                    dataType: 'json',
                    url: "https://localhost:44342/api/item/GetPersonEvolution/" + key,
                    headers: { 'x-api-key': '303d22a4-402b-4d3c-b279-9e81c0480711' }
                }).responseJSON

                var keys = [];
                keys = Object.keys(tweet);

                var label = [];
                var values = [];

                var backgroundcolors = ["rgba(237, 231, 246, 0.5)", "rgba(232, 245, 233, 0.5)", "rgba(3, 169, 244, 0.5)"];
                var borderColors = ["#673ab7", "#2196f3", "#4caf50"]
                var points = ["#512da8", "#1976d2", "#388e3c"];

                for (var i = 0; i < keys.length; i++) {
                    label.push(keys[i]);
                    values.push(tweet[keys[i]] / 10);
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
            });
        };

        drawDonut = function (can) {
            var count = "";
            count = $.ajax({
                async: false,
                type: 'GET',
                dataType: 'json',
                url: "https://localhost:44342/api/item/GetTrendingHashtagsCount/" + getTopTrending(),
                headers: Headers
            }).responseJSON

            var keys = [];
            keys = Object.keys(count);
            var label = [];
            var values = [];

            for (var i = 0; i < keys.length; i++) {
                label.push(keys[i]);
                values.push(count[keys[i]] / 10);
            }

            new Chart(can, {
                type: 'doughnut',
                data: {
                    labels: label,
                    datasets: [
                        {
                            label: "Hashtags",
                            data: values,
                            backgroundColor: ["#ff6384", "#36a2eb", "#cc65fe", "#ffce56", "#7DDF64", "#7BDFF2", "#FDE74C", "#47E5BC", "#DAD6D6", "#EF3E36"]
                        }
                    ]
                },
                options: {

                }
            });
        };

        drawPie = function (id, canvas) {
            var count = "";
            count = $.ajax({
                async: false,
                type: 'GET',
                dataType: 'json',
                url: "https://localhost:44342/api/item/GetTrendingMentionsCount/" + id,
                headers: Headers
            }).responseJSON

            var keys = [];
            keys = Object.keys(count);
            var label = [];
            var values = [];

            for (var i = 0; i < keys.length; i++) {
                label.push(keys[i]);
                values.push(count[keys[i]] / 10);
            }

            //var can = $('#pie-chart');
            //console.log(can.attr('Style', 'height: 50%;'));
            //can.attr('Style', 'width: 300px; height: 300px');

            new Chart(canvas, {
                type: 'pie',
                data: {
                    labels: label,
                    datasets: [{
                        backgroundColor: ["#ff6384", "#36a2eb", "#cc65fe", "#ffce56", "#7DDF64"],
                        data: values
                    }]
                },
                options: {
                    responsive: true
                }
            });
        };

        drawBarChart = function (can, id) {
            var bar = "";
            bar = $.ajax({
                async: false,
                type: 'GET',
                dataType: 'json',
                url: "https://localhost:44342/api/item/getpersontweet/" + id,
                headers: { 'x-api-key': '303d22a4-402b-4d3c-b279-9e81c0480711' }

            }).responseJSON

            var keys = [];
            keys = Object.keys(bar);
            var label = [];
            var values = [];

            for (var i = 0; i < keys.length; i++) {
                label.push(keys[i]);
                values.push(bar[keys[i]]);
            }

            //console.log(label);
            //console.log(values);

            //var can = $('#bar2-chart');
            //can.attr('width', '300px');
            //can.attr('height', '300px');

            new Chart(can, {
                type: 'bar',
                data: {
                    labels: label,
                    datasets: [
                        {
                            label: "Aantal tweets",
                            backgroundColor: "#03a9f4",
                            borderColor: "#rgba(3, 169, 244, 0.5)",
                            data: values
                        }
                    ]
                },
                options: {
                    legend: { display: true },
                    responsive: true,
                    scales: {
                        xAxes: [
                            {
                                display: false
                            }
                        ]
                    }

                }
            });
        };

        $(".grid-stack").each(function () {
            this.grid = $(this).data('gridstack');

            loadGrid($(this));

            $(this).parent().siblings('h4').children('.delete-zone').on('click', function () {
                removeZone($(this).parent().parent());
            });

            $(this).parent().siblings('h4').children('.edit-zone').on('click', function () {
                editZone($(this));
            });

            $(this).on('change', function (e, items) {
                changeElements(items, $(this));
            });
        });
    };
});