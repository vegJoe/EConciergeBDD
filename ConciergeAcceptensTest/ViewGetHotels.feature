Feature: ViewGetHotels
	Check the information about hotels
	that is passed to the browser/application

@mytag
Scenario: Check hotels information
	Given eConcierge backend is up and running
	And There are atleast 2 hotels
	Then the correct information should be delivered via getHotels
