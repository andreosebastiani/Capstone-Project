
//Reference .JS File in Razor View
<script>
        (function (helper) {
            $(function () {
                helper.notificationDetailsUrl = '@Url.Action("GetNotificationGroupDetails")';
                helper.onready();
            });
        })(homeHelper);
</script>


//.JS File
var homeHelper = (function ($) {
    "use strict";

    var module = {
        notificationDetailsUrl: null,
        onready: function () {

            //Changes the notification Group Description and Group Members based on what is highlighted in the incident creation modal's select menu.
            var $notificationSelect = $("#notificationGroupId");
            var url = this.notificationDetailsUrl;

            $notificationSelect.on("change", function () {
                var id = $notificationSelect.val();
                var theUrl = url + "?id=" + id;
                $.ajax({
                    url: theUrl,
                    success: function (data) {
                        //'data' contains a javascript serialized c# list of strings(this means 'data' is a string). Index 0 is the notification
                        //group description and the rest of the indices are the usernames within the notification group.
                        var descriptionAndUserList = JSON.parse(data);
                        $("#groupDescription").text(descriptionAndUserList[0]);
                        var htmlUserString = "";
                        var dataLength = descriptionAndUserList.length;
                        for(var i = 1; i < dataLength; i++)
                        {
                            htmlUserString = htmlUserString.concat(descriptionAndUserList[i]);
                        }
                        $("#groupUserList").html(htmlUserString);
                    }
                });
            });

            //Prevents a double click on the Create Incident Button inside of the modal.
            $("form").submit(function () {
                $(this).find("button[type='submit']").prop("disabled", true);
            });

        }
    };

    return module;

})(jQuery);