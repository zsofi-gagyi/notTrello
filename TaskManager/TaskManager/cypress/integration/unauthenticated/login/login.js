Cypress.Commands.add("set_up_login", () => {
    cy.visit('https://localhost:44374/login')

    cy.get('[name="Name"]')
        .type('Guest')   

})

it('login succeeds with correct password', () => {
    cy.set_up_login()

    cy.get('[name="Password"]')  
        .type('Guest1234.')  

    cy.get('[type=submit]')      
        .click()                 

    cy.location('pathname')                 
        .should('eq', '/users')

    cy.getCookie('TodoCookie')
        .should('exist')

    cy.logout()
});

it('login fails with incorrect password', () => {
    cy.set_up_login()

    cy.get('[name="Password"]')
        .type('wrongPassword')

    cy.get('[type=submit]')
        .click()

    cy.location('pathname')
        .should('eq', '/login')

    cy.getCookie('TodoCookie')
        .should('not.exist')
});