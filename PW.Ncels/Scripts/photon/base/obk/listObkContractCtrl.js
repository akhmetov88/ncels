function obkContractForm($scope, $http, $interval) {
    $scope.ExpertOrganizations = [];

    $scope.showContactInformation = false;

    $scope.declarantNotFound = false;

    $scope.iinSearchActive = false;

    var curDate = new Date();
    $scope.mode = 0; // 0 - unknown, 1 - add, 2 - edit
    $scope.declarant = {};
    $scope.object = {};
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

            $scope.loadProductServiceNames();

            $scope.object.ProductServiceName = null;
        });
    };

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
        //$scope.selectedSeriesIndex
        gridApi.selection.on.rowSelectionChanged($scope, function (row) {
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
        });
    };

    $scope.gridOptionsProducts.columnDefs = [
        { name: 'Id', displayName: 'Id', visible: false },
        { name: 'ProductId', displayName: 'ProductId', visible: false },
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
    $scope.object.drugEndDateExpired = false;
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
                //$scope.copyOrg(resp.data, currentOrg, copySpecificFieldsFn);
                //switch (sourceName) {
                //    case 'payerSource':
                //        setPayerType('payerType', 'Payer');
                //        break;
                //    case 'payerTranslationSource':
                //        setPayerType('payerTranslation', 'PayerTranslation');
                //        break;
                //};

                $scope.formatArray(resp.data);

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
        $scope.mode = 1;
    };

    $scope.editDrug = function editDrug() {
        if ($scope.selectedProductIndex != null) {
            $scope.deletedServicesId = [];
            $scope.mode = 2;

            $scope.showAddEditDrugBlock = true;

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
            $scope.product.TnvedCode = selectedObj.TnvedCode;
            $scope.product.KpvedCode = selectedObj.KpvedCode;
            $scope.product.Price = selectedObj.Price;
            $scope.product.Currency = selectedObj.Currency;
            $scope.productSeries.push.apply($scope.productSeries, selectedObj.Series);

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
            var id = $scope.product.ProductId;
            //var NameRu = $scope.product.NameRu;
            //var CountryNameRu = $scope.product.CountryNameRu;
            //var TnvedCode = $scope.product.TnvedCode;
            //var KpvedCode = $scope.product.KpvedCode;
            if (id) {
                if (!$scope.existInArray($scope.addedProducts, id)) {

                    //$scope.product.ProductId = row.entity.Id;
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
                        TnvedCode: $scope.product.TnvedCode,
                        KpvedCode: $scope.product.KpvedCode,
                        Price: $scope.product.Price,
                        Currency: $scope.product.Currency,
                        Series: []
                    }
                    product.Series = $scope.productSeries.slice();

                    $scope.addedProducts.push(product);

                    $scope.saveProductInformation(product);

                    $scope.showAddEditDrugBlock = false;
                    $scope.clearSearchAndProductFields();
                    $scope.mode = 0;
                    alert("Информация о продукции добавлена");
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
            //$scope.product.ProductId = selectedObj.Id;
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
            selectedObj.ProductId = $scope.product.ProductId;
            selectedObj.RegTypeId = $scope.product.RegTypeId;
            selectedObj.DegreeRiskId = $scope.product.DegreeRiskId;
            selectedObj.NameRu = $scope.product.NameRu;
            selectedObj.NameKz = $scope.product.NameKz;
            selectedObj.ProducerNameRu = $scope.product.ProducerNameRu;
            selectedObj.ProducerNameKz = $scope.product.ProducerNameKz;
            selectedObj.CountryNameRu = $scope.product.CountryNameRu;
            selectedObj.CountryNameKz = $scope.product.CountryNameKz;
            selectedObj.TnvedCode = $scope.product.TnvedCode;
            selectedObj.KpvedCode = $scope.product.KpvedCode;
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

            //for (var i = 0; i < $scope.addedProductServices.length; i++) {
            //    var service = $scope.addedProductServices[i];
            //    var newService = {
            //        Id: service.Id,
            //        ServiceName: service.ServiceName,
            //        ServiceId: service.ServiceId,
            //        UnitOfMeasurementName: service.UnitOfMeasurementName,
            //        UnitOfMeasurementId: service.UnitOfMeasurementId,
            //        PriceWithoutTax: service.PriceWithoutTax,
            //        Count: service.Count,
            //        FinalCostWithoutTax: service.FinalCostWithoutTax,
            //        FinalCostWithTax: service.FinalCostWithTax,
            //        ProductId: $scope.product.ProductId,
            //        ProductName: $scope.product.NameRu
            //    };
            //    $scope.addedServices.push(newService);
            //}

            $scope.saveProductInformation(selectedObj);

            $scope.showAddEditDrugBlock = false;
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

                //if ($scope.addedProductServices != null && $scope.addedProductServices.length > 0) {
                //    for (var i = 0; i < $scope.addedProductServices.length; i++) {
                //        var service = $scope.addedProductServices[i];
                //        var newService = {
                //            Id: service.Id,
                //            ServiceName: service.ServiceName,
                //            ServiceId: service.ServiceId,
                //            UnitOfMeasurementName: service.UnitOfMeasurementName,
                //            UnitOfMeasurementId: service.UnitOfMeasurementId,
                //            PriceWithoutTax: service.PriceWithoutTax,
                //            Count: service.Count,
                //            FinalCostWithoutTax: service.FinalCostWithoutTax,
                //            FinalCostWithTax: service.FinalCostWithTax,
                //            ProductId: response.Id,
                //            ProductName: null
                //        };
                //    }
                //}
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
                url: "/OBKDictionaries/GetServiceNames",
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
        $scope.checkFileValidation();
    }

    $scope.editProject = function () {
        var generatedGuid = $("#generatedGuid").val();
        $http({
            url: '/OBKContract/ContractSave',
            method: 'POST',
            data: { Guid: generatedGuid, contractViewModel: $scope.object }
        }).success(function (response) {
            //debugger;

            $scope.object.Id = response.Id;
            //$scope.object.Number = response.Number;
            //$scope.object.CreatedDate = getDate(response.CreatedDate);
            //$scope.object.Status = response.Status;
            ////$scope.object.Contract.StatusId = response.Id;
            ////$scope.object.Contract.ContractStatus = response;
            console.log("SAVED...");
        });
        //$scope.isSendProjectVisible = true;
        //$scope.isEnableDownload = true;
    }


    $scope.companyChange = function (loadOrgData) {
        //if ($scope.object.DeclarantId == "00000000-0000-0000-0000-000000000000") {
        //    $scope.enableCompanyData = true;
        //    $scope.showResidentsBlock = true;
        //    $scope.showHideBin();
        //    if (loadOrgData) {
        //        $scope.loadOrganizationData(null);
        //    }
        //}
        //else {
        //    $scope.enableCompanyData = false;
        //    $scope.showResidentsBlock = false;
        //    $scope.hideBin();
        //    var orgGuid = $scope.object.DeclarantId;
        //    if (loadOrgData) {
        //        $scope.loadOrganizationData(orgGuid);
        //    }
        //}


        alert("Saved company!");
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

                    //$scope.object.DeclarantOrganizationFormId = resp.data.OrganizationFormId;
                    //$scope.object.DeclarantIsResident = resp.data.IsResident;
                    //$scope.object.DeclarantNameKz = resp.data.NameKz;
                    //$scope.object.DeclarantNameRu = resp.data.NameRu;
                    //$scope.object.DeclarantNameEn = resp.data.NameEn;
                    //$scope.object.DeclarantCountryId = resp.data.CountryId;

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
                    $scope.object.BossDocEndDate = getDate(resp.data.BossDocEndDate);
                    $scope.object.BossDocUnlimited = resp.data.BossDocUnlimited;
                    $scope.object.SignLastName = resp.data.SignLastName;
                    $scope.object.SignFirstName = resp.data.SignFirstName;
                    $scope.object.SignMiddleName = resp.data.SignMiddleName;
                    $scope.object.SignPosition = resp.data.SignPosition;
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
            $scope.object.BossDocType = null;
            $scope.object.IsHasBossDocNumber = null;
            $scope.object.BossDocNumber = null;
            $scope.object.BossDocCreatedDate = getDate(null);
            $scope.object.BossDocEndDate = getDate(null);
            $scope.object.BossDocUnlimited = false;
            $scope.object.SignLastName = null;
            $scope.object.SignFirstName = null;
            $scope.object.SignMiddleName = null;
            $scope.object.SignPosition = null;
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
        $scope.object.BossDocType = null;
        $scope.object.IsHasBossDocNumber = null;
        $scope.object.BossDocNumber = null;
        $scope.object.BossDocCreatedDate = getDate(null);
        $scope.object.BossDocEndDate = getDate(null);
        $scope.object.BossDocUnlimited = false;
        $scope.object.SignLastName = null;
        $scope.object.SignFirstName = null;
        $scope.object.SignMiddleName = null;
        $scope.object.SignPosition = null;
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
                                $scope.addDeclarant();
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
                                $scope.addDeclarant();
                            }
                        }
                    }
                });

                //$scope.declarant.DeclarantId = resp.data.DeclarantId;
                //$scope.declarant.DeclarantIsResident = resp.data.DeclarantIsResident;
                //$scope.declarant.DeclarantOrganizationFormId = resp.data.DeclarantOrganizationFormId;
                //$scope.declarant.DeclarantBin = resp.data.DeclarantBin;
                //$scope.declarant.DeclarantNameKz = resp.data.DeclarantNameKz;
                //$scope.declarant.DeclarantNameRu = resp.data.DeclarantNameRu;
                //$scope.declarant.DeclarantNameEn = resp.data.DeclarantNameEn;
                //$scope.declarant.DeclarantCountryId = resp.data.DeclarantCountryId;

                //$scope.companyChange(false);

                $scope.object.AddressLegalRu = resp.data.AddressLegalRu;
                $scope.object.AddressLegalKz = resp.data.AddressLegalKz;
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
                $scope.object.BossDocEndDate = getDate(resp.data.BossDocEndDate);
                $scope.object.BossDocUnlimited = resp.data.BossDocUnlimited;
                $scope.object.SignLastName = resp.data.SignLastName;
                $scope.object.SignFirstName = resp.data.SignFirstName;
                $scope.object.SignMiddleName = resp.data.SignMiddleName;
                $scope.object.SignPosition = resp.data.SignPosition;
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
        $('.file-validation').text("");
        $('.file-validation:visible').each(function () {
            var ct = $(this).attr('countFile');
            var attcode = $(this).attr('attcode');
            var count = parseInt(ct, 10) || 0;
            if (count === 0 && attcode === "1") {
                $(this).text("Необходимо вложить файлы");
                validFile = false;
            } else {
                $(this).text("");
            }
        });
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
                    $scope.object.BossDocType = resp.data.BossDocType;
                    $scope.object.IsHasBossDocNumber = resp.data.IsHasBossDocNumber;
                    $scope.object.BossDocNumber = resp.data.BossDocNumber;
                    $scope.object.BossDocCreatedDate = getDate(resp.data.BossDocCreatedDate);
                    $scope.object.BossDocEndDate = getDate(resp.data.BossDocEndDate);
                    $scope.object.BossDocUnlimited = resp.data.BossDocUnlimited;
                    $scope.object.SignLastName = resp.data.SignLastName;
                    $scope.object.SignFirstName = resp.data.SignFirstName;
                    $scope.object.SignMiddleName = resp.data.SignMiddleName;
                    $scope.object.SignPosition = resp.data.SignPosition;
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
                $scope.declarant.Id = response;
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
        var dd = d.getDate() < 9 ? "0" + d.getDate() : d.getDate();
        return dd + "." + mm + "." + yyyy;
    }
    return "";
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

function initCalculator($scope, $interval, $http) {

    //loadServiceNames($scope);

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

                //$scope.product.ProductId = selectedObj.Id;
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
        { name: 'ServiceName', displayName: 'Тип услуги', width: "*" },
		{ name: 'ServiceId', displayName: 'Тип услуги - ИД', width: "*", visible: false },
        { name: "ProductId", displayName: "Продукция - ИД", width: "*", visible: false },
        { name: "ProductName", displayName: "Продукция", width: "*" },
        { name: 'UnitOfMeasurementName', displayName: 'Единица измерения', width: "*" },
		{ name: 'UnitOfMeasurementId', displayName: 'Единица измерения - ИД', width: "*", visible: false },
        { name: 'PriceWithoutTax', displayName: 'Цена в тенге, без НДС', width: "*" },
        { name: 'Count', displayName: 'Количество услуг (работ)', width: "*" },
        { name: 'FinalCostWithoutTax', displayName: 'Итоговая стоимость услуги, в тенге без НДС', width: "*" },
        { name: 'FinalCostWithTax', displayName: 'Итоговая стоимость услуги, в тенге с НДС', width: "*" }
    ];


    //$scope.calcItems = [];
    $scope.gridOptionsCalculator.data = $scope.addedServices;

    //var obj1 = { ServiceType: "Тип 1", UnitOfMeasurement: "м.", PriceWithoutTax: 10000, Count: 1, FinalCostWithoutTax: 10000, FinalCostWithTax: 11000  };

    //$scope.calcItems.push(obj1);


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

        $scope.totalCostCalculator = sum;
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
        //enableRowSelection: true,
        //enableRowHeaderSelection: false,
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
    .controller('obkContractForm', ['$scope', '$http', '$interval', obkContractForm])