(function ($, window, document) {
    $(function () {
        $("#addstory").on("click", "button.submit", function () {
            addStory().done(function (data) {
                if (data.status == false) {
                    showErrors(data.messages);
                    return false;
                }
                else {
                    showErrors();
                }

                window.location.href = $.QueryString["returnUrl"];
            });
            return false;
        });

        $("#addstory").on("click", "button.reset", function () {
            var $container = $(this).closest("#addstory");

            $container.find("input").each(function () { $(this).val(""); });
            $container.find("textarea").each(function () { $(this).val(""); })
            $container.find("input:checked").each(function () { $(this).attr('checked', false); })
        });

        $("div.story").on("click", "i.fa-trash", function () {
            var $this = $(this);
            var url = $this.closest("div.delete-story").data("url");
            var hashId = $this.closest("div.story").data("hashid");

            var data = {
                url: url,
                form: {
                    hashId: hashId
                }
            };

            deleteStory(data).success(function (data) {
                if (data.status == false) {
                    showErrors(data.messages);
                    return false;
                }
                else {
                    showErrors();
                }

                window.location.href = "/";
                return false;
            });
        });

        $("div.story").on("click", "i.fa-flag", function () {
            var $this = $(this);
            var url = $this.data("url");
            var hashId = $this.closest("div.story").data("hashid");

            var data = {
                url: url,
                body: {
                    hashId: hashId
                }
            };

            flagStory(data).success(function (data) {
                if (data.status == false) {
                    showErrors(data.messages);
                    return false;
                }
                else {
                    showErrors();
                }

                return false;
            });
        });
    });

    function addStory() {
        var data = {
            Title: $("#addstory .title").val(),
            Url: $("#addstory .url").val(),
            DescriptionMarkdown: $("#addstory .description").val(),
            IsAuthor: $("#addstory .isauthor").val()
        };

        return $.ajax({
            url: $("#addstory").data("url"),
            type: 'POST',
            dataType: "json",
            data: data
        });
    };

    function deleteStory(data) {
        return $.ajax({
            url: data.url,
            type: "POST",
            dataType: "json",
            data: data.form
        });
    };

    function flagStory(data) {
        return $.ajax({
            url: data.url,
            type: "POST",
            dataType: "json",
            data = data.body
        });
    };

    function showErrors(messages) {
        var $errorDiv = $("div#error");
        $errorDiv.find(".error").remove();

        if (!messages || messages.length <= 0) {
            if (!$errorDiv.hasClass("hidden"))
                $errorDiv.addClass("hidden");

            return;
        }

        if ($errorDiv.hasClass('hidden'))
            $errorDiv.removeClass('hidden');

        for (var i = 0; i < messages.length; i++) {
            $errorDiv.append($("<div></div>").addClass("error").append($("<span></span>").text(messages[i])));
        }
    };
}(window.jQuery, window, document));