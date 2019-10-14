const urls_for_anon = [
    '/',
    '/main',
    '/login',
    '/signUp',
    '/APIguide',
]

const urls_for_user = [
    '/APIguide',
    '/users',
    '/users/addProject',
    '/users/changeRole'
]

Cypress.Commands.add("test_footer", (projectUrl) => {
    cy.visit(projectUrl)
    cy.get('[data-test=footer] > [data-test=APIguide]')
        .should("contain", "For developers")
        .should('have.attr', 'href')
        .and('eq', '/APIguide')

    cy.get('[data-test=footer] > [data-test=credits]')
        .should("contain", "This app uses")
})

describe('footer displays correctly', () => {
    urls_for_anon.forEach((url) => {
        it(`for ${url}`, () => {
            cy.test_footer(url)
            cy.visit(url)
            cy.get('[data-test=footer] > [data-test=APIguide]')
                .should("contain", "For developers")
                .should('have.attr', 'href')
                .and('eq', '/APIguide')

            cy.get('[data-test=footer] > [data-test=credits]')
                .should("contain", "This app uses")
        })

        urls_for_user.forEach((url) => {
            it(`for ${url}`, () => {
                cy.login()
                cy.visit(url)
                cy.get('[data-test=footer] > [data-test=APIguide]')
                    .should("contain", "For developers")
                    .should('have.attr', 'href')
                    .and('eq', '/APIguide')

                cy.get('[data-test=footer] > [data-test=credits]')
                    .should("contain", "This app uses")
                cy.logout()
            })
        })
    })
})