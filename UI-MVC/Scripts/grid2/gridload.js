$(function () {
  var options = {
    float: true,
    width: 4,
    height: 4,
    animate: true,
    always_show_resize_handle: true,
    cellHeight: 110,
    verticalMargin: 18,
    horizontalMargin: 9,
    placeholder_class: 'grid-stack-placeholder',
    acceptWidgets: '.grid-stack-item'
  };

  $('.grid-stack').gridstack(_.defaults(options));

  var items = [{
    x: 0,
    y: 0,
    width: 1,
    height: 1
  }, {
    x: 1,
    y: 0,
    width: 1,
    height: 1
  }, {
    x: 2,
    y: 0,
    width: 1,
    height: 1
  }, {
    x: 0,
    y: 1,
    width: 1,
    height: 1
  }, {
    x: 3,
    y: 1,
    width: 1,
    height: 1
  }, {
    x: 1,
    y: 2,
    width: 1,
    height: 1
  }];

  $('.grid-stack').each(function () {
    var grid = $(this).data('gridstack');

    _.each(items, function (node) {
      grid.addWidget($('<div><div class="grid-stack-item-content" /><div/>'),
        node.x, node.y, node.width, node.height);
    }, this);
  });

  var sidebar_options = {
    float: true,
    width: 1,
    cellHeight: 110,
    verticalMargin: 18,
    horizontalMargin: 9,
    placeholder_class: 'grid-stack-placeholder',
  };

  $('.sidebar').gridstack(_.defaults(sidebar_options));

  var droppables = [{
    x: 0,
    y: 0,
    width: 1,
    height: 1
  }];

  $('.sidebar').each(function () {
    var sidebar = $(this).data('gridstack');

    _.each(droppables, function (node) {
      sidebar.addWidget($('<div><div class="grid-stack-item-content">I\'m new</div></div>'),
        node.x, node.y, node.width, node.height);
    }, this);
  });
});