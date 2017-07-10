function dateformatHtml(data, type, full, meta) {
    if (data == null)
        return '';
    var date = new Date(parseInt(data.slice(6, -2)));
    var month = date.getMonth() + 1;
    return date.getDate() + "." + (month.length > 1 ? month : "0" + month) + "." + date.getFullYear();

}
$(document).ready(function () {
    var number;
    var inputs = document.getElementsByTagName('input');

    for (var i = 0; i < inputs.length; i++) {
        number = inputs[i];
        if (inputs[i].type.toLowerCase() == 'number') {
            number.onkeydown = function (e) {
                if (!((e.keyCode > 95 && e.keyCode < 106)
                    || (e.keyCode > 47 && e.keyCode < 58)
                    || e.keyCode == 8)) {
                    return false;
                }
            }
        }
    }
});
function safetyDeclataionGrid($scope, DTColumnBuilder) {
    $scope.dtColumns = [

        DTColumnBuilder.newColumn("TypeName", "Тип заявление").withOption('name', 'TypeName'),
        DTColumnBuilder.newColumn("Number", "Номер").withOption('name', 'Number'),
        DTColumnBuilder.newColumn("StausName", "Текущий статус").withOption('name', 'StausName'),
        DTColumnBuilder.newColumn("CreatedDate", "Дата начала").withOption('name', 'CreatedDate').renderWith(dateformatHtml),
        DTColumnBuilder.newColumn("SendDate", "Дата отправки").withOption('name', 'SendDate').renderWith(dateformatHtml),
        DTColumnBuilder.newColumn("SortDate", "Дата решение").withOption('name', 'SortDate').renderWith(dateformatHtml).notVisible(),
        DTColumnBuilder.newColumn("Id", "").withOption('name', 'Id').notSortable().renderWith(actionsDeclarationHtmlAction)
        //DTColumnBuilder.newColumn("IsRegisterProject", "IsRegisterProject").withOption('name', 'IsRegisterProject'),
        //DTColumnBuilder.newColumn("Type", "Type").withOption('name', 'Type'),
    ];

}

angular
    .module('app')
    .controller('safetyDeclataionGrid', ['$scope', 'DTColumnBuilder', safetyDeclataionGrid]);
