@cati
Feature: Daybatch
	In order to capture respondents data accurately
	As a responsible data owner
	I want to be able to create a daybatch to schedule the capture of respondents answers

@smoke
Scenario: Create a daybatch to schedule the capture respondent data
	Given I have an instrument installed on a Blaise environment
	And I have created sample cases for the instrument
		| primarykey | outcomecode | telephoneno  |
		| 900001     | 110         | 07000 000 00 |
	And I log on to the Cati Dashboard
	When I create a daybatch for today
	Then the sample cases are present on the daybatch entry screen
		| primarykey | outcomecode | telephoneno  |
		| 900001     | 110         | 07000 000 00 |