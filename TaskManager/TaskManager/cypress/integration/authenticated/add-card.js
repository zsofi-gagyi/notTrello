it('create card', () => {
    cy.create_project()
        .then(() => {
            cy.contains('Add new card')
                .click()

            cy.get('[name=Title]')
                .type('test card title')

            cy.get('[name=Description]')
                .type('test card description')

            cy.get('[name=Deadline]')
                .click()

            cy.get('.flatpickr-day')
                .first()
                .click()

            cy.get('[type=submit]')
                .click()

            cy.location('pathname')
                .should('contain', "/users/projects/")

            cy.get('[data-test=toDoCard]')
                .first()
                    .find('[data-test=title]')
                    .should('contain', 'test card title')

                .parent()
                    .find('[data-test=description]')
                    .should('contain', 'test card description')

                .parent()
                    .find('a')
                    .should('contain', 'Mark as done')

            cy.url()
                .then(($urlWithProjectId) => {
                    cy.delete_project($urlWithProjectId)
                })
        })
});