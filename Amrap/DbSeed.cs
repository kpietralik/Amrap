using Amrap.Core.Domain;
using Amrap.Core.Infrastructure;

namespace Amrap;

internal static class DbSeed
{
    internal static async Task SeedDataIfNeeded(DatabaseHandler databaseHandler, bool force = false)
    {
        string isFirstRun = await SecureStorage.Default.GetAsync(Consts.FirstRunKey);

        if (!string.Equals(isFirstRun, Consts.TrueValue, StringComparison.InvariantCultureIgnoreCase)
            || force)
        {
            // ToDo: Seed from json?
            // Seeding ExerciseType objects at first run of the app
            await SecureStorage.Default.SetAsync(Consts.FirstRunKey, Consts.TrueValue);

            var glutesDip = new ExerciseType("e19692de-2a43-4be9-b4ed-676676ffe6ab", ExerciseKind.Core, "Glutes dip", "", "");
            var benchPress = new ExerciseType("84d9a6d2-7a03-4a06-91e5-227d96411adf", ExerciseKind.Push, "Bench press", null, "https://cdn-0.weighttraining.guide/wp-content/uploads/2016/10/push-up-tall-resized.png");
            var deadlift = new ExerciseType("ec2b1e30-d65b-42e1-9cd9-988e45790c5c", ExerciseKind.Push, "Deadlift", "standard", null);
            var lyingLegCurs = new ExerciseType("8724c2b5-0bde-4a25-90ba-58918c71bf5b", ExerciseKind.Legs, "Lying leg curls", "", "");
            var weightedDip = new ExerciseType("ee0e9be9-c6e9-4186-9d03-dd1a63eeae6c", ExerciseKind.Push, "Weighted dip", "", "");
            var cableUprightRow = new ExerciseType("3b14f0b8-cf1c-401f-9022-3de6c66a4240", ExerciseKind.Pull, "Cable upright row", null, "https://i.shgcdn.com/4a56b60f-112a-4090-b9c0-2bdb4b7891ac/-/format/auto/-/preview/3000x3000/-/quality/lighter/");
            var bentOverDumbbell = new ExerciseType("e7368570-4b8e-4875-b1d6-e11e84f4026c", ExerciseKind.Pull, "Bent over dumbbell", "", "");
            var concenrationCurl = new ExerciseType("3a742c88-7d0c-48ff-926b-b688851dbc62", ExerciseKind.Pull, "Concentration curl", "", "");
            var straightBarCurl = new ExerciseType("b809e07a-656d-4167-a1e4-2609eb55df0b", ExerciseKind.Pull, "Straight bar curl", "", "");
            var legRaise = new ExerciseType("70b8950f-a0d5-4000-9ae8-650f3d84ff11", ExerciseKind.Core, "Leg raise", "", "");
            var ezBarBizepCurls = new ExerciseType("bf4b1e66-2570-4c09-9171-9a2d18df9357", ExerciseKind.Pull, "Ez bar bicep curl", "", "");
            var backHyperextension = new ExerciseType("c18e7af1-4854-420c-9515-40b2e0b3c7ec", ExerciseKind.Core, "Back hyperextension", "", "");
            var pullUpWidegrip = new ExerciseType("3c06d986-e7af-4adb-a76b-7598fbcc4bb3", ExerciseKind.Pull, "Pull up", "Wide grip", "");
            var legPress = new ExerciseType("a7264596-c03a-4d4e-9f16-7684f627065e", ExerciseKind.Legs, "Leg press", "", "");
            var chestSupportedRow = new ExerciseType("6c19e7f7-65d6-406e-9df3-f52105104060", ExerciseKind.Pull, "Chest supported row", "", "");
            var standingArnoldPress = new ExerciseType("e549a95b-ad09-48ae-b74e-97c007462519", ExerciseKind.Push, "Standing arnold press", "", "");
            var seatedCalfRaises = new ExerciseType("205d8384-6493-4667-bdbb-717ccdd459e9", ExerciseKind.Legs, "Seated calf raises", "", "");
            var inclineDumbbellPress = new ExerciseType("806c3106-0a75-4501-942c-8980566e74c1", ExerciseKind.Push, "Incline dumbbell press", "", "");
            var hammerCurl = new ExerciseType("e56d6a84-b8cb-4421-9f67-5f309026cb4d", ExerciseKind.Pull, "Hammer curl", "", "");
            var cableSeatedRow = new ExerciseType("1211a4b3-7f91-4665-92cb-7536ca902a69", ExerciseKind.Pull, "Cable seated row", "", "");
            var legExtension = new ExerciseType("35560243-773d-454f-9a33-2c4d78f0c7e8", ExerciseKind.Legs, "Leg extension", "", "");
            var unilateralLatPulldown = new ExerciseType("9a8c4544-2191-4186-b97a-81cf3c3c876f", ExerciseKind.Pull, "Unilateral lat pulldown", "", "");
            var pullUpInsideGrip = new ExerciseType("807b59cb-0afe-4c0a-ad68-a8eea5c2d1ce", ExerciseKind.Pull, "Pull up", "Inside grip", "");
            var ropeFacePull = new ExerciseType("bd3a788e-3ad4-4b90-a53d-3ed6df3d434f", ExerciseKind.Pull, "Rope face pull", "", "");
            var tricepPressdown = new ExerciseType("f2ccf96a-8f71-4e77-9945-d9934a950172", ExerciseKind.Push, "Tricep pressdown", "single arm", "");
            var lateralRaise = new ExerciseType("a5e7afb2-8639-4a14-be3b-21dfe02541fb", ExerciseKind.Pull, "Lateral raise", "standard", "");
            var lowToHighCableFlye = new ExerciseType("bdce2420-ea83-41f2-a6a9-e0194748cd51", ExerciseKind.Push, "Low-to-high cable flye", "", "");

            await databaseHandler.SeedExerciseTypes(new List<ExerciseType>() {
                glutesDip,
                benchPress,
                deadlift,
                lyingLegCurs,
                weightedDip,
                cableUprightRow,
                bentOverDumbbell,
                concenrationCurl,
                straightBarCurl,
                legRaise,
                ezBarBizepCurls,
                backHyperextension,
                pullUpWidegrip,
                legPress,
                chestSupportedRow,
                standingArnoldPress,
                seatedCalfRaises,
                inclineDumbbellPress,
                hammerCurl,
                cableSeatedRow,
                legExtension,
                unilateralLatPulldown,
                pullUpInsideGrip,
                ropeFacePull,
                tricepPressdown,
                lateralRaise,
                lowToHighCableFlye
            });

#region KP first run only
            // ToDo: private temp code
            string isKpFirstRun = await SecureStorage.Default.GetAsync(Consts.KpFirstRunKey);

            if (!string.Equals(isKpFirstRun, Consts.TrueValue, StringComparison.InvariantCultureIgnoreCase)
                || force)
            {
                await SecureStorage.Default.SetAsync(Consts.KpFirstRunKey, Consts.TrueValue);
                var p1 = new PlannedExercise("f99c6db3-420e-466a-86d6-99e792031cc5", glutesDip, 2, 15, 24, null, false, false);
                var p2 = new PlannedExercise("e33fc5f8-42ff-4232-81db-4b88a9cc2eea", benchPress, 3, 6, 55, null, false, false);
                var p3 = new PlannedExercise("9b4dc290-cff8-4362-b737-efe1bbceb1db", lyingLegCurs, 3, 54, 60, null, false, false);
                var p4 = new PlannedExercise("e8acaa81-3460-446c-a932-9289840157e9", weightedDip, 2, 14, 1, null, false, false);
                var p5 = new PlannedExercise("cca4cbee-ab97-468b-aa3e-3ff55be06000", cableUprightRow, 3, 10, 27.5f, null, false, false);
                var p6 = new PlannedExercise("fb70f1bf-139d-488b-a2f1-0bde1685f38a", bentOverDumbbell, 3, 12, 30, null, false, false);
                var p7 = new PlannedExercise("a08c69b8-0a4b-45f0-8cbc-8e856d5762ee", straightBarCurl, 3, 12, 30, null, false, false);
                var p8 = new PlannedExercise("6824b20f-9ba4-48d3-9124-c1d488b82e45", legRaise, 3, 20, 1, null, false, false);
                var p9 = new PlannedExercise("91623cd3-e3d2-4c7f-9049-a86bb6cdf511", ezBarBizepCurls, 3, 12, 30, null, true, false);
                var p10 = new PlannedExercise("418bbe04-c34c-4cdb-b33c-42872aa4085b", backHyperextension, 3, 15, 20, null, false, false);
                var p11 = new PlannedExercise("54975f52-5639-4b6e-9215-a5dc442c6777", pullUpWidegrip, 3, 10, 1, null, false, false);
                var p12 = new PlannedExercise("be2ce1be-f50b-4451-94d1-89f71972eb09", legPress, 3, 15, 125, null, false, false);
                var p13 = new PlannedExercise("f2c4cc61-a3fc-4fc7-be85-0251cd277916", chestSupportedRow, 3, 12, 70, null, false, false);
                var p14 = new PlannedExercise("c0574e38-a1ad-474c-9430-a12fb234e559", standingArnoldPress, 3, 12, 25, null, false, false);
                var p15 = new PlannedExercise("25fef2cc-88bb-4bf8-a173-4201fc12f3d5", seatedCalfRaises, 2, 30, 60, null, false, false);
                var p16 = new PlannedExercise("fc806fc3-0e84-4ab4-b229-51ae6fff6500", inclineDumbbellPress, 3, 8, 30, null, false, false);
                var p17 = new PlannedExercise("ff1be172-c281-4477-9851-160586e09a62", hammerCurl, 3, 10, 15, null, false, false);
                var p18 = new PlannedExercise("ee1db0b2-8383-4e49-ae55-9c678d58a474", cableSeatedRow, 3, 12, 70, null, false, false);
                var p19 = new PlannedExercise("96d8475b-9af9-482a-9260-4b4eb5a45381", legExtension, 3, 20, 78, null, false, false);
                var p20 = new PlannedExercise("4b9fd762-1c42-476c-855b-555168e46b94", pullUpInsideGrip, 2, 10, 1, null, false, false);
                var p21 = new PlannedExercise("e14b8b09-713e-4913-bc1c-dea9d46a1baf", legRaise, 3, 20, 1, null, false, false);
                var p22 = new PlannedExercise("33fbbd75-d8e6-4760-8c33-79c040e64032", ropeFacePull, 3, 15, 27.5f, null, false, false);
                var p23 = new PlannedExercise("5f029fd3-ed49-45d1-81f3-a7ad1e509f61", tricepPressdown, 3, 15, 7.5f, null, false, false);
                var p24 = new PlannedExercise("da0c20bb-daa0-42cc-a65f-a2764e89a656", lateralRaise, 3, 12, 5, null, false, false);
                var p25 = new PlannedExercise("88db8d56-ba06-42de-81ec-a56de473e94f", lowToHighCableFlye, 3, 12, 15, null, false, false);

                #region Saving planned exercises
                await databaseHandler.AddPlannedExercise(p1);
                await databaseHandler.AddPlannedExercise(p2);
                await databaseHandler.AddPlannedExercise(p3);
                await databaseHandler.AddPlannedExercise(p4);
                await databaseHandler.AddPlannedExercise(p5);
                await databaseHandler.AddPlannedExercise(p6);
                await databaseHandler.AddPlannedExercise(p7);
                await databaseHandler.AddPlannedExercise(p8);
                await databaseHandler.AddPlannedExercise(p9);
                await databaseHandler.AddPlannedExercise(p10);
                await databaseHandler.AddPlannedExercise(p11);
                await databaseHandler.AddPlannedExercise(p12);
                await databaseHandler.AddPlannedExercise(p13);
                await databaseHandler.AddPlannedExercise(p14);
                await databaseHandler.AddPlannedExercise(p15);
                await databaseHandler.AddPlannedExercise(p16);
                await databaseHandler.AddPlannedExercise(p17);
                await databaseHandler.AddPlannedExercise(p18);
                await databaseHandler.AddPlannedExercise(p19);
                await databaseHandler.AddPlannedExercise(p20);
                await databaseHandler.AddPlannedExercise(p21);
                await databaseHandler.AddPlannedExercise(p22);
                await databaseHandler.AddPlannedExercise(p23);
                await databaseHandler.AddPlannedExercise(p24);
                await databaseHandler.AddPlannedExercise(p25);
                #endregion

                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("a0a3b759-2672-48ce-a4bc-257fdce63e05", p1, DayOfWeek.Monday, 1f));
                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("f1c4e31c-ddfd-4880-9d8d-b9c278f86d8c", p2, DayOfWeek.Monday, 2f));
                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("da4c62b3-d01d-4616-868a-e7a8b89885a2", p3, DayOfWeek.Monday, 3f));
                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("df923f86-503b-43f0-b470-94d34052b2f9", p4, DayOfWeek.Monday, 4f));
                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("966c040d-60ba-42a9-9862-e312a05974db", p5, DayOfWeek.Monday, 5f));
                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("8e8315b5-b322-4a32-8f57-a078a451f999", p6, DayOfWeek.Monday, 6f));
                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("f6f95e4a-a7fb-48e3-8136-4f85e2deb408", p7, DayOfWeek.Monday, 7f));
                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("951f8482-dc2e-4d8b-8d62-723f923fbd4d", p8, DayOfWeek.Monday, 8f));

                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("f157b0f8-d2c4-486a-8acd-e3526e0c340c", p9, DayOfWeek.Wednesday, 1f));
                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("440ad596-acb8-43d7-ae76-bb1853dc54d7", p10, DayOfWeek.Wednesday, 2f));
                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("c7f608f7-88fe-477d-885a-0cc39eb5695d", p11, DayOfWeek.Wednesday, 3f));
                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("2e2f630d-7fe8-42e7-bce9-d66a803bcc8c", p12, DayOfWeek.Wednesday, 4f));
                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("c3803b83-309f-4ade-9103-dd0ee77815f3", p13, DayOfWeek.Wednesday, 5f));
                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("4c8f9f69-71b1-4585-ac94-22da31dc30df", p14, DayOfWeek.Wednesday, 6f));
                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("cc53d957-f765-424f-a1d2-2cbbfc402ebb", p15, DayOfWeek.Wednesday, 7f));
                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("da9b62c7-a8e6-483f-9ecc-84b50a9d835a", p16, DayOfWeek.Wednesday, 8f));
                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("5e3d6d1e-d992-4d4a-a807-23a5a2e5827b", p17, DayOfWeek.Wednesday, 9f));

                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("0c2df6dc-65ee-437c-9949-2e244feee7e1", p18, DayOfWeek.Friday, 1f));
                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("29d20ccf-cfcb-456e-9443-572ebf066679", p19, DayOfWeek.Friday, 2f));
                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("5851eae1-63f3-4856-b339-0d06329b468b", p20, DayOfWeek.Friday, 3f));
                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("23213e90-2c33-4304-8c7e-a215812a568b", p21, DayOfWeek.Friday, 4f));
                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("18fd83cb-e19a-49bf-8804-cc9d61a98fd7", p22, DayOfWeek.Friday, 5f));
                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("fef72a4f-4977-49f1-9207-5da80221f632", p23, DayOfWeek.Friday, 6f));
                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("6795bc2e-031f-4830-b77b-31d0836b3043", p24, DayOfWeek.Friday, 7f));
                await databaseHandler.AddWorkoutPlanItem(new WorkoutPlanItem("e6774e2f-a362-4145-a9b2-dfee3e5a769c", p25, DayOfWeek.Friday, 8f));
#endregion
            }
        }
    }
}
