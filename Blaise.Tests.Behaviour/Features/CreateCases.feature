@create-cases
Feature: Create cases

@smoke
Scenario: Create cases
	Given there is a questionnaire installed
	When I create cases for the questionnaire
		| primarykey | outcomecode | telephoneno |
		| 9001       | 110         | 07000000000 |
		| 9002       | 210         | 07900000000 |
		| 9003       | 130         | 07800000000 |
	Then the cases are available in the questionnaire
		| primarykey | outcomecode | telephoneno |
		| 9001       | 110         | 07000000000 |
		| 9002       | 210         | 07900000000 |
		| 9003       | 130         | 07800000000 |
