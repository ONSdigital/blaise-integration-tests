Feature: bsm-survey

  Background:
    Given I am logged in to bsm

  Scenario: add a survey
     When I add a survey
     Then the survey exists
