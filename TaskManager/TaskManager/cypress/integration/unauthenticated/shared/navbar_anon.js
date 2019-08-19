const unauthenticated_urls = [
    'https://localhost:44374/',
    'https://localhost:44374/main',
    'https://localhost:44374/login',
    'https://localhost:44374/signUp',
    'https://localhost:44374/APIguide'
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