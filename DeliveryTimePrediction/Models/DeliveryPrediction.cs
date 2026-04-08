using Microsoft.ML.Data;

namespace DeliveryTimePrediction.Models
{
    public class DeliveryPrediction
    {
        [ColumnName("Score")]
        public float Score{ get; set; }
    }
}
