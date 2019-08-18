it('create card', () => {
    cy.login()
    cy.create_project() /

        //create card

    cy.location('pathname')
        .should('contain', "/users/projects/")

    //verify the card is there

    cy.url()
        .then(($urlWithProjectId) => {
            cy.delete_project($urlWithProjectId)
        })
});