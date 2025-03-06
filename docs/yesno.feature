Feature: check yes/no component

Background:
    Given the user is on the http://localhost:5164/form/yesnoComponent/start page
    And the user can see the "Do you want to fill in this form?" question

Scenario: user clicks continue and sees a validation error
    When the user clicks "continue"
    Then the user sees an error message, "Select yes or no"

Scenario: user selects no and sees the stop page
    When the user selects "no"
    And clicks on "continue"
    Then the user sees the stop page
    And the stop page shows the message "Ok no worries. Hope you have a nice day!"

Scenario: user selects yes and sees the summary page
    When the user selects "yes"
    And clicks on "continue"
    Then the user sees the summary page
    And the summary page shows the answer to "Do you want to fill in this form?" is "yes"