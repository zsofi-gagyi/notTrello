it('get valid token as user', () => {
    cy.login()

    cy.visit('/APIguide')

    cy.get('[data-test=tokenLink]')
        .click()

    cy.get('[data-test=token]')
        .then(($userToken) => {
            cy.clearCookies()

            cy.request({
                method: 'GET',
                url: '/api/users/me/userWithProjects',
                headers: {
                    'Authorization': 'Bearer ' + $userToken.text()
                }
            })

            let unauthorized_result = cy.request({
                method: 'POST',
                url: '/api/users',
                headers: {
                    'Authorization': 'Bearer ' + $userToken.text()
                },
                failOnStatusCode: false
            })

            unauthorized_result.its('status')
                .should('equal', 403);
        })
})