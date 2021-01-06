@interview
Feature: Access
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@smoke
Scenario: Access case via interview link
	Given I have an instrument installed on a Blaise environment
	And I have created sample cases for the instrument
		| primarykey | outcomecode | telephoneno  |
		| 900001     | 110         | 07000 000 00 |
	And I log on to Cati as an administrator
	And I have created a daybatch for today
	When I log on to the Interviewing Portal as an interviewer
	Then I am able to capture the respondents data for case '900001'