(function () {
    angular.module('edu.services.global', [])
        .factory('TipSvc', function ($timeout) {
            var service = {
                show: false,
                text: '',
                duration: function (txt, time) {
                    this.toShow(txt);
                    $timeout(function () {
                        service.toHide();
                    }, time || 2000);
                },

                toShow: function (txt) {
                    this.toTip(txt);
                },
                toTip: function (txt) {
                    this.show = true;
                    this.text = txt;
                },
                toHide: function () {
                    this.show = false;
                    this.text = '';
                }
            };
            return service;
        })
        .constant('AppConstant', {
            ngDateFormat: 'yyyy-MM-dd',
            ngDateFormatDetail: 'yyyy-MM-dd h:mm:ss a',
            uiDateFormat: 'yy-mm-dd',
            perPageLittle: 10,
            perPage: 5,
            perPageLarge: 50,

            apiv1: '/APIv1/',
            apiv2: '/APIv2/'
        })
        .factory('GlobalSvc', function (AppConstant) {
            var service = {
                getApiUrl: function (entity, version) {
                    var url = (version === undefined ? AppConstant.apiv2 : AppConstant.apiv1) + enitty;
                    return url;
                }
            };
            return service;
        });
})();
