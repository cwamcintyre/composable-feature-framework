Feature: check checkbox component

Background:
    Given the user is on the http://localhost:5164/form/checkboxComponent/start page
    And the user can see the "Which of these colours do you like?" question

Scenario: user clicks continue without selecting an option and sees a validation error
    When the user clicks "continue"
    Then the user sees an error message, "Select at least one colour"

Scenario: user selects an option and sees the summary page
    When the user selects "Red"
    And clicks on "continue"
    Then the user sees the summary page
    And the summary page shows the answer to "Which of these colours do you like?" is "Red"