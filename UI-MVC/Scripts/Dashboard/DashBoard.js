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
        var testwizard = $('#wizard');

        console.log(testwizard);

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
            console.log(grid);

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
                $newdiv.children('.DashZone').append($('<div class="grid-stack grid-stack-12" id="zone-' + ZoneId + '"></div >'));
                var newZone = $('#mainContent').append($newdiv);

                addZone = $('#mainContent').append(addZone);

                $('.grid-stack').gridstack(options);

                grid = newZone.children('.zone-' + ZoneId).children('.DashZone').children('#zone-' + ZoneId);

                var hidezone = grid.parent().parent().children('h4').children('.arrow-dashboard');

                addZone.children('.add-zone').children('h4').children('.arrow-dashboard').on('click', function () {
                    $(this).parent().parent().children(".DashZone").toggle(300);
                    rotation = getRotationDegrees($(this)) + 180;
                    $(this).css({ 'transform': 'rotate(' + rotation + 'deg)' });
                });

                var deletezone = grid.parent().parent().children('h4').children('.delete-zone');

                var editzone = grid.parent().parent().children('h4').children('.edit-zone');

                hidezone.on('click', function () {
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

                addZone = addZone.children('.add-zone').children('div').children('#zone-x');
                var plusElement = addZone.data('gridstack').addWidget($('<div><div class="grid-stack-item-content bgc-white bd" id ="Element-+"><div><img class="w-3r bdrs-50p alert-img add-element" src="/Content/Images/plus-icon.png"><div/><div/><div/>'), 0, 0, 3, 3, true);

                plusElement.children('.grid-stack-item-content').children('div').children('img').on("click", function () {
                    addElement(addZone);
                });
            };

            var newElement = grid.data('gridstack').addWidget($('<div><div class="grid-stack-item-content bgc-white bd" id="Element-x"><i class="ti-trash float-right mR-15 mT-15 delete-element"></i><canvas></canvas><div/><div/>'), 0, 0, 3, 3, true);
            var X = newElement.data().gsX;
            var Y = newElement.data().gsY;

            var Element = JSON.parse('{"X" : "' + X + '", "Y" : "' + Y + '", "Width": "3", "Height": "3", "IsDraggable": "true","IsUnfinished":"true", "ZoneId": "' + ZoneId + '", "GraphType": "6"}');

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
            var DataType = null;

            //$('#next-1').attr('disabled', 'disabled');

            //var onderwerpRadio = [$('#person'), $('#organisation'), $('#theme')];

            $('.btn-next').on('click', function () {
                switch ($(this).attr('id')) {
                    case "next-1":
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
                        var dropdown = $('#locality-dropdown');

                        itemData = {
                            items: []
                        };

                        $('#output').off('change');
                        $('#output').on('change', function () {
                            output = $('#output option:selected').text();
                        });

                        dropdown.empty();

                        dropdown.append('<option selected="true" disabled>selecteer een ' + onderwerp + '</option>');
                        dropdown.prop('selectedIndex', 0);

                        // Populate dropdown with list of data
                        $.each(data, function (key, entry) {
                            dropdown.append($('<option></option>').attr('value', entry.ItemId).text(entry.Name));
                        });

                        var cardnumber = 1;
                        $('#locality-dropdown').off('change');
                        $('#locality-dropdown').on('change', function () {
                            if ($('#card' + (cardnumber - 1)).children('.card').children('.container').children('h4').text() !== '#') {
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
                            }
                        });
                        $(this).attr('id', 'next-2');
                        break;
                    case "next-2":
                        $('#cardholder').empty();
                        switch ($('input:checked').attr('id')) {
                            case 'mention':
                                DataType = 0;
                                $('#bar').parent().parent().show();
                                $('#pie').parent().parent().show();
                                $('#donut').parent().parent().show();
                                $('#word').parent().parent().show();
                                $('#line').parent().parent().hide();
                                $('#map').parent().parent().hide();
                                break;
                            case 'hashtag':
                                DataType = 1;
                                $('#bar').parent().parent().show();
                                $('#pie').parent().parent().show();
                                $('#donut').parent().parent().show();
                                $('#word').parent().parent().show();
                                $('#line').parent().parent().hide();
                                $('#map').parent().parent().hide();
                                break;
                            case 'evolution':
                                DataType = 2;
                                $('#bar').parent().parent().show();
                                $('#pie').parent().parent().hide();
                                $('#donut').parent().parent().hide();
                                $('#word').parent().parent().hide();
                                $('#line').parent().parent().show();
                                $('#map').parent().parent().hide();
                                break;
                            case 'sentiment':
                                DataType = 3;
                                $('#bar').parent().parent().show();
                                $('#pie').parent().parent().hide();
                                $('#donut').parent().parent().hide();
                                $('#word').parent().parent().hide();
                                $('#line').parent().parent().show();
                                $('#map').parent().parent().hide();
                                break;
                            case 'age':
                                DataType = 4;
                                $('#bar').parent().parent().show();
                                $('#pie').parent().parent().show();
                                $('#donut').parent().parent().show();
                                $('#word').parent().parent().hide();
                                $('#line').parent().parent().hide();
                                $('#map').parent().parent().hide();
                                break;
                            case 'gender':
                                DataType = 5;
                                $('#bar').parent().parent().show();
                                $('#pie').parent().parent().show();
                                $('#donut').parent().parent().show();
                                $('#word').parent().parent().hide();
                                $('#line').parent().parent().hide();
                                $('#map').parent().parent().hide();
                                break;
                            case 'words':
                                DataType = 6;
                                $('#bar').parent().parent().show();
                                $('#pie').parent().parent().show();
                                $('#donut').parent().parent().show();
                                $('#word').parent().parent().show();
                                $('#line').parent().parent().hide();
                                $('#map').parent().parent().hide();
                                break;
                            case 'location':
                                DataType = 7;
                                $('#bar').parent().parent().show();
                                $('#pie').parent().parent().show();
                                $('#donut').parent().parent().show();
                                $('#word').parent().parent().show();
                                $('#line').parent().parent().hide();
                                $('#map').parent().parent().show();
                                break;
                        }
                        $(this).attr('id', 'next-3');
                        $('#previous-1').attr('id', 'previous-2');
                        break;
                    case "next-3":
                        $(this).attr('id', 'next-4');
                        $('#previous-2').attr('id', 'previous-3');
                        break;
                };
            });

            $('.btn-previous').on('click', function () {
                switch ($(this).attr('id')) {
                    case 'previous-1':
                        $('#next-2').attr('id', 'next-1');
                        break;
                    case 'previous-2':
                        $(this).attr('id', 'previous-1');
                        $('#next-3').attr('id', 'next-2');
                        break;
                    case 'previous-3':
                        $(this).attr('id', 'previous-2');
                        $('#next-4').attr('id', 'next-3');
                        break;
                };
            });

            $('.btn-finish').off('click');
            $('.btn-finish').on('click', function () {
                var GraphType = 0;
                $('input:checked').attr('id');
                switch ($('input:checked').attr('id')) {
                    case 'bar': GraphType = 0;
                        break;
                    case 'line': GraphType = 1;
                        break;
                    case 'pie': GraphType = 2;
                        break;
                    case 'donut': GraphType = 3;
                        break;
                    case 'word': GraphType = 4;
                        break;
                    case 'map': GraphType = 5;
                        break;
                }
                Wizard.hide();

                //edit later
                var items = getItems($('#cardholder'));

                var itemsJSON = JSON.stringify(items);

                var Element = JSON.parse('{"ElementId":"' + ElementId + '", "X" : "' + newElement.data().gsX + '", "Y" : "' + newElement.data().gsY + '", "Width": "' + newElement.data().gsWidth + '", "Height": "' + newElement.data().gsHeight + '", "IsDraggable": "' + true + '", "ZoneId": "' + ZoneId + '", "GraphType" : "' + GraphType + '","DataType":"' + DataType + '", "IsUnfinished":"false", "Items": ' + itemsJSON + '}');

                $.ajax({
                    async: false,
                    type: 'PUT',
                    data: Element,
                    dataType: 'json',
                    headers: Headers,
                    url: "https://localhost:44342/api/dashboard/putelement/" + ElementId
                })

                var ItemIds = []

                _.each(Element.Items, function (item) {
                    ItemIds.push(item.ItemId);
                });

                console.log(ItemIds);

                chooseChart(GraphType, newElement.children('#Element-' + ElementId).children('canvas'), ItemIds, DataType);
            });
            addingElement = false;
        };

        editZone = function (grid) {
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

                            addZone.children('.add-zone').children('h4').children('.arrow-dashboard').on('click', function () {
                                $(this).parent().parent().children(".DashZone").toggle(300);
                                rotation = getRotationDegrees($(this)) + 180;
                                $(this).css({ 'transform': 'rotate(' + rotation + 'deg)' });
                            });

                            $('.grid-stack').gridstack(options);

                            grid.on('change', function (e, items) {
                                changeElements(items, $(this));
                            });

                            addZone = addZone.children('.add-zone').children('div').children('#zone-x');

                            grid = newZone.children('.zone-' + ZoneId).children('.DashZone').children('#zone-' + ZoneId);

                            addZone.data('gridstack').removeWidget($('#Element-' + ElementId).parent());

                            var deletezone = grid.parent().parent().children('h4').children('.delete-zone');

                            var editzone = grid.parent().parent().children('h4').children('.edit-zone');

                            var hidezone = grid.parent().parent().children('h4').children('.arrow-dashboard');

                            hidezone.on('click', function () {
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

                            //addelement = $(grid + ":nth-child(1)");
                            //console.log(addelement);

                            //addelement.on('click', function () {
                            //    console.log('adding element');
                            //});

                            $(".add-element").each(function () {
                                $(this).off('click');
                                $(this).on('click', function () {
                                    var zone = $(this)
                                    addElement(zone.parent().parent().parent().parent());
                                });
                            })

                            addZone.children().children('#Element-+').children().children('img').on('click', function () {
                                addElement(addZone);
                            });

                            var ItemIds = []

                            _.each(Element.Items, function (item) {
                                ItemIds.push(item.ItemId);
                            });

                            console.log(ItemIds);

                            chooseChart(Element.GraphType, $('#Element-' + ElementId).children('canvas'), ItemIds, Element.DataType);
                        }
                        var oldElement = $.ajax({
                            async: false,
                            type: 'GET',
                            headers: Headers,
                            url: "https://localhost:44342/api/dashboard/getelement/" + ElementId
                        }).responseJSON

                        var oldZoneId = oldElement.ZoneId;

                        Element = JSON.parse('{"ElementId":"' + ElementId + '", "X" : "' + element.x + '", "Y" : "' + element.y + '", "Width": "' + element.width + '", "Height": "' + element.height + '", "IsDraggable": "' + !element.noMove + '", "ZoneId": "' + ZoneId + '", "GraphType": "' + oldElement.GraphType + '", "DataType": "' + oldElement.DataType + '"}');

                        $.ajax({
                            async: false,
                            type: 'PUT',
                            data: Element,
                            dataType: 'json',
                            headers: Headers,
                            url: "https://localhost:44342/api/dashboard/putelement/" + ElementId
                        })

                        if (oldZoneId !== ZoneId) {
                            var ItemIds = []

                            _.each(oldElement.Items, function (item) {
                                ItemIds.push(item.ItemId);
                            });

                            console.log(ItemIds);

                            chooseChart(oldElement.GraphType, $('#Element-' + ElementId).children('canvas'), ItemIds, oldElement.DataType);
                        }
                    }
                }
            }
        };

        getItems = function (cards) {
            var items = [];

            $.each(cards.children(), function () {
                itemId = $(this).children('.card').children('.container').children('h4').children('.itemId').text();

                if (itemId !== '#') {
                    items.push($.ajax({
                        async: false,
                        type: 'GET',
                        headers: Headers,
                        url: "https://localhost:44342/api/item/getitem/" + itemId
                    }).responseJSON);
                }
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
                        if (!node.IsUnfinished) {
                            var newElement = griddata.addWidget($('<div><div class="grid-stack-item-content bgc-white bd" id="Element-' + node.ElementId + '"><i class="ti-trash float-right mR-15 mT-15 delete-element"></i><div/><div/>'),
                                node.X, node.Y, node.Width, node.Height);
                            newElement.children('#Element-' + node.ElementId).children('.delete-element').on('click', function () {
                                deleteElement($(this), grid);
                            });

                            newElement.children('#Element-' + node.ElementId).append($('<canvas></canvas>')).children('canvas').attr('id', 'pie-chart');

                            var ItemIds = []

                            _.each(node.Items, function (item) {
                                ItemIds.push(item.ItemId);
                            });

                            chooseChart(node.GraphType, newElement.children('#Element-' + node.ElementId).children('canvas'), ItemIds, node.DataType);
                        } else {
                            $.ajax({
                                async: false,
                                type: 'Delete',
                                headers: Headers,
                                url: "https://localhost:44342/api/dashboard/deleteelement/" + node.ElementId
                            })
                        }
                    }, this);

                    var plusElement = griddata.addWidget($('<div><div class="grid-stack-item-content bgc-white bd" id ="Element-+"><div><img class="w-3r bdrs-50p alert-img add-element" src="/Content/Images/plus-icon.png"><div/><div/><div/>'), 0, 0, 3, 3, true);

                    plusElement.children('.grid-stack-item-content').children('div').children('img').on("click", function () {
                        addElement(grid);
                    });
                });
            }
        };

        chooseChart = function (type, can, ids, data) {
            console.log(type);
            switch (data) {
                case 0: DrawMentions(can, id, type);
                    break;
                case 1: DrawHashtags(can, ids, type);
                    break;
                case 2: DrawEvolution(can, ids, type);
                    break;
                case 3: DrawSentiment(can, ids, type);
                    break;
                case 4: DrawAge(can, ids, type);
                    break;
                case 5: DrawGender(can, ids, type);
                    break;
                case 6: DrawWords(can, ids, type);
                    break;
                case 7: DrawLocation(can, ids, type);
                    break;
            }
        };

        function makeAjaxCall(url, methodType) {
            var promiseObj = new Promise(function (resolve) {
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
                            console.log("xhr failed");
                        }
                    } else {
                        console.log("xhr processing going on");
                    }
                }
                console.log("request sent succesfully");
            });
            return promiseObj;
        };

        DrawHashtags = function (can, itemIds, graph) {
            switch (graph) {
                case 0: graph = 'bar';
                    break;
                case 1: graph = 'line';
                    break;
                case 2: graph = 'pie';
                    break;
                case 3: graph = 'doughnut';
                    break;
                case 4: graph = 'wordcloud';
                    break;
                case 5: graph = 'map';
                    break;
            }
            if (itemIds.length == 1) {
                var URL = "https://localhost:44342/api/item/GetTrendingHashtagsCount/" + itemIds[0];
                makeAjaxCall(URL, "GET").then(process);

                function process(output) {

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
                            type: graph,
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
                }
            } else {
                console.log("TTTTTTTTTTTTTTTTTTTTTTTTTTTTTT");

                var datagrafiek = {
                    datasets: [],
                    labels: ["Tweets"]

                };
                var myChart = new Chart(can,
                    {
                        type: graph,
                        data: datagrafiek,
                        options: {
                            responsive: true,
                            legend: {
                                display: true
                            }
                        }
                    });

                var counter = 0;
                $.each(itemIds,
                    function (index, value) {
                        var URL = "https://localhost:44342/api/item/GetTrendingHashtagsCountOverall/" + value;
                        makeAjaxCall(URL, "GET").then(process);
                        function process(output2) {
                            var keys = [];
                            keys = Object.keys(output2);

                            var label = [];
                            var values = [];
                            for (var i = 0; i < keys.length; i++) {
                                label.push(keys[i]);
                                values.push(output2[keys[i]]);
                            }
                            console.log("Values: " + values);
                            var backgroundColors = ["#36a2eb", "#ffce56", "#7DDF64", "#ff6384", "#36a2eb", "#cc65fe"];
                            var myNewDataSet = {
                                label: label,
                                data: values,
                                backgroundColor: backgroundColors[counter]
                            }
                            datagrafiek.datasets.push(myNewDataSet);
                            myChart.update();
                            counter++;
                        }
                    });

            }
        };

        DrawMentions = function (can, itemIds, graph) {
            switch (graph) {
                case 0: graph = 'bar';
                    break;
                case 1: graph = 'line';
                    break;
                case 2: graph = 'pie';
                    break;
                case 3: graph = 'doughnut';
                    break;
                case 4: graph = 'wordcloud';
                    break;
                case 5: graph = 'map';
                    break;
            }
            if (itemIds.length == 1) {
                var URL = "https://localhost:44342/api/item/GetTrendingMentionsCount/" + itemIds[0];
                makeAjaxCall(URL, "GET").then(process);

                function process(output) {

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
                            type: graph,
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

                }
            } else {
                console.log("TTTTTTTTTTTTTTTTTTTTTTTTTTTTTT");

                var datagrafiek = {
                    datasets: [],
                    labels: ["Tweets"]
                };
                var myChart = new Chart(can,
                    {
                        type: graph,
                        data: datagrafiek,
                        options: {
                            responsive: true,
                            legend: {
                                display: true
                            }
                        }
                    });

                var counter = 0;
                $.each(itemIds,
                    function (index, value) {
                        var URL = "https://localhost:44342/api/item/GetTrendingMentionsCountOverall/" + value;
                        makeAjaxCall(URL, "GET").then(process);

                        function process(output2) {
                            var keys = [];
                            keys = Object.keys(output2);
                            var label = [];
                            var values = [];
                            for (var i = 0; i < keys.length; i++) {
                                label.push(keys[i]);
                                values.push(output2[keys[i]]);
                            }
                            console.log("Values: " + values);
                            var backgroundColors = ["#36a2eb", "#ffce56", "#7DDF64", "#ff6384", "#36a2eb", "#cc65fe"];
                            var myNewDataSet = {
                                label: label,
                                data: values,
                                backgroundColor: backgroundColors[counter]
                            }
                            datagrafiek.datasets.push(myNewDataSet);
                            myChart.update();
                            counter++;
                        }
                    });

            }
        };

        DrawWords = function (can, itemIds, graph) {
            switch (graph) {
                case 0: graph = 'bar';
                    break;
                case 1: graph = 'line';
                    break;
                case 2: graph = 'pie';
                    break;
                case 3: graph = 'doughnut';
                    break;
                case 4: graph = 'wordcloud';
                    break;
                case 5: graph = 'map';
                    break;
            }
            if (itemIds.length == 1) {
                var URL = "https://localhost:44342/api/item/GetTrendingWordsCount/" + itemIds[0];
                makeAjaxCall(URL, "GET").then(process);

                function process(output) {

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
                            type: graph,
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
                }
            } else {
                console.log("TTTTTTTTTTTTTTTTTTTTTTTTTTTTTT");

                var datagrafiek = {
                    datasets: [],
                    labels: ["Tweets"]
                };
                var myChart = new Chart(can,
                    {
                        type: graph,
                        data: datagrafiek,
                        options: {
                            responsive: true,
                            legend: {
                                display: true
                            }
                        }
                    });

                var counter = 0;
                $.each(itemIds,
                    function (index, value) {
                        var URL = "https://localhost:44342/api/item/GetTrendingWordsCountOverall/" + value;
                        makeAjaxCall(URL, "GET").then(process);
                        function process(output2) {
                            var keys = [];
                            keys = Object.keys(output2);
                            var label = [];
                            var values = [];
                            for (var i = 0; i < keys.length; i++) {
                                label.push(keys[i]);
                                values.push(output2[keys[i]]);
                            }
                            console.log("Values: " + values);
                            var backgroundColors = ["#36a2eb", "#ffce56", "#7DDF64", "#ff6384", "#36a2eb", "#cc65fe"];
                            var myNewDataSet = {
                                label: label,
                                data: values,
                                backgroundColor: backgroundColors[counter]
                            }
                            datagrafiek.datasets.push(myNewDataSet);
                            myChart.update();
                            counter++;
                        }
                    });
            }
        };

        DrawAge = function (can, itemIds, graph) {
            console.log(graph);
            switch (graph) {
                case 0: graph = 'bar';
                    break;
                case 1: graph = 'line';
                    break;
                case 2: graph = 'pie';
                    break;
                case 3: graph = 'doughnut';
                    break;
                case 4: graph = 'wordcloud';
                    break;
                case 5: graph = 'map';
                    break;
            }
            //var can = $('#pie');
            var datagrafiek = {
                datasets: [],
                labels: ["25+", "25-", "onbekend"]
            };

            var myChart = new Chart(can,
                {
                    type: graph,
                    data: datagrafiek,
                    options: {
                        responsive: true,
                        title: { display: true, text: 'Leeftijds vergelijking' }
                    }

                });

            var counter = 0;
            $.each(itemIds,
                function (index, value) {
                    var URL = "https://localhost:44342/api/item/GetAges/" + value;
                    makeAjaxCall(URL, "GET").then(process);

                    function process(output2) {
                        var keys = [];
                        keys = Object.keys(output2);

                        var label = [];
                        var values = [];

                        for (var i = 0; i < keys.length; i++) {
                            label.push(keys[i]);
                            values.push(output2[keys[i]]);
                        }
                        var myNewDataSet = {
                            label: value,
                            data: values,
                            backgroundColor: ["#36a2eb", "#ffce56", "#7DDF64", "#ff6384", "#36a2eb", "#cc65fe"]
                        }

                        datagrafiek.datasets.push(myNewDataSet);
                        myChart.update();
                        counter++;
                    }
                });
        }

        DrawGender = function (can, itemIds, graph) {
            switch (graph) {
                case 0: graph = 'bar';
                    break;
                case 1: graph = 'line';
                    break;
                case 2: graph = 'pie';
                    break;
                case 3: graph = 'doughnut';
                    break;
                case 4: graph = 'wordcloud';
                    break;
                case 5: graph = 'map';
                    break;
            }
            //var can = $('#pie');
            var datagrafiek = {
                datasets: [],
                labels: ["♂ man", "♀ vrouw"]
            };

            var myChart = new Chart(can,
                {
                    type: graph,
                    data: datagrafiek,
                    options: {
                        responsive: true,
                        title: { display: true, text: 'Geslachts vergelijking' }
                    }
                });

            var counter = 0;
            $.each(itemIds,
                function (index, value) {
                    var URL = "https://localhost:44342/api/item/GetGender/" + value;
                    makeAjaxCall(URL, "GET").then(process);

                    function process(output2) {
                        var keys = [];
                        keys = Object.keys(output2);

                        var label = [];
                        var values = [];

                        for (var i = 0; i < keys.length; i++) {
                            label.push(keys[i]);
                            values.push(output2[keys[i]]);
                        }
                        var myNewDataSet = {
                            label: value,
                            data: values,
                            backgroundColor: ["#36a2eb", "#ff99ed"]
                        }
                        datagrafiek.datasets.push(myNewDataSet);
                        myChart.update();
                        counter++;
                    }
                });
        }

        DrawSentiment = function (can, itemIds, graph) {
            switch (graph) {
                case 0: graph = 'bar';
                    break;
                case 1: graph = 'line';
                    break;
                case 2: graph = 'pie';
                    break;
                case 3: graph = 'doughnut';
                    break;
                case 4: graph = 'wordcloud';
                    break;
                case 5: graph = 'map';
                    break;
            }
            var datagrafiek = {
                labels: ["Dag 1", "Dag 2", "Dag 3", "Dag 4", "Dag 5", "Dag 6", "Dag 7", "Dag 8", "Dag 9", "Dag 10"],
                datasets: []
            };
            var myLineChart = new Chart(can,
                {
                    type: graph,
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
                        },
                        title: { display: true, text: 'Sentiment vergelijking' }
                    }
                });

            var counter = 0;
            $.each(itemIds,
                function (index, value) {
                    var URL = "https://localhost:44342/api/item/GetPersonEvolution/" + value;
                    makeAjaxCall(URL, "GET").then(process);

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
        }

        DrawLocation = function (can, itemIds, graph) {
            switch (graph) {
                case 0: graph = 'bar';
                    break;
                case 1: graph = 'line';
                    break;
                case 2: graph = 'pie';
                    break;
                case 3: graph = 'doughnut';
                    break;
                case 4: graph = 'wordcloud';
                    break;
                case 5: graph = 'map';
                    break;
            }

            if (graph === 'map') {
                var div = can.parent();
                can.replaceWith('<h4>Hoeveelheid tweets per provincie</h4><div class="map" style="height: 100%"></div>');
                div = div.children(".map");

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

                    div.vectorMap({
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
                        zoomOnScroll: false,
                        zoomButtons: false,
                        normalizeFunction: "linear",
                        onRegionTipShow: function (e, el, code) {
                            el.html(el.html() + ' (Tweets - ' + mapData[code] + ')');
                        }
                    });
                }
            } else {
                //insert other graphs here
            }
        }

        DrawEvolution = function (can, itemIds, graph) {
            switch (graph) {
                case 0: graph = 'bar';
                    break;
                case 1: graph = 'line';
                    break;
                case 2: graph = 'pie';
                    break;
                case 3: graph = 'doughnut';
                    break;
                case 4: graph = 'wordcloud';
                    break;
                case 5: graph = 'map';
                    break;
            }
            var datagrafiek = {
                labels: ["Dag 1", "Dag 2", "Dag 3", "Dag 4", "Dag 5", "Dag 6", "Dag 7", "Dag 8", "Dag 9", "Dag 10"],
                datasets: []
            };
            var myLineChart = new Chart(can,
                {
                    type: graph,
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
                        },
                        title: { display: true, text: 'Sentiment vergelijking' }
                    }
                });

            var counter = 0;
            $.each(itemIds,
                function (index, value) {
                    var URL = "https://localhost:44342/api/item/GetItemTweet/" + value;
                    makeAjaxCall(URL, "GET").then(process);

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