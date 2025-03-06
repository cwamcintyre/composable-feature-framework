Feature: check phonenumber component

Background:
    Given the user is on the 'http://localhost:5164/form/phonenumberComponent/start' page
    And the user can see the 'what_is_your_phone_number' question, with text 'What is your phone number?'

Scenario: user clicks continue and sees a validation error
    When the user clicks continue
    Then the user sees an error message, 'Enter a UK phone number'

Scenario: user enters a space, clicks continue and sees a validation error
    When the user enters ' ' in the 'what_is_your_phone_number' field
    And the user clicks continue
    Then the user sees an error message, 'Enter a UK phone number'

Scenario: user enters not a number, clicks continue and sees a validation error
    When the user enters 'not a number' in the 'what_is_your_phone_number' field
    And the user clicks continue
    Then the user sees an error message, 'Enter a UK phone number'

Scenario: user enters a short number, clicks continue and sees a validation error
    When the user enters '567890' in the 'what_is_your_phone_number' field
    And the user clicks continue
    Then the user sees an error message, 'Enter a UK phone number'

Scenario: user enters a long number, clicks continue and sees a validation error
    When the user enters '01234 567890 987654' in the 'what_is_your_phone_number' field
    And the user clicks continue
    Then the user sees an error message, 'Enter a UK phone number'

Scenario: user enters an american number, clicks continue and sees a validation error
    When the user enters '+1-555-123-4567' in the 'what_is_your_phone_number' field
    And the user clicks continue
    Then the user sees an error message, 'Enter a UK phone number'

Scenario: user enters phone number and sees the next page
    When the user enters '01234 567890' in the 'what_is_your_phone_number' field
    And the user clicks continue
    Then the user sees the summary page
	And the summary page shows the answer to 'what_is_your_phone_number' is '01234 567890'