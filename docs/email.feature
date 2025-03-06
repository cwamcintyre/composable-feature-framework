Feature: check email component

Background:
    Given the user is on the http://localhost:5164/form/emailComponent/start page
    And the user can see the "What is your email address?" question

Scenario: user clicks continue without entering email and sees a validation error
    When the user clicks "continue"
    Then the user sees an error message, "Enter your email address"

Scenario: user enters not an email and sees a validation error
    When the user enters "not an email"
    And clicks on "continue"
    Then the usser sees an error message, "Enter an email address in the correct format, like name@example.com"

Scenario: user enters email and sees the summary page
    When the user enters "test@example.com"
    And clicks on "continue"
    Then the user sees the summary page
    And the summary page shows the answer to "What is your email address?" is "test@example.com"