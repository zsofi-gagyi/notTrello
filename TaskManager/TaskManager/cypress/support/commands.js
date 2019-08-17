Cypress.Commands.add("login", () => {
    cy.request({
        method: 'POST',
        url: 'https://localhost:44374/login',
        form: true,
        body: {
            name: 'Guest',
            password: 'guest'
        }
    })
})

Cypress.Commands.add("logout", () => {
    cy.visit('https://localhost:44374/logout')
})

Cypress.Commands.add("get_user_token", () => {
    cy.visit('https://localhost:44374/users/token')

    cy.get('#token')
        .then(($userToken) => {
            return $userToken.text()
    })
})

Cypress.Commands.add("delete_project", (projectUrl) => {
    var projectId = projectUrl.substring(39)
    cy.get_user_token().then(($token) => { 

        cy.request({
            method: 'DELETE',
                url: 'https://localhost:44374/api/users/me/projects/' + projectId,
            headers: {
                'Authorization': 'Bearer ' + $token
            },
        })
    })
})