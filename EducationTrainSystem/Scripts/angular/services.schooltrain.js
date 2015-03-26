angular.module('edu.services.schooltrain', [])
   .factory('SchoolTrainSvc', function ($http, AppConstant) {
       var service = {
           getApi: function (append) {
               return AppConstant.getApi({
                   entity: 'SchoolTrain',
                   params: append
               });
           },
           get: function (id) {
               var promise = $http({
                   method: 'get',
                   url: this.getApi(id)
               });
               return promise;
           }
       };
       return service;
   });