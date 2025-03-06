Feature: check yesno component

Background:
    Given the user is on the 'http://localhost:5164/form/yesnoComponent/start' page
    And the user can see the 'do_you_want_to_fill_in_this_form' question, with text 'Do you want to fill in this form?'

Scenario: user clicks continue and sees a validation error
    When the user clicks continue
    Then the user sees an error message, 'Select yes or no'

Scenario: user selects no and sees the stop page
    When the user selects 'no' for the 'do_you_want_to_fill_in_this_form' radio question
    And the user clicks continue
    Then the user sees the stop page

Scenario: user selects yes and sees the next page
    When the user selects 'yes' for the 'do_you_want_to_fill_in_this_form' radio question
    And the user clicks continue
    Then the user sees the summary page
    And the summary page shows the answer to 'do_you_want_to_fill_in_this_form' is 'yes'
