@user
Feature: As a service I wish to be able to create and update users in Blaise
	In order to facilitate custom workflows
	As a service
	I want to be able to create and maintain users in Blaise

Scenario: Create a new user in Blaise
	Given I have a new user I want to add
	When I add the user
	Then the user is successfully added

Scenario: Get an existing user in Blaise
	Given a user exists in blaise 
	| username | password | role     | serverparks   | DefaultServerPark |
	| user1    | pass     | DST_TECH | Tel, val, ftf | Tel               |

	When I retrieve the user
	Then the correct user is returned
	| username | password | role     | serverparks   | DefaultServerPark |
	| user1    | pass     | DST_TECH | Tel, val, ftf | Tel               |

Scenario: Update an existing user in Blaise
	Given a user exists in blaise 
	| username | password | role     | serverparks   | DefaultServerPark |
	| user1    | pass     | DST_TECH | Tel, val, ftf | Tel               |

	When I update the user
	Given a user exists in blaise 
	| username | password | role       | serverparks | DefaultServerPark |
	| user1    | pass     | TO_Manager | Tel         | Tel               |

	Then the user is updated successfully
	| username | password | role       | serverparks | DefaultServerPark |
	| user1    | pass     | TO_Manager | Tel         | Tel               |

Scenario: Check that a user exists in blaise returns true if they exist
	Given a user exists in blaise with the user name 'test'
	When I check to see if the user exists
	Then the user exists


Scenario: Check that a user exists in blaise returns false if they do not exist
	Given a user does not exist in blaise with the user name 'test'
	When I check to see if the user exists
	Then the user does not exist


Scenario: Change the password of an existing user in Blaise
	Given the first number is 50
	And the second number is 70
	When the two numbers are added
	Then the result should be 120

Scenario: Delete an existing user in Blaise
	Given a user exists in blaise with the user name 'test'
	When I delete the user
	Then the user no longer exists