/// <reference path="../../Vendors/angular.min.js" />


angular.module('edu.services.regentity', []).factory('RegEntitySvc', function ($http, AppConstant) {
	var service = {
		getApi: function (append) {
			return AppConstant.getApi({
				entity: 'RegEntity',
				params: append
			});
		},

		get: function (id) {
			var promise = undefined;
			if (!id) { throw 'error registration id at edu.services.reg -> get'; }
			if (id.indexOf('-') != -1) {
				promise = $http({
					method: 'get',
					url: this.getApi(),
					params: { gid: id }
				});
			} else {
				promise = $http({
					method: 'get',
					url: this.getApi(id)
				});
			}
			return promise;
		},
		getList: function (page) {
			var take = AppConstant.perPage;
			var skip = (page - 1) * take;
			var promise = $http({
				method: 'get',
				url: this.getApi(),
				params: { take: take, skip: skip }
			});
			return promise;
		},

		add: function (reg, user, train) {

			var data = this._combineData(reg, user, train);
			var promise = $http({
				method: 'put',
				url: this.getApi(),
				data: data
			});
			return promise;
		},

		update: function (reg, user, train) {
			var data = this._combineData(reg, user, train);
			var promise = $http({
				method: 'post',
				url: this.getApi(),
				data: data
			});
			return promise;
		},

		_combineData: function (reg, user, train) {
			var data = { TrainCategory: train.Category, Reg: reg, User: user };

			switch (train.Category) {
				case 'EduTrain':
				case 'EduTrains':
				case '学历教育':
				case '学历教育培训':
					data.Edu = train;
					break;
				case 'CertificationTrain':
				case 'CertificationTrains':
				case 'CertTrain':
				case '资格证培训':
					data.Cert = train;
					break;
				case 'SchoolTrain':
				case 'SchoolTrains':
				case '中小学培训':
					data.School = train;
					break;
				default:
					data = undefined;
			}

			if (data === undefined) { throw 'error train category at services.registration -> add'; }
			return data;
		}
	};
	return service;
});