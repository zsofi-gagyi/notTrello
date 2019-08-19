it('get valid token as user', () => {
    cy.login()

    cy.visit('https://localhost:44374/APIguide')

    cy.get('[data-test=tokenLink]')
        .click()

    cy.get('[data-test=token]')
        .then(($userToken) => {
            cy.clearCookies()

            cy.request({
                method: 'GET',
                url: 'https://localhost:44374/api/users/me/userWithProjects',
                headers: {
                    'Authorization': 'Bearer ' + $userToken.text()
                }
            })

            var unauthorized_result = cy.request({
                method: 'POST',
                url: 'https://localhost:44374/api/users',
                headers: {
                    'Authorization': 'Bearer ' + $userToken.text()
                },
                failOnStatusCode: false
            })

            unauthorized_result.its('status')
                .should('equal', 403);
        })
})