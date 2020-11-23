Feature: SmokeTest
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario: 1 Upload instrument
	Given I have an instrument we wish to use
	When I upload the instrument
	Then the instrument is available for use

Scenario: 2 Create cases
	Given we have a sample set of cases we wish to use
	| primarykey | outcome | telno        |
	| 900000     | 110     | 07000 000 00 |
	| 900001     | 120     | 07000 000 00 |
	When I create the cases in the instrument
	Then cases are present in the db
	| primarykey | outcome | telno        |
	| 900000     | 110     | 07000 000 00 |
	| 900001     | 120     | 07000 000 00 |

Scenario: Create daybatch
	Given an instrument is available with a sample case (s)
	When I create a daybatch
	Then the cases are ready for data capture

Scenario: Access case
	Given I have a case ready for data capture
	When I access the case information
	Then I am presented with the data capture screen for the case

Scenario: 0 Creating an account for a new user
	Given I have a new user with the following details
	| username | password | role     | serverparks   | DefaultServerPark |
	| user1    | pass     | DST		 | tel			 | tel               |
	When I create the user account
	Then The user is created with the following details
