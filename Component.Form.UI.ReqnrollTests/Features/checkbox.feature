Feature: check checkbox component

Background:
    Given the user is on the 'http://localhost:5164/form/checkboxComponent/start' page
    And the user can see the 'what_is_your_favourite_colour' question, with checkbox options 'Red, Blue, Green, Yellow, Pink, Black, White, Purple, Orange, Brown, Grey, Other'

Scenario: user clicks continue and sees a validation error
    When the user clicks continue
    Then the user sees an error message, 'Select at least one colour'

Scenario: user selects colours and sees the next page
    When the user selects 'Red, Blue' for the 'what_is_your_favourite_colour' checkbox question
    And the user clicks continue
    Then the user sees the summary page
    And the summary page shows the answer to 'what_is_your_favourite_colour' is 'Red,Blue'
