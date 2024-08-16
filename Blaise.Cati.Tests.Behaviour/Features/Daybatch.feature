@daybatch
Feature: Daybatch

Scenario: Create a daybatch to schedule the capture of respondent data
	Given there is a CATI questionnaire installed
	And I have created sample cases for the questionnaire
		| primarykey | outcomecode | telephoneno |
		| 9003       |             | 07000000000 |
	And I log into the CATI dashboard as an administrator
	When I create a daybatch for today
	Then the sample cases are present on the daybatch page
		| primarykey | outcomecode | telephoneno |
		| 9003       |             | 07000000000 |
