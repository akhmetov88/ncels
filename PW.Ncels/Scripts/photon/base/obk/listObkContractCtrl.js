function obkContractForm($scope, $http, $interval, $uibModal, $window) {
    $scope.ExpertOrganizations = [];
    $scope.ContractSigners = [];

    $scope.showContactInformation = false;

    $scope.declarantNotFound = false;

    $scope.iinSearchActive = false;

    $scope.searchViewMode = "drugform";

    $scope.mtParts = [];
    $scope.drugForms = [];
    $scope.selectedMtParts = [];

    var curDate = new Date();
    $scope.mode = 0; // 0 - unknown, 1 - add, 2 - edit
    $scope.declarant = {};
    $scope.object = {};

    $scope.object.Status = 1;

    $scope.showComments = false;
    $scope.viewMpde = false;

    $scope.declarant.IsResident = true;
    $scope.object.seriesValue = "";
    $scope.object.seriesCreateDate = curDate.getTime();
    $scope.object.seriesExpireDate = curDate.getTime();

    $scope.object.BossDocCreatedDate = "";
    $scope.object.BossDocEndDate = "";
    $scope.object.BossDocUnlimited = false;
    $scope.object.SignDocCreatedDate = "";
    $scope.object.SignDocEndDate = "";
    $scope.object.SignDocUnlimited = false;

    $scope.enableCompanyData = false;
    $scope.showBin = false;
    $scope.showResidentsBlock = false;

    $scope.productSeries = [];

    $scope.addedProducts = [];

    $scope.product = {};

    $scope.gridOptions = {
        enableRowSelection: true,
        enableRowHeaderSelection: false,
        multiSelect: false,
        noUnselect: true
    };

    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridOptionsApi = gridApi;

        gridApi.selection.on.rowSelectionChanged($scope, function (row) {
            $scope.product.ProductId = row.entity.ProductId;
            $scope.product.RegTypeId = row.entity.RegTypeId;
            $scope.product.DegreeRiskId = row.entity.DegreeRiskId;
            $scope.product.NameRu = row.entity.Name;
            $scope.product.NameKz = row.entity.NameKz;
            $scope.product.ProducerNameRu = row.entity.ProducerName;
            $scope.product.ProducerNameKz = row.entity.ProducerNameKz;
            $scope.product.CountryNameRu = row.entity.CountryName;
            $scope.product.CountryNameKz = row.entity.CountryNameKz;
            $scope.product.TnvedCode = row.entity.TnvedCode;
            $scope.product.KpvedCode = row.entity.KpvedCode;
            $scope.product.Price = row.entity.Price;
            $scope.product.Currency = row.entity.Currency;

            $scope.selectedMtParts.length = 0;

            $scope.loadDrugFormsAndMtParts();

            $scope.loadProductServiceNames();

            $scope.object.ProductServiceName = null;
        });
    };

    $scope.gridOptionsDrugForm = {
        enableRowSelection: true,
        enableRowHeaderSelection: false,
        multiSelect: false,
        noUnselect: true
    };

    $scope.gridOptionsDrugForm.columnDefs = [
        { name: 'Id', displayName: 'ИД', visible: false },
        { name: 'RegisterId', displayName: 'ИД продукта', visible: false },
        { name: 'BoxCount', displayName: 'Кол-во в потр.уп.', visible: true },
        { name: 'FullName', displayName: 'Полное наименование', visible: true },
        { name: 'FullNameKz', displayName: 'Полное наименование', visible: false }
    ];

    $scope.gridOptionsDrugForm.data = $scope.drugForms;

    $scope.gridOptionsDrugForm.onRegisterApi = function (gridApi) {
        $scope.gridOptionsDrugFormApi = gridApi;

        $scope.gridOptionsDrugFormApi.selection.on.rowSelectionChanged($scope, function (row) {
            $scope.product.DrugFormId = row.entity.Id;
            $scope.product.DrugFormRegisterId = row.entity.RegisterId;
            $scope.product.DrugFormBoxCount = row.entity.BoxCount;
            $scope.product.DrugFormFullName = row.entity.FullName;
            $scope.product.DrugFormFullNameKz = row.entity.FullNameKz;
        });
    };

    $scope.clearDrugFormComponents = function () {
        $scope.product.DrugFormId = null;
        $scope.product.DrugFormRegisterId = null;
        $scope.product.DrugFormBoxCount = null;
        $scope.product.DrugFormFullName = null;
        $scope.product.DrugFormFullNameKz = null;
    }

    $scope.gridOptionsMtPart = {
    };

    $scope.gridOptionsMtPart.columnDefs = [
        { name: "Selected", displayName: "Выбор", visible: true, type: "boolean", cellTemplate: '<input type="checkbox" ng-model="row.entity.Selected" ng-change="grid.appScope.cBoxChange(row);">' },
        { name: 'Id', displayName: 'ИД', visible: false },
        { name: 'RegisterId', displayName: 'ИД продукции', visible: false },
        { name: 'PartNumber', displayName: '№', visible: true },
        { name: 'Model', displayName: 'Модель', visible: true },
        { name: 'Specification', displayName: 'Тех.характеристика', visible: true },
        { name: 'SpecificationKz', displayName: 'Тех.характеристика', visible: false },
        { name: 'Name', displayName: 'Наименование изделия', visible: true },
        { name: 'NameKz', displayName: 'Наименование изделия', visible: false },
        { name: 'ProducerName', displayName: 'Производитель', visible: true },
        { name: 'ProducerNameKz', displayName: 'Производитель', visible: false },
        { name: 'CountryName', displayName: 'Страна', visible: true },
        { name: 'CountryNameKz', displayName: 'Страна', visible: false }
    ];

    $scope.gridOptionsMtPart.data = $scope.mtParts;

    $scope.gridOptionsMtPart.onRegisterApi = function (gridApi) {
        $scope.gridOptionsMtPartApi = gridApi;
    };

    $scope.cBoxChange = function (row) {
        if (row.entity.Selected) {
            $scope.addItemToSelectedMtParts(row.entity);
        }
        else {
            $scope.removeItemFromSelectedMtParts(row.entity);
        }
    }

    $scope.changeViewMode = function () {
        // 1 Черновик
        if ($scope.object.Status == 1) {
            $scope.object.viewMpde = false;
        }
        // 7 На корректировке у заявителя
        else if ($scope.object.Status == 7) {
            $scope.object.viewMpde = false;
            $scope.showComments = true;
        }
        else {
            $scope.object.viewMpde = true;
        }
    }

    $scope.addItemToSelectedMtParts = function (item) {
        if (!$scope.selectedMtPartsContainsItem(item)) {
            var mtPart = {
                Id: item.Id,
                RegisterId: item.RegisterId,
                PartNumber: item.PartNumber,
                PartNumber: item.PartNumber,
                Model: item.Model,
                Specification: item.Specification,
                SpecificationKz: item.SpecificationKz,
                Name: item.Name,
                NameKz: item.NameKz,
                ProducerName: item.ProducerName,
                ProducerNameKz: item.ProducerNameKz,
                CountryName: item.CountryName,
                CountryNameKz: item.CountryNameKz
            };
            $scope.selectedMtParts.push(mtPart);
        }
    }

    $scope.removeItemFromSelectedMtParts = function (item) {
        for (var i = $scope.selectedMtParts.length - 1; i >= 0; i--) {
            if ($scope.selectedMtParts[i].Id == item.Id) {
                $scope.selectedMtParts.splice(i, 1);
            }
        }
    }

    $scope.selectedMtPartsContainsItem = function (item) {
        var res = false;
        for (var i = 0; i < $scope.selectedMtParts.length; i++) {
            if ($scope.selectedMtParts[i].Id === item.Id) {
                res = true;
                break;
            }
        }
        return res;
    }


    $scope.gridOptionsSelectedMtPart = {
        enableRowSelection: true,
        enableRowHeaderSelection: false,
        multiSelect: false,
        noUnselect: true
    };

    $scope.gridOptionsSelectedMtPart.columnDefs = [
        { name: 'Id', displayName: 'ИД', visible: false },
        { name: 'RegisterId', displayName: 'ИД продукции', visible: false },
        { name: 'PartNumber', displayName: '№', visible: true },
        { name: 'Model', displayName: 'Модель', visible: true },
        { name: 'Specification', displayName: 'Тех.характеристика', visible: true },
        { name: 'SpecificationKz', displayName: 'Тех.характеристика', visible: false },
        { name: 'Name', displayName: 'Наименование изделия', visible: true },
        { name: 'NameKz', displayName: 'Наименование изделия', visible: false },
        { name: 'ProducerName', displayName: 'Производитель', visible: true },
        { name: 'ProducerNameKz', displayName: 'Производитель', visible: false },
        { name: 'CountryName', displayName: 'Страна', visible: true },
        { name: 'CountryNameKz', displayName: 'Страна', visible: false }
    ];

    $scope.gridOptionsSelectedMtPart.data = $scope.selectedMtParts;

    $scope.gridOptionsSelectedMtPart.onRegisterApi = function (gridApi) {
        $scope.gridOptionsSelectedMtPartApi = gridApi;

        $scope.gridOptionsSelectedMtPartApi.selection.on.rowSelectionChanged($scope, function (row) {
            //alert("Selected MtPart Selected");
        });
    };

    $scope.loadDrugFormsAndMtParts = function () {
        if ($scope.product.ProductId) {
            if ($scope.product.RegTypeId == 1) {
                $scope.loadDrugForms($scope.product.ProductId);
                $scope.loadMtParts(null);
            }
            else if ($scope.product.RegTypeId == 2) {
                $scope.loadDrugForms($scope.product.ProductId);
                $scope.loadMtParts($scope.product.ProductId);
            }
        }
        else {
            $scope.loadDrugForms(null);
            $scope.loadMtParts(null);
        }
    }

    $scope.loadDrugForms = function (productId) {
        $scope.drugForms.length = 0;
        if (productId) {
            $http({
                method: 'GET',
                url: '/OBKContract/GetDrugForms',
                params: {
                    productId: productId
                }
            }).then(function (resp) {
                if (resp.data) {
                    $scope.drugForms.push.apply($scope.drugForms, resp.data);
                }
            });
        }
    }

    $scope.loadMtParts = function (productId) {
        $scope.mtParts.length = 0;
        if (productId) {
            $http({
                method: 'GET',
                url: '/OBKContract/GetMtParts',
                params: {
                    productId: productId
                }
            }).then(function (resp) {
                if (resp.data) {
                    $scope.mtParts.push.apply($scope.mtParts, resp.data);
                }
            });
        }
    }


    $scope.gridOptions.columnDefs = [
    { name: 'ProductId', displayName: 'ProductId', visible: false },
    { name: 'RegNumber', displayName: 'Рег. номер' },
    { name: 'RegTypeName', displayName: 'Тип' },
    { name: 'RegTypeId', displayName: 'Тип - ИД', visible: false },
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
    { name: 'Currency', displayName: 'Валюта', visible: false },
    { name: 'DegreeRiskId', displayName: 'Класс ИМН', visible: false }
    ];

    // gridSeries
    $scope.gridOptionsSeries = {
        enableRowSelection: true,
        enableRowHeaderSelection: false,
        multiSelect: false,
        noUnselect: true
    };

    $scope.selectedSeriesIndex = null;

    $scope.gridOptionsSeries.onRegisterApi = function (gridApi) {
        $scope.gridOptionsSeriesApi = gridApi;
        gridApi.selection.on.rowSelectionChanged($scope, function (row) {
            $scope.selectedSeriesIndex = $scope.gridOptionsSeries.data.indexOf(row.entity);
        });
    };

    $scope.gridOptionsSeries.columnDefs = [
{ name: 'Id', displayName: 'ID', visible: false },
{ name: 'Series', displayName: 'Номер серии' },
{ name: 'CreateDate', displayName: 'Произведена' },
{ name: 'ExpireDate', displayName: 'Истекает' },
{ name: 'Part', displayName: 'Размер партии' },
{ name: 'UnitName', displayName: 'Ед. измерения' },
{ name: 'UnitId', displayName: 'Ед. измерения - код', visible: false },
{ name: 'ButtonComments', displayName: '', cellTemplate: '<span class="input-group-addon"><a valval="{{row.entity.Id}}" class="obkproductseriedialog" href="#"><i class="glyphicon glyphicon-info-sign"></i></a></span>' }
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
        });
    };

    $scope.gridOptionsProducts.columnDefs = [
        { name: 'Id', displayName: 'Id', visible: false },
        { name: 'ProductId', displayName: 'ProductId', visible: false },
        { name: 'NameRu', displayName: 'Наименование' },
        { name: 'ProducerNameRu', displayName: 'Производитель' },
        { name: 'CountryNameRu', displayName: 'Страна-производитель' },
        { name: 'ButtonComments', displayName: '', cellTemplate: '<span class="input-group-addon"><a valval="{{row.entity.Id}}" class="obkproductdialog" href="#"><i class="glyphicon glyphicon-info-sign"></i></a></span>' }
    ];

    $scope.addedProducts = [];

    $scope.gridOptionsProducts.data = $scope.addedProducts;

    // Grid Products End

    $scope.showAddEditDrugBlock = false;
    $scope.showSearchDrugInReestr = false;
    $scope.object.drugRegType = 1;
    $scope.object.drugEndDateExpired = false;
    $scope.searchResults = null;


    loadExpertOrganizations($scope, $http);
    loadContractSigners($scope, $http);
    loadDictionary($scope, 'Currency', $http);
    loadObkRefTypes($scope, $http);
    loadObkOrganizations($scope, $http);
    loadDictionary($scope, 'OpfType', $http);
    loadDictionary($scope, 'Country', $http);
    loadDictionaryOBKContractDocumentType($scope, $http);
    loadDictionaryMeasure($scope, $http);

    $scope.BoolDic = [{
        Id: true,
        Name: "Да"
    }, {
        Id: false,
        Name: "Нет"
    }];

    $scope.loadNamesNonResidents = function () {
        $scope.NamesNonResidents = [];

        $http({
            method: 'GET',
            url: '/OBKContract/GetNamesNonResidents',
            params: {
                countryId: $scope.object.Country
            }
        }).then(function (resp) {
            if (resp.data) {
                $scope.NamesNonResidents = resp.data;
            }
        });
    }

    $scope.loadNamesNonResidents();

    $scope.nonResidentCountryChange = function () {
        $scope.loadNamesNonResidents();
        $scope.object.NameNonResident = null;
        $scope.declarantNotFound = false;
        $scope.showContactInformation = false;

        $scope.clearDeclarantForm();
        $scope.removeDeclarantId();

        $scope.clearContactForm();
        $scope.clearContactData();
    }

    $scope.searchDrug = function () {
        var drugRegType = $scope.object.drugRegType;
        var drugNumber = $scope.object.drugNumber;
        var drugTradeName = $scope.object.drugTradeName;
        var drugEndDateExpired = $scope.object.drugEndDateExpired;

        $scope.searchViewMode = "drugform";
        $scope.mtParts.length = 0;
        $scope.drugForms.length = 0;

        $http({
            method: 'GET',
            url: '/OBKContract/SearchDrug',
            params: {
                regType: drugRegType,
                drugNumber: drugNumber,
                drugTradeName: drugTradeName,
                drugEndDateExpired: drugEndDateExpired
            }
        }).then(function (resp) {
            if (resp.data) {
                $scope.formatArray(resp.data);

                $scope.searchResults = resp.data;
                $scope.gridOptions.data = resp.data;
            }
            else {
                $scope.searchResults = null;
                $scope.gridOptions.data = null;
            }
        }, function (response) {
            alert(JSON.stringify(response));
        });

        $scope.clearDrugFormComponents();
    };

    $scope.clearSearchDrugArea = function () {
        $scope.object.drugNumber = null;
        $scope.object.drugRegType = 1;
        $scope.object.drugEndDateExpired = false;
        $scope.object.drugTradeName = null;
    }

    $scope.formatArray = function (arr) {
        if (arr && arr.length) {
            for (var i = 0; i < arr.length; i++) {
                if (arr[i].RegDate) {
                    arr[i].RegDate = convertDateToStringDDMMYYYY(arr[i].RegDate);
                }
                if (arr[i].ExpireDate) {
                    arr[i].ExpireDate = convertDateToStringDDMMYYYY(arr[i].ExpireDate);
                }
            }
        }
    }

    $scope.addDrug = function addDrug() {
        $scope.showAddEditDrugBlock = true;
        $scope.showSearchDrugInReestr = true;
        $scope.mode = 1;
    };

    $scope.editDrug = function editDrug() {
        if ($scope.selectedProductIndex != null) {
            $scope.deletedServicesId = [];
            $scope.mode = 2;

            $scope.showAddEditDrugBlock = true;
            $scope.showSearchDrugInReestr = false;

            var selectedObj = $scope.addedProducts[$scope.selectedProductIndex];

            $scope.product.Id = selectedObj.Id;
            $scope.product.ProductId = selectedObj.ProductId;
            $scope.product.RegTypeId = selectedObj.RegTypeId;
            $scope.product.DegreeRiskId = selectedObj.DegreeRiskId;
            $scope.product.NameRu = selectedObj.NameRu;
            $scope.product.NameKz = selectedObj.NameKz;
            $scope.product.ProducerNameRu = selectedObj.ProducerNameRu;
            $scope.product.ProducerNameKz = selectedObj.ProducerNameKz;
            $scope.product.CountryNameRu = selectedObj.CountryNameRu;
            $scope.product.CountryNameKz = selectedObj.CountryNameKz;
            $scope.product.Price = selectedObj.Price;
            $scope.product.Currency = selectedObj.Currency;
            $scope.product.DrugFormId = selectedObj.DrugFormId;
            $scope.product.DrugFormRegisterId = selectedObj.DrugFormRegisterId;
            $scope.product.DrugFormBoxCount = selectedObj.DrugFormBoxCount;
            $scope.product.DrugFormFullName = selectedObj.DrugFormFullName;
            $scope.product.DrugFormFullNameKz = selectedObj.DrugFormFullNameKz;

            $scope.productSeries.push.apply($scope.productSeries, selectedObj.Series);
            $scope.selectedMtParts.push.apply($scope.selectedMtParts, selectedObj.MtParts);

            for (var i = 0; i < $scope.addedServices.length; i++) {
                var service = $scope.addedServices[i];
                if (service.ProductId == selectedObj.Id) {
                    var newService = {
                        Id: service.Id,
                        ServiceName: service.ServiceName,
                        ServiceId: service.ServiceId,
                        UnitOfMeasurementName: service.UnitOfMeasurementName,
                        UnitOfMeasurementId: service.UnitOfMeasurementId,
                        PriceWithoutTax: service.PriceWithoutTax,
                        Count: service.Count,
                        FinalCostWithoutTax: service.FinalCostWithoutTax,
                        FinalCostWithTax: service.FinalCostWithTax,
                    };
                    $scope.addedProductServices.push(newService);
                }
            }

            $scope.loadProductServiceNames();
        }
        else {
            alert("Выберите продукцию для изменения");
        }
    }

    $scope.deleteDrug = function deleteDrug() {
        if ($scope.selectedProductIndex != null) {
            var deleteConfirmed = confirm("Вы подтверждаете удаление продукции со списка?");
            if (deleteConfirmed) {
                var selectedObj = $scope.addedProducts[$scope.selectedProductIndex];
                var id = selectedObj.Id;
                for (var i = $scope.addedServices.length - 1; i >= 0; i--) {
                    if ($scope.addedServices[i].ProductId == selectedObj.ProductId) {
                        $scope.addedServices.splice(i, 1);
                    }
                }
                $scope.calcTotalCostCalculator();

                $scope.deleteProductInformation(id);

                $scope.addedProducts.splice($scope.selectedProductIndex, 1);
                $scope.selectedProductIndex = null;
            }
        }
        else {
            alert("Выберите продукцию для удаления");
        }
    }

    $scope.addSeries = function addSeries() {
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
            var id = $scope.product.ProductId;
            if (id) {
                if (!$scope.existInArray($scope.addedProducts, id)) {
                    if ($scope.drugForms.length == 0 || ($scope.drugForms.length > 0 && $scope.product.DrugFormId)) {
                        if ($scope.productSeries.length > 0) {
                            if ($scope.addedProductServices.length > 0) {
                                var product = {
                                    Id: null,
                                    ProductId: $scope.product.ProductId,
                                    RegTypeId: $scope.product.RegTypeId,
                                    DegreeRiskId: $scope.product.DegreeRiskId,
                                    NameRu: $scope.product.NameRu,
                                    NameKz: $scope.product.NameKz,
                                    ProducerNameRu: $scope.product.ProducerNameRu,
                                    ProducerNameKz: $scope.product.ProducerNameKz,
                                    CountryNameRu: $scope.product.CountryNameRu,
                                    CountryNameKz: $scope.product.CountryNameKz,
                                    Price: $scope.product.Price,
                                    Currency: $scope.product.Currency,
                                    DrugFormBoxCount: $scope.product.DrugFormBoxCount,
                                    DrugFormFullName: $scope.product.DrugFormFullName,
                                    DrugFormFullNameKz: $scope.product.DrugFormFullNameKz,
                                    Series: [],
                                    MtParts: []
                                }
                                product.Series = $scope.productSeries.slice();
                                product.MtParts = $scope.selectedMtParts.slice();

                                $scope.addedProducts.push(product);

                                $scope.saveProductInformation(product);

                                $scope.showAddEditDrugBlock = false;
                                $scope.showSearchDrugInReestr = false;
                                $scope.clearSearchAndProductFields();
                                $scope.mode = 0;
                                alert("Информация о продукции добавлена");
                            }
                            else {
                                alert("Введите информацию об услуге");
                            }
                        }
                        else {
                            alert("Введите информацию о серии продукта");
                        }
                    }
                    else {
                        alert("Выберите форму выпуска продукции");
                    }
                }
                else {
                    alert("Выбранная продукция уже имеется в таблице");
                }
            }
            else {
                alert("Выберите продукт");
            }
        }
        if ($scope.mode == 2) {
            var selectedObj = $scope.addedProducts[$scope.selectedProductIndex];
            selectedObj.Id = $scope.product.Id;
            selectedObj.ProductId = $scope.product.ProductId;
            selectedObj.RegTypeId = $scope.product.RegTypeId;
            selectedObj.DegreeRiskId = $scope.product.DegreeRiskId;
            selectedObj.NameRu = $scope.product.NameRu;
            selectedObj.NameKz = $scope.product.NameKz;
            selectedObj.ProducerNameRu = $scope.product.ProducerNameRu;
            selectedObj.ProducerNameKz = $scope.product.ProducerNameKz;
            selectedObj.CountryNameRu = $scope.product.CountryNameRu;
            selectedObj.CountryNameKz = $scope.product.CountryNameKz;
            selectedObj.Price = $scope.product.Price;
            selectedObj.Currency = $scope.product.Currency;
            selectedObj.Series.length = 0;
            selectedObj.Series.push.apply(selectedObj.Series, $scope.productSeries);


            for (var i = $scope.addedServices.length - 1; i >= 0; i--) {
                if ($scope.addedServices[i].ProductId == selectedObj.ProductId) {
                    $scope.addedServices.splice(i, 1);
                }
            }
            $scope.calcTotalCostCalculator();

            $scope.saveProductInformation(selectedObj);

            $scope.showAddEditDrugBlock = false;
            $scope.showSearchDrugInReestr = false;
            $scope.clearSearchAndProductFields();
            $scope.mode = 0;
            alert("Информация о продукции обновлена");
        }
    }

    $scope.saveProductInformation = function (product) {
        var projectId = $scope.object.Id;
        if (projectId) {
            $http({
                url: '/OBKContract/SaveProduct',
                method: 'POST',
                data: { contractId: projectId, product: product }
            }).success(function (response) {
                if (response.ProductId && response.Id) {
                    $scope.updateIdOfProduct(response.ProductId, response.Id);
                }

                for (var i = 0; i < $scope.addedProductServices.length; i++) {
                    var service = $scope.addedProductServices[i];
                    if (service.Id == null) {
                        var newService = {
                            Id: service.Id,
                            ServiceName: service.ServiceName,
                            ServiceId: service.ServiceId,
                            UnitOfMeasurementName: service.UnitOfMeasurementName,
                            UnitOfMeasurementId: service.UnitOfMeasurementId,
                            PriceWithoutTax: service.PriceWithoutTax,
                            Count: service.Count,
                            FinalCostWithoutTax: service.FinalCostWithoutTax,
                            FinalCostWithTax: service.FinalCostWithTax,
                            ProductId: response.Id,
                            ProductName: $scope.product.NameRu
                        };
                        $scope.addedServices.push(newService);
                        var index = $scope.addedServices.length - 1;
                        $scope.saveServiceInformation(newService, index);
                    }
                    else {
                        for (var i = 0; i < $scope.addedServices.length; i++) {
                            if ($scope.addedServices[i].Id != null && $scope.addedServices[i].Id == service.Id) {
                                $scope.addedServices[i].ServiceName = service.ServiceName;
                                $scope.addedServices[i].ServiceId = service.ServiceId,
                                $scope.addedServices[i].UnitOfMeasurementName = service.UnitOfMeasurementName,
                                $scope.addedServices[i].UnitOfMeasurementId = service.UnitOfMeasurementId,
                                $scope.addedServices[i].PriceWithoutTax = service.PriceWithoutTax,
                                $scope.addedServices[i].Count = service.Count,
                                $scope.addedServices[i].FinalCostWithoutTax = service.FinalCostWithoutTax,
                                $scope.addedServices[i].FinalCostWithTax = service.FinalCostWithTax,
                                $scope.addedServices[i].ProductId = response.Id;
                                $scope.saveServiceInformation($scope.addedServices[i], i);
                            }
                        }
                    }
                }

                $scope.addedProductServices.length = 0;

                if ($scope.deletedServicesId && $scope.deletedServicesId.length > 0) {
                    for (var i = 0; i < $scope.deletedServicesId.length; i++) {
                        for (var j = $scope.addedServices.length - 1; j >= 0; j--) {
                            if ($scope.addedServices[j].Id == $scope.deletedServicesId[i]) {
                                $scope.addedServices.splice(j, 1);
                            }
                        }
                        $scope.deleteServiceInformation($scope.deletedServicesId[i]);
                    }
                    $scope.deletedServicesId.length = 0;
                }

                $scope.calcTotalCostCalculator();
            });
        }
    }

    $scope.deleteProductInformation = function (id) {
        var projectId = $scope.object.Id;
        if (projectId) {
            $http({
                url: '/OBKContract/DeleteProduct',
                method: 'POST',
                data: { contractId: projectId, productId: id }
            }).success(function (response) {
            });
        }
    }

    $scope.updateIdOfProduct = function (productId, id) {
        for (var i = 0; i < $scope.addedProducts.length; i++) {
            if ($scope.addedProducts[i].ProductId == productId && $scope.addedProducts[i].Id == null) {
                $scope.addedProducts[i].Id = id;
            }
        }
    }

    $scope.cancelSaveProduct = function cancelSaveProduct() {
        $scope.showAddEditDrugBlock = false;
        $scope.showSearchDrugInReestr = false;
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
        $scope.object.drugEndDateExpired = false;
        $scope.object.drugTradeName = null;

        $scope.gridOptions.data.length = 0;
        //
        $scope.mtParts.length = 0;
        $scope.drugForms.length = 0;

        $scope.product.ProductId = null;
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
        $scope.clearDrugFormComponents();

        $scope.object.seriesValue = null;
        $scope.object.partValue = null;
        $scope.object.seriesUnit = null;

        // clear series table
        $scope.productSeries.length = 0;

        // clear mt parts
        $scope.selectedMtParts.length = 0;
    }

    $scope.resetMode = function resetMode() {
        $scope.mode = 0; // 0 - unknown, 1 - add, 2 - edit
    }

    $scope.refTypeChange = function (save) {
        $scope.loadServiceNames();
        $scope.loadProductServiceNames();
        if (save) {
            $scope.editProject();
        }
    }

    $scope.loadServiceNames = function loadServiceNames() {
        $scope.serviceNames = [];
        if ($scope.object.Type) {
            $http({
                method: "GET",
                url: "/OBKDictionaries/GetServiceNamesServiceTypeDocument",
                data: "JSON",
                params: {
                    type: $scope.object.Type
                }
            }).success(function (result) {
                $scope.serviceNames = result;
            });
        }
    }

    $scope.saveBtnClick = function () {
        var formValid = $scope.contractCreateForm.$valid;
        var filesValid = $scope.checkFileValidation();
        $scope.editProject();
    }

    $scope.editProject = function () {
        var generatedGuid = $("#generatedGuid").val();
        $http({
            url: '/OBKContract/ContractSave',
            method: 'POST',
            data: { Guid: generatedGuid, contractViewModel: $scope.object }
        }).success(function (response) {
            $scope.object.Id = response.Id;
        });
    }


    $scope.companyChange = function (loadOrgData) {

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
                    $scope.declarant.Id = resp.data.Id;
                    $scope.declarant.OrganizationFormId = resp.data.OrganizationFormId;
                    $scope.declarant.NameKz = resp.data.NameKz;
                    $scope.declarant.NameRu = resp.data.NameRu;
                    $scope.declarant.NameEn = resp.data.NameEn;
                    $scope.declarant.CountryId = resp.data.CountryId;

                    $scope.object.AddressLegalRu = resp.data.AddressLegalRu;
                    $scope.object.AddressLegalKz = resp.data.AddressLegalKz
                    $scope.object.AddressFact = resp.data.AddressFact;
                    $scope.object.Phone = resp.data.Phone;
                    $scope.object.Email = resp.data.Email;
                    $scope.object.BossLastName = resp.data.BossLastName;
                    $scope.object.BossFirstName = resp.data.BossFirstName;
                    $scope.object.BossMiddleName = resp.data.BossMiddleName;
                    $scope.object.BossPosition = resp.data.BossPosition;
                    $scope.object.BossPositionKz = resp.data.BossPositionKz;
                    $scope.object.BossDocType = resp.data.BossDocType;
                    $scope.object.IsHasBossDocNumber = resp.data.IsHasBossDocNumber;
                    $scope.object.BossDocNumber = resp.data.BossDocNumber;
                    $scope.object.BossDocCreatedDate = getDate(resp.data.BossDocCreatedDate);
                    $scope.object.BossDocEndDate = getDate(resp.data.BossDocEndDate);
                    $scope.object.BossDocUnlimited = resp.data.BossDocUnlimited;
                    $scope.object.SignerIsBoss = resp.data.SignerIsBoss;
                    $scope.object.SignLastName = resp.data.SignLastName;
                    $scope.object.SignFirstName = resp.data.SignFirstName;
                    $scope.object.SignMiddleName = resp.data.SignMiddleName;
                    $scope.object.SignPosition = resp.data.SignPosition;
                    $scope.object.SignPositionKz = resp.data.SignPositionKz;
                    $scope.object.SignDocType = resp.data.SignDocType;
                    $scope.object.IsHasSignDocNumber = resp.data.IsHasSignDocNumber;
                    $scope.object.SignDocNumber = resp.data.SignDocNumber;
                    $scope.object.SignDocCreatedDate = getDate(resp.data.SignDocCreatedDate);
                    $scope.object.SignDocEndDate = getDate(resp.data.SignDocEndDate);
                    $scope.object.SignDocUnlimited = resp.data.SignDocUnlimited;
                    $scope.object.BankIik = resp.data.BankIik;
                    $scope.object.BankBik = resp.data.BankBik;
                    $scope.object.CurrencyId = resp.data.CurrencyId;
                    $scope.object.BankNameRu = resp.data.BankNameRu;
                    $scope.object.BankNameKz = resp.data.BankNameKz;

                    $scope.saveDeclarantId(resp.data.Id);
                    $scope.editProject();

                    $scope.showContactInformation = true;
                }
            }, function (response) {
                alert(JSON.stringify(response));
            });
        }
        else {
            $scope.declarant.Id = null;
            $scope.declarant.OrganizationFormId = null;
            $scope.declarant.NameKz = null;
            $scope.declarant.NameRu = null;
            $scope.declarant.NameEn = null;
            $scope.declarant.CountryId = null;

            $scope.object.AddressLegalRu = null;
            $scope.object.AddressLegalKz = null
            $scope.object.AddressFact = null;
            $scope.object.Phone = null;
            $scope.object.Email = null;
            $scope.object.BossLastName = null;
            $scope.object.BossFirstName = null;
            $scope.object.BossMiddleName = null;
            $scope.object.BossPosition = null;
            $scope.object.BossPositionKz = null;
            $scope.object.BossDocType = null;
            $scope.object.IsHasBossDocNumber = null;
            $scope.object.BossDocNumber = null;
            $scope.object.BossDocCreatedDate = getDate(null);
            $scope.object.BossDocEndDate = getDate(null);
            $scope.object.BossDocUnlimited = false;
            $scope.object.SignerIsBoss = false;
            $scope.object.SignLastName = null;
            $scope.object.SignFirstName = null;
            $scope.object.SignMiddleName = null;
            $scope.object.SignPosition = null;
            $scope.object.SignPositionKz = null;
            $scope.object.SignDocType = null;
            $scope.object.IsHasSignDocNumber = null;
            $scope.object.SignDocNumber = null;
            $scope.object.SignDocCreatedDate = getDate(null);
            $scope.object.SignDocEndDate = getDate(null);
            $scope.object.SignDocUnlimited = false;
            $scope.object.BankIik = null;
            $scope.object.BankBik = null;
            $scope.object.CurrencyId = null;
            $scope.object.BankNameRu = null;
            $scope.object.BankNameKz = null;
        }
    }

    $scope.clearContactForm = function () {
        $scope.object.AddressLegalRu = null;
        $scope.object.AddressLegalKz = null
        $scope.object.AddressFact = null;
        $scope.object.Phone = null;
        $scope.object.Email = null;
        $scope.object.BossLastName = null;
        $scope.object.BossFirstName = null;
        $scope.object.BossMiddleName = null;
        $scope.object.BossPosition = null;
        $scope.object.BossPositionKz = null;
        $scope.object.BossDocType = null;
        $scope.object.IsHasBossDocNumber = null;
        $scope.object.BossDocNumber = null;
        $scope.object.BossDocCreatedDate = getDate(null);
        $scope.object.BossDocEndDate = getDate(null);
        $scope.object.BossDocUnlimited = false;
        $scope.object.SignerIsBoss = false;
        $scope.object.SignLastName = null;
        $scope.object.SignFirstName = null;
        $scope.object.SignMiddleName = null;
        $scope.object.SignPosition = null;
        $scope.object.SignPositionKz = null;
        $scope.object.SignDocType = null;
        $scope.object.IsHasSignDocNumber = null;
        $scope.object.SignDocNumber = null;
        $scope.object.SignDocCreatedDate = getDate(null);
        $scope.object.SignDocEndDate = getDate(null);
        $scope.object.SignDocUnlimited = false;
        $scope.object.BankIik = null;
        $scope.object.BankBik = null;
        $scope.object.CurrencyId = null;
        $scope.object.BankNameRu = null;
        $scope.object.BankNameKz = null;
    }

    $scope.clearContactData = function () {
        if ($scope.object.Id) {
            $http({
                url: '/OBKContract/ClearContactData',
                method: 'POST',
                data: { contractId: $scope.object.Id }
            }).success(function (response) {
            });
        }
    }

    $scope.showHideBin = function () {
        if ($scope.declarant.IsResident == true) {
            $scope.showBin = true;
            $scope.cancelFindDeclarant();
        }
        else {
            $scope.showBin = false;
            $scope.object.Country = null;
            $scope.object.NameNonResident = null;
            $scope.clearDeclarantForm();
            $scope.removeDeclarantId();
        }
        $scope.declarant.Bin = null;
        $scope.showContactInformation = false;
        $scope.declarantNotFound = false;
        $scope.clearContactForm();
        $scope.clearContactData();
    }

    $scope.hideBin = function () {
        $scope.showBin = false;
    }

    $scope.residentChange = function () {
        $scope.showHideBin();
    }

    $scope.signerIsLeaderCheckBoxChanged = function () {
        if ($scope.object.SignerIsBoss == true) {
            $scope.object.SignLastName = $scope.object.BossLastName;
            $scope.object.SignFirstName = $scope.object.BossFirstName;
            $scope.object.SignMiddleName = $scope.object.BossMiddleName;
            $scope.object.SignPosition = $scope.object.BossPosition;
            $scope.object.SignPositionKz = $scope.object.BossPositionKz;
            $scope.object.SignDocType = $scope.object.BossDocType;
            $scope.object.SignDocUnlimited = $scope.object.BossDocUnlimited;
            $scope.object.IsHasSignDocNumber = $scope.object.IsHasBossDocNumber;
            $scope.object.SignDocNumber = $scope.object.BossDocNumber;
            $scope.object.SignDocCreatedDate = $scope.object.BossDocCreatedDate;
            $scope.object.SignDocEndDate = $scope.object.BossDocEndDate;
        }
        $scope.editProject();
    }

    initProductServiceModule($scope, $http, $interval);

    initCalculator($scope, $interval, $http);

    initExample($scope, $interval);

    $scope.loadContract = function () {
        var projectId = $("#projectId").val();

        $http({
            method: 'GET',
            url: '/OBKContract/LoadContract',
            params: {
                id: projectId
            }
        }).then(function (resp) {
            if (resp.data) {
                $scope.object.Id = resp.data.Id;
                $scope.object.Type = resp.data.Type;
                $scope.object.ExpertOrganization = resp.data.ExpertOrganization;
                $scope.object.Signer = resp.data.Signer;
                $scope.object.Status = resp.data.Status;
                $scope.changeViewMode();


                $scope.refTypeChange(false);

                $http({
                    method: 'GET',
                    url: '/OBKContract/GetDeclarant',
                    params: {
                        contractId: projectId
                    }
                }).then(function (resp) {
                    if (resp.data) {
                        $scope.declarant.Id = resp.data.Id;
                        $scope.declarant.IsResident = resp.data.IsResident;
                        $scope.declarant.OrganizationFormId = resp.data.OrganizationFormId;
                        $scope.declarant.NameKz = resp.data.NameKz;
                        $scope.declarant.NameRu = resp.data.NameRu;
                        $scope.declarant.NameEn = resp.data.NameEn;
                        $scope.declarant.CountryId = resp.data.CountryId;
                        $scope.declarant.Bin = resp.data.Bin;

                        if ($scope.declarant.IsResident) {
                            $scope.showBin = true;
                            if (resp.data.IsConfirmed) {
                                $scope.iinSearchActive = true;
                                $scope.declarantNotFound = false;
                                $scope.enableCompanyData = false;
                                $scope.showContactInformation = true;
                            }
                            else {
                                $scope.iinSearchActive = true;
                                $scope.declarantNotFound = true;
                                $scope.showContactInformation = true;
                                $scope.enableCompanyData = true;
                                $scope.addDeclarantDisabled = true;
                            }
                        }
                        else {
                            $scope.showBin = false;
                            if (resp.data.IsConfirmed) {
                                $scope.object.Country = $scope.declarant.CountryId;
                                $scope.loadNamesNonResidents();
                                $scope.object.NameNonResident = $scope.declarant.Id;

                                $scope.addDeclarantDisabled = true;
                                $scope.showContactInformation = true;
                            }
                            else {
                                $scope.object.Country = $scope.declarant.CountryId;
                                $scope.loadNamesNonResidents();
                                $scope.object.NameNonResident = "00000000-0000-0000-0000-000000000000";
                                $scope.showContactInformation = true;
                                $scope.enableCompanyData = true;
                                $scope.addDeclarantDisabled = true;
                            }
                        }
                    }
                });

                $scope.object.AddressLegalRu = resp.data.AddressLegalRu;
                $scope.object.AddressLegalKz = resp.data.AddressLegalKz;
                $scope.object.AddressFact = resp.data.AddressFact;
                $scope.object.Phone = resp.data.Phone;
                $scope.object.Email = resp.data.Email;
                $scope.object.BossLastName = resp.data.BossLastName;
                $scope.object.BossFirstName = resp.data.BossFirstName;
                $scope.object.BossMiddleName = resp.data.BossMiddleName;
                $scope.object.BossPosition = resp.data.BossPosition;
                $scope.object.BossPositionKz = resp.data.BossPositionKz;
                $scope.object.BossDocType = resp.data.BossDocType;
                $scope.object.IsHasBossDocNumber = resp.data.IsHasBossDocNumber;
                $scope.object.BossDocNumber = resp.data.BossDocNumber;
                $scope.object.BossDocCreatedDate = getDate(resp.data.BossDocCreatedDate);
                $scope.object.BossDocEndDate = getDate(resp.data.BossDocEndDate);
                $scope.object.BossDocUnlimited = resp.data.BossDocUnlimited;
                $scope.object.SignerIsBoss = resp.data.SignerIsBoss;
                $scope.object.SignLastName = resp.data.SignLastName;
                $scope.object.SignFirstName = resp.data.SignFirstName;
                $scope.object.SignMiddleName = resp.data.SignMiddleName;
                $scope.object.SignPosition = resp.data.SignPosition;
                $scope.object.SignPositionKz = resp.data.SignPositionKz;
                $scope.object.SignDocType = resp.data.SignDocType;
                $scope.object.IsHasSignDocNumber = resp.data.IsHasSignDocNumber;
                $scope.object.SignDocNumber = resp.data.SignDocNumber;
                $scope.object.SignDocCreatedDate = getDate(resp.data.SignDocCreatedDate);
                $scope.object.SignDocEndDate = getDate(resp.data.SignDocEndDate);
                $scope.object.SignDocUnlimited = resp.data.SignDocUnlimited;
                $scope.object.BankIik = resp.data.BankIik;
                $scope.object.BankBik = resp.data.BankBik;
                $scope.object.CurrencyId = resp.data.CurrencyId;
                $scope.object.BankNameRu = resp.data.BankNameRu;
                $scope.object.BankNameKz = resp.data.BankNameKz;
            }
        }, function (response) {

        });

        $http({
            method: 'GET',
            url: '/OBKContract/GetProducts',
            params: {
                contractId: projectId
            }
        }).then(function (resp) {
            if (resp.data) {
                $scope.addedProducts.push.apply($scope.addedProducts, resp.data);
            }
        });

        $http({
            method: 'GET',
            url: '/OBKContract/GetContractPrices',
            params: {
                contractId: projectId
            }
        }).then(function (resp) {
            if (resp.data) {
                $scope.addedServices.push.apply($scope.addedServices, resp.data);
                $scope.calcTotalCostCalculator();
            }
        });
    }

    var projectId = $("#projectId").val();
    if (projectId) {
        $scope.loadContract();
    }
    else {

    }


    $scope.checkFileValidation = function () {
        var invalidFiles = 0;

        var containerName = "";
        if ($scope.declarant.IsResident === true) {
            containerName = "#filesResident";
        }
        else {
            containerName = "#filesNonResident";
        }

        $(containerName + ' .file-validation').text("");
        $(containerName + ' .file-validation').each(function () {
            var ct = $(this).attr('countFile');
            var attcode = $(this).attr('attcode');
            var count = parseInt(ct, 10) || 0;
            if (count === 0 && attcode === "1") {
                $(this).text("Необходимо вложить файлы");
                invalidFiles++;
            } else {
                $(this).text("");
            }
        });
        return invalidFiles === 0;
    }

    $scope.findDeclarant = function () {
        if ($scope.declarant.Bin && $scope.declarant.Bin.length == 12) {
            $http({
                method: 'GET',
                url: '/OBKContract/FindDeclarant',
                params: {
                    bin: $scope.declarant.Bin
                }
            }).then(function (resp) {
                $scope.iinSearchActive = true;
                if (resp.data) {
                    $scope.showContactInformation = true;
                    $scope.declarantNotFound = false;
                    $scope.enableCompanyData = false;

                    $scope.declarant.Id = resp.data.Id;
                    $scope.declarant.OrganizationFormId = resp.data.OrganizationFormId;
                    $scope.declarant.IsResident = resp.data.IsResident;
                    $scope.declarant.NameKz = resp.data.NameKz;
                    $scope.declarant.NameRu = resp.data.NameRu;
                    $scope.declarant.NameEn = resp.data.NameEn;
                    $scope.declarant.CountryId = resp.data.CountryId;

                    $scope.object.AddressLegalRu = resp.data.AddressLegalRu;
                    $scope.object.AddressLegalKz = resp.data.AddressLegalKz
                    $scope.object.AddressFact = resp.data.AddressFact;
                    $scope.object.Phone = resp.data.Phone;
                    $scope.object.Email = resp.data.Email;
                    $scope.object.BossLastName = resp.data.BossLastName;
                    $scope.object.BossFirstName = resp.data.BossFirstName;
                    $scope.object.BossMiddleName = resp.data.BossMiddleName;
                    $scope.object.BossPosition = resp.data.BossPosition;
                    $scope.object.BossPositionKz = resp.data.BossPositionKz;
                    $scope.object.BossDocType = resp.data.BossDocType;
                    $scope.object.IsHasBossDocNumber = resp.data.IsHasBossDocNumber;
                    $scope.object.BossDocNumber = resp.data.BossDocNumber;
                    $scope.object.BossDocCreatedDate = getDate(resp.data.BossDocCreatedDate);
                    $scope.object.BossDocEndDate = getDate(resp.data.BossDocEndDate);
                    $scope.object.BossDocUnlimited = resp.data.BossDocUnlimited;
                    $scope.object.SignerIsBoss = resp.data.SignerIsBoss;
                    $scope.object.SignLastName = resp.data.SignLastName;
                    $scope.object.SignFirstName = resp.data.SignFirstName;
                    $scope.object.SignMiddleName = resp.data.SignMiddleName;
                    $scope.object.SignPosition = resp.data.SignPosition;
                    $scope.object.SignPositionKz = resp.data.SignPositionKz;
                    $scope.object.SignDocType = resp.data.SignDocType;
                    $scope.object.IsHasSignDocNumber = resp.data.IsHasSignDocNumber;
                    $scope.object.SignDocNumber = resp.data.SignDocNumber;
                    $scope.object.SignDocCreatedDate = getDate(resp.data.SignDocCreatedDate);
                    $scope.object.SignDocEndDate = getDate(resp.data.SignDocEndDate);
                    $scope.object.SignDocUnlimited = resp.data.SignDocUnlimited;
                    $scope.object.BankIik = resp.data.BankIik;
                    $scope.object.BankBik = resp.data.BankBik;
                    $scope.object.CurrencyId = resp.data.CurrencyId;
                    $scope.object.BankNameRu = resp.data.BankNameRu;
                    $scope.object.BankNameKz = resp.data.BankNameKz;
                    $scope.saveDeclarantId(resp.data.Id);
                }
                else {
                    $scope.declarantNotFound = true;
                    $scope.addDeclarantDisabled = false;
                    $scope.enableCompanyData = true;

                    $scope.declarant.Id = null;
                }
            }, function (response) {

            });
        }
    }

    $scope.saveDeclarantId = function (declarantId) {
        if ($scope.object.Id) {
            $http({
                url: '/OBKContract/SaveDeclarantId',
                method: 'POST',
                data: { contractId: $scope.object.Id, declarantId: declarantId }
            }).success(function (response) {
            });
        }
    }

    $scope.clearDeclarantForm = function () {
        $scope.declarant.Id = null;
        $scope.declarant.OrganizationFormId = null;
        $scope.declarant.NameKz = null;
        $scope.declarant.NameRu = null;
        $scope.declarant.NameEn = null;
        $scope.declarant.CountryId = null;
    }

    $scope.removeDeclarantId = function () {
        if ($scope.object.Id) {
            $http({
                url: '/OBKContract/RemoveDeclarantId',
                method: 'POST',
                data: { contractId: $scope.object.Id }
            }).success(function (response) {
            });
        }
    }

    $scope.addDeclarant = function () {
        $scope.showContactInformation = true;
        $scope.enableCompanyData = true;
        $scope.addDeclarantDisabled = true;

        if ($scope.declarant.IsResident == false) {
            $scope.declarant.CountryId = $scope.object.Country;
        }
        else if ($scope.declarant.IsResident == true) {
            $scope.declarant.CountryId = null;
        }
    }

    $scope.cancelFindDeclarant = function () {
        $scope.iinSearchActive = false;
        $scope.declarantNotFound = false;
        $scope.showContactInformation = false;
        $scope.addDeclarantDisabled = false;

        $scope.clearDeclarantForm();

        $scope.removeDeclarantId();

        $scope.clearContactForm();

        $scope.clearContactData();
    }

    $scope.editDeclarant = function () {
        if ($scope.object.Id) {
            $http({
                url: '/OBKContract/ContractDeclarantSave',
                method: 'POST',
                data: { guid: $scope.object.Id, declarantViewModel: $scope.declarant }
            }).success(function (response) {
                if (!response.Exist) {
                    $scope.declarant.Id = response.DeclarantId;
                }
                else {
                    if (response.IsResident) {
                        alert("Заявитель с указанным ИИН/БИН уже существует!");
                    }
                    else {
                        alert("Заявитель с указанной страной и наименованием уже существует!");
                    }
                }
            });
        }
    }

    $scope.nameNonResidentChange = function () {
        $scope.showContactInformation = false;
        if ($scope.object.NameNonResident) {
            if ($scope.object.NameNonResident == "00000000-0000-0000-0000-000000000000") {
                $scope.declarantNotFound = true;
                $scope.addDeclarantDisabled = false;
                $scope.clearDeclarantForm();
                $scope.removeDeclarantId();
                $scope.clearContactForm();
                $scope.clearContactData();
            }
            else {
                $scope.declarantNotFound = false;
                $scope.addDeclarantDisabled = true;
                $scope.loadOrganizationData($scope.object.NameNonResident);
            }
        }
    }

    
    $scope.sendWithoutDigitalSign = function () {
        var formValid = $scope.contractCreateForm.$valid;
        //alert("formValid = " + formValid);
        var outputErrors = true;
        if (!formValid && $scope.contractCreateForm.$error && outputErrors) {
            //var errors = [];

            //for (var key in $scope.contractCreateForm.$error) {
            //    for (var index = 0; index < $scope.contractCreateForm.$error[key].length; index++) {
            //        errors.push($scope.contractCreateForm.$error[key][index].$name + ' is required.');
            //    }
            //}

            //for (var i = 0; i < errors.length; i++) {
            //    alert(errors[i]);
            //}
        }
        var filesValid = $scope.checkFileValidation();
        //alert("filesValid = " + filesValid);
        if (formValid && filesValid) {
            var modalInstance = $uibModal.open({
                templateUrl: '/Project/Agreement',
                controller: modalSendContract,
                scope: $scope,
                size: 'size-custom'
            });
        }
    }

    $scope.viewContract = function (id) {
        debugger;
        if ($scope.object.Type == null) {
            alert("Выберите тип договра");
            return;
        }
        var modalInstance = $uibModal.open({
            templateUrl: '/OBKContract/ContractTemplate?id=' + id,
            controller: ModalRegisterInstanceCtrl
        });
    };
}

function ModalRegisterInstanceCtrl($scope, $uibModalInstance) {
    debugger;
    $scope.ok = function () {
        $uibModalInstance.close();
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
}

function modalSendContract($scope, $http, $window, $uibModalInstance) {
    $scope.sendProject = function () {
        var projectId = $scope.object.Id;
        if (projectId) {
            $http({
                url: '/OBKContract/SendContractInProcessing',
                method: 'POST',
                data: { contractId: projectId }
            }).success(function (response) {
                $scope.object.Status = response;
                $scope.changeViewMode();
                $uibModalInstance.close();
                $window.location.href = '/OBKContract';
            });
        }
    };

    $scope.cancelSend = function () {
        $uibModalInstance.dismiss('cancel');
    };
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

function convertDateToStringDDMMYYYY(value) {
    if (value) {
        var d = new Date(parseInt(value.substr(6)));
        var yyyy = d.getFullYear();
        var mm = d.getMonth() < 9 ? "0" + (d.getMonth() + 1) : (d.getMonth() + 1);
        var dd = d.getDate() < 10 ? "0" + d.getDate() : d.getDate();
        return dd + "." + mm + "." + yyyy;
    }
    return "";
}

function loadExpertOrganizations($scope, $http) {
    $http({
        method: "GET",
        url: "/OBKContract/GetExpertOrganizations",
        data: "JSON"
    }).success(function (result) {
        $scope.ExpertOrganizations = result;
    });
}

function loadContractSigners($scope, $http) {
    $http({
        method: "GET",
        url: "/OBKContract/GetSigners",
        data: "JSON"
    }).success(function (result) {
        $scope.ContractSigners = result;
    });
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

function loadDictionaryOBKContractDocumentType($scope, $http) {
    var name = "OBKContractDocumentType";
    $http({
        method: "GET",
        url: "/OBKDictionaries/GetOBKContractDocumentTypeDictionary",
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

function initCalculator($scope, $interval, $http) {

    $scope.totalCostCalculator = 0;

    $scope.selectedServiceIndex = null;

    $scope.modeService = undefined;

    $scope.showAddEditServiceBlock = false;

    $scope.addedServices = [];

    $scope.addService = function () {
        if ($scope.object.Type) {
            $scope.showAddEditServiceBlock = true;
            $scope.modeService = 1;
        }
        else {
            alert("Выберите тип договора");
        }
    }

    $scope.editService = function () {
        if ($scope.selectedServiceIndex != null) {

            var selectedObj = $scope.addedServices[$scope.selectedServiceIndex];

            if (selectedObj.ProductId) {
                alert("Вы не можете редактировать данную услугу, для редактирования данной услуги перейдите во вкладку \"Информация о заявляемой продукции\"");
            }
            else {
                $scope.modeService = 2;

                $scope.showAddEditServiceBlock = true;



                $scope.object.ServiceName = null;

                for (var i = 0; i < $scope.serviceNames.length; i++) {
                    if ($scope.serviceNames[i].Id === selectedObj.ServiceId) {
                        $scope.object.ServiceName = $scope.serviceNames[i];
                        break;
                    }
                }

                $scope.object.UnitOfMeasurementName = selectedObj.UnitOfMeasurementName;
                $scope.object.UnitOfMeasurementId = selectedObj.UnitOfMeasurementId;
                $scope.object.PriceWithoutTax = selectedObj.PriceWithoutTax;
                $scope.object.CountServices = selectedObj.Count;
                $scope.object.ResultPriceWithoutTax = selectedObj.FinalCostWithoutTax;
                $scope.object.ResultPriceWithTax = selectedObj.FinalCostWithTax;
            }

        }
        else {
            alert("Выберите услугу для изменения");
        }
    }

    $scope.deleteService = function () {
        if ($scope.selectedServiceIndex != null) {

            var selectedObj = $scope.addedServices[$scope.selectedServiceIndex];

            if (selectedObj.ProductId) {
                alert("Вы не можете удалить данную услугу, для удаления данной услуги перейдите во вкладку \"Информация о заявляемой продукции\"");
            }
            else {
                var id = selectedObj.Id;
                $scope.deleteServiceInformation(id);
                $scope.addedServices.splice($scope.selectedServiceIndex, 1);
                $scope.selectedServiceIndex = null;

                $scope.calcTotalCostCalculator();
            }
        }
        else {
            alert("Выберите услугу для удаления");
        }
    }

    $scope.saveService = function () {
        if ($scope.modeService == 1) {

            if ($scope.object.ServiceName && $scope.object.CountServices) {
                var service = {
                    Id: null,
                    ServiceName: $scope.object.ServiceName.Name,
                    ServiceId: $scope.object.ServiceName.Id,
                    UnitOfMeasurementName: $scope.object.UnitOfMeasurementName,
                    UnitOfMeasurementId: $scope.object.UnitOfMeasurementId,
                    PriceWithoutTax: $scope.object.PriceWithoutTax,
                    Count: $scope.object.CountServices,
                    FinalCostWithoutTax: $scope.object.ResultPriceWithoutTax,
                    FinalCostWithTax: $scope.object.ResultPriceWithTax
                };

                $scope.addedServices.push(service);
                var index = $scope.addedServices.length - 1;
                $interval(function () {
                    $scope.gridOptionsCalculatorApi.core.handleWindowResize();
                }, 500, 10);

                $scope.saveServiceInformation(service, index);

                $scope.showAddEditServiceBlock = false;
                $scope.clearServiceForm();
                $scope.modeService = null;

                $scope.calcTotalCostCalculator();

                alert("Информация об услуге добавлена");
            }
            else {
                alert("Введите информацию об услуге");
            }
        }
        if ($scope.modeService == 2) {
            if ($scope.object.ServiceName && $scope.object.CountServices) {
                // Edit Value
                var selectedObj = $scope.addedServices[$scope.selectedServiceIndex];
                selectedObj.ServiceName = $scope.object.ServiceName.Name;
                selectedObj.ServiceId = $scope.object.ServiceName.Id;
                selectedObj.UnitOfMeasurementName = $scope.object.UnitOfMeasurementName;
                selectedObj.UnitOfMeasurementId = $scope.object.UnitOfMeasurementId;
                selectedObj.PriceWithoutTax - $scope.object.PriceWithoutTax;
                selectedObj.Count = $scope.object.CountServices;
                selectedObj.FinalCostWithoutTax = $scope.object.ResultPriceWithoutTax;
                selectedObj.FinalCostWithTax = $scope.object.ResultPriceWithTax;

                $scope.saveServiceInformation(selectedObj, $scope.selectedServiceIndex);

                $scope.showAddEditServiceBlock = false;
                $scope.clearServiceForm();
                $scope.modeService = null;

                $scope.calcTotalCostCalculator();

                alert("Информация об услуге обновлена");
            }
            else {
                alert("Введите информацию об услуге");
            }
        }
    }

    $scope.saveServiceInformation = function (service, index) {
        var projectId = $scope.object.Id;
        if (projectId) {
            $http({
                url: '/OBKContract/SaveContractPrice',
                method: 'POST',
                data: { contractId: projectId, service: service }
            }).success(function (response) {
                if (index != null && response.Id && $scope.addedServices[index].Id == null) {
                    $scope.addedServices[index].Id = response.Id;
                }
            });
        }
    }

    $scope.deleteServiceInformation = function (id) {
        var projectId = $scope.object.Id;
        if (projectId) {
            $http({
                url: '/OBKContract/DeleteContractPrice',
                method: 'POST',
                data: { contractId: projectId, serviceId: id }
            }).success(function (response) {
            });
        }
    }

    $scope.cancelSaveService = function () {
        $scope.showAddEditServiceBlock = false;
        $scope.clearServiceForm();
    }

    $scope.clearServiceForm = function () {
        $scope.object.ServiceName = null;
        $scope.object.ServiceId = null;
        $scope.object.UnitOfMeasurementName = null;
        $scope.object.UnitOfMeasurementId = null;
        $scope.object.PriceWithoutTax = null;
        $scope.object.CountServices = null;
        $scope.object.ResultPriceWithoutTax = null;
        $scope.object.ResultPriceWithTax = null;
    }

    $scope.gridOptionsCalculator = {
        enableRowSelection: true,
        enableRowHeaderSelection: false,
        multiSelect: false,
        noUnselect: true
    };

    $scope.gridOptionsCalculator.onRegisterApi = function (gridApi) {
        $scope.gridOptionsCalculatorApi = gridApi;

        $interval(function () {
            $scope.gridOptionsCalculatorApi.core.handleWindowResize();
        }, 500, 10);

        $scope.gridOptionsCalculatorApi.selection.on.rowSelectionChanged($scope, function (row) {
            $scope.selectedServiceIndex = $scope.gridOptionsCalculator.data.indexOf(row.entity);
        });
    };

    $scope.gridOptionsCalculator.columnDefs = [
        { name: 'Id', displayName: 'ИД', width: "*", visible: false },
        { name: 'ServiceName', displayName: 'Тип услуги', width: "*", cellTemplate: '<div class="ui-grid-cell-contents" >{{grid.getCellValue(row, col)}}</div>' },
		{ name: 'ServiceId', displayName: 'Тип услуги - ИД', width: "*", visible: false },
        { name: "ProductId", displayName: "Продукция - ИД", width: "*", visible: false },
        { name: "ProductName", displayName: "Продукция", width: "*" },
        { name: 'UnitOfMeasurementName', displayName: 'Единица измерения', width: "*" },
		{ name: 'UnitOfMeasurementId', displayName: 'Единица измерения - ИД', width: "*", visible: false },
        { name: 'PriceWithoutTax', displayName: 'Цена в тенге, без НДС', width: "*" },
        { name: 'Count', displayName: 'Количество услуг (работ)', width: "*" },
        { name: 'FinalCostWithoutTax', displayName: 'Итоговая стоимость услуги, в тенге без НДС', width: "*" },
        { name: 'FinalCostWithTax', displayName: 'Итоговая стоимость услуги, в тенге с НДС', width: "*" },
        { name: 'ButtonComments', displayName: '', width: '*', cellTemplate: '<span class="input-group-addon"><a valval="{{row.entity.Id}}" class="obkpricedialog" href="#"><i class="glyphicon glyphicon-info-sign"></i></a></span>' }
    ];


    $scope.gridOptionsCalculator.data = $scope.addedServices;

    $scope.serviceTypeChange = function () {
        if ($scope.object.ServiceName && $scope.object.ServiceName.Id) {
            $http({
                method: 'GET',
                url: '/OBKContract/GetService',
                params: {
                    service: $scope.object.ServiceName.Id
                }
            }).then(function (resp) {
                if (resp.data) {
                    $scope.object.UnitOfMeasurementName = resp.data.UnitOfMeasurementName;
                    $scope.object.UnitOfMeasurementId = resp.data.UnitOfMeasurementId;
                    $scope.object.PriceWithoutTax = resp.data.Price;
                    $scope.object.CountServices = null;
                    $scope.object.ResultPriceWithoutTax = null;
                    $scope.object.ResultPriceWithTax = null;
                    $scope.calcPrice();
                }
                else {
                    $scope.object.UnitOfMeasurementName = null;
                    $scope.object.UnitOfMeasurementId = null;
                    $scope.object.PriceWithoutTax = null;
                    $scope.object.CountServices = null;
                    $scope.object.ResultPriceWithoutTax = null;
                    $scope.object.ResultPriceWithTax = null;
                    $scope.calcPrice();
                }
            }, function (response) {

            });
        }

    }

    $scope.loadTax = function () {
        $scope.taxValue = null;
        $http({
            method: 'GET',
            url: '/OBKDictionaries/GetTax'
        }).then(function (resp) {
            if (resp.data) {
                $scope.taxValue = resp.data;
            }
        }, function (response) {

        });
    }

    $scope.loadTax();

    $scope.calcPrice = function () {
        if ($scope.object.PriceWithoutTax && $scope.taxValue && $scope.object.CountServices) {
            var sum = $scope.object.PriceWithoutTax * $scope.object.CountServices;
            var res = sum + sum * ($scope.taxValue * 0.01);
            $scope.object.ResultPriceWithoutTax = sum;
            $scope.object.ResultPriceWithTax = res;
        }
        else {
            $scope.object.ResultPriceWithoutTax = null;
            $scope.object.ResultPriceWithTax = null;
        }
    }

    $scope.countServiceChange = function () {
        $scope.calcPrice();
    }

    $scope.calcTotalCostCalculator = function () {
        $scope.totalCostCalculator = 0;

        var sum = 0.0;
        if ($scope.addedServices && $scope.addedServices.length > 0) {
            for (var i = 0; i < $scope.addedServices.length; i++) {
                sum += $scope.addedServices[i].FinalCostWithTax;
            }
        }

        $scope.totalCostCalculator = sum.toFixed(2);
    }
}

function initProductServiceModule($scope, $http, $interval) {
    $scope.showAddEditProductServiceBlock = false;
    $scope.addedProductServices = [];
    $scope.selectedProductServiceIndex = null;

    $scope.gridOptionsProductServices = {
        enableRowSelection: true,
        enableRowHeaderSelection: false,
        multiSelect: false,
        noUnselect: true
    };

    $scope.gridOptionsProductServices.onRegisterApi = function (gridApi) {
        $scope.gridOptionsProductServicesApi = gridApi;

        $interval(function () {
            $scope.gridOptionsProductServicesApi.core.handleWindowResize();
        }, 500, 10);

        $scope.gridOptionsProductServicesApi.selection.on.rowSelectionChanged($scope, function (row) {
            $scope.selectedProductServiceIndex = $scope.gridOptionsProductServices.data.indexOf(row.entity);
        });
    };

    $scope.gridOptionsProductServices.columnDefs = [
        { name: 'Id', displayName: 'ИД', width: "*", visible: false },
        { name: 'ServiceName', displayName: 'Тип услуги', width: "*" },
		{ name: 'ServiceId', displayName: 'Тип услуги - ИД', width: "*", visible: false },
        { name: 'UnitOfMeasurementName', displayName: 'Единица измерения', width: "*" },
		{ name: 'UnitOfMeasurementId', displayName: 'Единица измерения - ИД', width: "*", visible: false },
        { name: 'PriceWithoutTax', displayName: 'Цена в тенге, без НДС', width: "*" },
        { name: 'Count', displayName: 'Количество услуг (работ)', width: "*" },
        { name: 'FinalCostWithoutTax', displayName: 'Итоговая стоимость услуги, в тенге без НДС', width: "*" },
        { name: 'FinalCostWithTax', displayName: 'Итоговая стоимость услуги, в тенге с НДС', width: "*" }
    ];

    $scope.gridOptionsProductServices.data = $scope.addedProductServices;

    $scope.addProductService = function () {
        if ($scope.object.Type && $scope.product.RegTypeId) {
            $scope.showAddEditProductServiceBlock = true;
            $scope.modeProductService = 1;
        }
        else {
            alert("Выберите тип договора и продукт");
        }
    }

    $scope.editProductService = function () {
        if ($scope.selectedProductServiceIndex != null) {
            $scope.showAddEditProductServiceBlock = true;
            $scope.modeProductService = 2;

            var selectedObj = $scope.addedProductServices[$scope.selectedProductServiceIndex];

            $scope.object.ProductServiceName = null;

            for (var i = 0; i < $scope.productServiceNames.length; i++) {
                if ($scope.productServiceNames[i].Id === selectedObj.ServiceId) {
                    $scope.object.ProductServiceName = $scope.productServiceNames[i];
                    break;
                }
            }

            $scope.object.ProductServiceUnitOfMeasurementName = selectedObj.UnitOfMeasurementName;
            $scope.object.ProductServiceUnitOfMeasurementId = selectedObj.UnitOfMeasurementId;
            $scope.object.ProductServicePriceWithoutTax = selectedObj.PriceWithoutTax;
            $scope.object.ProductServiceCountServices = selectedObj.Count;
            $scope.object.ProductServiceResultPriceWithoutTax = selectedObj.FinalCostWithoutTax;
            $scope.object.ProductServiceResultPriceWithTax = selectedObj.FinalCostWithTax;
        }
        else {
            alert("Выберите услугу для изменения");
        }
    }

    $scope.deleteProductService = function () {
        if ($scope.selectedProductServiceIndex != null) {
            var selectedObj = $scope.addedProductServices[$scope.selectedProductServiceIndex];
            if (selectedObj.Id) {
                $scope.deletedServicesId.push(selectedObj.Id);
            }
            $scope.addedProductServices.splice($scope.selectedProductServiceIndex, 1);
            $scope.selectedProductServiceIndex = null;
        }
        else {
            alert("Выберите услугу для удаления");
        }
    }

    $scope.loadProductServiceNames = function loadProductServiceNames() {
        $scope.productServiceNames = [];
        if ($scope.object.Type && $scope.product.RegTypeId) {
            $http({
                method: "GET",
                url: "/OBKDictionaries/GetServiceNames",
                data: "JSON",
                params: {
                    type: $scope.object.Type,
                    productType: $scope.product.RegTypeId,
                    degreeRiskId: $scope.product.DegreeRiskId
                }
            }).success(function (result) {
                $scope.productServiceNames = result;
            });
        }
    }

    $scope.productServiceTypeChange = function () {
        if ($scope.object.ProductServiceName && $scope.object.ProductServiceName.Id) {
            $http({
                method: 'GET',
                url: '/OBKContract/GetService',
                params: {
                    service: $scope.object.ProductServiceName.Id
                }
            }).then(function (resp) {
                if (resp.data) {
                    $scope.object.ProductServiceUnitOfMeasurementName = resp.data.UnitOfMeasurementName;
                    $scope.object.ProductServiceUnitOfMeasurementId = resp.data.UnitOfMeasurementId;
                    $scope.object.ProductServicePriceWithoutTax = resp.data.Price;
                    $scope.object.ProductServiceCountServices = null;
                    $scope.object.ProductServiceResultPriceWithoutTax = null;
                    $scope.object.ProductServiceResultPriceWithTax = null;
                    $scope.productServiceCalcPrice();
                }
                else {
                    $scope.object.ProductServiceUnitOfMeasurementName = null;
                    $scope.object.ProductServiceUnitOfMeasurementId = null;
                    $scope.object.ProductServicePriceWithoutTax = null;
                    $scope.object.ProductServiceCountServices = null;
                    $scope.object.ProductServiceResultPriceWithoutTax = null;
                    $scope.object.ProductServiceResultPriceWithTax = null;
                    $scope.productServiceCalcPrice();
                }
            }, function (response) {

            });
        }
    }

    $scope.productServiceCountServiceChange = function () {
        $scope.productServiceCalcPrice();
    }

    $scope.productServiceCalcPrice = function () {
        if ($scope.object.ProductServicePriceWithoutTax && $scope.taxValue && $scope.object.ProductServiceCountServices) {
            var sum = $scope.object.ProductServicePriceWithoutTax * $scope.object.ProductServiceCountServices;
            var res = sum + sum * ($scope.taxValue * 0.01);
            $scope.object.ProductServiceResultPriceWithoutTax = sum;
            $scope.object.ProductServiceResultPriceWithTax = res;
        }
        else {
            $scope.object.ProductServiceResultPriceWithoutTax = null;
            $scope.object.ProductServiceResultPriceWithTax = null;
        }
    }

    $scope.saveProductService = function () {
        if ($scope.modeProductService == 1) {
            if ($scope.object.ProductServiceName && $scope.object.ProductServiceCountServices) {
                var service = {
                    Id: null,
                    ServiceName: $scope.object.ProductServiceName.Name,
                    ServiceId: $scope.object.ProductServiceName.Id,
                    UnitOfMeasurementName: $scope.object.ProductServiceUnitOfMeasurementName,
                    UnitOfMeasurementId: $scope.object.ProductServiceUnitOfMeasurementId,
                    PriceWithoutTax: $scope.object.ProductServicePriceWithoutTax,
                    Count: $scope.object.ProductServiceCountServices,
                    FinalCostWithoutTax: $scope.object.ProductServiceResultPriceWithoutTax,
                    FinalCostWithTax: $scope.object.ProductServiceResultPriceWithTax
                };

                $scope.addedProductServices.push(service);
                $scope.showAddEditProductServiceBlock = false;
                $scope.clearProductServiceForm();
                $scope.modeProductService = null;

                alert("Информация об услуге добавлена");
            }
            else {
                alert("Введите информацию об услуге");
            }
        }
        if ($scope.modeProductService == 2) {
            if ($scope.object.ProductServiceName && $scope.object.ProductServiceCountServices) {
                // Edit Value
                var selectedObj = $scope.addedProductServices[$scope.selectedProductServiceIndex];
                selectedObj.ProductServiceName = $scope.object.ProductServiceName.Name;
                selectedObj.ServiceId = $scope.object.ProductServiceName.Id;
                selectedObj.UnitOfMeasurementName = $scope.object.ProductServiceUnitOfMeasurementName;
                selectedObj.UnitOfMeasurementId = $scope.object.ProductServiceUnitOfMeasurementId;
                selectedObj.PriceWithoutTax - $scope.object.PriceWithoutTax;
                selectedObj.Count = $scope.object.ProductServiceCountServices;
                selectedObj.FinalCostWithoutTax = $scope.object.ProductServiceResultPriceWithoutTax;
                selectedObj.FinalCostWithTax = $scope.object.ProductServiceResultPriceWithTax;

                $scope.showAddEditProductServiceBlock = false;
                $scope.clearProductServiceForm();
                $scope.modeProductService = null;

                alert("Информация об услуге обновлена");
            }
            else {
                alert("Введите информацию об услуге");
            }
        }
    }

    $scope.clearProductServiceForm = function () {
        $scope.object.ProductServiceName = null;
        $scope.object.ProductServiceUnitOfMeasurementName = null;
        $scope.object.ProductServiceUnitOfMeasurementId = null;
        $scope.object.ProductServicePriceWithoutTax = null;
        $scope.object.ProductServiceCountServices = null;
        $scope.object.ProductServiceResultPriceWithoutTax = null;
        $scope.object.ProductServiceResultPriceWithTax = null;
    }

    $scope.cancelSaveProductService = function () {
        $scope.showAddEditProductServiceBlock = false;
        $scope.clearProductServiceForm();
    }
}

function initExample($scope, $interval) {
    $scope.gridOptionsExample = {
        enableCellEditOnFocus: true
    };

    $scope.gridOptionsExample.columnDefs = [
  { name: 'id', enableCellEdit: false },
  { name: 'age', enableCellEditOnFocus: false, displayName: 'age (f2/dblClick edit)', width: 200 },
  { name: 'address.city', enableCellEdit: true, width: 300 },
  { name: 'name', displayName: 'Name (editOnFocus)', width: 200 }
    ];

    $scope.myExampleData = [];


    $scope.currentFocused = "";

    $scope.getCurrentFocus = function () {
        var rowCol = $scope.gridOptionsExampleApi.cellNav.getFocusedCell();
        if (rowCol !== null) {
            $scope.currentFocused = 'Row Id:' + rowCol.row.entity.id + ' col:' + rowCol.col.colDef.name;
        }
    }

    $scope.gridOptionsExample.onRegisterApi = function (gridApi) {
        $scope.gridOptionsExampleApi = gridApi;

        $interval(function () {
            $scope.gridOptionsExampleApi.core.handleWindowResize();
        }, 500, 10);
    };



    var obj = { id: 1, age: 29, address: { city: "Astana" }, name: "Bob" };
    $scope.myExampleData.push(obj);

    obj = { id: 2, age: 24, address: { city: "Moscow" }, name: "User" };
    $scope.myExampleData.push(obj);

    $scope.gridOptionsExample.data = $scope.myExampleData;

}

angular
    .module('app')
    .controller('obkContractForm', ['$scope', '$http', '$interval', '$uibModal', '$window', obkContractForm])