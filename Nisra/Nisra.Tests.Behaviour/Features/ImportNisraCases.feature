Feature: Import Nisra cases
	In order to process cases gathered online
	As a service
	I want to be given cases to import representing the data captured online

Scenario: There is a no Nisra file available in the bucket
	Given there is a not a Nisra file available 
	And  blaise contains no cases
	When the nisra process is triggered
	Then blaise will contain '0' cases

Scenario: There is a no Nisra file available in the bucket
	Given there is a not a Nisra file available 
	And blaise contains '10' cases
	When the nisra process is triggered
	Then blaise will contain '10' cases

Scenario: There is a Nisra file available in the bucket and the blaise database is empty
	Given there is a Nisra file that contains '10' cases 
	And blaise contains no cases
	When the nisra process is triggered
	Then blaise will contain '10' cases

#Scenarios https://collaborate2.ons.gov.uk/confluence/display/QSS/Blaise+5+NISRA+Case+Processor+Flow
Scenario: There is a Nisra file available that contains cases that already exists in the blaise database, the cases are updated depending on the outcome codes
	Given there is a Nisra file that contains the following cases
	| primarykey | outcome | mode |
	#scenario 1
	| 900001     | 110     | Web  |
	#scenario 2
	| 900002     | 210     | Web  |
	#scenario 3
	| 900003     | 110     | Web  |
	#scenario 4
	| 900004     | 110     | Web  |
	| 900005     | 110     | Web  |
	| 900006     | 110     | Web  |
	| 900007     | 110     | Web  |
	| 900008     | 110     | Web  |
	| 900009     | 110     | Web  |
	#scenario 5
	| 900010     | 0       | Web  |
	#scenario 6
	| 900011     | 210     | Web  |
	#scenario 7
	| 900012     | 210     | Web  |
	| 900013     | 210     | Web  |
	| 900014     | 210     | Web  |
	| 900015     | 210     | Web  |
	| 900016     | 210     | Web  |
	| 900017     | 210     | Web  |
	#scenario 8
	| 900018     | 0       | Web  |
	#scenario 9
	| 900019     | 210     | Web  |
	#scenario 10
	| 900020     | 110     | Web  |
	
	And blaise contains the following cases
	| primarykey | outcome | mode |
	#scenario 1
	| 900001     | 110     | Tel  |
	#scenario 2
	| 900002     | 110     | Tel  |
	#scenario 3
	| 900003     | 210     | Tel  |
	#scenario 4
	| 900004     | 310     | Tel  |
	| 900005     | 430     | Tel  |
	| 900006     | 460     | Tel  |
	| 900007     | 461     | Tel  |
	| 900008     | 541     | Tel  |
	| 900009     | 542     | Tel  |
	#scenario 5
	| 900010     | 110     | Tel  |
	#scenario 6
	| 900011     | 210     | Tel  |
	#scenario 7
	| 900012     | 310     | Tel  |
	| 900013     | 430     | Tel  |
	| 900014     | 460     | Tel  |
	| 900015     | 461     | Tel  |
	| 900016     | 541     | Tel  |
	| 900017     | 542     | Tel  |
	#scenario 8
	| 900018     | 310     | Tel  |
	#scenario 9
	| 900019     | 562     | Tel  |
	#scenario 10
	| 900020     | 561     | Tel  |

	When the nisra process is triggered
	Then blaise will contain the following cases
	| primarykey | outcome | mode |
	#scenario 1
	| 900001     | 110     | Web  |
	#scenario 2
	| 900002     | 110     | Tel  |
	#scenario 3
	| 900003     | 110     | Web  |
	#scenario 4
	| 900004     | 110     | Web  |
	| 900005     | 110     | Web  |
	| 900006     | 110     | Web  |
	| 900007     | 110     | Web  |
	| 900008     | 110     | Web  |
	| 900009     | 110     | Web  |
	#scenario 5
	| 900010     | 110     | Tel  |
	#scenario 6
	| 900011     | 210     | Web  |
	#scenario 7
	| 900012     | 310     | Tel  |
	| 900013     | 430     | Tel  |
	| 900014     | 460     | Tel  |
	| 900015     | 461     | Tel  |
	| 900016     | 541     | Tel  |
	| 900017     | 542     | Tel  |
	#scenario 8
	| 900018     | 310     | Tel  |
	#scenario 9
	| 900019     | 562     | Tel  |
	#scenario 10
	| 900020     | 561     | Tel  |


