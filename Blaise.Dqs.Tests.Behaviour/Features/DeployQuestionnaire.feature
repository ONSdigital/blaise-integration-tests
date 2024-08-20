@deploy-questionnaire
Feature: Deploy questionnaire

Background:
	Given I am a BDSS user
	And I have logged into DQS

Scenario: Option to deploy questionnaire
	Given I have launched DQS
	When I view the landing page
	Then I have the option to deploy a questionnaire

@smoke
Scenario: List deployed questionnaires
	Given I have launched DQS
	And there is a questionnaire installed in Blaise
	When I view the landing page
	Then I am presented with a list of deployed questionnaires

@smoke
Scenario: Deploy questionnaire with TO start date
	Given I have selected the questionnaire package I wish to deploy
	When I confirm my selection
	And I set a TO start date for today
	And the deployment summary confirms the TO start date for today
	And I deploy the questionnaire
	Then I am presented with a successful deployment information banner
	And the questionnaire is active in Blaise

Scenario: Deploy questionnaire without TO start date
	Given I have selected the questionnaire package I wish to deploy
	When I confirm my selection
	And I dont select a TO start date
	And the deployment summary confirms no TO start date
	And I deploy the questionnaire
	Then I am presented with a successful deployment information banner
	And the questionnaire is active in Blaise

Scenario: Back out of deploying an existing questionnaire
	Given I have been presented with questionnaire already exists screen
	When I select cancel
	Then I am returned to the landing page
	And the questionnaire has not been overwritten

Scenario: Overwrite an existing questionnaire that does not have data
	Given I have been presented with questionnaire already exists screen
	And the questionnaire does not have data
	When I select overwrite
	And confirm my selection
	And I dont select a TO start date
	And the deployment summary confirms no TO start date
	And I deploy the questionnaire
	Then the questionnaire is deployed and overwrites the existing questionnaire
