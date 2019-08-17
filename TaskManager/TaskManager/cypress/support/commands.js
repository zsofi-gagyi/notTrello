// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************
//
//
// -- This is a parent command --
// Cypress.Commands.add("login", (email, password) => { ... })

Cypress.Commands.add("login", () => {
    cy.request({
        method: 'POST',
        url: 'https://localhost:44374/login',
        form: true,
        body: {
            name: 'Guest',
            password: 'guest'
        }
    })
})

Cypress.Commands.add("logout", () => {
    cy.visit('https://localhost:44374/logout')
})

Cypress.Commands.add("get_user_token", () => {
    cy.login()
    cy.visit('https://localhost:44374/APIguide')

    cy.get('a')
        .contains('token')
        .click()

    cy.get('#token')
        .then(($userToken) => {
            return $userToken
    })
})



//
//
// -- This is a child command --
// Cypress.Commands.add("drag", { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add("dismiss", { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This is will overwrite an existing command --
// Cypress.Commands.overwrite("visit", (originalFn, url, options) => { ... })
