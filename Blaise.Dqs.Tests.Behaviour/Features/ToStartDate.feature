@ToStartDate
Feature: ToStartDate
	Simple calculator for adding two numbers

Background: 
	Given I am a BDSS user
	And I have logged into to DQS

#Smoke
Scenario: An Instrument has been deployed without a start date and I wish to add one
	Given An instrument is installed in Blaise
	And The instrument has no TO start date
	When I add a TO start date of 'today'
	Then The TO start date for 'today' is stored against the instrument

#Smoke
Scenario: An Instrument has been deployed with a start date and I wish to change it
	Given An instrument is installed in Blaise
	And The instrument has a start date of 'today'
	When I change the TO start date to 'tomorrow'
	Then The TO start date for 'tomorrow' is stored against the instrument

#Smoke
Scenario: An Instrument has been deployed with a start date and I wish to remove it
	Given An instrument is installed in Blaise
	And The instrument has a start date of 'today'
	When I change the TO start date to no TO start date
	Then The TO start date should not be set