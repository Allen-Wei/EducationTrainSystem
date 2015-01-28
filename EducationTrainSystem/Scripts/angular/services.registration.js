/// <reference path="../../Vendors/angular.min.js" />


angular.module('edu.services.reg', []).factory('RegSvc', function ($http, AppConstant) {
    var service = {
        getApi: function (append) {
            return AppConstant.getApi({
                entity: 'Registration',
                params: append
            });
        },

        get: function (id) {
            var promise = undefined;
            if (!id) { throw 'error registration id at edu.services.reg -> get'; }
            if (id.indexOf('-') != -1) {
                promise = $http({
                    method: 'get',
                    url: this.getApi(),
                    params: { gid: id }
                });
            } else {
                promise = $http({
                    method: 'get',
                    url: this.getApi(id)
                });
            }
            return promise;
        },
        getList: function (page) {
            var take = AppConstant.perPage;
            var skip = (page - 1) * take;
            var promise = $http({
                method: 'get',
                url: this.getApi(),
                params: { take: take, skip: skip }
            });
            return promise;
        },

        add: function (entity) {
            var promise = $http({
                method: 'PUT',
                url: this.getApi(),
                data: entity
            });
        },

        update: function (reg) {
            var promise = $http({
                method: 'post',
                url: this.getApi(),
                data: reg
            });
            return promise;
        },
        remove: function (id) {
            var promise = $http({
                method: 'delete',
                url: this.getApi(id)
            });
            return promise;
        },
        loadAllColleges: function () {
            var promise = undefined;
            promise = $http({
                method: 'get',
                url: AppConstant.getApi({ entity: 'College' })
            });
            return promise;
        },
        loadRegAddresses: function () {
            var promise = $http({
                method: 'get',
                url: AppConstant.getApi({ entity: 'KeyValue' }),
                params: { gname: 'regaddress' }
            });
            return promise;
        }
    };
    return service;
});