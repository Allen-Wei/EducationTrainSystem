﻿
var app = angular.module('Registration', [
    'ngRoute',
    'edu.directives',
    'edu.filters',
    'edu.services.global',
    'edu.services.regentity',
    'edu.services.reg',
    'edu.services.train',
    'edu.services.course',
    'edu.services.keyvalue',
    'edu.services.keyvaluegroup',
    'edu.services.keyvaluematch',
    'edu.services.edutrain'
]);
app.config(function ($routeProvider) {
    $routeProvider.when('/', {
        controller: 'ListCtrl',
        templateUrl: '/Partials/Education/Registration/list.html'
    }).when('/detail/:id', {
        controller: 'DetailCtrl',
        templateUrl: '/Partials/Education/Registration/detail.html'
    }).when('/edit/:id', {
        controller: 'EditCtrl',
        templateUrl: '/Partials/Education/Registration/edit.html'
    }).when('/add', {
        controller: 'AddCtrl',
        templateUrl: '/Partials/Education/Registration/add.html'
    }).otherwise({
        redirectTo: '/'
    });
});


app.controller('BodyCtrl', function ($scope, AppConstant, $location) {
    $scope.constant = AppConstant;
    $scope.$on('$routeChangeStart', function (next, current) {
        var paths = [{ href: '/', text: '网站' }, { href: '#/', text: '主页' }];
        var path = $location.path();
        switch (path) {
            case '/': paths[1].href = undefined; break;
            case '/add': paths.push({ text: '新增' }); break;
            default: (function () {
                if (path.indexOf('/detail/') != -1) {
                    paths.push({ text: '详情' });
                }
                if (path.indexOf('/edit/') != -1) {
                    paths.push({ text: '编辑' });
                }
            })(); break;
        }

        amplify.publish('path.refresh', paths);
    });
});


app.controller('ListCtrl', function ($scope, RegEntitySvc, RegSvc, TrainSvc, CourseSvc) {

    $scope.main = {
        regs: [],
        page: 1,
        pages: 1,
        total: 1,
        load: function (p) {
            RegEntitySvc.getList(p).then(function (rep) {
                $scope.main.regs = rep.data;
                $scope.main.pages = parseInt(rep.headers('X-HeyHey-Pages'));
                $scope.main.total = parseInt(rep.headers('X-HeyHey-Total'));
            });
        },
        refresh: function () {
            this.load(this.page);
        },
        remove: function (index) {
            var reg = this.regs[index];
            if (!reg) {
                throw 'Cannot find ' + index;
            }

            RegSvc.remove(reg.Id).then(function (rep) {
                if (rep.data) {
                    $scope.main.regs.splice(index, 1);
                }
            }, function (rep) {
                console.log('deleted fail.');
            });
        }
    };

    $scope.$watch('main.page', function (newVal, oldVal) {
        if (newVal <= 0) {
            throw 'Erro page number: ' + newVal;
        }
        if (newVal > $scope.main.pages) {
            throw 'Out of max pages: ' + newVal;
        }
        $scope.main.load(newVal);
    });
});
app.controller('DetailCtrl', function ($scope, $routeParams, RegEntitySvc, RegSvc, TrainSvc, CourseSvc) {
    $scope.main = {
        reg: {}
    };
    RegEntitySvc.get($routeParams.id).then(function (rep) {
        $scope.main.reg = rep.data;
    });
});

app.controller('AddCtrl', function (
    $scope,
    $filter,
    $location,
    AppConstant,
    KeyValueSvc,
    RegEntitySvc,
    RegSvc,
    TrainSvc,
    CourseSvc,
    EduTrainSvc) {

    $scope.main = {
        user: {
            RegDate: $filter('date')(new Date(), AppConstant.ngDateFormat),
            Name: '',
            Nation: '汉',
            Gender: 'true',
            CardId: '',
            Phone: '',
            Phone2: '',
            LiveAddress: '',
            HomeAddress: '',
        },
        reg: {
            ReceiptNumber: '',
            Price: 0,
            Agent: '',
            Payee: '',
            Note: '',
            Confirmed: true,
            Address: ''
        },
        eduTrain: {
            Course: '',
            RegCollege: '',
            RegMajor: '',
            EduType: '专科',
            CurrentCollege: '',
            CurrentGrade: '大一'
        },
        certTrain: {
            Course: '',
            CurrentCollege: '',
            CurrentGrade: '大一',
            EduType: '专科'
        },

        submit: function () {
            var trainCategory = (this.assist.train || {})['Category'];
            if (!trainCategory) { throw 'Invalid train category at AddCtrl -> submit'; return; }
            console.log('reg: ', this.reg);
            console.log('user: ', this.user);
            this.reg.Address = this.assist.regAddress.Name;
            var train = undefined;
            //学历教育
            if (trainCategory == 'EduTrains') {
                this.eduTrain.Course = this.assist.course.Name;
                this.eduTrain.RegCollege = this.assist.regCollege.Name;
                this.eduTrain.CurrentCollege = this.assist.currentCollege.Name;
                console.log('edu: ', this.eduTrain);
                train = this.eduTrain;
            }
            //资格证培训
            if (trainCategory == 'CertificationTrains') {
                this.certTrain.Course = this.assist.course.Name;
                this.certTrain.CurrentCollege = this.assist.currentCollege.Name;
                console.log('cert: ', this.certTrain);
                train = this.certTrain;
            }
            if (train == undefined) { throw 'error train at AddCtrl -> add'; }
            train['Category'] = trainCategory;
            var promise = RegEntitySvc.add(this.reg, this.user, train);
            promise.then(function (rep) {
                console.log(rep.data);
                if (rep.data) {
                    $location.path('/');
                }
            });
        },

        assist: {
            trains: [],
            train: {},
            courses: [],
            course: {},
            regAddresses: [],
            regAddress: {},
            currentColleges: [],
            currentCollege: {},
            regColleges: [],
            regCollege: {}
        }
    };


    TrainSvc.getAll().then(function (rep) {
        $scope.main.assist.trains = rep.data;
        $scope.main.assist.train = $scope.main.assist.trains[0];
    });

    //load courses
    $scope.$watch('main.assist.train.Id', function (newVal, oldVal) {
        if (!newVal) { return; }
        CourseSvc.getByTrain(newVal).then(function (rep) {
            $scope.main.assist.courses = rep.data;
            $scope.main.assist.course = $scope.main.assist.courses[0];
        });
    });
    //load registration address
    RegSvc.loadRegAddresses().then(function (rep) {
        $scope.main.assist.regAddresses = rep.data;
        $scope.main.assist.regAddress = $scope.main.assist.regAddresses[0];
    });
    //load all colleges
    RegSvc.loadAllColleges().then(function (rep) {
        $scope.main.assist.currentColleges = rep.data;
        $scope.main.assist.currentCollege = $scope.main.assist.currentColleges[0];
    });
    //load registration college
    $scope.$watch('main.assist.course.Name', function (newVal, oldVal) {
        if (!newVal) { return; }
        KeyValueSvc.getByGroup(newVal).then(function (rep) {
            if (!rep.data) {
                rep.data = [{}];
            }
            $scope.main.assist.regColleges = rep.data;
            $scope.main.assist.regCollege = $scope.main.assist.regColleges[0];
        });;
    });
});

app.controller('EditCtrl', function (
    $scope,
    $filter,
    $location,
    $routeParams,
    AppConstant,
    KeyValueSvc,
    RegSvc,
    RegEntitySvc,
    TrainSvc,
    CourseSvc,
    EduTrainSvc) {


    $scope.main = {
        user: {
            RegDate: $filter('date')(new Date(), AppConstant.ngDateFormat),
            Name: '',
            Nation: '汉',
            Gender: true,
            CardId: '',
            Phone: '',
            Phone2: '',
            LiveAddress: '',
            HomeAddress: '',
        },
        reg: {
            Gid:'',
            ReceiptNumber: '',
            Price: 0,
            Agent: '',
            Payee: '',
            Note: '',
            Confirmed: true,
            Address: '',
            TrainId: '',
            TrainCategory: ''
        },
        eduTrain: {
            Course: '',
            RegCollege: '',
            RegMajor: '',
            EduType: '专科',
            CurrentCollege: '',
            CurrentGrade: '大一'
        },
        certTrain: {
            Course: '',
            CurrentCollege: '',
            CurrentGrade: '大一',
            EduType: '专科'
        },

        submit: function () {
            var trainCategory = (this.assist.train || {})['Category'];
            if (!trainCategory) { throw 'Invalid train category at EditCtrl -> submit'; return; }
            this.reg.Address = this.assist.regAddress.Name;
            var train = undefined;
            //学历教育
            if (trainCategory == 'EduTrains') {
                this.eduTrain.Course = this.assist.course.Name;
                this.eduTrain.RegCollege = this.assist.regCollege.Name;
                this.eduTrain.CurrentCollege = this.assist.currentCollege.Name;
                console.log('edu: ', this.eduTrain);
                train = this.eduTrain;
            }
            //资格证培训
            if (trainCategory == 'CertificationTrains') {
                this.certTrain.Course = this.assist.course.Name;
                this.certTrain.CurrentCollege = this.assist.currentCollege.Name;
                console.log('cert: ', this.certTrain);
                train = this.certTrain;
            }

            if (train == undefined) { throw 'error train at EditCtrl -> submit'; }
            train.Gid = this.reg.TrainId;
            train['Category'] = trainCategory;
            if (this.reg.TrainCategory != trainCategory) {
                this.reg.TrainCategory = trainCategory;
                this.reg.TrainId = '';
            }
            console.log('reg: ', this.reg);
            console.log('user: ', this.user);
            console.log('train: ', train);

            var promise = RegEntitySvc.update(this.reg, this.user, train);
            promise.then(function (rep) {
                console.log(rep.data);
                if (rep.data) {
                    //$location.path('/');
                }
            });
        },

        assist: {
            trains: [],
            train: {},
            courses: [],
            course: {},
            regAddresses: [],
            regAddress: {},
            currentColleges: [],
            currentCollege: {},
            regColleges: [],
            regCollege: {}
        }
    };

    //load courses
    $scope.$watch('main.assist.train.Id', function (newVal, oldVal) {
        if (!newVal) { return; }
        CourseSvc.getByTrain(newVal).then(function (rep) {
            $scope.main.assist.courses = rep.data;
            var index = 0;

            if (oldVal === undefined) {
                //first load
                var trainCategory = $scope.main.assist.train.Category;
                var courseName = undefined;
                if (trainCategory == 'EduTrains') {
                    courseName = $scope.main.eduTrain.Course;
                }
                if (trainCategory == 'CertificationTrains') {
                    courseName = $scope.main.certTrain.Course;
                }
                index = EduUtils.getIndex(rep.data, 'Name', courseName);
            }
            $scope.main.assist.course = $scope.main.assist.courses[index];
        });
    });

    //load registration college
    $scope.$watch('main.assist.course.Name', function (newVal, oldVal) {
        if (!newVal) { return; }
        KeyValueSvc.getByGroup(newVal).then(function (rep) {
            if (!rep.data) {
                rep.data = [{}];
            }
            $scope.main.assist.regColleges = rep.data;
            $scope.main.assist.regCollege = $scope.main.assist.regColleges[0];
        });;
    });

    //initialize
    RegEntitySvc.get($routeParams.id).then(function (regRep) {
        var regEntity = regRep.data;
        regEntity.User.RegDate = $filter('date')(regEntity.User.RegDate, AppConstant.ngDateFormat);
        $.extend($scope.main.reg, regEntity.Reg);
        $.extend($scope.main.user, regEntity.User);
        if (regEntity.Edu) {
            $.extend($scope.main.eduTrain, regEntity.Edu);
        }
        if (regEntity.Cert) {
            $.extend($scope.main.certTrain, regEntity.Cert);
        }

        //load trains
        TrainSvc.getAll().then(function (trainRep) {
            $scope.main.assist.trains = trainRep.data;
            var index = EduUtils.getIndex(trainRep.data, 'Category', regEntity.Reg.TrainCategory);
            $scope.main.assist.train = $scope.main.assist.trains[index];
        });

        //load registration address
        RegSvc.loadRegAddresses().then(function (addrRep) {
            $scope.main.assist.regAddresses = addrRep.data;
            var index = EduUtils.getIndex(addrRep.data, 'Name', regEntity.Reg.Address);
            $scope.main.assist.regAddress = $scope.main.assist.regAddresses[index];
        });


        //load all colleges
        RegSvc.loadAllColleges().then(function (rep) {
            $scope.main.assist.currentColleges = rep.data;
            $scope.main.assist.currentCollege = $scope.main.assist.currentColleges[0];
        });
    });
});



