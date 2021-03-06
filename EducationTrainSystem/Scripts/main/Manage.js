﻿/// <reference path="E:\Repositories\EducationTrainSystem\EducationTrainSystem\Vendors/references.js" />



var app = angular.module('ManageApp', [
    'ngRoute',
    'edu.services.global',
    'edu.services.reg',
    'edu.services.keyvalue',
    'edu.services.keyvaluegroup',
    'edu.services.keyvaluematch',
    'edu.directives',
    'edu.filters'
]);

app.config(function ($routeProvider) {
    $routeProvider.when('/', {
        controller: 'SummaryCtrl',
        templateUrl: '/Partials/Education/Manage/summary.html'
    }).when('/keyvalue', {
        controller: 'KeyValueCtrl',
        templateUrl: '/Partials/Education/Manage/keyvalue.html'
    }).when('/keyvaluegroup', {
        controller: 'KeyValueGroupCtrl',
        templateUrl: '/Partials/Education/Manage/keyvaluegroup.html'
    }).when('/keyvaluematch', {
        controller: 'KeyValueMatchCtrl',
        templateUrl: '/Partials/Education/Manage/keyvaluematch.html'
    }).otherwise({
        redirectTo: '/'
    });
});


app.controller('BodyCtrl', function ($scope, AppConstant, $location) {
    $scope.constant = AppConstant;
    $scope.$on('$routeChangeStart', function (next, current) {
        var paths = [{ href: '/', text: '网站' }, { href: '/Education/Registration', text: '报名' }, { href: '#/', text: '主页' }];
        var path = $location.path();
        switch (path) {
            case '/': paths[paths.length-1].href = undefined; break;
            case '/keyvalue': paths.push({ href: '#' + path, text: '集合' }); break;
            case '/keyvaluegroup': paths.push({ href: '#' + path, text: '集合分组' }); break;
            case '/keyvaluematch': paths.push({ href: '#' + path, text: '集合分配' }); break;
            default: (function () {
            })(); break;
        }
        amplify.publish('path.refresh', paths);
    });
});
app.controller('NavCtrl', function ($scope) { });
app.controller('SummaryCtrl', function ($scope) { });
app.controller('KeyValueCtrl', function ($scope, KeyValueSvc) {
    $scope.main = {
        mode: 'list',

        list: {
            entities: [],
            page: 1,
            pages: 1,
            total: 0,
            remove: function (index) {
                var entity = this.entities[index];
                KeyValueSvc.remove(entity.Id).then(function (rep) {
                    if (rep.data) {
                        $scope.main.list.entities.splice(index, 1);
                        $scope.main.list.total = $scope.main.list.total - 1;
                    }
                });
            },
            load: function (p) {
                if (!p || p <= 0) { throw 'error page'; return; }
                KeyValueSvc.getList(p).then(function (rep) {
                    $scope.main.list.entities = rep.data;
                    var responsePages = parseInt(rep.headers('X-Edu-Pages'));
                    $scope.main.list.pages = isNaN(responsePages) ? 1 : responsePages;
                    var responseTotal = parseInt(rep.headers('X-Edu-Total'));
                    $scope.main.list.total = isNaN(responseTotal) ? 1 : responseTotal;
                });
            },
            refresh: function () {
                this.load(this.page);
            }
        },

        edit: {
            entity: {},
            show: function (index) {
                this.entity = $scope.main.list.entities[index];
                $scope.main.mode = 'edit';
            },
            submit: function () {
                KeyValueSvc.update(this.entity).then(function (rep) {
                    $scope.main.mode = 'list';
                });
            }
        },

        add: {
            entity: {/* Name: '', Value: '', Description: '', Mark: '' */ },
            show: function () {
                $scope.main.mode = 'add';
            },
            submit: function () {
                KeyValueSvc.add(this.entity).then(function (rep) {
                    if (rep.data) {
                        $scope.main.list.entities.push(rep.data);
                        $scope.main.list.total = $scope.main.list.total + 1;
                        $scope.main.mode = 'list';
                        $scope.main.add.entity = {};
                    }
                });
            }
        }
    };
    $scope.$watch('main.list.page', function (newVal) {
        $scope.main.list.load(newVal);
    });
});

app.controller('KeyValueGroupCtrl', function ($scope, KeyValueGroupSvc, KeyValueSvc, KeyValueMatchSvc) {
    $scope.main = {
        mode: 'list',

        list: {
            entities: [],
            page: 1,
            pages: 1,
            total: 0,
            remove: function (index) {
                var entity = this.entities[index];
                KeyValueGroupSvc.remove(entity.Id).then(function (rep) {
                    if (rep.data) {
                        $scope.main.list.entities.splice(index, 1);
                        $scope.main.list.total = $scope.main.list.total - 1;
                    }
                });
            },
            load: function (p) {
                if (!p || p <= 0) { throw 'error page'; return; }
                KeyValueGroupSvc.getList(p).then(function (rep) {
                    $scope.main.list.entities = rep.data;
                    var responsePages = parseInt(rep.headers('X-Edu-Pages'));
                    $scope.main.list.pages = isNaN(responsePages) ? 1 : responsePages;
                    var responseTotal = parseInt(rep.headers('X-Edu-Total'));
                    $scope.main.list.total = isNaN(responseTotal) ? 1 : responseTotal;
                });
            },
            refresh: function () {
                this.load(this.page);
            }
        },

        edit: {
            entity: {},
            show: function (index) {
                this.entity = $scope.main.list.entities[index];
                $scope.main.mode = 'edit';
            },
            submit: function () {
                KeyValueGroupSvc.update(this.entity).then(function (rep) {
                    $scope.main.mode = 'list';
                });
            }
        },

        add: {
            entity: {/* Name: '', Category: '', Description: '' */ },
            show: function () {
                $scope.main.mode = 'add';
            },
            submit: function () {
                KeyValueGroupSvc.add(this.entity).then(function (rep) {
                    if (rep.data) {
                        $scope.main.list.entities.push(rep.data);
                        $scope.main.list.total = $scope.main.list.total + 1;
                        $scope.main.mode = 'list';
                        $scope.main.add.entity = {};
                    }
                });
            }
        },

        manage: {
            group: {},
            includes: {
                list: [],
                page: 1,
                pages: 1,
                total: 0,
                load: function (p) {
                    KeyValueSvc.getInclude($scope.main.manage.group.Id, this.page).then(function (rep) {
                        $scope.main.manage.includes.list = rep.data;
                        $scope.main.manage.includes.pages = parseInt(rep.headers('X-Edu-Pages'));
                        $scope.main.manage.includes.total = parseInt(rep.headers('X-Edu-Total'));
                    });
                },
                refresh: function () {
                    this.load(this.page);
                },
                remove: function (index) {
                    var groupId = $scope.main.manage.group.Id;
                    var valueId = this.list[index]['Id'];
                    KeyValueMatchSvc.remove(groupId, valueId).then(function (rep) {
                        if (rep.data) {
                            var removed = $scope.main.manage.includes.list.splice(index, 1);
                            $scope.main.manage.excludes.refresh();
                        }
                    });
                }
            },
            excludes: {
                list: [],
                page: 1,
                pages: 1,
                total: 0,
                load: function (p) {
                    KeyValueSvc.getExclude($scope.main.manage.group.Id, this.page).then(function (rep) {
                        $scope.main.manage.excludes.list = rep.data;
                        $scope.main.manage.excludes.pages = parseInt(rep.headers('X-Edu-Pages'));
                        $scope.main.manage.excludes.total = parseInt(rep.headers('X-Edu-Total'));
                    });
                },
                refresh: function () {
                    this.load(this.page);
                },
                remove: function (index) {
                    var groupId = $scope.main.manage.group.Id;
                    var valueId = this.list[index]['Id'];
                    KeyValueMatchSvc.add(groupId, valueId).then(function (rep) {
                        if (rep.data) {
                            var removed = $scope.main.manage.excludes.list.splice(index, 1);
                            $scope.main.manage.includes.refresh();
                        }
                    });
                }
            },
            show: function (index) {
                this.group = $scope.main.list.entities[index];
                $scope.main.mode = 'manage';

                //load include
                this.includes.refresh();

                //load exclude
                this.excludes.refresh();
            },
            submit: function () { }
        }
    };
    $scope.$watch('main.list.page', function (newVal) {
        $scope.main.list.load(newVal);
    });

    $scope.$watch('main.manage.includes.page', function (newVal) {
        $scope.main.manage.includes.load(newVal);
    });
    $scope.$watch('main.manage.excludes.page', function (newVal) {
        $scope.main.manage.excludes.load(newVal);
    });
});

app.controller('KeyValueMatchCtrl', function ($scope, KeyValueGroupSvc, KeyValueMatchSvc) {
    $scope.main = {
        mode: 'list',

        list: {
            entities: [],
            page: 1,
            pages: 1,
            total: 0,
            remove: function (index) {
                var entity = this.entities[index];
                KeyValueGroupSvc.remove(entity.Id).then(function (rep) {
                    if (rep.data) {
                        $scope.main.list.entities.splice(index, 1);
                        $scope.main.list.total = $scope.main.list.total - 1;
                    }
                });
            },
            load: function (p) {
                if (!p || p <= 0) { throw 'error page'; return; }
                KeyValueGroupSvc.getList(p).then(function (rep) {
                    $scope.main.list.entities = rep.data;
                    var responsePages = parseInt(rep.headers('X-Edu-Pages'));
                    $scope.main.list.pages = isNaN(responsePages) ? 1 : responsePages;
                    var responseTotal = parseInt(rep.headers('X-Edu-Total'));
                    $scope.main.list.total = isNaN(responseTotal) ? 1 : responseTotal;
                });
            }
        },

        add: {
            entity: { Name: '', Category: '', Description: '' },
            show: function () {
                $scope.main.mode = 'add';
            },
            submit: function () {
                if (!this.entity.Name) { return; }
                KeyValueGroupSvc.add(this.entity).then(function (rep) {
                    if (rep.data) {
                        $scope.main.list.entities.push(rep.data);
                        $scope.main.list.total = $scope.main.list.total + 1;
                        $scope.main.mode = 'list';
                    }
                });
            }
        }
    };
    $scope.$watch('main.list.page', function (newVal) {
        $scope.main.list.load(newVal);
    });
});

