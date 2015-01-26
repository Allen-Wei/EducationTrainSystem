/// <reference path="../../Vendors/angular.min.js" />
angular.module('edu.services.edutrain', [])
    .factory('EduTrainSvc', function ($http, AppConstant) {
        var service = {
            getApi: function (append) {
                var url = '/APIv1/EduTrain';
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