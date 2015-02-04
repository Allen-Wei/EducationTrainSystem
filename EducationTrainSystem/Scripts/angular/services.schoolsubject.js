angular.module('edu.services.schoolsubject', [])
   .factory('SchoolSubjectSvc', function ($http, AppConstant) {
       var service = {
           getApi: function (append) {
               return AppConstant.getApi({
                   entity: 'SchoolSubject',
                   params: append
               });
           },
           get: function (id) {
               var promise = $http({
                   method: 'get',
                   url: this.getApi(id)
               });
               return promise;
           },
           getByTrain: function (trainId) {
               var promise = $http({
                   method: 'get',
                   url: this.getApi({ trainId: trainId })
               });
               return promise;
           }
       };
       return service;
   });