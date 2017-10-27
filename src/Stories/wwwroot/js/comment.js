; (function ($, window, document) {
    "use strict";

    $(function () {
        $("#add-comment").on("click", "button.add-comment-btn", function () {
            var $this = $(this);
            var $textarea = $this.parent().closest("#add-comment").find("textarea.add-comment");
            var data = {
                StoryHashId: $(".story").data("hashid"),
                CommentMarkdown: $textarea.val(),
            };

            addComment(data).success(function (data) {
                if (data.status === false) {
                    showErrors(data.messages);
                    return false;
                }
                else {
                    showErrors();
                }

                $.get($("#comments").data("get") + "/?hashId=" + data.commentHashId, function (data) {
                    $(data).prependTo("#comments");
                });

                $textarea.val("");

                return false;
            });
        });

        $("#comments").on("click", "button.add-comment-btn", function () {
            var $parent = $(this).parent().closest(".comment");
            var $textarea = $parent.find("textarea.add-comment");

            var data = {
                ParentCommentHashId: $parent.data("hashid"),
                StoryHashId: $(".story").data("hashid"),
                CommentMarkdown: $textarea.val(),
            };

            addComment(data).success(function (data) {
                if (data.status === false) {
                    showErrors(data.messages);
                    return false;
                }
                else {
                    showErrors();
                }

                $.get($("#comments").data("get") + "/?hashId=" + data.commentHashId, function (data) {
                    $parent.after($("<ol><li></li></ol>").append(data).addClass("reply"));
                });

                $parent.find(".comment-reply").remove();

                return false;
            });
        });

        $("div#comments").on("click", "a.reply", function () {
            var $this = $(this);

            var exists = $this.closest(".comment").find(".comment-reply");

            if (exists.length > 0) {
                return false;
            }

            var div = $("<div></div>").addClass("comment-reply");
            var textArea = $("<textarea></textarea>").addClass("form-control add-comment").attr("rows", 5);

            div.append(textArea);
            $("#add-comment .btn-block").clone().appendTo(div);

            $this.closest(".comment").append(div);
            return false;
        });

        $("div#comments").on("click", "i.fa-flag", function () {
            var $this = $(this);
            var url = $this.data("url");
            var hashId = $this.closest("div.comment").data("hashid");

            var data = {
                url: url,
                body: {
                    hashId: hashId
                }
            };

            flagStory(data).success(function (data) {
                if (data.status === false) {
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

    function addComment(data) {
        return $.ajax({
            url: $("#add-comment").data("url"),
            type: "POST",
            dataType: "json",
            data: data
        });
    }

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
    }

    function flagStory(data) {
        return $.ajax({
            url: data.url,
            type: "POST",
            dataType: "json",
            data: data.body
        });
    }
}(window.jQuery, window, document));