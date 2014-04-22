function MapCtrl($scope) {

    $scope.vendors = [
        {
            name: 'Happy Grilled Cheese',
            id: 1,
            lat: 30.332,
            lng: -81.600,
            from: '11 PM',
            to: '2 PM'
        },
        {
            name: 'Chew Chew',
            id: 2,
            lat: 30.300,
            lng: -81.650,
            from: '11 PM',
            to: '2 PM'
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
                content: '<a href="truck"><b>' + vendor.name + '</b></a><br />From: ' + vendor.from + '<br />To: ' + vendor.to
            });

            infoWindow.open($scope.map, marker);
        });
    });

    $scope.search = function () {
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
}