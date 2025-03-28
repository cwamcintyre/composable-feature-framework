Feature: check text component

Background:
    Given the user is on the 'http://localhost:5164/form/textComponent/start' page
    And the user can see the 'What is your name?' question

Scenario: user clicks continue and sees a validation error
    When the user clicks continue
    Then the user sees an error message, 'Enter your name'

Scenario: user enters BOB and sees a validation error
    When the user enters 'BOB' in the 'what-is-your-name' field
    And the user clicks continue
    Then the user sees an error message, 'We all know that Bob is not your real name. Please provide your real name.'

Scenario: user enters hercules and sees the summary page
    When the user enters 'hercules' in the 'what-is-your-name' field
    And the user clicks continue
    Then the user sees the summary page
    And the summary page shows the answer to 'what-is-your-name' is 'hercules'