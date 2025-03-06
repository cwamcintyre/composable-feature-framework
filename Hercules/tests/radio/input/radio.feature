Feature: check radio component

Background:
    Given the user is on the http://localhost:5164/form/radioComponent/start page
    And the user can see the "You wake up and realize you’ve been transported into a video game world! You can only choose ONE special ability. Which one do you pick?" question

Scenario: user clicks continue without selecting an option and sees a validation error
    When the user clicks "continue"
    Then the user sees an error message, "Select an option"

Scenario: user selects an option and sees the summary page
    When the user selects "Infinite Sprint"
    And clicks on "continue"
    Then the user sees the summary page
    And the summary page shows the answer to "You wake up and realize you’ve been transported into a video game world! You can only choose ONE special ability. Which one do you pick?" is "Infinite Sprint"