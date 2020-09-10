Feature: ProcessNisraCases
	In order to process cases gathered online
	As a service
	I want to be given cases to import representing the data captured online

Scenario: There is a Nisra file available
	Given there is a Nisra file available 
	And the file contains '20' cases which are 'Complete' with an outcome of '110'
	And the file contains '20' cases which are 'Partial' with an outcome of '120'
	When the file is processed
	Then the cases will be imported into blaise