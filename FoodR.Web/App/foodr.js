var app = angular.module('foodr', ['ngRoute']);

app.config(function($routeProvider) {
  $routeProvider
    .when('/', {
      controller:'HomeCtrl',
      templateUrl:'/Templates/home.html'
    })
	.when('/locate', {
      controller:'LocateCtrl',
      templateUrl: '/Templates/locate.html'
    })
    .when('/map/:zipcode', {
      controller:'MapCtrl',
      templateUrl: '/Templates/map.html'
    })
	.when('/map', {
      controller:'MapCtrl',
      templateUrl: '/Templates/map.html'
    })
    .when('/schedule', {
      controller:'ScheduleCtrl',
      templateUrl: '/Templates/schedule.html'
    })
	.when('/login', {
      controller:'LoginCtrl',
      templateUrl: '/Templates/login.html'
    })
    .otherwise({
      redirectTo:'/'
    });
})

app.controller('HomeCtrl', function($scope) {
	
})

app.controller('LocateCtrl', function($scope) {
	
})

app.controller('MapCtrl', function($scope, $routeParams) {
	$scope.zipcode = $routeParams.zipcode;
})

app.controller('ScheduleCtrl', function($scope) {
    $scope.vendors = [
		{
			name: 'Happy Grilled Cheese',
			rating: 3,
			comments: 'Some comments from the food truck',
			schedules: [
				{
					where: 'Av Med Building',
					address: '1234 Main Street',
					from: '11:00 AM',
					to: '2:00 PM'
				},
				{
					where: 'Arrd Wolf',
					address: '1234 Other Street',
					from: '6:00 PM',
					to: '10:00 PM'
				}
			]
		}
	];
})

app.controller('LoginCtrl', function($scope) {
	
})