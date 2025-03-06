Feature: check UK address component

Background:
    Given the user is on the http://localhost:5164/form/ukaddressComponent/start page
    And the user can see the "What is your address?" question

Scenario: user clicks continue without entering address and sees a validation error
    When the user clicks "continue"
    Then the user sees an error message, "Enter your address"

Scenario: user enters an address without address line 1
    When the user enters "Faketown, FK1 2AB"
    And clicks on "continue"
    Then the user sees an error message, "Address Line 1 is required"

Scenario: user enters an address without a post code
    When the user enters "123 Fake Street, Faketown"
    And clicks on "continue"
    Then the user sees an error message, "Postcode is required"

Scenario: users enters an address without address line 1 or post code
    When the user enters "Faketown"
    And clicks on "continue"
    Then the user sees an error message, "Postcode is required"
    And the user sees an error message, "Address Line 1 is required"

Scenario: user enters address and sees the summary page
    When the user enters "123 Fake Street, Faketown, FK1 2AB"
    And clicks on "continue"
    Then the user sees the summary page
    And the summary page shows the answer to "What is your address?" is "123 Fake Street, Faketown, FK1 2AB"