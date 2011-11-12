var jquerycssmenu = {
    fadesettings: { overduration: 600, outduration: 600 }, // 600 - повільно, 200 - швидко

    buildmenu: function (menuid, arrowsvar) {
        jQuery(document).ready(function ($) {
            var $mainmenu = $("#" + menuid + ">ul");
            var $headers = $mainmenu.find("ul").parent();
            $headers.each(function (i) {
                var $curobj = $(this);
                var $subul = $(this).find('ul:eq(0)');

                this._dimensions = {
                    w: this.offsetWidth,
                    h: this.offsetHeight-8,
                    subulw: $subul.outerWidth(),
                    subulh: $subul.outerHeight()
                }
                this.istopheader = ($curobj.parents("ul").length == 1) ? true : false;
                $subul.css({ top: this.istopheader ? this._dimensions.h + 3 + "px" : 0 });

                $curobj
					.children("a:eq(0)").css(this.istopheader ? { paddingRight: arrowsvar.down[2]} : {})
					.append('<img align="absmiddle" alt="" src="' + (this.istopheader ? arrowsvar.down[1] : arrowsvar.right[1]) + '" class="' + (this.istopheader ? arrowsvar.down[0] : arrowsvar.right[0]) + '" border="" />');
                $curobj.hover(
					function (e) {
					    var $targetul = $(this).children("ul:eq(0)");
					    this._offsets = {
					        left: $(this).offset().left,
					        top: $(this).offset().top
					    };
					    var menuleft = this.istopheader ? 0 : this._dimensions.w;
					    menuleft = (this._offsets.left + menuleft + this._dimensions.subulw > $(window).width()) ? (this.istopheader ? -this._dimensions.subulw + this._dimensions.w : -this._dimensions.w) : menuleft;
					    $targetul.css({ left: menuleft + "px" }).fadeIn(jquerycssmenu.fadesettings.overduration);					
					},
					function (e) {
					    $(this).children("ul:eq(0)").fadeOut(jquerycssmenu.fadesettings.outduration);
					}
				)
            });

            $mainmenu.find("ul").css({ display: 'none', visibility: 'visible' });
        });
    }
}

var arrowimages = {
    down: ['downarrowclass', "http://" + document.location.host + "/Content/img/navigation/arrow_down.png", 25],
    right: ['rightarrowclass', "http://" + document.location.host + "/Content/img/navigation/arrow_right.png"]
        }

jquerycssmenu.buildmenu("myjquerymenu", arrowimages)