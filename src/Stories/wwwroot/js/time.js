(function ($, window, document) {
    "use strict";

    $(function () {
        Time.init();
    });
}(window.jQuery, window, document));


var Time = {
    /*
    settings: {}
    */

    init: function () {
        this.setTime($("time"));
    },

    setTime: function ($element) {
        if (!$element)
            return;

        $element.text(function () {
            var $this = $(this);
            return moment.utc($this.attr("datetime")).local().fromNow();
        }).prop('title', function () {
            var $this = $(this);
            return moment.utc($this.attr("datetime")).local().format("MMMM Do YYYY, HH:mm:ss");
            });
        return $element;
    }
};