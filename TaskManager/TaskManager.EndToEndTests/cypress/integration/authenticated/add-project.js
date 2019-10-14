it('create solo project', () => {
    cy.login()
    cy.visit('/users/addProject')

    cy.get('[name="Title"]')
        .type('test title')

    cy.get('[name="Description"]')
        .type('test description')

    cy.get('[type=submit]')
        .click()

    cy.location('pathname')
        .should('contain', "/users/projects/")

    cy.url()
        .then(($urlWithProjectId) => {
            cy.delete_project($urlWithProjectId)
        })
});