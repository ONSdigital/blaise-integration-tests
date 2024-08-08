@questionnaire
Feature: Install Questionnaire
	In order to capture respondents data accurately
	As a responsible data owner
	I want to be able to install a questionnaire with a defined set of questions

@smoke
Scenario: Install a questionnaire into a Blaise environment
	Given I have an questionnaire I want to use to capture respondents data
	When I install the questionnaire into a Blaise environment
	Then the questionnaire is available to use in the Blaise environment

@smoke
Scenario: Install a questionnaire into a Blaise environment using Cati configuration
	Given I have an questionnaire I want to use to capture respondents data
	When I install the questionnaire into a Blaise environment specifying a Cati configuration
	Then the questionnaire is configured to capture respondents data via Cati
