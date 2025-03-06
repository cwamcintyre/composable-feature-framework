Feature: check text input component

Background:
    Given the user is on the http://localhost:5164/form/textInputComponent/start page
    And the user can see the "What is your name?" question

Scenario: user clicks continue and sees a validation error
    When the user clicks "continue"
    Then the user sees an error message, "Enter your name"

Scenario: user enters BOB and sees a validation error
    When the user enters "BOB"
    And clicks on "continue"
    Then the user sees an error message, "We all know that Bob is not your real name. Please provide your real name."

Scenario: users enters hercules and sees the summary page
    When the user enters "hercules"
    And clicks on "continue"
    Then the user sees the summary page
    And the summary page shows the answer to "What is your name?" is "hercules"
    