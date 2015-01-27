/// <reference path="../../Vendors/angular.min.js" />


angular.module('edu.services.reg', []).factory('RegSvc', function ($http, AppConstant) {
    var service = {
        get: function (id) {
            var promise = undefined;
            if (!id) { throw 'error registration id at edu.services.reg -> get'; }
            if (id.indexOf('-') != -1) {
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
                train = arguments[2];

            if (arguments.length == 1) {
                //arguments: reg
                return this.__add1(reg);
            }
            if (arguments.length == 2) {
                //arguments: reg, user
                return this.__add2(reg, user);
            }
            if (arguments.length == 3) {
                //arguments: reg, user, train type, train

                var data = { TrainType: train.Category, Reg: reg, User: user};

                switch (train.Category) {
                    case 'EduTrain':
                    case 'EduTrains':
                    case '学历教育':
                    case '学历教育培训': data.Edu = train; break;
                    case 'CertificationTrain':
                    case 'CertificationTrains':
                    case 'CertTrain':
                    case '资格证培训': data.Cert = train; break;
                    case 'SchoolTrain':
                    case 'SchoolTrains':
                    case '中小学培训': data.School= train; break;
                    default: data = undefined;
                }

                if (data === undefined) { throw 'error train category at services.registration -> add';}

                var promise = $http({
                    method: 'put',
                    url: '/APIv1/Registration?level=3',
                    data: data
                });
                return promise;

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