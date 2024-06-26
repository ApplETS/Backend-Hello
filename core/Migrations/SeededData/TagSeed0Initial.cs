﻿using api.core.data.entities;

namespace api.core.Migrations.SeededData;

/// <summary>
/// Prevent changing as much as possible the seeded ID from this initial seed
/// </summary>
public class TagSeed0Initial
{
    public static string[] Columns = new[] { "Id", "Name", "PriorityValue", "CreatedAt", "UpdatedAt" };

    public static object[,] TagValues = new object[,]
    {
        // { Id, Name, PriorityValue, CreatedAt, UpdatedAt }
        { Guid.Parse("ac5398e4-a8c3-491a-b1cc-0f3bd2c18681"), "Génie logiciel", 3, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("001dc46b-b77f-43ce-9323-0cabd41409db"), "Développement mobile", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("4db42f52-b9ff-4ed7-b400-e42e5fae9c93"), "Jeux vidéo", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("f54b05d0-1c1c-49c1-90e3-28b15ca94b16"), "Conception de logiciels", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("e4482adf-1afa-4c0a-875b-c3fbf00a3bb6"), "Intelligence artificielle", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("5fca4b3e-9022-4a87-8aa9-7575259e0a57"), "Ingénierie des données", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("078f1a52-3985-46c7-a1e3-12e24e2aa755"), "Réseau", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("4799a3ca-d66d-4b29-a404-5f96f048376e"), "Assurance qualité", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("8290583c-e20a-4496-bd47-169790db90d9"), "Cybersécurité", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("bbfa2bf5-7dcf-4da0-a28a-f6e049c70bee"), "Réalité virtuelle", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("e58cf26b-9cf7-4507-86ea-82aec358bb92"), "Génie de la construction", 3, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("0bae9b68-314b-4f40-bb8f-6f0560596d6e"), "Investigation", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("c6a28e8c-7c13-4640-82d7-87bc212b2f5d"), "Conception plans et devis", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("7efbe861-1ee8-45ed-9c7b-55a55c6e0d84"), "Environnement", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("50b22d90-ea6e-4703-b1a9-5c63f0e38f26"), "Bâtiments", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("c0a29816-0ba7-4fa7-8508-1a9e29889b44"), "Infrastructures", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("ba7e2fe8-a768-47f5-b7eb-37d6c5154f59"), "Projets internationaux", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("2efc2265-63ce-47d1-9e61-1f5785cd91c8"), "Génie de la production automatisée", 3, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("599410b6-950a-4c10-b7a4-0391eddd678a"), "Robotique", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("de9763a9-587d-446c-ae43-b630d6414673"), "Intelligence artificielle", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("f7827a2a-7b8e-4621-9b31-5afb737f2336"), "Apprentissage machine", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("23b0634b-c5f3-4194-8cf3-a8f03512e677"), "Infonuagique", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("5013718a-e3a8-4334-a5db-627fee72f708"), "Internet des objets", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("7044fd2a-f52f-40e0-ba3e-1b0ca45db2a7"), "Mécatronique", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("87c53bfe-25bd-4c82-b375-7858aa15ea25"), "Aéronautique", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("00f0ec42-da00-4afc-8851-f54474573a25"), "Génie électrique", 3, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("1fe309ab-e640-4f46-af8d-1e7cb876b66a"), "Ordinateurs", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("a4e7ec82-22d4-47c8-9142-5347ff1f26eb"), "Aérospatial", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("a875f2b7-d65f-4778-b6f3-e460939c8bad"), "Électronique", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("0caf8618-ed3f-4a30-9489-f1b3ee4cf13a"), "Microélectronique", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("0caf2c3b-3b16-4d80-bacb-d5d460407c64"), "Photonique", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("fd01d074-da27-4858-bac1-a8d7f59dddbf"), "Télécommunications", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("b09329da-30fd-4f60-b78f-f6ee3589e21c"), "Systèmes embarquées", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("d74a6cda-bb66-4c14-a20e-353caf6ab7fb"), "Microsystèmes", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("001a56c2-c852-4928-804b-f6fb3f3a6e13"), "Biomédical", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("6d755432-c929-4b27-88f8-585ada95c22d"), "Génie mécanique", 3, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("768db5fd-8252-4a1f-95cb-020fbc96122b"), "Mécanique appliquée", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("89ef6aaa-4f5b-4629-af3e-82fc937cca78"), "SST", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("156a0318-55cc-4ca4-9d56-954056eb98fb"), "Automatisation", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("189febdd-a874-4487-bd9e-7d20402009f6"), "Développement durable", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("3872ab19-0979-4806-bee2-9ab6f600b1db"), "Systèmes manufacturiers", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("71729eca-17bb-4c3e-8dc4-ff40ddb02856"), "Mécanique du bâtiment", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("19a31a09-7064-4bf9-8537-2d938e628093"), "Conception de systèmes mécaniques", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("1d308d51-d8af-482d-9d4a-bf48364f9947"), "Fabrication et matériaux", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("e636e735-ab78-46ee-86df-7af5b8b23da6"), "Aérospatial", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("d17e6350-7dc4-4959-8c8a-ffab18feedbf"), "Génie des opérations et logistique", 3, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("b12e6bb2-54c8-4a18-830c-50b5f5c18f63"), "Aérospatial", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("bf907607-7c93-47e2-87d4-fd578ee0c65e"), "Ingénierie des entreprises numériques", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("a05fe887-1dca-4a3a-8490-7c23bf4c1342"), "Science des données", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("41cc751a-e60c-4383-a812-376a4ee61d06"), "Ingénierie du risque", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("995d1599-1c38-4bd4-b3ab-ca68e5398532"), "Innovation organisationnelle", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("28095de3-bab1-4fbc-ac65-e24cea5d0363"), "Gestion de la qualité", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("c1238242-d422-4441-8405-6e77e3726f51"), "ÉTS", 3, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("9ec5ea51-a036-43a6-a519-313fe97ea34b"), "5 à 7", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("5f837eaf-cf7b-4f9d-a5ff-0314b3ca5190"), "Apprentissage", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("76181688-9974-45d4-8337-60c17d6ab48e"), "Ateliers", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("ad42286f-cd23-48d1-a3bf-dbdd1c3bda63"), "Bourses", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("f1bae6c5-33e1-41af-8860-b41414be8823"), "Carrières", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("b60174d8-e80f-4c95-9421-2dc4dbcedf3b"), "Compétitions", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("d31c3c8e-b025-4234-b4c1-0cbc3e4710ea"), "Conférences", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("a5ec7ac2-c8c6-4d28-9676-916c75515ffb"), "Échanges internationaux", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("dbf13234-076c-406c-afd2-a4aadbf25703"), "Entrepreneuriat", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("0b33931e-4b77-414a-a8fb-d0ce9fe9f6e9"), "Évènements", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("f584fd73-7097-48e0-b7db-ebb910b6c87b"), "Festifs", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("b8b303fa-4301-4fd9-8f76-4ff9d7572f8c"), "Formations", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("d5c006ed-aec5-4a3b-a137-83255f2b50f2"), "Génie", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("ce24275e-0cca-43f0-9e00-63af3e2fdd6a"), "Lancements", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("034c61f4-af78-465e-86aa-1baf2963fa8c"), "Partenariats", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("1d6d5945-723b-4c93-8d67-c77bafed4b95"), "PFE", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("0982d398-72f9-4540-91f5-1ef24c8875fe"), "Projets", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("339594c8-063d-4bae-b28f-26c79250c9eb"), "Recherche", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("81745a4e-1730-49a7-8692-ac10dbd3faa5"), "Réseautage", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("7d74f82a-d6e7-45fd-ad56-101fd8d1d95d"), "Sports", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("c1c9baac-d759-4a11-9a22-2a2052172a07"), "Stages", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("ad9bd244-23a3-4b02-b7e0-c12d6739a710"), "Tutorat", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("97a5645f-1743-48fe-805b-60185677581b"), "Vie étudiante", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("81e35d01-7a2c-4e88-90c4-171e3f1a1a59"), "Génies", 3, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("f1233115-edf3-4a66-b238-3fae2b5e604b"), "Développement durable", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("448e0b22-68f1-476d-862d-469992e4273a"), "Intelligence artificielle", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("9377bee6-e73e-41dc-8d0f-7efcb4ebce86"), "Robotique", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("47d8a014-4dfb-4931-bc27-493f75ff12c6"), "SST", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("cfea79dc-1ad6-4b19-a6c4-cf00ad24cf2a"), "Formule", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("e398a1be-eaad-4c58-82db-8a15abb53508"), "Thèmes", 3, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("df38ee68-52d3-4c24-8ba7-7d566a80f780"), "Big data", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("dfb2f32a-c156-42e6-8969-38fffb71ee66"), "Danse", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("2c3f6023-7f03-42ab-b1ab-a3301878c963"), "Jeux de société", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("5e743466-4cd7-4299-9f56-302e77256996"), "Innovations", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("0adf9644-6c46-466d-abf0-49e05c1704a5"), "Environnement", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("652f590f-c1fe-4157-8dc6-988bc87902fe"), "Éthique", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("21e091c8-0a4c-401a-bb78-66f7036704ac"), "Collaboration", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("06444ae6-24e9-43eb-a6b0-39402cb9a44e"), "Santé", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("79593f19-4c6a-4450-89f3-d9971e5a169f"), "Qualité de vie", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("7501d748-aea1-4fba-9cd2-0e7ac241a646"), "Éducation", 1, DateTime.UtcNow, DateTime.UtcNow },
        { Guid.Parse("47054ed7-ec13-4880-ace6-8165f85ce831"), "STEM", 1, DateTime.UtcNow, DateTime.UtcNow }
    };



}
