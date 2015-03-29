/// <reference path="E:\Repositories\EducationTrainSystem\EducationTrainSystem\Vendors/angular.min.js" />
/// <reference path="E:\Repositories\EducationTrainSystem\EducationTrainSystem\Vendors/jquery-2.1.3.min.js" />

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
            perPage: 10,
            perPageLarge: 50,

            getApi: function(inOptions) {
                var options  = {
                    url: undefined,
                    version: 1,
                    entity: 'api entity',

                    params: undefined //{para: value} append with get params
                };
                $.extend(options, inOptions);
                var apiUrl = '/APIv' + options.version + '/' + options.entity;
                if (options.url) { apiUrl = options.url; }
                if (options.params) {
                    if ($.isPlainObject(options.params)) {
                        apiUrl += '?' + $.param(options.params);
                    } else {
                        apiUrl += '/' + options.params;
                    }
                }
                return apiUrl;
            }
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
