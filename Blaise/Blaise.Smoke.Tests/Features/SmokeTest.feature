Feature: SmokeTest
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario: Upload instrument
	Given I have an instrument we wish to use
	When I upload the instrument
	Then the instrument is available for use

#Scenario:  Create cases
#	Given we have a sample set of cases we wish to use
#	When I create the cases in the instrument
#	Then cases are present in the db
#
#Scenario: Create daybatch
#	Given an instrument is available with a sample case (s)
#	When I create a daybatch
#	Then the cases are ready for data capture
#
#Scenario: Access case
#	Given I have a case ready for data capture
#	When I access the case information
#	Then I am presented with the data capture screen for the case