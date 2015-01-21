(function () {

    var filters = angular.module('registration.filters', []);
    filters.filter('range', function () {
        return function (input, min, max) {
            var range = [];
            for (var i = min; i <= max; i++) {
                range.push(i);
            }
            return range;
        };
    });


    filters.filter('propsFilter', function () {
        return function (items, props) {
            var out = [];

            if (angular.isArray(items)) {
                items.forEach(function (item) {
                    var itemMatches = false;

                    var keys = Object.keys(props);
                    for (var i = 0; i < keys.length; i++) {
                        var prop = keys[i];
                        var text = props[prop].toLowerCase();
                        if (item[prop].toString().toLowerCase().indexOf(text) !== -1) {
                            itemMatches = true;
                            break;
                        }
                    }

                    if (itemMatches) {
                        out.push(item);
                    }
                });
            } else {
                // Let the output be the input untouched
                out = items;
            }

            return out;
        };
    });

    filters.filter('jsonDateConvert', function () {
        return function (jsonDate) {
            var dateRegex = /^\/Date\((\d+)\)\/$/;
            if (dateRegex.test(jsonDate)) {
                var splitedDate = dateRegex.exec(jsonDate);
                if (splitedDate && splitedDate.length == 2) {
                    var dateNumbers = parseInt(splitedDate[1]);
                    if (!isNaN(dateNumbers)) {
                        return new Date(dateNumbers);
                    }
                }
            }
            return jsonDate;
        };
    });
    filters.filter('toShortDate', function () {
        return function (date) {
            if (date) {
                return (date.getMonth() + 1) + '/' + date.getDate() + '/' + (date.getYear() + 1900);
            } else {
                return date;
            }
        }
    });
    filters.filter('toPercent', function () {
        return function (value) {
            var number = parseFloat(value)
            if (isNaN(number)) { return value; }
            return ((number * 100).toFixed(2)) + '%';
        }
    });
})();