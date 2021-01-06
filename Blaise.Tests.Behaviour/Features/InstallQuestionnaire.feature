@instrument
Feature: Install
	In order to capture respondents data accurately
	As a responsible data owner
	I want to be able to install an instrument with a defined set of questions

@smoke
Scenario: Install an instrument into a Blaise environment
	Given I have an instrument I want to use to capture respondents data
	When I install the instrument into a Blaise environment 
	Then the instrument is available to use in the Blaise environment 

@smoke
Scenario: Install an instrument into a Blaise environment using Cati configuration
	Given I have an instrument I want to use to capture respondents data
	When I install the instrument into a Blaise environment specifying a Cati configuration
	Then the instrument is configured to capture respondents data via Cati