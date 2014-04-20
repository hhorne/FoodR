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
    .when('/map/:address', {
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
    $scope.address = $routeParams.address;
    $scope.vendors = [
        {
            name: 'Happy Grilled Cheese',
            id: 1,
            lat: 30.332,
            lng: -81.600
        },
        {
            name: 'Chew Chew',
            id: 2,
            lat: 30.300,
            lng: -81.650
        }
    ];

    //google maps api stuff
    $scope.geocoder = new google.maps.Geocoder();
    var latlng = new google.maps.LatLng(30.332, -81.655); //center of jacksonville
    var mapOptions = {
        zoom: 11,
        center: latlng,
        streetViewControl: false
    }
    $scope.map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);

    angular.forEach($scope.vendors, function (vendor) {
        var latlng = new google.maps.LatLng(vendor.lat, vendor.lng);
        var marker = new google.maps.Marker({
            position: latlng,
            title: vendor.name,
            map: $scope.map
        });
        
        google.maps.event.addListener(marker, 'click', function () {
            var infoWindow = new google.maps.InfoWindow({
                content: vendor.name
            });

            infoWindow.open($scope.map, marker);
        });
    });

    $scope.search = function() {
        var address = document.getElementById("address").value;
        $scope.geocoder.geocode({ 'address': address }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                $scope.map.setCenter(results[0].geometry.location);
                $scope.map.setZoom(12);
                var marker = new google.maps.Marker({
                    map: $scope.map,
                    position: results[0].geometry.location
                });
            }
        });
    }
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

