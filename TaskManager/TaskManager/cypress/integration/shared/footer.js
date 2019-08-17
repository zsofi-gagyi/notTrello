const urlsForAnon = [
    'https://localhost:44374/',
    'https://localhost:44374/main',
    'https://localhost:44374/login',
    'https://localhost:44374/signUp',
    'https://localhost:44374/APIguide',
]

const urlsForUser = [
    'https://localhost:44374/APIguide',
    'https://localhost:44374/users',
    'https://localhost:44374/users/addProject',
    'https://localhost:44374/users/changeRole'
]

describe('footer displays correctly', () => {
    urlsForAnon.forEach((url) => {
        it(`for ${url}`, () => {
            cy.visit(url)
            cy.get('[data-test=footer] > [data-test=APIguide]')
                .should("contain", "For developers")
                .should('have.attr', 'href')
                .and('eq', '/APIguide')

            cy.get('[data-test=footer] > [data-test=credits]')
                .should("contain", "This app uses")
        })

        urlsForUser.forEach((url) => {
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