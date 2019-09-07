const unauthenticated_urls = [
    '/',
    '/main',
    '/login',
    '/signUp',
    '/APIguide'
]

describe('navbar for anon displays correctly', () => {
    unauthenticated_urls.forEach((url) => {
        it(`for ${url}`, () => {
            cy.visit(url)

            cy.get('[data-test=logo]')
                .should('have.text', "TaskManager")

            cy.contains("Log in")
                .should('have.attr', 'href')
                .and('eq', '/login')

            cy.contains("Sign up")
                .should('have.attr', 'href')
                .and('eq', '/signUp')
        })
    })
})