$(document).ready(function() {
    $("#setupGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: baseURL + "Setup/GetList",
                    dataType: "json"
                },
                update: {
                    url: baseURL + "Setup/Update",
                    dataType: "json",
                    type: "POST",
                    complete: function(data) {
                        $("#setupGrid").data("kendoGrid").dataSource.read();
                    }
                },
                destroy: {
                    url: baseURL + "Delete",
                    dataType: "json",
                    type: "POST",
                    complete: function(data) {
                        $("#setupGrid").data("kendoGrid").dataSource.read();
                    }
                }
            },
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: {
                            type: "number",
                            editable: false
                        },
                        Name: {
                            type: "string",
                            editable: true
                        },
                        CreateDate: {
                            type: "date",
                            editable: false,
                            parse: function(e) { return fromJsonDate(e); }
                        },
                        FieldCount: {
                            type: "number",
                            editable: false
                        }
                    }
                }
            }
        },
        height: 500,
        sortable: true,
        editable: "inline",
        resizable: true,
        columns: [
            {
                field: "Name",
                title: "Név",
                template: '<a href="#= baseURL + \'Edit/\' + data.Id #" target="_blank">#= data.Name #</a>'
            },
            {
                field: "CreateDate",
                title: "Létrehozva",
                format: "{0:yyyy.MM.dd}",
                width: 150
            },
            {
                field: "FieldCount",
                title: "Mezők száma",
                attributes: { style: "text-align: right;" },
                width: 120
            },
            {
                command: ["edit", "destroy"],
                title: "&nbsp;",
                width: 200
            }
        ]
    });
});