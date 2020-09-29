@case
Feature: As a service I wish to be able to create and update cases in Blaise
	In order to facilitate custom workflows
	As a service
	I want to be able to manipulate cases in Blaise

Scenario: Create a new case in Blaise
	Given I have a new case I want to create
	When I create the case
	Then the case is successfully created 

Scenario: Get an existing case in Blaise
	Given a case exists in blaise with the following data
	| fieldname   | fieldvalue |
	| QID.Case_ID | 1          |   

	When I retrieve the case
	Then the correct case is returned 
	| fieldname   | fieldvalue |
	| QID.Case_ID | 1          |   

Scenario: Update an existing case in Blaise
	Given a case exists in blaise with the following data
	| fieldname   | fieldvalue |
	| QID.Case_ID | 1          |      

	When I update the case with the following data
	| fieldname   | fieldvalue |
	| QID.Case_ID | 2          |

	Then the case is updated successfully 
	| fieldname   | fieldvalue |
	| QID.Case_ID | 2          |

Scenario: Check a case exists in Blaise returns true if the case exists
	Given a case exists in blaise with the primary key '900000'
	When I check to see if the case exists 
	Then the case exists

Scenario: Check a case exists in Blaise returns false if the case does not exist
	Given a case does not exist in blaise with the primary key '900000'
	When I check to see if the case exists
	Then the case does not exist

Scenario: Delete an existing case in Blaise
	Given a case exists in blaise with the primary key '900000'
	When I delete the case
	Then the case no longer exists in blaise

Scenario: Get a list of existing cases in Blaise
	Given there are a number of existing cases in Blaise
	| primarykey |
	| 900000     |
	| 900001     |
	| 900002     |
	When I retrieve a list of cases
	Then all existing cases are returned
	| primarykey |
	| 900000     |
	| 900001     |
	| 900002     |