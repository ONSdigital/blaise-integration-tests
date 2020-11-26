Feature: Create
	In order to capture respondents data accurately
	As a responsible data owner
	I want to be able to create a new case to capture respondents answers

@smoke
Scenario: Create sample cases to capture respondent data
Given I have an instrument installed on a Blaise environment
When I create sample cases for the instrument
	| primarykey | outcomecode | telephoneno  |
	| 900001     | 110         | 07000 000 00 |
Then the sample cases are available in the Blaise environment
	| primarykey | outcomecode | telephoneno  |
	| 900001     | 110         | 07000 000 00 |