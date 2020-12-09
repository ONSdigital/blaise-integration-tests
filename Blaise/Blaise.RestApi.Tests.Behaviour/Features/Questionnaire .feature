Feature: Questionnaire 
	As a stakeholder
	I want to see a list of all questionnaires in Blaise
	So that I can see that the restful API is working

@mytag
Scenario: Return a list of available questionnaires where there is a single questionnaire
	Given I have an instrument installed on a Blaise environment
	When the API is queried to return all active questionnaires
	Then details of questionnaire a is returned
