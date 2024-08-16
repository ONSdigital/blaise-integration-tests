@delete-questionnaire
Feature: Delete questionnaire

Background:
	Given I am a BDSS user
	And I have logged into DQS

@smoke
Scenario: Confirm deletion of a questionnaire
	Given I have a questionnaire I want to delete
	And I select delete on the questionnaire details page
	And I am taken to the delete confirmation page
	When I confirm that I want to proceed
	Then the questionnaire is removed from Blaise
