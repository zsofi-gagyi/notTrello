it('create card', () => {
    cy.login()
    cy.create_project() // to define method

    //create card 

    cy.url()
        .contains("/users/projects/")
        .then(($urlWithProjectId) => {
            cy.delete_project($urlWithProjectId)
        })
});