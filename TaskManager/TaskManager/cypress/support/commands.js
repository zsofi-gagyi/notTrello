Cypress.Commands.add("login", () => {
    cy.request({
        method: 'POST',
        url: '/login',
        form: true,
        body: {
            name: 'Guest',
            password: 'Guest1234.'
        }
    })
})

Cypress.Commands.add("logout", () => {
    cy.visit('/logout')
})

Cypress.Commands.add("get_user_token", () => {
    cy.visit('/users/token')

    cy.get('[data-test=token]')
        .then(($userToken) => {
            return $userToken.text()
    })
})

Cypress.Commands.add("create_project", () => {
    cy.login()
    cy.visit('/users/addProject')

    cy.get('[name="Title"]')
        .type('test title')

    cy.get('[name="Description"]')
        .type('test description')

    cy.get('[type=submit]')
        .click()
});

Cypress.Commands.add("delete_project", (projectUrl) => {
    let projectId = projectUrl.substring(39)
    cy.get_user_token().then(($token) => { 

        cy.request({
            method: 'DELETE',
                url: '/api/users/me/projects/' + projectId,
            headers: {
                'Authorization': 'Bearer ' + $token
            },
        })
    })
})