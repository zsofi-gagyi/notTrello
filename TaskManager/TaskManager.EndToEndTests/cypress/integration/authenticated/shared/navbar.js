const urls_for_user = [
    '/APIguide',
    '/users',
    '/users/addProject',
    '/users/changeRole'
]

describe('navbar for user displays correctly', () => {
    urls_for_user.forEach((url) => {
        it(`for ${url}`, () => {
            cy.login()
            cy.visit(url)

            cy.get('[data-test=logo]')
                .should('have.text', "TaskManager")

            cy.contains("Guest as User")

            cy.contains("Sign out")
                .should('have.attr', 'href')
                .and('eq', '/logout')

            cy.contains("Change role")
                .should('have.attr', 'href')
                .and('eq', '/users/changeRole')
        })
    })
})