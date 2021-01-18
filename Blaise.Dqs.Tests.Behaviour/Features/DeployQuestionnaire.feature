@questionnaire
Feature: Deploy Questionnaire
	As a Survey Team Research Officer
	I want to deploy a questionnaire to a Blaise server in the production environment
	So that I can set up a new survey in Blaise 5 for Telephone Operations nudge/data collection

@Smoke @LU-7990
Scenario: Successful log in to Questionnaire Deployment Service
Given I have launched the Questionnaire Deployment Service
When I view the landing page
Then I am presented with an option to deploy a new questionnaire

@Smoke @LU-7990
Scenario: Deploy selected file
Given I have selected the questionnaire package I wish to deploy
When I confirm my selection
Then I am presented with a successful deployment information banner
And the questionnaire is active in blaise

@Smoke @LU-7994
Scenario: Questionnaire package already in Blaise
Given I have selected the questionnaire package I wish to deploy
And the package I have selected already exists in Blaise
When I confirm my selection
Then I am presented with questionnaire already exists screen

@regression @LU-7994
Scenario: Back-out of deploying a questionnaire
Given I have been presented with questionnaire already exists screen
When I select cancel
Then I am returned to the landing page
And the questionnaire has not been overwritten
