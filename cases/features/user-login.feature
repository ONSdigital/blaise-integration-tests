Feature: user-login

  Scenario: valid-user-login-to-blaise
    Given a valid username for blaise
     When I log in
     Then I am logged in

  Scenario: invalid-user-login-to-blaise
    Given an invalid username for blaise
     When I log in
     Then I am not logged in

  Scenario: valid-user-login-to-bsm
    Given a valid username for bsm
     When I log in
     Then I am logged in

  Scenario: invalid-user-login-to-bsm
    Given an invalid username for bsm
     When I log in
     Then I am not logged in
