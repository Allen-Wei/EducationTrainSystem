/// <reference path="../../Vendors/angular.min.js" />
angular.module('edu.services.keyvalue', [])
    .factory('KeyValueSvc', function ($http, AppConstant) {
        var service = {
            getApi: function (append) {
                var url = '/APIv1/KeyValue';
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
            //get include keyvalues by group
            //group: group id/name
            getInclude: function (group, page) {
                var take = AppConstant.perPage;
                var skip = (page - 1) * take;
                var urlParams = { take: take, skip: skip };
                if (isNaN(parseInt(group))) {
                    urlParams['gname'] = group;
                } else {
                    urlParams['gid'] = group;
                }
                var promise = $http({
                    method: 'get',
                    url: this.getApi(),
                    params: urlParams
                });
                return promise;
            },
            //get exclude keyvalues by group
            //group: group id
            getExclude: function (group, page) {
                var take = AppConstant.perPage;
                var skip = (page - 1) * take;
                var urlParams = { take: take, skip: skip };
                if (isNaN(parseInt(group))) {
                    urlParams['gnameexclude'] = group;
                } else {
                    urlParams['gidexclude'] = group;
                }
                var promise = $http({
                    method: 'get',
                    url: this.getApi(),
                    params: urlParams
                });
                return promise;
            },
            //get all by group
            getByGroup: function (group) {
                var urlParams = {};
                if (isNaN(parseInt(group))) {
                    urlParams['gname'] = group;
                } else {
                    urlParams['gid'] = group;
                }

                var promise = $http({
                    method: 'get',
                    url: this.getApi(),
                    params: urlParams
                });
                return promise;
            },
            getMarks: function () {
                var promise = $http({
                    method: 'get',
                    url: this.getApi(),
                    params: { getMark: true }
                });
                return promise;
            },

            add: function (entity) {
                var promise = $http({
                    method: 'PUT',
                    url: this.getApi(),
                    data: entity
                });
                return promise;
            },
            update: function (entity) {
                var promise = $http({
                    method: 'POST',
                    url: this.getApi(),
                    data: entity
                });
                return promise;
            },
            remove: function (id) {
                var promise = $http({
                    method: 'delete',
                    url: this.getApi(id)
                });
                return promise;
            }
        };
        return service;
    });