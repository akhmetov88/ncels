/*оплата DirectionPayment*/
function panelDirectionPaymentSelect(e) {
    debugger;
    var selectType = $(e.item).find("> .k-link").attr('ItemType');
    if (selectType !== null) {
        var selectValue = $(e.item).find("> .k-link").attr('ItemId');
        var gridId = $(e.item).find("> .k-link").attr('ModelId');
        var grid = $("#gridOBKPayment" + gridId).data("kendoGrid");
        var filter = new Array();

        if (selectType === "StatusCode") {

            filter.push({ field: "StatusCode", operator: "eq", value: selectValue });
        }

        if (selectValue === 'reqSign') {
            $("#generateDoc" + gridId).show();
            $("#sendToDeclarant" + gridId).show();
            $("#signDocument" + gridId).show();
        } else {
            $("#generateDoc" + gridId).hide();
            $("#sendToDeclarant" + gridId).hide();
            $("#signDocument" + gridId).hide();
        }

        if (selectValue === '') {
            grid.dataSource.filter([]);
        } else {
            grid.dataSource.filter({
                logic: "and",
                filters: filter
            });
        }
    }
}

function gridSelectRow(e) {
    debugger;
    var data = this.dataItem(this.select());
    $.ajax({
        type: 'GET',
        url: '/OBKPayment/GetEmployee',
        success: function (result) {
            debugger;
            var gridId = $(e.item).find("> .k-link").attr('ModelId');
            if (result.Id === data.ExecutorId && data.ExecutorSign === "False" ||
                result.Id === data.ChiefAccountantId && data.ChiefAccountantSign === "False") {
                
            } else {
                
            }
        }
    });
}

function signPayment(id) {
    debugger;
    kendo.ui.progress($("#mainWindowLoader"), true);
    $.ajax({
        type: 'GET',
        url: '/OBKPayment/GetSignDirectionToPayment?id=' + id,
        success: function(result) {
            debugger;
            startSign(result.data, id, saveSignedPayment);
        },
        complete: function() {
            kendo.ui.progress($("#mainWindowLoader"), false);
        }
    });

    function saveSignedPayment(signedData, paymentId) {
        debugger;
        var data = {
            paymentId: paymentId,
            signedData: signedData
        };
        var json = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/OBKPayment/SaveSignedPayment',
            contentType: 'application/json; charset=utf-8',
            data: json,
            success: function(result) {
                debugger;
                alert("Счет на оплату подписан");
            },
            complete: function() {
                debugger;
                var gridId = $(e.item).find("> .k-link").attr('ModelId');
                var grid = $("#gridOBKPayment" + gridId).data("kendoGrid");
                grid.dataSource.read();
            }
        });
    }
}