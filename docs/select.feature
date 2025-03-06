Feature: check select component

Background:
    Given the user is on the http://localhost:5164/form/selectComponent/start page
    And the user can see the "What is your quest?" question

Scenario: user clicks continue without selecting an option and sees a validation error
    When the user clicks "continue"
    Then the user sees an error message, "Select an option"

Scenario: user selects an option and sees the summary page
    When the user selects "To find the holy grail"
    And clicks on "continue"
    Then the user sees the summary page
    And the summary page shows the answer to "What is your quest?" is "To find the holy grail"