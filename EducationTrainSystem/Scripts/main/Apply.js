/// <reference path="E:\Repositories\EducationTrainSystem\EducationTrainSystem\Vendors/references.js" />


var app = angular.module('ApplyApp', [
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
    'edu.services.edutrain',
    'edu.services.schoolsubject',
    'edu.services.user'
]);
app.config(function ($routeProvider) {
    $routeProvider.when('/', {
        controller: 'AddCtrl',
        templateUrl: '/Partials/Education/Apply/add.html'
    }).otherwise({
        redirectTo: '/'
    });
});


app.controller('BodyCtrl', function ($scope, AppConstant, $location) {
    $scope.constant = AppConstant;
    $scope.$on('$routeChangeStart', function (next, current) {
        var paths = [{ href: '/', text: '首页' }, { text: '报名申请' }];
      
        amplify.publish('path.refresh', paths);
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
    EduTrainSvc,
    SchoolSubjectSvc) {

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
        schoolTrain: {
            Course: '',
            SchoolSubjects: [/*{Name, Hours, PricePerHours}*/],
            RegStage: '初中',


            tempSubject: { Name: '', Hours: 1, PricePerHours: 1, subject: {} },
            addSubject: function () {
                this.tempSubject.Name = this.tempSubject.subject.Name;
                this.SchoolSubjects.push($.extend({}, this.tempSubject));
                this.tempSubject.Hours = 1;
                this.tempSubject.PricePerHours = 1;
                this.tempSubject.subject = $scope.main.assist.subjects[0];
            },
            removeSubject: function (index) {
                this.SchoolSubjects.splice(index, 1);
            }
        },

        submit: function () {
            var trainCategory = (this.assist.train || {})['Category'];
            if (!trainCategory) { throw 'Invalid train category at AddCtrl -> submit'; return; }
            this.reg.Address = this.assist.regAddress.Name;

            var train = undefined;
            //学历教育
            if (trainCategory == 'EduTrains') {
                this.eduTrain.Course = this.assist.course.Name;
                this.eduTrain.RegCollege = this.assist.regCollege.Name;
                this.eduTrain.CurrentCollege = this.assist.currentCollege.Name;
                train = this.eduTrain;
            }
            //资格证培训
            if (trainCategory == 'CertificationTrains') {
                this.certTrain.Course = this.assist.course.Name;
                this.certTrain.CurrentCollege = this.assist.currentCollege.Name;
                train = this.certTrain;
            }
            //中小学培训
            if (trainCategory == 'SchoolTrains') {
                this.schoolTrain.Course = this.assist.course.Name;
                train = this.schoolTrain;
                console.log(this.schoolTrain);
            }

            if (train == undefined) { throw 'error train at AddCtrl -> add'; }
            train['Category'] = trainCategory;

            var promise = RegEntitySvc.apply(this.reg, this.user, train);
            promise.then(function (rep) {
                if (rep.data) {
                    location.href = '/Education/ApplySuccess?key=' + rep.data.gid;
                } else {
                    alert(rep.data.msg || '申请失败');
                }
            }, function () {
                alert('申请失败.');
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
            regCollege: {},
            subjects: []
        }
    };

    //load registration address
    RegSvc.loadRegAddresses().then(function (rep) {
        $scope.main.assist.regAddresses = rep.data;
        $scope.main.assist.regAddress = $scope.main.assist.regAddresses[0];
    });

    //load trains
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

    //load subjects for school trains
    KeyValueSvc.getByGroup('school-trains-subjects').then(function (rep) {
        if (rep.data) {
            $scope.main.assist.subjects = rep.data;
            $scope.main.schoolTrain.tempSubject.subject = $scope.main.assist.subjects[0];
        }
    });
});
