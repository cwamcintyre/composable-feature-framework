Feature: check email component

Background:
    Given the user is on the 'http://localhost:5164/form/emailComponent/start' page
    And the user can see the 'what_is_your_email' question, with text 'What is your email address?'

Scenario: user clicks continue and sees a validation error
    When the user clicks continue
    Then the user sees an error message, 'Enter an email address in the correct format, like name@example.com'

Scenario: user enters not an email and sees a validation error
    When the user enters 'not an email' in the 'what_is_your_email' field
    And the user clicks continue
    Then the user sees an error message, 'Enter an email address in the correct format, like name@example.com'

Scenario: user enters email and sees the next page
    When the user enters 'test@example.com' in the 'what_is_your_email' field
    And the user clicks continue
    Then the user sees the summary page
    And the summary page shows the answer to 'what_is_your_email' is "test@example.com"