using System.ComponentModel.DataAnnotations;
using Microsoft.ML.Data;

namespace DeliveryTimePrediction.Models
{
    public class DeliveryData
    {
        [LoadColumn(0)]
        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "Distance cannot be negative")]
        public float Distance { get; set; }

        [LoadColumn(1)]
        [Required]
        public string? TrafficLevel { get; set; }

        [LoadColumn(2)]
        [Required]
        public string? TimeOfDay { get; set; }

        [LoadColumn(3)]
        [Required]
        public string? DayOfWeek { get; set; }

        [LoadColumn(4)]
        [Required]
        public string? Weather { get; set; }

        [LoadColumn(5)]
        [Range(-10, 60)]
        public float Temperature { get; set; }

        [LoadColumn(6)]
        [Required]
        public string? VehicleType { get; set; }

        [LoadColumn(7)]
        [Required]
        public string? DeliveryType { get; set; }

        [LoadColumn(8)]
        public float Label { get; set; }

        [LoadColumn(9)]
        public float PickupLat { get; set; }

        [LoadColumn(10)]
        public float PickupLong { get; set; }

        [LoadColumn(11)]
        public float DropLat { get; set; }

        [LoadColumn(12)]
        public float DropLong { get; set; }

        //new dataa
        [LoadColumn(13)]
        [Required]
        [Range(0, 100, ErrorMessage = "Number of stops must be between 0 and 100")]
        public float NumberOfStops { get; set; }

        [LoadColumn(14)]
        [Required]
        [Range(0, 50, ErrorMessage = "Driver experience must be between 0 and 50 years")]
        public float DriverExperience { get; set; }

        [LoadColumn(15)]
        [Required]
        [Range(1, 150, ErrorMessage = "Average speed must be between 1 and 150 km/h")]
        public float AverageSpeed { get; set; }

        [LoadColumn(16)]
        [Required]
        [Range(1, 5, ErrorMessage = "Driver rating must be between 1 and 5")]
        public float DriverRating { get; set; }
    }
}