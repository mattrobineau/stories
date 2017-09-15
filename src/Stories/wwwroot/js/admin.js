(function ($, window, document) {
    $(function () {
        // The DOM is ready!
        $("#create-user").on('click', "div.actions button.submit", function () {
            var $this = $(this);
            var $parent = $this.closest("#create-user");

            var data = {
                Username: $parent.find("input.username").val(),
                Email: $parent.find("input.email").val(),
                Password: $parent.find("input.password").val(),
                ConfirmPassword: $parent.find("input.confirmPassword").val(),
                Roles: []
            };

            $parent.find("input.select-role:checked").each(function (idx, role) {
                var $role = $(role);
                data.Roles.push($role.val());
            });

            createUser(data).done(function (data) {
                if (data.status === false) {
                    showErrors(data.Messages);
                    return false;
                }
                else {
                    showErrors();
                }
            });

            return false;
        });

        $("#banuser").on('click', 'div.actions button.submit', function () {
            var $parent = $(this).closest("#banuser");

            var data = {
                UserId: $parent.data("uid"),
                ExpiryDate: $parent.find("#expiry").val(),
                Reason: $parent.find("#reason").val(),
                Notes: $parent.find("#notes").val()
            };

            banUser(data).done(function (data) {
                var $errorDiv = $("div#error");
                if (data.status === false) {
                    showErrors(data.messages);
                    return false;
                } else {
                    showErrors();
                }
            });
        });
    });

    function createUser(data) {
        return $.ajax({
            url: $("#create-user").data("url"),
            type: "POST",
            dataType: "json",
            data: data
        });
    }

    function banUser(data) {
        return $.ajax({
            url: $("#banuser").data("url"),
            type: "POST",
            dataType: "json",
            data: data
        });
    }

    function showErrors(messages) {
        var $errorDiv = $("div#error");

        if (!messages || messages.length <= 0) {
            if (!$errorDiv.hasClass("hidden"))
                $errorDiv.addClass("hidden");

            $errorDiv.find(".error").remove();
            return;
        }

        if ($errorDiv.hasClass('hidden'))
            $errorDiv.removeClass('hidden');

        for (var i = 0; i < messages.length; i++) {
            $errorDiv.append($("<div></div>").addClass("error").append($("<span></span>").text(messages[i])));
        }
    }
}(window.jQuery, window, document));
