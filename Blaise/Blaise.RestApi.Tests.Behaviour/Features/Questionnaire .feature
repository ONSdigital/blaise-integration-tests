Feature: Questionnaire
	As a stakeholder
	I want to see a list of all questionnaires in Blaise
	So that I can see that the restful API is working

@mytag
Scenario: Return a list of available questionnaires where there is a single questionnaire
	Given There is an instrument installed on a Blaise environment
	And the questionnaire is active
	When the API is queried to return all active questionnaires
	Then details of questionnaire a is returned

Scenario: Return a an empty list of questionnaires when there are no active questionnaires are available
	Given There is an instrument installed on a Blaise environment
	And the questionnaire is inactive
	When the API is queried to return all active questionnaires
	Then an empty list is returned

