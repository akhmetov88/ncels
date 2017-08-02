



function obkContractForm($scope, $http) {
    loadDictionary($scope, 'Currency', $http);
}

angular
    .module('app')
    .controller('obkContractForm', ['$scope', '$http', obkContractForm])