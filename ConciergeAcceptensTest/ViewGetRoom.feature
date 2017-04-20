Feature: ViewGetRoom
	Check the information about a specific room
	that is passed to the browser/application

@mytag
Scenario: Check room information
	Given eConcierge backend is up and running
	And There are atleast 1 room
	Then the correct information should be delivered when getRoom is used
