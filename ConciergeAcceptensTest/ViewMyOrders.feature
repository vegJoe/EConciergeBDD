Feature: ViewMyOrders
	I want to check that when a product is added to a rooms existing order
	it only appers on one line with an updated amount of that specific product

@mytag
Scenario: Check product order apperence
	Given eConcierge backend is up and running
	And There exists atleast 5 different products
	And The orderlist is clear
	When Hotel guest orders 1 product with product id 8
	Then Then 1 line with 1 item should be displayed
