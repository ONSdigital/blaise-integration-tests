@tobi
Feature: tobi

Scenario: View live surveys
Given there are live surveys
When I launch TOBI
Then I am presented with a list of live surveys

Scenario: Select survey
Given I can view a list of live surveys
When I select a survey
Then I am presented with a list of questionnaires for the survey

@smoke
Scenario: Select questionnaire
Given I can view a list of questionnaires for a live survey
When I select a questionnaire
Then I am presented with the Blaise login
