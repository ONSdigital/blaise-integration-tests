@questionnaire
Feature: DeployQuestionnaire
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@Smoke
Scenario: Successful log in to Questionnaire Deployment Service
Given I have launched the Questionnaire Deployment Service
When I view the landing page
Then I am presented with an option to deploy a new questionnaire

@Smoke
Scenario: Deploy selected file
Given I have selected the questionnaire package I wish to deploy
When I confirm my selection
Then I am presented with a successful deployment information banner
And the questionnaire is active in blaise


