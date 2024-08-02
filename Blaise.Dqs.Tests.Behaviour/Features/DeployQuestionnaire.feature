@questionnaire
Feature: Deploy Questionnaire
	As a Survey Team Research Officer
	I want to deploy a questionnaire to a Blaise server in the production environment
	So that I can set up a new survey in Blaise 5 for Telephone Operations nudge/data collection

Background: 
	Given I am a BDSS user
	And I have logged into to DQS

@smoke
Scenario: List all questionnaires in Blaise
	Given I have launched the Questionnaire Deployment Service
	And there is a questionnaire installed in Blaise
	When I view the landing page
	Then I am presented with a list of the questionnaires already deployed to Blaise

Scenario: Successful log in to Questionnaire Deployment Service
	Given I have launched the Questionnaire Deployment Service
	When I view the landing page
	Then I am presented with an option to deploy a new questionnaire

@smoke
Scenario: Deploy selected file without Start Dates
	Given I have selected the questionnaire package I wish to deploy
	When I confirm my selection
	And I dont select a Start date
	And The set start date for questionnaire returns Start Date Not Specified
	And I Deploy The Questionnaire
	Then I am presented with a successful deployment information banner
	And the questionnaire is active in blaise

@smoke
Scenario: Deploy selected file with live dates
	Given I have selected the questionnaire package I wish to deploy
	When I confirm my selection
	And I set a start date to today
	And The set start date for questionnaire returns today
	And I Deploy The Questionnaire
	Then I am presented with a successful deployment information banner
	And the questionnaire is active in blaise

Scenario: Back-out of deploying a questionnaire
	Given I have been presented with questionnaire already exists screen
	When I select to cancel
	Then I am returned to the landing page
	And the questionnaire has not been overwritten

@smoke
Scenario: Overwrite a questionnaire that does not have data
	Given I have been presented with questionnaire already exists screen
	And the questionnaire does not have data records
	When I select to overwrite
	And confirm my selection
	And I dont select a Start date
	And The set start date for questionnaire returns Start Date Not Specified
	And I Deploy The Questionnaire
	Then Then the questionnaire package is deployed and overwrites the existing questionnaire
