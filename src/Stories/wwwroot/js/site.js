(function ($, window, document) {
    $(function () {
        // The DOM is ready!
        $(document).keypress(function (e) {
            if (e.which == 13) {
                $(".return-submit").click();
            }
        });

        $("#signin").on("click", "button.submit", function () {
            signin().done(function (data) {
                if (data.status == false) {
                    $("div#error").removeClass("hidden").append('<div class="error">' + data.message + '</div>');
                    return false;
                }
                else {
                    if (!$("div#error").hasClass("hidden")) {
                        $("div#error").addClass("hidden");
                    }
                }

                window.location.href = $.QueryString["returnUrl"];
                return false;
            });
        });

        $("#signup").on("click", "button.signup", function () {
            signup().done(function (data) {
                if (data.status == false) {
                    $("div#error").removeClass("hidden").append('<div class="error">' + data.message + '</div>');
                    return false;
                }
                else {
                    if (!$("div#error").hasClass("hidden")) {
                        $("div#error").addClass("hidden");
                    }
                }

                window.location.href = $.QueryString["returnUrl"];
                return false;
            });
        });

        $("#addstory").on("click", "button.submit", function () {
            addStory().done(function (data) {
                if (data.status == false) {
                    $("div#error").removeClass("hidden").append('<div class="error">' + data.message + '</div>');
                    return false;
                }
                else {
                    if (!$("div#error").hasClass("hidden")) {
                        $("div#error").addClass("hidden");
                    }
                }

                window.location.href = $.QueryString["returnUrl"];
            });
            return false;
        });

        $("#add-comment").on("click", "button.add-comment-btn", function () {
            var $this = $(this);
            var $textarea = $this.parent().closest("#add-comment").find("textarea.add-comment");
            var data = {
                StoryHashId: $(".story").data("hashid"),
                CommentMarkdown: $textarea.val(),
            };

            addComment(data).success(function (data) {
                if (data.status == false) {
                    $("div#error").removeClass("hidden").append('<div class="error">' + data.message + '</div>');
                    return false;
                }
                else {
                    if (!$("div#error").hasClass("hidden")) {
                        $("div#error").addClass("hidden");
                    }
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
                if (data.status == false) {
                    $("div#error").removeClass("hidden").append('<div class="error">' + data.message + '</div>');
                    return false;
                }
                else {
                    if (!$("div#error").hasClass("hidden")) {
                        $("div#error").addClass("hidden");
                    }
                }

                $.get($("#comments").data("get") + "/?hashId=" + data.commentHashId, function (data) {
                    $parent.after($("<ol><li></li></ol>").append(data).addClass("reply"));
                });

                $parent.find(".comment-reply").remove();

                return false;
            });
        });

        $("#invite-user").on("click", "button.submit", function () {
            var $parent = $(this).closest("#invite-user");
            $parent.find("sucess").addClass("hidden");

            invite($parent.data("url"), $parent.find(".email").val()).done(function (data) {
                if (data.status == false) {
                    $("div#error").removeClass("hidden").append('<div class="error">' + data.message + '</div>');
                    return false;
                }
                else {
                    if (!$("div#error").hasClass("hidden")) {
                        $("div#error").addClass("hidden");
                    }

                    $parent.find("#success").removeClass("hidden");
                    $parent.find(".email").val("");
                }
            });
        });

        $("#referral").on("click", "button.submit", function () {
            var $parent = $(this).closest("#referral");

            var data = {
                Username: $parent.find("input.username").val(),
                Email: $parent.find("input.email").val(),
                Password: $parent.find("input.password").val(),
                ConfirmPassword: $parent.find("input.confirmPassword").val(),
                Code: $parent.find("input#Code").val(),
            };

            var url = $parent.data("url");

            referral(url, data).done(function() {
                if (data.status == false) {
                    var $errorDiv = $("div#error").removeClass("hidden");

                    for (var i = 0; i < data.messages.length - 1; i++) {
                        $errorDiv.append('<div class="error">' + data.messages[i] + '</div>'); 
                    }
                    
                    return false;
                }
                else {
                    if (!$("div#error").hasClass("hidden")) {
                        $("div#error").addClass("hidden");
                    }

                    window.location.href = "/";
                }
            });
        });

        $("#updatepassword").on('click', "button.submit", function() {
            var $parent = $(this).parent().closest("#changepassword");
            var data = {
                OldPassword: $parent.find("#OldPassword").val(), 
                NewPassword: $parent.find("#NewPassword").val(),
                ConfirmNewPassword: $parent.find("#ConfirmNewPassword").val()                
            };

            changepassword(data).success(function (data) {
                $parent.find("#OldPassword").val("");
                $parent.find("#NewPassword").val("");
                $parent.find("#ConfirmNewPassword").val("");
                
                if (data.status == false) {
                    $("div#error").removeClass("hidden").append('<div class="error">' + data.message + '</div>');
                    return false;
                }
                else {
                    if (!$("div#error").hasClass("hidden")) {
                        $("div#error").addClass("hidden");
                    }
                }
            });
        });

        $("div.vote").on("click", "i", function () {
            var $this = $(this);
            var $parent = null;
            var $voteParent = $this.parents(".vote");

            if ($this.parents("div").hasClass("story")) {
                $parent = $this.closest(".story");
            }
            else if ($this.parents("div").hasClass("comment")) {
                $parent = $this.closest(".comment");
            }

            toggleVote($voteParent.data("url"), $parent.data("hashid")).done(function (data) {
                if (data.status = false) {
                    $("div#error").removeClass("hidden").append('<div class="error">' + data.message + '</div>');
                    return false;
                }
                else {
                    if (!$("div#error").hasClass("hidden")) {
                        $("div#error").addClass("hidden");
                    }
                    $this.toggleClass("active");

                    var $scoreElement = $voteParent.find(".score");
                    var score = parseInt($scoreElement.html());
                    if ($this.hasClass("active")) {
                        score++;
                    } else {
                        score--;
                    }

                    $scoreElement.html(score);
                }

                return false;
            });
        });

        $("div#comments").on("click", "a.reply", function () {
            var $this = $(this);

            var url = $("#add-comment").data("url");
            var div = $("<div></div>").addClass("comment-reply");
            var textArea = $("<textarea></textarea>").addClass("form-control add-comment").attr("rows", 5);

            div.append(textArea);
            $("#add-comment .btn-block").clone().appendTo(div);
            
            $this.closest(".comment").append(div);
            return false;
        });

        $("div.story").on("click", "i.fa-trash", function () {
            var $this = $(this);
            var url = $(this).closest("div.delete-story").data("url");
            var hashId = $(this).closest("div.story").data("hashid");

            var data = {
                url: url,
                form: {
                    hashId: hashId
                }
            };

            deleteStory(data).success(function (data) {
                if (data.status == false) {
                    $("div#error").removeClass("hidden").append('<div class="error">' + data.message + '</div>');
                    return false;
                }
                else {
                    if (!$("div#error").hasClass("hidden")) {
                        $("div#error").addClass("hidden");
                    }
                }

                window.location.href = "/";
                return false;
            });
        });
    });

    function signin() {
        var data = { Email: $("#signin .email").val(), Password: $("#signin .password").val() }

        return $.ajax({
            url: $("#signin").data("url"),
            type: 'POST',
            dateType: "json",
            data: data
        });
    };

    function signup() {
        var data = {
            Email: $("#signup .email").val(),
            Password: $("#signup .password").val(),
            ConfirmPassword: $("#signup .confirmPassword").val(),
            Username: $("#signup .username").val()
        };

        return $.ajax({
            url: $("#signup").data("url"),
            type: 'POST',
            dateType: "json",
            data: data
        });
    };

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

    function addComment(data) {
        return $.ajax({
            url: $("#add-comment").data("url"),
            type: "POST",
            dataType: "json",
            data: data
        });
    };

    function deleteStory(data)
    {
        return $.ajax({
            url: data.url,
            type: "POST",
            dataType: "json",
            data: data.form
        });
    };

    function changepassword(data) {
        return $.ajax({
            url: $("#changepassword").data("url"),
            type: "POST",
            dataType: "json",
            data: data
        });
    };

    function toggleVote(url, hashid) {
        return $.ajax({
            url: url,
            type: "POST",
            dataType: "json",
            data: { hashId: hashid }
        });
    };

    function invite(url, email) {
        return $.ajax({
            url: url,
            type: "POST",
            dataType: "json",
            data: { Email: email }
        });
    };

    function referral(url, data) {
        return $.ajax({
            url: url,
            type: "POST",
            dataType: "json",
            data: data
        });
    }

    $.QueryString = (function (a) {
        if (a == "") return {};
        var b = {};
        for (var i = 0; i < a.length; ++i) {
            var p = a[i].split('=', 2);
            if (p.length != 2) continue;
            b[p[0]] = decodeURIComponent(p[1].replace(/\+/g, " "));
        }
        return b;
    })(window.location.search.substr(1).split('&'))

}(window.jQuery, window, document));