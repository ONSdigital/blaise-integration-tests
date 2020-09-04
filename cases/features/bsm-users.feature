Feature: bsm-users

  Background:
    Given I am logged in to bsm

  Scenario: create-a-DST-user
     When I add a DST user
     Then the DST user exists
