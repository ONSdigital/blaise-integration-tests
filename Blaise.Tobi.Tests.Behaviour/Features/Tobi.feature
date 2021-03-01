@tobi
Feature: tobi
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@smoke @regression @HappyPath
Scenario: View live survey list in TOBI
Given I have internet access
When I launch TOBI
Then I will be able to view all live surveys with questionnaires loaded in Blaise, identified by their three letter acronym (TLA), i.e. OPN, LMS

@regression @HappyPath
Scenario: Select survey
Given I can view a list of surveys on Blaise within TOBI
When I select the survey I am working on
Then I am presented with a list of active questionnaires to be worked on that day for that survey

@regression @HappyPath
Scenario: Select questionnaire
Given I can view a list of live questionnaires for the survey I am allocated to
When I select a link to interview against the questionnaire with the survey dates I am working on
Then I am presented with the Blaise log in

@regression @HappyPath
Scenario: Return to select survey
Given I have selected a survey
When I do not see the questionnaire that I am working on
Then I am able to go back to view the list of surveys
