/// <reference path="../../Vendors/angular.min.js" />
angular.module('edu.services.keyvaluematch', [])
    .factory('KeyValueMatchSvc', function ($http, AppConstant) {
        var service = {
            getApi: function (append) {
                var url = '/APIv1/KeyValueMatch';
                if (append) { return url + '/' + append; }
                return url;

            },
            get: function (id) {
                var promise = $http({
                    method: 'get',
                    url: this.getApi(id)
                });
                return promise;
            },
            getAll: function () {
                var promise = $http({
                    method: 'get',
                    url: this.getApi()
                });
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
            //group: group id
            getKeyValues: function (group, page) {
                var take = AppConstant.perPage;
                var skip = (page - 1) * take;
                var promise = $http({
                    method: 'get',
                    url: this.getApi(),
                    params: { group: group, take: take, skip: skip }
                });
                return promise;
            },
            add: function (groupId, valueId) {
                var entity = {GroupId: groupId, ValueId: valueId};
                var promise = $http({
                    method: 'PUT',
                    url: this.getApi(),
                    data: entity
                });
                return promise;
            },
            remove: function (para1, para2) {
                if (arguments.length == 1) {
                    var promise = $http({
                        method: 'delete',
                        url: this.getApi(para1)
                    });
                    return promise;
                }
                if (arguments.length == 2) {
                    var promise = $http({
                        method: 'delete',
                        url: this.getApi(),
                        params: { group: para1, value: para2}
                    });
                    return promise;
                }
                throw 'Error parameters';
            }
        };
        return service;
    });