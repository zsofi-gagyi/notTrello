it('submit solo project', () => {
    cy.login()
    cy.visit('https://localhost:44374/users/addProject')

    cy.get('[name="Title"]')
        .type('test title')

    cy.get('[name="Description"]')
        .type('test description')

    cy.get('[type=submit]')
        .click()

    var token = cy.get_user_token()
    cy.url()
        .then(($pathname) => {
            cy.request({
                method: 'DELETE',
                url: 'https://localhost:44374/api/users/me/projects/' + $pathname.substring(39),
                headers: {
                    'content-type': 'application/html',
                    'server': "Kestrel",
                    'Authorization':
                    {
                        'bearer': token
                    }
                },
            }).then(response => {
                //I need to get the token from the response here
                expect(response.status).to.eq(204);
            })
        })
});