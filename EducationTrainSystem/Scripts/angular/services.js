(function () {
    angular.module('registration.services', [])
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
        })
        .factory('RegSvc', function ($http, AppConstant) {
            var service = {
                loadColleges: function () {
                    var promise = $http({
                        method: 'get',
                        url: '/APIv1/College'
                    });
                    return promise;
                }
                     , add: function (reg) {
                         var promise = $http({
                             method: 'put',
                             url: '/APIv1/Registration',
                             data: reg
                         });
                         return promise;
                     }
                     , loadRegList: function (page) {
                         var take = AppConstant.perPage;
                         var skip = (page - 1) * take;
                         var promise = $http({
                             method: 'get',
                             url: '/APIv1/Registration',
                             params: { take: take, skip: skip }
                         });
                         return promise;
                     }
                     , getDetail: function (id) {
                         var promise = $http({
                             method: 'get',
                             url: '/APIv1/Registration/' + id
                         });
                         return promise;
                     }
                     , update: function (reg) {
                         var promise = $http({
                             method: 'post',
                             url: '/APIv1/Registration',
                             data: reg
                         });
                         return promise;
                     }
                     , remove: function (id) {
                         var promise = $http({
                             method: 'delete',
                             url: '/APIv1/Registration/' + id
                         });
                         return promise;
                     }
                     , loadCourseCategories: function () {
                         var promise = $http({
                             method: 'get',
                             url: '/APIv1/CourseCategory'
                         });
                         return promise;
                     }
                     , loadCourses: function (categoryName) {
                         var promise = $http({
                             method: 'get',
                             url: '/APIv1/Course?category=' + categoryName
                         });
                         return promise;
                     }
            };
            return service;
        });
})();
