﻿var DashBoardId = $('main').attr('id');
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
        }

        //done
        addElement = function (grid) {
            addingElement = true;

            var addZone = $('.add-zone');
            var ZoneId = grid.attr('id');

            ZoneId = ZoneId.substring(ZoneId.indexOf('-') + 1, ZoneId.length);

            var removeElement = grid.children()[grid.children().length - 1];

            grid.data('gridstack').removeWidget(removeElement);

            if (ZoneId == 'x') {
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
                $newdiv.children('h4').append($('<span class="title">New Zone</span>'));
                $newdiv.children('h4').append($('<span class="edit-zone"></span>'));
                $newdiv.children('h4').children('.edit-zone').append($('<i class="ti-pencil"></i>'));
                $newdiv.children('h4').append($('<span class="arrow-dashboard"></span>'));
                $newdiv.children('h4').children('.arrow-dashboard').append($('<i class="ti-angle-up"></i>'));
                $newdiv.children('h4').append($('<span class="delete-zone"></span>'));
                $newdiv.children('h4').children('.delete-zone').append($('<i class="ti-trash"></i>'));
                $newdiv.append($('<div class="DashZone"></div>'));
                $newdiv.append($('<div class="grid-stack grid-stack-12" id="zone-' + ZoneId + '"></div >'));
                var newZone = $('#mainContent').append($newdiv);

                var addZone = $('#mainContent').append(addZone);

                $('.grid-stack').gridstack(options);

                grid = newZone.children('.zone-' + ZoneId).children('#zone-' + ZoneId);

                var deletezone = grid.parent().children('h4').children('.delete-zone');

                deletezone.on('click', function () {
                    removeZone(grid.parent());
                });

                addZone = addZone.children('.add-zone').children('div').children('#zone-x');
                var plusElement = addZone.data('gridstack').addWidget($('<div><div class="grid-stack-item-content bgc-white bd" id ="Element-+"><div><img class="w-3r bdrs-50p alert-img add-element" src="/Content/Images/plus-icon.png"><div/><div/><div/>'), 0, 0, 3, 3, true);

                plusElement.children('.grid-stack-item-content').children('div').children('img').on("click", function () {
                    addElement(addZone);
                });
            }
            var newElement = grid.data('gridstack').addWidget($('<div><div class="grid-stack-item-content bgc-white bd" id="Element-x"><i class="ti-trash float-right mR-15 mT-15 delete-element"></i><div/><div/>'), 0, 0, 3, 3, true);
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

            $('.btn-finish').off('click');
            $('.btn-finish').on('click', function () {
                Wizard.hide();
                drawChart();
            });
            addingElement = false;
        }

        editZone = function (grid) {
            var ZoneId = grid.parent().parent().attr('class');

            ZoneId = ZoneId.substring(ZoneId.indexOf('zone-') + 5, ZoneId.length);

            titleTag = grid.siblings('.title');
            oldTitle = titleTag.html();

            titleWidth = titleTag.width();
            titleHeight = titleTag.height();

            titleTag.replaceWith($('<input type="text" name="title" style="width: ' + titleWidth + 'px; height:' + titleHeight + 'px;font-weight: bold; margin-right: 15px; max-width: 600px;">'));

            grid.siblings('input').focus();

            grid.siblings('input').val(oldTitle);

            grid.siblings('input').keypress(function (e) {
                var input = $(this);

                titleTag.html($(this).val());

                grid.parent().append(titleTag);

                var inputwidth = titleTag.width();

                titleTag.remove();

                input.width(inputwidth +10);

                if (e.which == 13) {
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
                }
            });
        }

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
        }

        changeElements = function (elements, grid) {
            if (!addingElement) {
                var ZoneId = grid.attr('id');
                ZoneId = ZoneId.substring(ZoneId.indexOf('-') + 1, ZoneId.length);
                var count = 0;
                for (var e in elements) {

                    var element = elements[e];

                    var ElementId = element.el.children('div').attr('id');
                    ElementId = ElementId.substring(ElementId.indexOf('-') + 1, ElementId.length);

                    if (ElementId != '+') {

                        var Element = JSON.parse('{"ElementId":"' + ElementId + '", "X" : "' + element.x + '", "Y" : "' + element.y + '", "Width": "' + element.width + '", "Height": "' + element.height + '", "IsDraggable": "' + !element.noMove + '", "ZoneId": "' + ZoneId + '"}');

                        $.ajax({
                            async: false,
                            type: 'PUT',
                            data: Element,
                            dataType: 'json',
                            headers: Headers,
                            url: "https://localhost:44342/api/dashboard/putelement/" + ElementId
                        })
                    }
                }
            }
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
            if (ZoneId == 'x') {
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

                        console.log(node.GraphType);

                        newElement.children('#Element-' + node.ElementId).append($('<canvas></canvas>')).children('canvas').attr('id', 'pie-chart');

                        drawPie(25, newElement.children('#Element-' + node.ElementId).children('canvas'));
                    }, this);

                    var plusElement = griddata.addWidget($('<div><div class="grid-stack-item-content bgc-white bd" id ="Element-+"><div><img class="w-3r bdrs-50p alert-img add-element" src="/Content/Images/plus-icon.png"><div/><div/><div/>'), 0, 0, 3, 3, true);

                    plusElement.children('.grid-stack-item-content').children('div').children('img').on("click", function () {
                        addElement(grid);
                    });
                });
            }
        }

        chooseChart = function (type, can) {
            switch (type) {
                case 0: console.log('Bar');
                    break;
                case 1: drawLineChart();
                    break;
                case 2: drawPie();
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
        }

        function getTopTrending() {
            var tmp = "";
            tmp = $.ajax({
                async: false,
                type: 'GET',
                dataType: 'json',
                url: "https://localhost:44342/api/item/getMostPopularPerson/",
                headers: { 'x-api-key': '303d22a4-402b-4d3c-b279-9e81c0480711' }
            }).responseJSON

            return tmp;
        }

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
        }

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

            //var can = $('#doughnut-chart');
            //console.log(can.parent().attr('Style', 'width: 400px; height: 350px'));
            //can.attr('Style', 'width: 300px; height: 300px');

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

        drawPie = function(id, canvas) {
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

            var can = $('#pie-chart');
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
        }

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