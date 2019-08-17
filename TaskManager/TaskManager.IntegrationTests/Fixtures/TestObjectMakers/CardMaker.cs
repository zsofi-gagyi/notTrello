using System;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Models.DAOs.JoinTables;
using TodoWithDatabase.UnitTests.Fixtures.TestObjectMakers;

namespace TaskManager.IntegrationTests.Fixtures.TestObjectMakers
{
    public static class CardMaker
    {
        public static Card MakeOriginal()
        {
            return new Card
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000011"),
                Title = "card title",
                Description = "card description",
                Deadline = new DateTime(2019, 1, 1)
            };
        }

        public static Card MakeCompleted()
        {
            var card = MakeOriginal();
            var user = AssigneeMaker.MakeUser();
            var collaborator = AssigneeMaker.MakeCollaborator();

            card.Done = false;

            card.AssigneeCards.Add(new AssigneeCard(user, card));
            card.AssigneeCards.Add(new AssigneeCard(collaborator, card));

            card.Project = ProjectMaker.Make();

            return card;
        }
    }
}