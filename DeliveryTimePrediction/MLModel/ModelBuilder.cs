using Microsoft.ML;
using Microsoft.ML.Data;
using DeliveryTimePrediction.Models;

namespace DeliveryTimePrediction.MLModel
{
    public class ModelBuilder
    {
        
        private static readonly MLContext mlContext = new MLContext(seed: 1);
        private static ITransformer? model;
        private static DataViewSchema? modelSchema;

        public static void TrainModel(string dataPath)
        {
            
            if (!File.Exists(dataPath))
                throw new FileNotFoundException($"Training file not found at {dataPath}");
            var data = mlContext.Data.LoadFromTextFile<DeliveryData>(
                path: dataPath,
                hasHeader: true,
                separatorChar: ',');
            var split = mlContext.Data.TrainTestSplit(data, testFraction: 0.2);//split

            var pipeline = mlContext.Transforms.Categorical.OneHotEncoding(new[]
           {
             new InputOutputColumnPair("TrafficLevelEncoded", "TrafficLevel"),
             new InputOutputColumnPair("TimeOfDayEncoded", "TimeOfDay"),
             new InputOutputColumnPair("DayOfWeekEncoded", "DayOfWeek"),
             new InputOutputColumnPair("WeatherEncoded", "Weather"),
             new InputOutputColumnPair("VehicleTypeEncoded", "VehicleType"),
             new InputOutputColumnPair("DeliveryTypeEncoded", "DeliveryType"),
           })
            .Append(mlContext.Transforms.Concatenate("Features",

            // Numeric features directly used
            "Distance",
            "Temperature",

            // Location-based
            "PickupLat",
            "PickupLong",
            "DropLat",
            "DropLong",

            "NumberOfStops",
            "DriverExperience", 
            "AverageSpeed",
            "DriverRating", 

            // Encoded featurres
             "TrafficLevelEncoded",
             "TimeOfDayEncoded",
             "DayOfWeekEncoded",
             "WeatherEncoded",
             "VehicleTypeEncoded",
             "DeliveryTypeEncoded"))

            
            
             .Append(mlContext.Regression.Trainers.FastTree(
             labelColumnName: "Label",
             featureColumnName: "Features")); // Input features

            // Train model using training data
            model = pipeline.Fit(split.TrainSet);
            var predictions = model.Transform(split.TestSet);

            var metrics = mlContext.Regression.Evaluate(predictions, labelColumnName: "Label");
            Console.WriteLine($"R2 Score: {metrics.RSquared}");
            Console.WriteLine($"RMSE: {metrics.RootMeanSquaredError}");
            modelSchema = split.TrainSet.Schema;

            // Save trained model to file
            mlContext.Model.Save(model, modelSchema, "model.zip");
        }

        // Load saved model
        public static void LoadModel(string modelPath)
        {
            // Check if model exists
            if (!File.Exists(modelPath))
                throw new FileNotFoundException("Model file not found.");

            // Load model from file
            model = mlContext.Model.Load(modelPath, out modelSchema);
        }
        public static float Predict(DeliveryData input)
        {
            if (model == null)
                throw new InvalidOperationException("Model is not loaded or trained.");
            if (input.Distance <= 0)
                throw new ArgumentException("Distance must be greater than 0");
            var engine = mlContext.Model.CreatePredictionEngine<DeliveryData, DeliveryPrediction>(model);

            var result = engine.Predict(input);
            return result.Score;
        }
    }
}