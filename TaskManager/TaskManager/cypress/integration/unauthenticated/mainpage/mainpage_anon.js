it('mainpage for anon redirects to correct url', () => {
    cy.visit('/')

    cy.location('pathname')
        .should('eq', '/main')
});

it('mainpage for anon displays correctly', () => {
    cy.visit('/')

    cy.get('[data-test=brandName]')
        .contains("TaskManager")

    cy.get('[data-test=signUpButton]')
        .contains("Sign up now!")
            .should('have.attr', 'href')
            .and('eq', '/signUp')
});