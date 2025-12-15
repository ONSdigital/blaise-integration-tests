@create-cases
Feature: Create cases

@smoke
Scenario: Create cases
	Given there is a questionnaire installed
	When I create cases for the questionnaire
		| primarykey | outcomecode | telephoneno |
		| 9011       | 110         | 07000000000 |
		| 9012       | 210         | 07900000000 |
		| 9013       | 130         | 07800000000 |
	Then the cases are available in the questionnaire
		| primarykey | outcomecode | telephoneno |
		| 9011       | 110         | 07000000000 |
		| 9012       | 210         | 07900000000 |
		| 9013       | 130         | 07800000000 |
