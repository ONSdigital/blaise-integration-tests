@tobi
Feature: tobi

@smoke
Scenario: View live surveys
Given there are live surveys
When I launch TOBI
Then I am presented with a list of live surveys

Scenario: Select survey
Given there are live surveys
When I launch TOBI
And I select a survey
Then I am presented with a list of questionnaires for the survey

Scenario: Select questionnaire
Given I can view a list of live questionnaires for the survey I am allocated to
When I select a link to interview against the questionnaire with the survey dates I am working on
Then I am presented with the Blaise log in

Scenario: Return to select survey
Given I have selected a survey
When I do not see the questionnaire that I am working on
Then I am able to go back to view the list of surveys
