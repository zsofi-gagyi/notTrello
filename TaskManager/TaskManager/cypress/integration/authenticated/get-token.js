it('get valid token as user', () => {
    cy.login()

    cy.visit('https://localhost:44374/APIguide')

    cy.get('[data-test=tokenLink]')
        .click()

    cy.get('[data-test=token]')
        .then(($userToken) => {

            cy.request({
                method: 'GET',
                url: 'https://localhost:44374/api/users/me/userWithProjects/',
                headers: {
                    'Authorization': 'Bearer ' + $userToken.text()
                },
            })
        })
})