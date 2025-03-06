Feature: check radio component

Background:
    Given the user is on the 'http://localhost:5164/form/radioComponent/start' page
    And the user can see the 'which_superpower' question, with radio options 'Infinite Sprint - You never get tired and you can run forever at top speed!, Inventory Magic - You can store anything in a limitless weightless backpack!, Save & Load - You can save your progress in life and reload if things go wrong!, Charisma Maxed - Everyone likes you instantly and you always win arguments!'

Scenario: user clicks continue and sees a validation error
    When the user clicks continue
    Then the user sees an error message, 'Select an option'

Scenario: user selects an option and sees the next page
    When the user selects 'Infinite Sprint' for the 'which_superpower' radio question
    And the user clicks continue
    Then the user sees the summary page
    And the summary page shows the answer to 'which_superpower' is 'Infinite Sprint'