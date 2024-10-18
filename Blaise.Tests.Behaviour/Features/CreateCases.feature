@create-casess
Feature: Create cases

@smoke
Scenario: Create cases
	Given there is a questionnaire installed
	When I create cases for the questionnaire
		| primaryKeyValues						| outcomecode | telephoneno |
		| {"QID.Serial_Number": "9001"}			| 110         | 07000000000 |
		| {"QID.Serial_Number": "9002"}			| 110         | 07000000000 |
		| {"QID.Serial_Number": "9003"}			| 110         | 07000000000 |
	Then the cases are available in the questionnaire
		| primaryKeyValues					  | outcomecode | telephoneno |
		| {"QID.Serial_Number": "9001"}       | 110         | 07000000000 |
		| {"QID.Serial_Number": "9002"}       | 110         | 07000000000 |
		| {"QID.Serial_Number": "9003"}       | 110         | 07000000000 |
