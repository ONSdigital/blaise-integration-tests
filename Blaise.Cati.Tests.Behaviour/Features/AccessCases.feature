@access-cases
Feature: Access cases

@smoke
Scenario: Access case via interview link
	Given there is a CATI questionnaire installed
	And I have created sample cases for the questionnaire
		| primarykey | outcomecode | telephoneno  |
		| 9000001    |             | 07000 000 00 |
	And I log on to Cati as an interviewer
	And I have created a daybatch for today
	When I click the play button for case '9000001'
	Then I am able to capture the respondents data for case '9000001'

@smoke
Scenario: Access case via Scheduler
	Given there is a CATI questionnaire installed
	And I have created sample cases for the questionnaire
		| primarykey | outcomecode | telephoneno  |
		| 9000002    |             | 07000 000 00 |
	And I log on to Cati as an administrator
	And I have created a daybatch for today
	When The time is within the day batch parameters
	And I Open the cati scheduler as an interviewer
	Then I am able to capture the respondents data for case '9000002'
