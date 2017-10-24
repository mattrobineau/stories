; (function ($, window, document) {
    "use strict";

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
                    showErrors(data.messages);

                    return false;
                }
                else {
                    showErrors();
                }

                window.location.href = $.QueryString["returnUrl"];
                return false;
            });
        });

        $("#signup").on("click", "button.signup", function () {
            signup().done(function (data) {
                if (data.status == false) {
                    showErrors(data.messages);
                    return false;
                }
                else {
                    showErrors();
                }

                window.location.href = $.QueryString["returnUrl"];
                return false;
            });
        });

        $("#invite-user").on("click", "button.submit", function () {
            var $parent = $(this).closest("#invite-user");
            $parent.find("sucess").addClass("hidden");

            invite($parent.data("url"), $parent.find(".email").val()).done(function (data) {
                if (data.status == false) {
                    showErrors(data.messages);
                    return false;
                }
                else {
                    showErrors();

                    $parent.find("#success").removeClass("hidden");
                    $parent.find(".email").val("");
                    $parent.find("label[for=email]").text("Email (" + data.remaninginvites + ")");
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

            referral(url, data).done(function () {
                if (data.status == false) {
                    showErrors(data.messages);
                    return false;
                }
                else {
                    showErrors();

                    window.location.href = "/";
                }
            });
        });

        $("#updatepassword").on('click', "button.submit", function () {
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
                    showErrors(data.messages);
                    return false;
                }
                else {
                    showErrors();
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
                if (data.status == false) {
                    showErrors(data.messages);
                    return false;
                }
                else {
                    showErrors();

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