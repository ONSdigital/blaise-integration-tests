Feature: InstallInstrument
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@smoke
Scenario: Install an instrument into a Blaise environment
Given I have an instrument we wish to use to capture respondents data
When I install the instrument into a Blaise environment 
Then the instrument is available to use in the Blaise environment 