it('mainpage for authenticated redirects to correct url', () => {
    cy.login()
    cy.visit('/')

    cy.location('pathname')
        .should('eq', '/users')
});

it('mainpage for user displays correctly', () => {
    cy.login()
    cy.visit('/')

    cy.get('[data-test=addProject]')
        .contains("Add new")
            .should('have.attr', 'href')
            .and('eq', '/users/addProject')

    cy.get('[data-test=project]')
        .then(projects => {
            expect(projects.length).to.eq(2)
        })

    cy.get('[data-test=project]')
        .each(($project) => {
            cy.wrap($project)
                .find('div > [data-test=detailsLink]')
                .should('contain.text', "Details")
                .invoke('attr', 'href')
                .then(href => {
                    href === "/users/projects/"
                });
        })

    cy.get('[data-test=project]')
        .first()
            .find('[data-test=title]')
            .should('have.text', "Personal project")

        .parent()
            .find('[data-test=description]')
            .contains("As an individual")

        .parent()
            .find('[data-test=collaborators]')
            .should('not.exist')

    cy.get('[data-test=project]')
        .last()
            .find('[data-test=description]')
            .contains("As a team")

        .parent()
            .find('[data-test=collaborators] > ul > [data-test=responsibleName]')
            .should(($listOfCollaborators) => {
                expect($listOfCollaborators).to.have.length(2)
            })
                .first()
                .should('have.text', "Alice")

    cy.get('[data-test=project]')
        .last()
        .find('[data-test=collaborators] > ul > [data-test=responsibleName]')
            .last()
            .should('have.text', "Bob")
});