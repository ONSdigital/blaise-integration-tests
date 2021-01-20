@delete
Feature: DeleteQuestionnaire
I want to be able to delete a questionnaire from Blaise via the questionnaire list
So that the system can be cleared of completed surveys

@smoke @LU7996
Scenario: Delete questionnaire not available from the list, when survey is live
Given I have a questionnaire I want to delete 
And that survey is live
When I locate that questionnaire in the list
Then I will not have the option to delete displayed

@smoke @LU7996
Scenario: Confirm deletion
Given I select Delete on a questionnaire that is not live
When I confirm that I want to proceed
Then the questionnaire is removed from Blaise