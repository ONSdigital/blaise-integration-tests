Feature: bsm-survey

# bsm-survey add/delete/list
# https://github.com/ONSdigital/blaise-survey-manager/blob/master/blaise_survey_manager/survey_config/views.py

  Background:
    Given I am logged in to bsm

  Scenario: add a survey
     When I add a survey
     Then the survey does exist

# TODO: test add-survey enforces 'manage_surveys' permission on role
#
# Scenario: add a survey without permission
#    When I log in as a user without 'manage_surveys' permission
#     And I add a survey
#    Then the survey does not exist
#
# Scenario: add a survey with permission
#    When I log in as a user with 'manage_surveys' permission
#     And I add a survey
#    Then the survey does exist
#
# TODO: tests for /edit_survey, etc
# TODO: matrix test for survey-types (ftf, ips, tel, etc)
# TODO: test for survey-tla field is 3 charatecters long
# TODO: test for survey-allocation-required (what does 'allocation-required' mean?)
