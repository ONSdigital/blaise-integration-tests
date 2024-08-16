@install-questionnaire
Feature: Install questionnaire

@smoke
Scenario: Install questionnaire
	Given I have a questionnaire I want to install
	When I install the questionnaire
	Then the questionnaire is available
