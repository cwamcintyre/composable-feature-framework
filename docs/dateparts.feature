Feature: check date parts component

Background:
    Given the user is on the http://localhost:5164/form/datepartsComponent/start page
    And the user can see the "Give me a date. Any date." question

Scenario: user clicks continue without entering date and sees a validation error
    When the user clicks "continue"
    Then the user sees an error message, "Enter a date"

Scenario: user enters date and sees the summary page
    When the user enters "01/01/2022"
    And clicks on "continue"
    Then the user sees the summary page
    And the summary page shows the answer to "Give me a date. Any date." is "01/01/2022"