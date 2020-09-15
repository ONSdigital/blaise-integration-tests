Feature: ProcessNisraCases
	In order to process cases gathered online
	As a service
	I want to be given cases to import representing the data captured online

Scenario: There is a Nisra file available in the bucket and the blaise database is empty
	Given there is a Nisra file available 
	And the nisra file contains '40' cases
	And the blaise database is empty
	When the nisra file is processed
	Then The blaise database will contain '40' cases

Scenario: There is a Nisra file available that contains cases that already exists in the blaise database
	Given there is a Nisra file available 
	And the nisra file contains the following cases
	| primarykey | outcome | mode |
	| 900001     | 110     | Web  |
	| 900002     | 120     | Web  |
	| 900003     | 110     | Web  |
	| 900004     | 110     | Web  |
	And the blaise database contains the following cases
	| primarykey | outcome | mode |
	| 900001     | 120     | Tel  |
	| 900002     | 110     | Tel  |
	| 900003     | 120     | Tel  |
	| 900004     | 120     | Tel  |
	When the nisra file is processed
	Then the blaise database will contain the following cases
	| primarykey | outcome | mode |
	| 900001     | 110     | Web  |
	| 900002     | 110     | Tel  |
	| 900003     | 110     | Web  |
	| 900004     | 110     | Web  |

Scenario: Load test x iterations every y minutes for z hour
	Given there is a Nisra file available 
	And the nisra file contains '2000' cases
	And the blaise database is empty
	When the nisra file is processed every '15' minutes for '1' hour(s)
	Then The blaise database will contain '2000' cases