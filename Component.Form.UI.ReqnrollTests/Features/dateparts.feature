Feature: check dateparts component

Background:
    Given the user is on the 'http://localhost:5164/form/datepartsComponent/start' page
    And the user can see the 'give_me_a_date' question, with text 'Give me a date. Any date.'

Scenario: user clicks continue and sees a validation error
    When the user clicks continue
    Then the user sees an error message, 'Day is between 1 and 31'

Scenario: user enters a day, clicks continue and sees a validation error
    When the user enters day '01' month '' and year '' in the 'give_me_a_date' date parts field
    When the user clicks continue
    Then the user sees an error message, 'Month is between 1 and 12'
	And the user sees an error message, 'Year is between 1900 and 2100'

Scenario: user enters a month, clicks continue and sees a validation error
    When the user enters day '' month '01' and year '' in the 'give_me_a_date' date parts field
    When the user clicks continue
    Then the user sees an error message, 'Day is between 1 and 31'
    And the user sees an error message, 'Year is between 1900 and 2100'

Scenario: user enters a year, clicks continue and sees a validation error
    When the user enters day '' month '' and year '20' in the 'give_me_a_date' date parts field
    When the user clicks continue
    Then the user sees an error message, 'Day is between 1 and 31'
    And the user sees an error message, 'Month is between 1 and 12'

Scenario: user enters date and sees the next page
    When the user enters day '01' month '01' and year '2020' in the 'give_me_a_date' date parts field
    And the user clicks continue
    Then the user sees the summary page
    And the summary page shows the answer to 'give_me_a_date' is '1/1/2020'