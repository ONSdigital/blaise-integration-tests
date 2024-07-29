@daybatch
Feature: Daybatch

Scenario: Create a daybatch to schedule the capture of respondent data
	Given there is a CATI questionnaire installed
	And I have created sample cases for the questionnaire
		| primarykey | outcomecode | telephoneno  |
		| 9000003    |             | 07000 000 00 |
	And I log on to Cati as an administrator
	When I create a daybatch for today
	Then the sample cases are present on the daybatch entry screen
		| primarykey | outcomecode | telephoneno  |
		| 9000003    |             | 07000 000 00 |
