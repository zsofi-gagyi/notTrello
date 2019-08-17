const urls_for_user = [
    'https://localhost:44374/APIguide',
    'https://localhost:44374/users',
    'https://localhost:44374/users/addProject',
    'https://localhost:44374/users/changeRole'
]

describe('navbar for user displays correctly', () => {
    urls_for_user.forEach((url) => {
        it(`for ${url}`, () => {
            cy.visit(url)

            cy.get('[data-test=logo]')
                .should('have.text', "TaskManager")

            //to finalize
        })
    })
})