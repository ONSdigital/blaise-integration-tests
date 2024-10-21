@access-cases
Feature: Access cases

@smoke
Scenario: Access case via play button
	Given there is a CATI questionnaire installed
	And I have created sample cases for the questionnaire
		| primaryKey | outcomecode | telephoneno |
		| 9001       |             | 07000000000 |
	And I log into the CATI dashboard as an administrator
	And I have created a daybatch for today
	When I click the play button for case '9001'
	Then I am able to capture the respondents data for case '9001'

Scenario: Access case via scheduler
	Given there is a CATI questionnaire installed
	And I have created sample cases for the questionnaire
		| primaryKey | outcomecode | telephoneno |
		| 9002       |             | 07000000000 |
	And I log into the CATI dashboard as an administrator
	And I have created a daybatch for today
	When the time is within the daybatch parameters
	And I open the CATI scheduler as an interviewer
	Then I am able to capture the respondents data for case '9002'
