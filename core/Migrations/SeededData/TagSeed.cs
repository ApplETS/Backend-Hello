using api.core.data.entities;

namespace api.core.Migrations.SeededData;

public class TagSeed
{
    public static string[] Columns = new[] { "Id", "Name", "PriorityValue", "CreatedAt", "UpdatedAt" };

    public static object[,] TagValues = new object[,]
    {
        // { Id, Name, PriorityValue, CreatedAt, UpdatedAt }
        { Guid.Parse("43e812cf-a06b-47f5-8e2b-5601fa87b7c9"), "Apprentissage", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("d3b07384-d113-4ec8-9e63-62b5a4f9b8e8"), "Atelier", 2, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("6b197d4f-790e-47c8-817f-6777b8c157c8"), "Bourses", 3, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("4e5bdf34-55dc-4f96-b87b-8e31d7f00c07"), "Carrière", 4, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("ec2e239f-2d7b-4d8d-9fb7-a7d38f56a037"), "Programmation", 5, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("a3f67e98-6f8e-44c3-9585-e86a50b289d3"), "Développement mobile", 6, DateTime.UtcNow, DateTime.UtcNow },
    };



}
