Feature: DeployQuestionnaire
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario: Successful log in to Questionnaire Deployment Service
Given I have launched the Questionnaire Deployment Service
When I view the landing page
Then I am presented with an option to deploy a new questionnaire

Scenario: Deploy selected file
Given I have selected the questionnaire package I wish to deploy
When I confirm my selection
Then I am presented with a successful deployment information banner
#
#Scenario: Survey is in Blaise
#Given I have successfully deployed a survey instrument to Blaise
#When I view the survey list in the CATI Dashboard
#Then I can see the deployed questionnaire in the list
#And the status is 'Active