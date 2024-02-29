@cati
Feature: Daybatch
	In order to capture respondents data accurately
	As a responsible data owner
	I want to be able to create a daybatch to schedule the capture of respondents answers

@regression
Scenario: Create a daybatch to schedule the capture respondent data
	Given There is a questionnaire installed on a Blaise environment
	And I have created sample cases for the questionnaire
		| primarykey | outcomecode | telephoneno  |
		| 9000003    |             | 07000 000 00 |
	And I log on to Cati as an administrator
	When I create a daybatch for today
	Then the sample cases are present on the daybatch entry screen
		| primarykey | outcomecode | telephoneno  |
		| 9000003    |             | 07000 000 00 |