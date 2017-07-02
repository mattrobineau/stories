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

            return false;
        });
    });

    function createUser(data) {
        return $.ajax({
            url: $("#create-user").data("url"),
            type: "POST",
            dataType: "json",
            data: data
        });
    };
}(window.jQuery, window, document));
