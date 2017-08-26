function actionsObkContractListHtmlAction(data, type, full, meta, $scope) {
    debugger;
    return '<a  class="pw-task-link" href="/OBKContract/Contract?id=' + full.Id + '" >' + data + '</a>';
}

function dateformatHtml(data, type, full, meta) {
    if (data == null)
        return '';
    var date = new Date(parseInt(data.slice(6, -2)));
    var month = date.getMonth() + 1;
    return date.getDate() + "." + (month.length > 1 ? month : "0" + month) + "." + date.getFullYear();

}
function obkContractGrid($scope, DTColumnBuilder) {
    function renderNumFunc(data, type, full, meta) {
        return actionsObkContractListHtmlAction(data, type, full, meta, $scope);
    };

    $scope.dtColumns = [
        DTColumnBuilder.newColumn("Number", "№ договора/доп. согл-я").withOption('name', 'Number').renderWith(renderNumFunc),
        DTColumnBuilder.newColumn("CreatedDate", "Дата создания").withOption('name', 'CreatedDate').renderWith(dateformatHtml),
        DTColumnBuilder.newColumn("Status", "Статус").withOption('name', 'Status'),
        DTColumnBuilder.newColumn("DeclarantName", "Заявитель").withOption('name', 'DeclarantName'),
        DTColumnBuilder.newColumn("StartDate", "Дата заключения/Дата начала действия договора").withOption('name', 'StartDate').renderWith(dateformatHtml),
        DTColumnBuilder.newColumn("EndDate", "Дата истечения договора").withOption('name', 'EndDate').renderWith(dateformatHtml)
    ];

}

angular
    .module('app')
    .controller('obkContractGrid', ['$scope', 'DTColumnBuilder', obkContractGrid]);
