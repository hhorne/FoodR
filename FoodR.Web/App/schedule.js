
function ScheduleCtrl($scope) {
    $scope.vendors = [
		{
            id: 1,
		    name: 'Happy Grilled Cheese',
		    rating: 3,
		    description: 'Some comments from the food truck',
		    events: [
				{
				    location: 'Av Med Building',
				    address1: '1234 Main Street',
				    from: '11:00 AM',
				    to: '2:00 PM'
				},
				{
				    location: 'Arrd Wolf',
				    address1: '1234 Other Street',
				    from: '6:00 PM',
				    to: '10:00 PM'
				}
		    ]
		},
        {
            id: 2,
            name: 'Taste Buds Express',
            rating: 2,
            description: 'They serve tacos of all kinds',
            events: [
				{
				    location: 'Main and Forsyth',
				    address1: '1234 Main Street',
				    from: '11:00 AM',
				    to: '2:00 PM'
				}
            ]
        },
        {
            id: 3,
            name: 'On the Fly',
            rating: 5,
            description: 'Serve a varity of food probably the best truck in town',
            events: [
				{
				    location: 'Courthouse',
				    address1: '1234 Main Street',
				    from: '11:00 AM',
				    to: '2:00 PM'
				}
            ]
        }
    ];
}
