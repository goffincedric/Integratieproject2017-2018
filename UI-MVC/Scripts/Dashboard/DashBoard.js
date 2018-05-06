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

                console.log(ZoneId);

                $('.add-zone').remove();

                //zorgen dat pagina niet herladen moet worden
                var $newdiv = $('<div class="p-10 mB-10 zone-'+ZoneId+'"></div>');
                $newdiv.append($('<h4 class="bb-2"></h4>'));
                $newdiv.children('h4').append($('<span class="title">New Zone</span>'));
                $newdiv.children('h4').append($('<span class="edit-zone"></span>'));
                $newdiv.children('h4').children('.edit-zone').append($('<i class="ti-pencil"></i>'));
                $newdiv.children('h4').append($('<span class="arrow-dashboard"></span>'));
                $newdiv.children('h4').children('.arrow-dashboard').append($('<i class="ti-angle-up"></i>'));
                $newdiv.children('h4').append($('<span class="delete-zone"></span>'));
                $newdiv.children('h4').children('.delete-zone').append($('<i class="ti-trash"></i>'));
                $newdiv.append($('<div class="DashZone"></div>'));
                $newdiv.append($('<div class="grid-stack grid-stack-12" id="zone-'+ZoneId+'"></div >'));
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
                var newElement = grid.data('gridstack').addWidget($('<div><div class="grid-stack-item-content bgc-white bd" id="Element-x"/><div/>'), 0, 0, 3, 3, true);
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

            addingElement = false;
        }

        resizeInput = function (event, titleTag) {

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

                console.log("press");

                titleTag.html($(this).val());

                grid.parent().append(titleTag);

                var inputwidth = titleTag.width();

                titleTag.remove();

                input.width(inputwidth + 10);

                if (e.which == 13) {
                    newTitle = $(this).val();

                    Zone = JSON.parse('{ "Title": "'+ newTitle +'", "ZoneId": "'+ ZoneId +'"}');

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
            //console.log(grid);

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

        addZone = function (grid) {

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
                        griddata.addWidget($('<div><div class="grid-stack-item-content bgc-white bd" id="Element-' + node.ElementId + '"/><div/>'),
                            node.X, node.Y, node.Width, node.Height);
                    }, this);

                    var plusElement = griddata.addWidget($('<div><div class="grid-stack-item-content bgc-white bd" id ="Element-+"><div><img class="w-3r bdrs-50p alert-img add-element" src="/Content/Images/plus-icon.png"><div/><div/><div/>'), 0, 0, 3, 3, true);

                    plusElement.resizable(false);

                    plusElement.children('.grid-stack-item-content').children('div').children('img').on("click", function () {
                        addElement(grid);
                    });
                });
            }
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