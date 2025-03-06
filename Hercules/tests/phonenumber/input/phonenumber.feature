Feature: check phone number component

Background:
    Given the user is on the http://localhost:5164/form/phonenumberComponent/start page
    And the user can see the "What is your phone number?" question

Scenario: user clicks continue without entering phone number and sees a validation error
    When the user clicks "continue"
    Then the user sees an error message, "Enter your phone number"

Scenario: user enters not a phone number and sees a validation error
    When the user enters "not an phone number"
    And clicks on "continue"
    Then the usser sees an error message, "Enter a UK phone number"

Scenario: user enters phone number and sees the summary page
    When the user enters "01234 567890"
    And clicks on "continue"
    Then the user sees the summary page
    And the summary page shows the answer to "What is your phone number?" is "01234 567890"