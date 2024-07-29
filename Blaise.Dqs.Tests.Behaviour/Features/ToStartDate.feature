@ToStartDate
Feature: ToStartDate
	Simple calculator for adding two numbers

Background: 
	Given I am a BDSS user
	And I have logged into to DQS

@smoke
Scenario: A questionnaire has been deployed without a start date and I wish to add one
	Given A questionnaire is installed in Blaise
	And the questionnaire has no TO start date
	When I add a TO start date of 'today'
	Then The TO start date for 'today' is stored against the questionnaire

@smoke
Scenario: A questionnaire has been deployed with a start date and I wish to change it
	Given A questionnaire is installed in Blaise
	And the questionnaire has a start date of 'today'
	When I change the TO start date to 'tomorrow'
	Then The TO start date for 'tomorrow' is stored against the questionnaire

@smoke
Scenario: A questionnaire has been deployed with a start date and I wish to remove it
	Given A questionnaire is installed in Blaise
	And the questionnaire has a start date of 'today'
	When I change the TO start date to no TO start date
	Then The TO start date should not be set
