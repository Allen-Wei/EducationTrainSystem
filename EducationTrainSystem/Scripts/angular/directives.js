(function () {

    var directive = angular.module('registration.directives', []);


    directive.directive('selectDate', function ($rootScope) {
        return {
            restrict: 'EA',
            scope: { minDate: '=minDate', maxDate: '=maxDate', dateFormat: '@dateFormat' },
            link: function (scope, iele, iattrs) {
                var $ele = $(iele);
                $ele.prop('readonly', true);
                $ele.css({ 'cursor': 'pointer' });
                var parameters = {};
                if (scope.minDate) {
                    parameters.minDate = scope.minDate;
                }
                if (scope.maxDate) {
                    parameters.maxDate = scope.maxDate;
                }
                if (scope.dateFormat) {
                    parameters.dateFormat = scope.dateFormat;
                }
                $ele.datepicker(parameters);

            }
        };
    });

    directive.directive('topTip', function (TipSvc) {
        return {
            restrict: 'EA',
            scope: {},
            replace: true,
            template: '<div class="tip text-warning ng-hide" ng-show="myTip.show" ng-class="{hide:!myTip.show, animated:myTip.show, bounceInDown: myTip.show }" ng-bind="myTip.text"></div>',
            link: function (scope, iEle, iAttrs) {
                scope.myTip = TipSvc;
            }

        };
    });

})();