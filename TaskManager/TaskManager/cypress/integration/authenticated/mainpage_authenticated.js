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

        .first(project => {
            project
                .find('[data-test=title]')
                .should('have.text', "Personal project")

            project
                .find('[data-test=description]')
                .contains("As an individual")

            project
                .find('[data-test=collaborators]')
                .length.to.eq(0)

            project.find('div > [data-test=detailsLink]')
                .then(detailsLink => {
                    expect(detailsLink).to.have.attr('href')
                        .and('contain', 'users/projects/')
                })
        })

        .last(project => {
            project
                .find('[data-test=title]')
                .should('have.text', "Team project")

            project
                .find('[data-test=description]')
                .contains("As a team")

            project.find('[data-test=collaborators]')
                .then(collaborators => {
                    collaborators.length.to.eq(2)
                    collaborators.first.should('have.text', "Alice")
                    collaborators.last.should('have.text', "Bob")
                })

            project.find('div > [data-test=detailsLink]')
                .then(detailsLink => {
                    expect(detailsLink).to.have.attr('href')
                        .and('contain', 'users/projects/')
                })
        })
});