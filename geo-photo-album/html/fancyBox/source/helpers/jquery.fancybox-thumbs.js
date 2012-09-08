 /*!
 * Thumbnail helper for fancyBox
 * version: 1.0.6
 * @requires fancyBox v2.0 or later
 *
 * Usage:
 *     $(".fancybox").fancybox({
 *         helpers : {
 *             thumbs: {
 *                 width  : 50,
 *                 height : 50
 *             }
 *         }
 *     });
 *
 * Options:
 *     width - thumbnail width
 *     height - thumbnail height
 *     source - function to obtain the URL of the thumbnail image
 *     position - 'top' or 'bottom'
 *
 */
(function ($) {
  //Shortcut for fancyBox object
  var F = $.fancybox;

  //Add helper object
  F.helpers.thumbs = {
    wrap  : null,
    list  : null,
    width : 0,

    //Default function to obtain the URL of the thumbnail image
    source: function ( item ) {
      var href;

      if (item.element) {
        href = $(item.element).find('img').attr('src');
      }

      if (!href && item.type === 'image' && item.href) {
        href = item.href;
      }

      return href;
    },

    init: function (opts, obj) {
      var that = this,
        thumbWidth  = opts.width  || 50,
        thumbHeight = opts.height || 50,
        thumbSource = opts.source || this.source;

      this.wrap = $('<div id="fancybox-thumbs"></div>').addClass(opts.position || 'bottom').appendTo('body');
      this.list = $('<ul></ul>').appendTo(this.wrap);
      var n = 0;
      do {
        var new_li = $('<li><a style="width:' + thumbWidth + 'px;height:' + thumbHeight + 'px;" href="javascript:jQuery.fancybox.jumpto(' + n + ');"></a></li>')
        new_li.appendTo(this.list);
        var href = thumbSource( obj.group[ n ] );

        if (!href) {
          continue;
        }

        $("<img />").load({index: n}, function (e) {
          var width  = this.width,
            height = this.height,
            widthRatio, heightRatio, parent;

          if (!that.list || !width || !height) {
            return;
          }

          //Calculate thumbnail width/height and center it
          widthRatio  = width / thumbWidth;
          heightRatio = height / thumbHeight;

          parent = that.list.children().eq(e.data.index).find('a');

          if (widthRatio >= 1 && heightRatio >= 1) {
            if (widthRatio > heightRatio) {
              width  = Math.floor(width / heightRatio);
              height = thumbHeight;

            } else {
              width  = thumbWidth;
              height = Math.floor(height / widthRatio);
            }
          }

          $(this).css({
            width  : width,
            height : height,
            top    : Math.floor(thumbHeight / 2 - height / 2),
            left   : Math.floor(thumbWidth / 2 - width / 2)
          });

          parent.width(thumbWidth).height(thumbHeight);

          $(this).hide().appendTo(parent).fadeIn(300);

        }).attr('src', href);
        this.width = this.list.children().eq(0).outerWidth(true);
        n++;
      } while(n < obj.group.length && (n-obj.index)*this.width < this.wrap.parent().width()*2);

      this.list.width(this.width * (n + 1)).css('left', Math.floor(this.wrap.parent().width() * 0.5 - (this.width * 0.5)));
    },

    beforeLoad: function (opts, obj) {
      //Remove self if gallery do not have at least two items
      if (obj.group.length < 2) {
        obj.helpers.thumbs = false;

        return;
      }

      //Increase bottom margin to give space for thumbs
      obj.margin[ opts.position === 'top' ? 0 : 2 ] += ((opts.height || 50) + 15);
    },

    afterShow: function (opts, obj) {
      //Check if exists and create or update list
      if (this.list) {
        this.onUpdate(opts, obj);

      } else {
        this.init(opts, obj);
      }

      //Set active element
      this.list.children().removeClass('active').eq(obj.index).addClass('active');
    },

    //Center list
    onUpdate: function (opts, obj) {
      var that = this,
        thumbWidth  = opts.width  || 50,
        thumbHeight = opts.height || 50,
        thumbSource = opts.source || this.source;
      if (this.list) {
        var n = this.list.children().length;
        this.list.stop(true).animate({
          'left': Math.floor(this.wrap.parent().width() * 0.5 - (obj.index * this.width + this.width * 0.5))
        });
        while(n < obj.group.length && (n-obj.index)*this.width < this.wrap.parent().width()) {
          var new_li = $('<li><a style="width:' + thumbWidth + 'px;height:' + thumbHeight + 'px;" href="javascript:jQuery.fancybox.jumpto(' + n + ');"></a></li>')
          new_li.appendTo(this.list);
          var href = thumbSource( obj.group[ n ] );

          if (!href) {
            continue;
          }

          $("<img />").load({index: n}, function (e) {
            var width  = this.width,
              height = this.height,
              widthRatio, heightRatio, parent;

            if (!that.list || !width || !height) {
              return;
            }

            //Calculate thumbnail width/height and center it
            widthRatio  = width / thumbWidth;
            heightRatio = height / thumbHeight;

            parent = that.list.children().eq(e.data.index).find('a');

            if (widthRatio >= 1 && heightRatio >= 1) {
              if (widthRatio > heightRatio) {
                width  = Math.floor(width / heightRatio);
                height = thumbHeight;

              } else {
                width  = thumbWidth;
                height = Math.floor(height / widthRatio);
              }
            }

            $(this).css({
              width  : width,
              height : height,
              top    : Math.floor(thumbHeight / 2 - height / 2),
              left   : Math.floor(thumbWidth / 2 - width / 2)
            });

            parent.width(thumbWidth).height(thumbHeight);

            $(this).hide().appendTo(parent).fadeIn(300);

          }).attr('src', href);
          n++;
        }
      this.list.width(this.width * (n + 1)).css('left', Math.floor(this.wrap.parent().width() * 0.5 - (obj.index * this.width + this.width * 0.5)));
      }
    },

    beforeClose: function () {
      if (this.wrap) {
        this.wrap.remove();
      }

      this.wrap  = null;
      this.list  = null;
      this.width = 0;
    }
  }

}(jQuery));