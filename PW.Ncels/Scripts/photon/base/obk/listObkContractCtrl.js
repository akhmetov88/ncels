



function obkContractForm($scope, $http) {

    //$scope.object.seriesCreateDate = new Date();
    //$scope.object.seriesCreateDate = new Date();

    $scope.productSeries = [];

    $scope.product = {};

    $scope.gridOptions = {
        enableRowSelection: true,
        enableRowHeaderSelection: false
    };

    $scope.gridOptions.multiSelect = false;
    $scope.gridOptions.noUnselect = true;

    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;

        gridApi.selection.on.rowSelectionChanged($scope, function (row) {
            var msg = 'row selected ' + row.isSelected;
            $scope.product.NameRu = row.entity.Name;
            $scope.product.NameKz = row.entity.NameKz;
            $scope.product.ProducerNameRu = row.entity.ProducerName;
            $scope.product.ProducerNameKz = row.entity.ProducerNameKz;
            $scope.product.CountryNameRu = row.entity.CountryName;
            $scope.product.CountryNameKz = row.entity.CountryNameKz;;
            $scope.product.TnvedCode = row.entity.TnvedCode;
            $scope.product.KpvedCode = row.entity.KpvedCode;
            $scope.product.Price = row.entity.Price;
            $scope.product.Currency = row.entity.Currency;
        });
    };

    $scope.gridOptions.columnDefs = [
    { name: 'Id', displayName: 'ID', visible: false },
    { name: 'RegNumber', displayName: 'Рег. номер' },
    { name: 'RegTypeName', displayName: 'Тип' },
    { name: 'Name', displayName: 'Торговое название' },
    { name: 'NameKz', displayName: 'Торговое название на казахском', visible: false },
    { name: 'RegDate', displayName: 'Дата регистрации' },
    { name: 'ExpireDate', displayName: 'Дата истечения' },
    { name: 'ProducerName', displayName: 'Производитель' },
    { name: 'ProducerNameKz', displayName: 'Производитель на казахском', visible: false },
    { name: 'CountryName', displayName: 'Страна' },
    { name: 'CountryNameKz', displayName: 'Страна на казахском', visible: false },
    { name: 'TnvedCode', displayName: 'ТН ВЭД', visible: false },
    { name: 'KpvedCode', displayName: 'КП ВЭД', visible: false },
    { name: 'Price', displayName: 'Цена', visible: false },
    { name: 'Currency', displayName: 'Валюта', visible: false }
    ];

    //  $scope.myData = [
    //{
    //    "testname": "Cox",
    //    "lastName": "Carney",
    //    "company": "Enormo",
    //    "employed": true
    //},
    //{
    //    "firstName": "Lorraine",
    //    "lastName": "Wise",
    //    "company": "Comveyer",
    //    "employed": false
    //},
    //{
    //    "firstName": "Nancy",
    //    "lastName": "Waters",
    //    "company": "Fuelton",
    //    "employed": false
    //}
    //  ];



    // gridSeries
    $scope.gridOptionsSeries = {
        enableRowSelection: true,
        enableRowHeaderSelection: false
    };

    $scope.gridOptionsSeries.multiSelect = false;
    $scope.gridOptionsSeries.noUnselect = true;

    $scope.gridOptionsSeries.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;

        gridApi.selection.on.rowSelectionChanged($scope, function (row) {
            //alert("selected");
        });
    };

    $scope.gridOptionsSeries.columnDefs = [
{ name: 'Id', displayName: 'ID', visible: false },
{ name: 'Series', displayName: 'Серия' },
{ name: 'CreateDate', displayName: 'Произведена' },
{ name: 'ExpireDate', displayName: 'Истекает' },
{ name: 'Part', displayName: 'Партия' },
{ name: 'Unit', displayName: 'Ед. измерения' }
    ];

    $scope.gridOptions.data = $scope.productSeries;




    $scope.showAddDrugBlock = false;
    $scope.object = { drugRegType: 1 };
    $scope.searchResults = null;


    loadDictionary($scope, 'Currency', $http);
    loadObkRefTypes($scope, $http);
    loadObkOrganizations($scope, $http);
    loadDictionary($scope, 'OpfType', $http);
    loadDictionary($scope, 'Country', $http);
    loadDictionary($scope, 'OBKContractDocumentType', $http);
    loadDictionaryMeasure($scope, $http);

    $scope.BoolDic = [{
        Id: true,
        Name: "Да"
    }, {
        Id: false,
        Name: "Нет"
    }];


    $scope.searchDrug = function () {
        var drugRegType = $scope.object.drugRegType;
        var drugNumber = $scope.object.drugNumber;
        var drugTradeName = $scope.object.drugTradeName;
        var drugManufacturer = $scope.object.drugManufacturer;
        var drugMnn = $scope.object.drugMnn;

        $http({
            method: 'GET',
            url: '/OBKContract/SearchDrug',
            params: {
                regType: drugRegType,
                drugNumber: drugNumber,
                drugTradeName: drugTradeName,
                drugManufacturer: drugManufacturer,
                drugMnn: drugMnn
            }
        }).then(function (resp) {
            //alert(JSON.stringify(resp));
            if (resp.data) {
                //$scope.copyOrg(resp.data, currentOrg, copySpecificFieldsFn);
                //switch (sourceName) {
                //    case 'payerSource':
                //        setPayerType('payerType', 'Payer');
                //        break;
                //    case 'payerTranslationSource':
                //        setPayerType('payerTranslation', 'PayerTranslation');
                //        break;
                //};

                $scope.searchResults = resp.data;
                $scope.gridOptions.data = resp.data;
                $scope.gridApi.grid.refresh();
            }
            else {
                $scope.searchResults = null;
                $scope.gridOptions.data = null;
                $scope.gridApi.grid.refresh();
            }
        }, function (response) {
            //alert(JSON.stringify(response));
        });
    };


    $scope.addDrug = function addDrug() {
        //alert("Add Drug");
        $scope.showAddDrugBlock = true;
    };


    $scope.addSeries = function addSeries() {
        //var x = $scope.object.seriesValue;
        //alert(x);
        //x = $scope.object.seriesCreateDate;
        //alert(x);
        //x = $scope.object.seriesExpireDate;
        //alert(x);
        //x = $scope.object.partValue;
        //alert(x);
        //x = $scope.object.seriesUnit;
        //alert(x);
        //x = $scope.object.seriesUnit.Id;
        //alert(x);
        //x = $scope.object.seriesUnit.Name;
        //alert(x);

        //var createDate = convertDateToString($scope.object.seriesCreateDate);
        //var expireDate = convertDateToString($scope.object.seriesExpireDate);

        //var obj = { Id: null, Series: $scope.object.seriesValue, CreateDate: createDate, ExpireDate: expireDate, Part: $scope.object.partValue, UnitId: $scope.object.seriesUnit.Id, UnitName: $scope.object.seriesUnit.Name };
        //$scope.productSeries.push(obj);

    };


}

function convertDateToString(dateObj) {
    //alert("Date is null");
    //alert("dateObj.constructor.name = " + dateObj.constructor.name);
    //alert("dateObj = " + dateObj);
    if (!dateObj) {
        return null;
    }
    //var yyyy = dateObj.getFullYear();
    var mm = dateObj.getMonth() < 9 ? "0" + (this.getMonth() + 1) : (this.getMonth() + 1);
    return mm + "." + yyyy;
}

function loadObkRefTypes($scope, $http) {
    var name = "ObkContractTypes";
    $http({
        method: "GET",
        url: "/OBKDictionaries/GetObkContractTypes",
        data: "JSON"
    }).success(function (result) {
        $scope[name] = result;
    });
}

function loadObkOrganizations($scope, $http) {
    var name = "Organizations";
    $http({
        method: "GET",
        url: "/OBKDictionaries/GetObkOrganizations",
        data: "JSON"
    }).success(function (result) {
        $scope[name] = result;
    });
}

function loadDictionaryMeasure($scope, $http) {
    var name = "SRMeasures";
    $http({
        method: "GET",
        url: "/OBKDictionaries/GetMeasureDictionary",
        data: "JSON"
    }).success(function (result) {
        $scope[name] = result;
    });
}

angular
    .module('app')
    .controller('obkContractForm', ['$scope', '$http', obkContractForm])