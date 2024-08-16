@to-start-date
Feature: TO start date

Background:
	Given I am a BDSS user
	And I have logged into DQS

@smoke
Scenario: A questionnaire has been deployed without a start date and I wish to add one
	Given a questionnaire has been deployed
	And the questionnaire has no TO start date
	When I add a TO start date of 'today'
	Then the TO start date for 'today' is stored against the questionnaire

Scenario: A questionnaire has been deployed with a start date and I wish to change it
	Given a questionnaire has been deployed
	And the questionnaire has a start date of 'today'
	When I change the TO start date to 'tomorrow'
	Then the TO start date for 'tomorrow' is stored against the questionnaire

Scenario: A questionnaire has been deployed with a start date and I wish to remove it
	Given a questionnaire has been deployed
	And the questionnaire has a start date of 'today'
	When I remove the TO start date
	Then the TO start date should not be set
