﻿@delete
Feature: DeleteQuestionnaire
I want to be able to delete a questionnaire from Blaise via the questionnaire list
So that the system can be cleared of completed surveys

Background: 
	Given I am a BDSS user
	And I have logged into to DQS

Scenario: Delete questionnaire when it is active
	Given I have a questionnaire I want to delete 
	And the questionnaire is active
	When I select the questionnaire in the list
	And I am taken to the questionnaire details page
	Then I will have the option to delete the questionnaire

Scenario: Delete questionnaire when it is not active
	Given I have a questionnaire I want to delete 
	And the questionnaire is not active
	When I select the questionnaire in the list
	And I am taken to the questionnaire details page
	Then I will have the option to delete the questionnaire

@smoke
Scenario: Confirm deletion of a questionnaire
	Given I have a questionnaire I want to delete 
	And I select delete on the questionnaire details page
	And I am taken to the delete confirmation screen
	When I confirm that I want to proceed
	Then the questionnaire is removed from Blaise