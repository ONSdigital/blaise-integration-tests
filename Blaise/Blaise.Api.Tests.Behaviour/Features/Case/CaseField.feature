@case
Feature: As a service I wish to be able to retrieve the value of a field in a case to determine it's status
	In order to facilitate custom workflows
	As a service
	I want to be able to retrieve the value of fields in a blaise case

Scenario Outline: Get the value of a field in a case
	Given a case exists in blaise where the field '<field>' has a value of '<value>'
	When I retrieve the value of the field '<field>'
	Then the value should be '<value>'
	Examples: 
	| field     | value |
	| CaseId    | 1     |
	| CaseId    | 100   |
	| HOut      | 110   |
	| HOut      | 210   |
	| Completed | 0     |
	| Completed | 1     |
	| Processed |       |
	| Processed | 1     |
