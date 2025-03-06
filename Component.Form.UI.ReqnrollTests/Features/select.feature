Feature: check select component

Background:
    Given the user is on the 'http://localhost:5164/form/selectComponent/start' page
    And the user can see the 'what_is_your_quest' question, with options 'To find the holy grail, To become a knight, To allow claimants to edit their redundancy claim, To do nothing'

Scenario: user selects an option and sees the next page
    When the user selects 'To do nothing' for the 'what_is_your_quest' question
    And the user clicks continue
    Then the user sees the summary page
    And the summary page shows the answer to 'what_is_your_quest' is 'To do nothing'
