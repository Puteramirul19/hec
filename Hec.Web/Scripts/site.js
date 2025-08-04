$(function () {
    $("#notifications").kendoNotification({
        position: {
            pinned: true,
            top: 60,
            left: null,
            bottom: null,
            right: 20
        }
    });
});

window.app = {

    makeDataSource: function (controllerName, config) {
        var dataSourceConfig = {
            type: "aspnetmvc-ajax",
            transport: {
                read: {
                    url: controllerName + "/Read"
                },
                create: {
                    url: controllerName + "/Create",
                    method: "POST"
                },
                update: {
                    url: controllerName + "/Update",
                    method: "POST"
                },
                destroy: {
                    url: controllerName + "/Destroy",
                    method: "POST"
                }
            },
            schema: {
                data: "Data",
                total: "Total",
                model: {
                    id: "Id",
                    fields: {} /* should come from var config */
                }
            },
            pageSize: 25,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
        };

        $.extend(true, dataSourceConfig, config);
        return new kendo.data.DataSource(dataSourceConfig);
    },

    newGuid: function () {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    },

    emptyGuid: "00000000-0000-0000-0000-000000000000",

    navigate: function(url) {
        location.href = url;
    },

    notify: function (msg, type) {
        var placeholder = "notifications";
        $("#" + placeholder).data("kendoNotification").show(msg, type);
    },

    notifyInfo:    function (msg, jqXHR) { app.notify(msg, 'info'); },
    notifyWarning: function (msg, jqXHR) { app.notify(msg, 'warning'); },
    notifySuccess: function (msg, jqXHR) { app.notify(msg, 'success'); },
    notifyError: function (msgOrjqXHR) {
        var msg = (msgOrjqXHR.responseText) ? msgOrjqXHR.responseText : msgOrjqXHR;
        msg = msg || "Some error occured. Please try again.";
        app.notify(msg, 'error');
    },
}

function gridError(e) {
    console.log(e);
    if (e.errors) {
        var message = "";
        // Create a message containing all errors.
        $.each(e.errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    message += this + "\n";
                });
            }
        });

        // Display the message.
        app.notifyError(message);

        // Cancel the changes.
        var grid = $("#grid").data("kendoGrid");
        grid.cancelChanges();
    }
}