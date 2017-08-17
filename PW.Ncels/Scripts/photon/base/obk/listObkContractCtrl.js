



function obkContractForm($scope, $http) {



    var curDate = new Date();
    $scope.mode = 0; // 0 - unknown, 1 - add, 2 - edit
    $scope.object = {};
    $scope.object.IsResident = 1;
    $scope.object.seriesValue = "SERIES-01";
    $scope.object.seriesCreateDate = curDate.getTime();
    $scope.object.seriesExpireDate = curDate.getTime();

    $scope.enableCompanyData = false;
    $scope.showBin = false;
    $scope.showResidentsBlock = false;

    $scope.productSeries = [];

    $scope.addedProducts = [];

    $scope.product = {};

    $scope.gridOptions = {
        enableRowSelection: true,
        enableRowHeaderSelection: false
    };

    $scope.gridOptions.multiSelect = false;
    $scope.gridOptions.noUnselect = true;

    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridOptionsApi = gridApi;

        gridApi.selection.on.rowSelectionChanged($scope, function (row) {
            var msg = 'row selected ' + row.isSelected;
            $scope.product.Id = row.entity.Id;
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

    $scope.selectedSeriesIndex = null;

    $scope.gridOptionsSeries.onRegisterApi = function (gridApi) {
        $scope.gridOptionsSeriesApi = gridApi;
        //$scope.selectedSeriesIndex
        gridApi.selection.on.rowSelectionChanged($scope, function (row) {
            //alert("selected");
            $scope.selectedSeriesIndex = $scope.gridOptionsSeries.data.indexOf(row.entity);
        });
    };

    $scope.gridOptionsSeries.columnDefs = [
{ name: 'Id', displayName: 'ID', visible: false },
{ name: 'Series', displayName: 'Серия' },
{ name: 'CreateDate', displayName: 'Произведена' },
{ name: 'ExpireDate', displayName: 'Истекает' },
{ name: 'Part', displayName: 'Партия' },
{ name: 'UnitName', displayName: 'Ед. измерения' },
{ name: 'UnitId', displayName: 'Ед. измерения - код', visible: false }
    ];

    $scope.gridOptionsSeries.data = $scope.productSeries;

    // Grid SEries End

    // Grid Products

    $scope.gridOptionsProducts = {
        enableRowSelection: true,
        enableRowHeaderSelection: false,
        multiSelect: false,
        noUnselect: true
    };

    $scope.selectedProductIndex = null;

    $scope.gridOptionsProducts.onRegisterApi = function (gridApi) {
        $scope.gridOptionsProductsApi = gridApi;

        gridApi.selection.on.rowSelectionChanged($scope, function (row) {
            $scope.selectedProductIndex = $scope.gridOptionsProducts.data.indexOf(row.entity);
            alert($scope.selectedProductIndex);
        });
    };

    $scope.gridOptionsProducts.columnDefs = [
        { name: 'Id', displayName: 'ID', visible: false },
        { name: 'NameRu', displayName: 'Наименование' },
        { name: 'ProducerNameRu', displayName: 'Производитель' },
        { name: 'CountryNameRu', displayName: 'Страна-производитель' },
        { name: 'TnvedCode', displayName: 'ТН ВЭД' },
        { name: 'KpvedCode', displayName: 'КП ВЭД' }
    ];

    $scope.addedProducts = [];

    $scope.gridOptionsProducts.data = $scope.addedProducts;

    // Grid Products End

    $scope.showAddEditDrugBlock = false;
    $scope.object.drugRegType = 1;
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
                //$scope.gridOptionsApi.grid.refresh();
            }
            else {
                $scope.searchResults = null;
                $scope.gridOptions.data = null;
                //$scope.gridOptionsApi.grid.refresh();
            }
        }, function (response) {
            //alert(JSON.stringify(response));
        });
    };


    $scope.addDrug = function addDrug() {
        $scope.showAddEditDrugBlock = true;
        $scope.mode = 1;
    };

    $scope.editDrug = function editDrug() {
        if ($scope.selectedProductIndex != null) {
            $scope.mode = 2;

            $scope.showAddEditDrugBlock = true;

            var selectedObj = $scope.addedProducts[$scope.selectedProductIndex];

            $scope.product.Id = selectedObj.Id;
            $scope.product.NameRu = selectedObj.NameRu;
            $scope.product.NameKz = selectedObj.NameKz;
            $scope.product.ProducerNameRu = selectedObj.ProducerNameRu;
            $scope.product.ProducerNameKz = selectedObj.ProducerNameKz;
            $scope.product.CountryNameRu = selectedObj.CountryNameRu;
            $scope.product.CountryNameKz = selectedObj.CountryNameKz;
            $scope.product.TnvedCode = selectedObj.TnvedCode;
            $scope.product.KpvedCode = selectedObj.KpvedCode;
            $scope.product.Price = selectedObj.Price;
            $scope.product.Currency = selectedObj.Currency;
            $scope.productSeries.push.apply($scope.productSeries, selectedObj.Series);

        }
        else {
            alert("Выберите продукцию для изменения");
        }
    }

    $scope.deleteDrug = function deleteDrug() {
        if ($scope.selectedProductIndex != null) {
            $scope.addedProducts.splice($scope.selectedProductIndex, 1);
            $scope.selectedProductIndex = null;
        }
        else {
            alert("Выберите продукцию для удаления");
        }
    }

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

        var createDate = convertDateToString($scope.object.seriesCreateDate);
        var expireDate = convertDateToString($scope.object.seriesExpireDate);

        if (!$scope.object.seriesValue ||
            !createDate ||
            !expireDate ||
            !$scope.object.partValue ||
            !$scope.object.seriesUnit
            ) {
            alert("Заполните поля серии");
        }
        else {
            var obj = { Id: null, Series: $scope.object.seriesValue, CreateDate: createDate, ExpireDate: expireDate, Part: $scope.object.partValue, UnitId: $scope.object.seriesUnit.Id, UnitName: $scope.object.seriesUnit.Name };
            $scope.productSeries.push(obj);
            $scope.object.seriesValue = null;
            $scope.object.partValue = null;
            $scope.object.seriesUnit = null;
        }
    };

    $scope.deleteSeries = function deleteSeries() {
        if ($scope.selectedSeriesIndex != null) {
            $scope.productSeries.splice($scope.selectedSeriesIndex, 1);
            $scope.selectedSeriesIndex = null;
        }
        else {
            alert("Выберите серию для удаления");
        }
    }

    $scope.saveProduct = function saveProduct() {

        if ($scope.mode == 1) {
            var id = $scope.product.Id;
            //var NameRu = $scope.product.NameRu;
            //var CountryNameRu = $scope.product.CountryNameRu;
            //var TnvedCode = $scope.product.TnvedCode;
            //var KpvedCode = $scope.product.KpvedCode;
            if (id) {
                if (!$scope.existInArray($scope.addedProducts, id)) {

                    //$scope.product.Id = row.entity.Id;
                    //$scope.product.NameRu = row.entity.Name;
                    //$scope.product.NameKz = row.entity.NameKz;
                    //$scope.product.ProducerNameRu = row.entity.ProducerName;
                    //$scope.product.ProducerNameKz = row.entity.ProducerNameKz;
                    //$scope.product.CountryNameRu = row.entity.CountryName;
                    //$scope.product.CountryNameKz = row.entity.CountryNameKz;;
                    //$scope.product.TnvedCode = row.entity.TnvedCode;
                    //$scope.product.KpvedCode = row.entity.KpvedCode;
                    //$scope.product.Price = row.entity.Price;
                    //$scope.product.Currency = row.entity.Currency;


                    var product = {
                        Id: $scope.product.Id,
                        NameRu: $scope.product.NameRu,
                        NameKz: $scope.product.NameKz,
                        ProducerNameRu: $scope.product.ProducerNameRu,
                        ProducerNameKz: $scope.product.ProducerNameKz,
                        CountryNameRu: $scope.product.CountryNameRu,
                        CountryNameKz: $scope.product.CountryNameKz,
                        TnvedCode: $scope.product.TnvedCode,
                        KpvedCode: $scope.product.KpvedCode,
                        Price: $scope.product.Price,
                        Currency: $scope.product.Currency,
                        Series: []
                    }
                    product.Series = $scope.productSeries.slice();

                    $scope.addedProducts.push(product);
                    $scope.showAddEditDrugBlock = false;
                    $scope.clearSearchAndProductFields();
                    $scope.mode = 0;
                }
                else {
                    alert("Выбранная продукция уже имеется в таблице!");
                }
            }
            else {
                alert("Выберите продукт");
            }
        }
        if ($scope.mode == 2) {
            //$scope.product.Id = selectedObj.Id;
            //$scope.product.NameRu = selectedObj.NameRu;
            //$scope.product.NameKz = selectedObj.NameKz;
            //$scope.product.ProducerNameRu = selectedObj.ProducerNameRu;
            //$scope.product.ProducerNameKz = selectedObj.ProducerNameKz;
            //$scope.product.CountryNameRu = selectedObj.CountryNameRu;
            //$scope.product.CountryNameKz = selectedObj.CountryNameKz;
            //$scope.product.TnvedCode = selectedObj.TnvedCode;
            //$scope.product.KpvedCode = selectedObj.KpvedCode;
            //$scope.product.Price = selectedObj.Price;
            //$scope.product.Currency = selectedObj.Currency;
            //$scope.productSeries.push.apply($scope.productSeries, selectedObj.Series);


            var selectedObj = $scope.addedProducts[$scope.selectedProductIndex];
            selectedObj.Id = $scope.product.Id;
            selectedObj.NameRu = $scope.product.NameRu;
            selectedObj.NameKz = $scope.product.NameKz;
            selectedObj.ProducerNameRu = $scope.product.ProducerNameRu;
            selectedObj.ProducerNameKz = $scope.product.ProducerNameKz;
            selectedObj.CountryNameR = $scope.product.CountryNameRuu;
            selectedObj.CountryNameKz = $scope.product.CountryNameKz;
            selectedObj.TnvedCode = $scope.product.TnvedCode;
            selectedObj.KpvedCode = $scope.product.KpvedCode;
            selectedObj.Price = $scope.product.Price;
            selectedObj.Currency = $scope.product.Currency;
            selectedObj.Series.length = 0;
            selectedObj.Series.push.apply(selectedObj.Series, $scope.productSeries);

            $scope.showAddEditDrugBlock = false;
            $scope.clearSearchAndProductFields();
            $scope.mode = 0;
        }
    }

    $scope.cancelSaveProduct = function cancelSaveProduct() {
        $scope.showAddEditDrugBlock = false;
        $scope.clearSearchAndProductFields();
    }

    $scope.existInArray = function existInArray(arr, id) {
        if (arr && id) {
            for (var i = 0; i < arr.length; i++) {
                if (arr[i].Id == id) {
                    return true;
                }
            }
        }
        return false;
    }

    $scope.clearSearchAndProductFields = function clearSearchAndProductFields() {
        $scope.object.drugNumber = null;
        $scope.object.drugRegType = 1;
        $scope.object.drugTradeName = null;
        $scope.object.drugManufacturer = null;
        $scope.object.drugMnn = null;

        $scope.gridOptions.data.length = 0;

        $scope.product.Id = null;
        $scope.product.NameRu = null;
        $scope.product.NameKz = null;
        $scope.product.ProducerNameRu = null;
        $scope.product.ProducerNameKz = null;
        $scope.product.CountryNameRu = null;
        $scope.product.CountryNameKz = null;
        $scope.product.TnvedCode = null;
        $scope.product.KpvedCode = null;
        $scope.product.Price = null;
        $scope.product.Currency = null;

        $scope.object.seriesValue = null;
        $scope.object.partValue = null;
        $scope.object.seriesUnit = null;

        // clear series table
        $scope.productSeries.length = 0;
    }

    //$scope.disableSelectingAddedProductsGrid = function disableSelectingAddedProductsGrid() {
    //    $scope.addedProductsGridSelectable = false;
    //}

    //$scope.enableSelectingAddedProductsGrid = function enableSelectingAddedProductsGrid {
    //    $scope.addedProductsGridSelectable = true;
    //}

    $scope.resetMode = function resetMode() {
        $scope.mode = 0; // 0 - unknown, 1 - add, 2 - edit
    }

    $scope.editProject = function () {
        $http({
            url: '/OBKContract/ContractSave',
            method: 'POST',
            data: JSON.stringify($scope.object)
        }).success(function (response) {
            debugger;

            $scope.object.Id = response.Id;
            $scope.object.Number = response.Number;
            $scope.object.CreatedDate = getDate(response.CreatedDate);
            $scope.object.Status = response.Status;
            //$scope.object.Contract.StatusId = response.Id;
            //$scope.object.Contract.ContractStatus = response;
            alert('Ок');
        });
        //$scope.isSendProjectVisible = true;
        //$scope.isEnableDownload = true;
    }


    $scope.companyChange = function () {
        if ($scope.object.DeclarantId == "00000000-0000-0000-0000-000000000000") {
            $scope.enableCompanyData = true;
            $scope.showResidentsBlock = true;
            $scope.showHideBin();
            $scope.loadOrganizationData(null);
        }
        else {
            $scope.enableCompanyData = false;
            $scope.showResidentsBlock = false;
            $scope.hideBin();
            var orgGuid = $scope.object.DeclarantId;
            $scope.loadOrganizationData(orgGuid);
        }
    }

    $scope.loadOrganizationData = function (id) {
        if (id) {
            $http({
                method: 'GET',
                url: '/OBKContract/GetOrganizationData',
                params: {
                    guid: id
                }
            }).then(function (resp) {
                if (resp.data) {
                    $scope.object.DeclarantOrganizationFormId = resp.data.OrganizationFormId;
                    $scope.object.DeclarantNameKz = resp.data.NameKz;
                    $scope.object.DeclarantNameRu = resp.data.NameRu;
                    $scope.object.DeclarantNameEn = resp.data.NameEn;
                    $scope.object.DeclarantCountryId = resp.data.CountryId;
                    $scope.object.AddressLegalRu = resp.data.AddressLegalRu;
                    $scope.object.AddressLegalKz = resp.data.AddressLegalKz
                    $scope.object.AddressFact = resp.data.AddressFact;
                    $scope.object.Phone = resp.data.Phone;
                    $scope.object.Email = resp.data.Email;
                    $scope.object.BossLastName = resp.data.BossLastName;
                    $scope.object.BossFirstName = resp.data.BossFirstName;
                    $scope.object.BossMiddleName = resp.data.BossMiddleName;
                    $scope.object.BossPosition = resp.data.BossPosition;
                    $scope.object.BossDocType = resp.data.BossDocType;
                    $scope.object.IsHasBossDocNumber = resp.data.IsHasBossDocNumber;
                    $scope.object.BossDocNumber = resp.data.BossDocNumber;
                    $scope.object.BossDocCreatedDate = getDate(resp.data.BossDocCreatedDate);
                    $scope.object.SignLastName = resp.data.SignLastName;
                    $scope.object.SignFirstName = resp.data.SignFirstName;
                    $scope.object.SignMiddleName = resp.data.SignMiddleName;
                    $scope.object.SignPosition = resp.data.SignPosition;
                    $scope.object.SignDocType = resp.data.SignDocType;
                    $scope.object.IsHasSignDocNumber = resp.data.IsHasSignDocNumber;
                    $scope.object.SignDocNumber = resp.data.SignDocNumber;
                    $scope.object.SignDocCreatedDate = getDate(resp.data.SignDocCreatedDate);
                    $scope.object.BankIik = resp.data.BankIik;
                    $scope.object.BankBik = resp.data.BankBik;
                    $scope.object.CurrencyId = resp.data.CurrencyId;
                    $scope.object.BankNameRu = resp.data.BankNameRu;
                    $scope.object.BankNameKz = resp.data.BankNameKz;
                }
            }, function (response) {
                alert(JSON.stringify(response));
            });
        }
        else {
            $scope.object.DeclarantOrganizationFormId = null;
            $scope.object.DeclarantNameKz = null;
            $scope.object.DeclarantNameRu = null;
            $scope.object.DeclarantNameEn = null;
            $scope.object.DeclarantCountryId = null;
            $scope.object.AddressLegalRu = null;
            $scope.object.AddressLegalKz = null
            $scope.object.AddressFact = null;
            $scope.object.Phone = null;
            $scope.object.Email = null;
            $scope.object.BossLastName = null;
            $scope.object.BossFirstName = null;
            $scope.object.BossMiddleName = null;
            $scope.object.BossPosition = null;
            $scope.object.BossDocType = null;
            $scope.object.IsHasBossDocNumber = null;
            $scope.object.BossDocNumber = null;
            $scope.object.BossDocCreatedDate = getDate(null);
            $scope.object.SignLastName = null;
            $scope.object.SignFirstName = null;
            $scope.object.SignMiddleName = null;
            $scope.object.SignPosition = null;
            $scope.object.SignDocType = null;
            $scope.object.IsHasSignDocNumber = null;
            $scope.object.SignDocNumber = null;
            $scope.object.SignDocCreatedDate = getDate(null);
            $scope.object.BankIik = null;
            $scope.object.BankBik = null;
            $scope.object.CurrencyId = null;
            $scope.object.BankNameRu = null;
            $scope.object.BankNameKz = null;
        }
    }

    $scope.showHideBin = function () {
        if ($scope.object.IsResident == 1) {
            $scope.showBin = true;
        }
        else {
            $scope.showBin = false;
        }
    }

    $scope.hideBin = function () {
        $scope.showBin = false;
    }

    $scope.residentChange = function () {
        $scope.showHideBin();
    }
}

function getDate(value) {
    if (value) {
        var dt = new Date(parseInt(value.substr(6)));
        return dt;
    }
    return "";
}

function convertDateToString(dateMilliseconds) {
    var d = new Date();
    d.setTime(dateMilliseconds);
    if (!d) {
        return null;
    }
    var yyyy = d.getFullYear();
    var mm = d.getMonth() < 9 ? "0" + (d.getMonth() + 1) : (d.getMonth() + 1);
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