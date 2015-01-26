/// <reference path="../../Vendors/angular.min.js" />


angular.module('edu.services.reg', []).factory('RegSvc', function ($http, AppConstant) {
    var service = {
        get: function (id) {
            var promise = undefined;
            if (isNaN(parseInt(id))) {
                promise = $http({
                    method: 'get',
                    url: '/APIv1/Registration',
                    params: { gid: id }
                });
            } else {
                promise = $http({
                    method: 'get',
                    url: '/APIv1/Registration/' + id
                });
            }
            return promise;
        },
        getList: function (page) {
            var take = AppConstant.perPage;
            var skip = (page - 1) * take;
            var promise = $http({
                method: 'get',
                url: '/APIv1/Registration',
                params: { take: take, skip: skip }
            });
            return promise;
        },

        add: function () {
            var reg = arguments[0],
                user = arguments[1],
                trainType = arguments[2],
                train = arguments[3];

            if (arguments.length == 1) {
                //arguments: reg
                return this.__add1(reg);
            }
            if (arguments.length == 2) {
                //arguments: reg, user
                return this.__add2(reg, user);
            }
            if (arguments.length == 4) {
                //arguments: reg, user, train type, train
                var promiseFn = undefined;

                switch (trainType) {
                    case 'EduTrain':
                    case '学历教育':
                    case '学历教育培训': promiseFn = this.__addWithEduTrain; break;
                    case 'CertificationTrain':
                    case 'CertTrain':
                    case '资格证培训': promiseFn = this.__addWithCertTrain; break;
                    case 'SchoolTrain':
                    case '中小学培训': promiseFn = this.__addWithSchoolTrain; break;
                    default: promiseFn = undefined;
                }
                if (!promiseFn) {
                    throw 'error train type at services.registration -> add';
                }
                return promiseFn(reg, user, train);
            }
            throw 'error parameters at service.registrations -> add';
        },
        __add1: function (reg) {
            var promise = $http({
                method: 'put',
                url: '/APIv1/Registration?level=1',
                data: { Reg: reg }
            });
            return promise;
        },
        __add2: function (reg, user) {
            var promise = $http({
                method: 'put',
                url: '/APIv1/Registration?level=2',
                data: { Reg: reg, User: user }
            });
            return promise;
        },
        __addWithEduTrain: function (reg, user, edu) {
            var promise = $http({
                method: 'put',
                url: '/APIv1/Registration?level=3',
                data: { TrainType: 'EduTrains', Reg: reg, User: user, Edu: edu }
            });
            return promise;
        },
        __addWithCertTrain: function (reg, user, cert) {
            var promise = $http({
                method: 'put',
                url: '/APIv1/Registration?level=3',
                data: { TrainType: 'CertificationTrains', Reg: reg, User: user, Cert: cert }
            });
            return promise;
        },
        __addWithSchoolTrain: function (reg, user, school) {
            var promise = $http({
                method: 'put',
                url: '/APIv1/Registration?level=3',
                data: { TrainType: 'SchoolTrains', Reg: reg, User: user, School: school }
            });
            return promise;
        },

        update: function (reg) {
            var promise = $http({
                method: 'post',
                url: '/APIv1/Registration',
                data: reg
            });
            return promise;
        },
        remove: function (id) {
            var promise = $http({
                method: 'delete',
                url: '/APIv1/Registration/' + id
            });
            return promise;
        },
        loadAllColleges: function () {
            var promise = undefined;
            promise = $http({
                method: 'get',
                url: '/APIv1/College'
            });
            return promise;
        },
        loadRegAddresses: function () {
            var promise = $http({
                method: 'get',
                url: '/APIv1/KeyValue',
                params: { gname: 'regaddress' }
            });
            return promise;
        }
    };
    return service;
});