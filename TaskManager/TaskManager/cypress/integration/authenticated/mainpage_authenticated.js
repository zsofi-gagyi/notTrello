it('mainpage for authenticated redirects to correct url', () => {
    cy.login()
    cy.visit('https://localhost:44374/')

    cy.location('pathname')
        .should('eq', '/users')
});

it('mainpage for user displays correctly', () => {
    cy.login()
    cy.visit('https://localhost:44374/')

    cy.get('[data-test=addProject]')
        .contains("Add new")
        .should('have.attr', 'href')
        .and('eq', 'users/addProject')

    cy.get('[data-test=project]')
        .then(projects => {
            expect(projects.length).to.eq(2)
        })

    cy.get('[data-test=project]')
        .each(($project) => {
            cy.wrap($project)
                .find('div > [data-test=detailsLink]')
                .should('have.text', "Details")
                .invoke('attr', 'href')
                .then(href => {
                    href === "/users/projects/"
                });
        })

    cy.get('[data-test=project]')
        .first()
        .find('[data-test=title]')
        .should('have.text', "Personal project")

    cy.get('[data-test=project]')
        .first()
        .find('[data-test=description]')
        .contains("As an individual")

    cy.get('[data-test=project]')
        .first()
        .find('[data-test=collaborators]')
        .should('not.exist')

    cy.get('[data-test=project]')
        .last()
        .find('[data-test=description]')
        .contains("As a team")

    cy.get('[data-test=project]')
        .last()
        .find('[data-test=collaborators] > [data-test=responsibleName]')
        .should(($listOfCollaborators) => {
            expect($listOfCollaborators).to.have.length(2)
        })
        .first()
        .should('have.text', "Bob")

    cy.get('[data-test=project]')
        .last()
        .find('[data-test=collaborators] > [data-test=responsibleName]')
        .last()
        .should('have.text', "Alice")
});