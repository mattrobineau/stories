(function ($, window, document) {
    "use strict";

    $(function () {
        $('time').text(function () {
            var $this = $(this);
            return moment.utc($this.attr("datetime")).local().fromNow();
        }).prop('title', function () {
            var $this = $(this);
            return moment.utc($this.attr("datetime")).local().format("MMMM Do YYYY, HH:mm:ss");
        });
    });
}(window.jQuery, window, document));