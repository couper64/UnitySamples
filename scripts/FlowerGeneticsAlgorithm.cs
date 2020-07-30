// Serializeable, Random, Math.
using System;
using UnityEngine;
// AssetDatabase.
using UnityEditor;

namespace GeneticAlgorithms.Flower
{
    public class FlowerGeneticsAlgorithm : MonoBehaviour
    {
        #region Structs

        [Serializable]
        public struct Insect
        {
            // How many beneficial insects it needs.
            public int beneficial;

            // How many harmfull insects it can resist.
            public int harmfull;
        }

        [Serializable]
        public struct Condition
        {
            // Temperature under which plant could survive.
            public int temperature;

            // Amount of water it needs to survive.
            public int water;

            // Amount of sunlight required to be sustainable.
            public int sunlight;

            // Amount of nutrients it consumes.
            public int nutrient;

            // Insects it interacts with.
            public Insect insect;

            /// <summary>
            /// Genetics Algorithm requires a lot of randomisation
            /// operations. It makes sense to function it.
            /// </summary>
            /// <param name="condition">Condition you want to randomise.</param>
            /// <param name="randomMaximum">Maximum random integer value for the random function.</param>
            /// <returns></returns>
            public static Condition Randomise(Condition condition, int randomMaximum)
            {
                // Local random variable.
                System.Random random = new System.Random();

                // Randomise.
                condition.temperature = random.Next(1, randomMaximum);
                condition.water = random.Next(1, randomMaximum);
                condition.sunlight = random.Next(1, randomMaximum);
                condition.nutrient = random.Next(1, randomMaximum);
                condition.insect.beneficial = random.Next(1, randomMaximum);
                condition.insect.harmfull = random.Next(1, randomMaximum);

                // Return result.
                return (condition);
            }

            /// <summary>
            /// Subtraction operation was required because of algorithm 
            /// of fitness computation.
            /// Intention was to make it look more readable.
            /// </summary>
            /// <param name="left">Variable on the left hand side.</param>
            /// <param name="right">Variable on the right hand side.</param>
            /// <returns></returns>
            public static Condition operator -(Condition left, Condition right)
            {
                // Container.
                Condition result;

                // Straight subtraction, no checks, no abs.
                result.temperature = left.temperature - right.temperature;
                result.water = left.water - right.water;
                result.sunlight = left.sunlight - right.sunlight;
                result.nutrient = left.nutrient - right.nutrient;
                result.insect.beneficial = left.insect.beneficial - right.insect.beneficial;
                result.insect.harmfull = left.insect.harmfull - right.insect.harmfull;

                // Result.
                return (result);
            }
        }

        #endregion

        #region Properties

        [Header("Number of generations to run")]
        [SerializeField]
        private int generations;

        [Header("Current generation index")]
        [SerializeField]
        private int generation;

        [Header("Number of flowers in a generation")]
        [SerializeField]
        private int flowersCount = 11;

        [Header("Environment conditions")]
        [SerializeField]
        private Condition environmentCondition;

        [Header("Features of all flowers in a generation")]
        [SerializeField]
        Condition[] flowerConditions;

        [Header("Flower game objects on the scene")]
        [SerializeField]
        private GameObject[] flowers;

        #endregion

        #region Custom Functions

        private void Default()
        {
            // Bring back to original values.
            generations = 8400;
            generation = 0;
            flowerConditions = null;
            flowers = null;
        }

        private void Encode()
        {
            // Generator.
            System.Random random = new System.Random();

            // Maximum value per feature.
            int randomFeatureMaximum = 76;

            // For for-loop.
            for (int i = 0; i < flowersCount; i++)
            {
                // Basic absolutely random changes.
                flowerConditions[i] = Condition.Randomise
                (
                    flowerConditions[i],
                    randomFeatureMaximum
                );
            }

            // Setup enviroment which should be absolutely undefined.
            environmentCondition = Condition.Randomise
            (
                environmentCondition,
                randomFeatureMaximum
            );
        }

        public void Evolve()
        {
            // Random number generator.
            System.Random random = new System.Random();

            // Maximum number of flowers we could 
            // mix between.
            int randomEvolutionMaximum = flowersCount;

            // Used to get probabilty from 0 to 100%.
            // Extra one because 0 - is excluded.
            int randomMutationMaximum = 101;

            // Maximum value each feature of a condition 
            // could be given.
            int randomFeatureMaximum = 76;

            // List of conditions to find the least fit 
            // condition from.
            Condition[] fitConditions = new Condition[flowersCount];

            // Value of leas fit condition.
            int leastFit = 0;

            // Index to the condition with the least fit 
            // features.
            int leastFitIndex = 0;

            // Search for the least adapted flower.
            for (int i = 1; i < flowersCount; i++)
            {
                // Higher the value then less adapted flower is.
                if (Fitness(i) > leastFit)
                {
                    // Preserve fitness to 
                    // compare against other 
                    // conditions.
                    leastFit = Fitness(i);

                    // Store index to the least 
                    // adapted condition so far.
                    leastFitIndex = i;
                }
            }

            // Replace the least fit flower. There is a change it preserves its features.
            flowerConditions[leastFitIndex] = Condition.Randomise
            (
                flowerConditions[random.Next(1, randomEvolutionMaximum)],
                randomEvolutionMaximum
            );

            // Evolution for the entire generation.
            for (int i = 1; i < flowersCount; i++)
            {
                // Simply mixing of features between survived conditions.
                fitConditions[i] = Condition.Randomise
                (
                    flowerConditions[random.Next(1, randomEvolutionMaximum)],
                    randomEvolutionMaximum
                );
            }

            // Replace old generation with new generation.
            flowerConditions = (Condition[])fitConditions.Clone();

            // Mutations of 0.01 chance.
            for (int i = 0; i < flowersCount; i++)
            {
                // Risk of mutation is per feature rather then per condition.

                // Temparture.
                if (random.Next(1, randomMutationMaximum) == 1)
                {
                    flowerConditions[i].temperature = random.Next(1, randomFeatureMaximum);
                }

                // Water.
                if (random.Next(1, randomMutationMaximum) == 1)
                {
                    flowerConditions[i].water = random.Next(1, randomFeatureMaximum);
                }

                // Sunlight.
                if (random.Next(1, randomMutationMaximum) == 1)
                {
                    flowerConditions[i].sunlight = random.Next(1, randomFeatureMaximum);
                }

                // Nutrient.
                if (random.Next(1, randomMutationMaximum) == 1)
                {
                    flowerConditions[i].nutrient = random.Next(1, randomFeatureMaximum);
                }

                // Benefecial insect.
                if (random.Next(1, randomMutationMaximum) == 1)
                {
                    flowerConditions[i].insect.beneficial = random.Next(1, randomFeatureMaximum);
                }

                // Harmfull insect.
                if (random.Next(1, randomMutationMaximum) == 1)
                {
                    flowerConditions[i].insect.harmfull = random.Next(1, randomFeatureMaximum);
                }
            }
        }

        public int Fitness(int flowerIndex)
        {
            // How fit the features are? (Coefficient).
            int fitness;

            // Calculating our fitness. (Algorithm).
            // First find deltas of features of a condition.
            Condition delta = flowerConditions[flowerIndex] - environmentCondition;

            // Second, sum up an absolute values of features. 
            fitness = Math.Abs(delta.temperature);
            fitness += Math.Abs(delta.water);
            fitness += Math.Abs(delta.sunlight);
            fitness += Math.Abs(delta.nutrient);
            fitness += Math.Abs(delta.insect.beneficial);
            fitness += Math.Abs(delta.insect.harmfull);

            // Result. (Coefficient).
            return (fitness);
        }

        private void Align()
        {
            // Position this transform in the center of the screen.
            Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2));

            // Make some relative offsets.
            position.x = -(5.00f * flowersCount) / 2.00f;
            position.y = -2.00f * flowersCount;
            position.z = 5.00f * flowersCount;

            // Finally, apply.
            transform.position = position;
        }

        #endregion

        #region Monobehaviour Callbacks

        private void Reset()
        {
            // Defaults.
            Default();
        }

        private void Start()
        {
            // Make this transform stay relatively 
            // in the centre of the screen regardless of 
            // the size or aspect ratio.
            Align();

            // Init all arrays.
            flowers = new GameObject[flowersCount];
            flowerConditions = new Condition[flowersCount];

            // Encode.
            Encode();

            // Path to flower prefab.
            string flowerPath = "Assets/_Prefabs/Flower.prefab";

            // Loading from file.
            GameObject flowerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(flowerPath);

            for (int i = 0; i < flowersCount; i++)
            {
                // Copy object and save a reference.
                GameObject clone = Instantiate(flowerPrefab, transform);

                // Default rotation.
                clone.transform.rotation = Quaternion.identity;

                // Cache position to modify.
                Vector3 position = clone.transform.localPosition;

                // Position in a row.
                position.x = 5.0f * i;

                // Apply.
                clone.transform.localPosition = position;

                // Save the reference in a list of flowers.
                flowers[i] = clone;
            }
        }

        private void Update()
        {
            // Run until we made as many generations
            // as we desired.
            if (generation < generations)
            {
                // Evolve. Make some "natural" changes to a new generation.
                Evolve();

                // Prepare message container.
                string consoleMessage = generation + ": ";

                // Cycle through all flowers.
                for (int i = 1; i < flowersCount; i++)
                {
                    // Fitness.
                    int fitness = Fitness(i);

                    // Append container.
                    consoleMessage += fitness.ToString() + ", ";

                    // Visual feedback.
                    Vector3 scale = new Vector3(1.00f, fitness / 10.00f, 1.00f);

                    // Apply.
                    flowers[i].transform.localScale = scale;
                }

                // Output.
                print(consoleMessage);
            }

            // Next generation.
            generation++;
        }

        #endregion
    }
}
