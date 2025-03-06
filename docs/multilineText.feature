Feature: check multiline text component

Background:
    Given the user is on the http://localhost:5164/form/multilineTextComponent/start page
    And the user can see the "Why don't you want to help claimants edit their redundancy claim?" question

Scenario: user clicks continue without entering text and sees a validation error
    When the user clicks "continue"
    Then the user sees an error message, "Enter your response"

Scenario: user enters text and sees the summary page
    When the user enters "I don't have the time"
    And clicks on "continue"
    Then the user sees the summary page
    And the summary page shows the answer to "Why don't you want to help claimants edit their redundancy claim?" is "I don't have the time"