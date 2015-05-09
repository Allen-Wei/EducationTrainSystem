/// <reference path="../../Vendors/angular.min.js" />
angular.module('edu.services.user', [])
    .factory('UserSvc', function ($http, AppConstant) {
        var service = {

            getApi: function (append) {
                return AppConstant.getApi({
                    entity: 'User',
                    params: append
                });
            },
            isLogin: function () {
                return $http({
                    method: 'get',
                    url: this.getApi()
                });
            },
            logIn: function (code, password) {
                var info = {code:code, password:password};
                return $http({
                    method: 'get',
                    url:this.getApi(info)
                });
            },
            logOut: function () {
                return $http({
                    method: 'get',
                    url: this.getApi('SignOut')
                });
            }
        };
        return service;
    });