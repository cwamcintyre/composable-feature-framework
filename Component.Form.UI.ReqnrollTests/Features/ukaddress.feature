Feature: check ukaddress component

Background:
    Given the user is on the 'http://localhost:5164/form/ukaddressComponent/start' page
    And the user can see the 'what_is_your_address' question, with text 'What is your address?'

Scenario: user clicks continue and sees a validation error
    When the user clicks continue
    Then the user sees an error message, 'Address Line 1 is required'
    And the user sees an error message, 'Postcode is required'

Scenario: user enters address and sees the next page
    When the user enters 'Fake House' in the 'what_is_your_address' Address Line 1 uk address field
    And the user enters '123 Fake St' in the 'what_is_your_address' Address Line 2 uk address field
    And the user enters 'Faketown' in the 'what_is_your_address' Town uk address field
    And the user enters 'Fakeshire' in the 'what_is_your_address' County uk address field
    And the user enters 'FA9 9KE' in the 'what_is_your_address' Postcode uk address field
    And the user clicks continue
    Then the user sees the summary page
    And the summary page shows the answer to 'what_is_your_address' is 'Fake House, 123 Fake St, Faketown, Fakeshire, FA9 9KE'