Feature: check multilineText component

Background:
    Given the user is on the 'http://localhost:5164/form/multilineTextComponent/start' page
    And the user can see the 'why_not_help_claimants' question, with text "Why don't you want to help claimants edit their redundancy claim?"

Scenario: user clicks continue and sees a validation error
    When the user clicks continue
    Then the user sees an error message, 'Enter your reason'

Scenario: user enters reason and sees the next page
    When the user enters "I don't have time" in the 'why_not_help_claimants' field
    And the user clicks continue
    Then the user sees the summary page
    And the summary page shows the answer to 'why_not_help_claimants' is "I don't have time"