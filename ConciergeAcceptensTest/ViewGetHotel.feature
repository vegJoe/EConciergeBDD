Feature: ViewGetHotel
	Check the information about hotel
	that is passed to the browser/application

@mytag
Scenario: Check hotel information
	Given eConcierge backend is up and running
	And There are atleast 1 hotel
	When getHotel is used
	Then the correct information should be delivered via getHotel
