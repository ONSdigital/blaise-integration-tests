@interview
Feature: Access
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@smoke @regression
Scenario: Access case via interview link
	Given There is a questionnaire installed on a Blaise environment
	And I have created sample cases for the questionnaire
		| primarykey | outcomecode | telephoneno  |
		| 9000001    |             | 07000 000 00 |
	And I log on to Cati as an interviewer
	And I have created a daybatch for today
	When I click the play button for case '9000001'
	Then I am able to capture the respondents data for case '9000001'

@regression
Scenario: Access case via Scheduler
	Given I have an questionnaire installed on a Blaise environment
	And I have created sample cases for the questionnaire
		| primarykey | outcomecode | telephoneno  |
		| 9000002    |             | 07000 000 00 |
	And I log on to Cati as an administrator
	And I have created a daybatch for today
	When The time is within the day batch parameters
	And I Open the cati scheduler as an interviewer
	Then I am able to capture the respondents data for case '9000002'