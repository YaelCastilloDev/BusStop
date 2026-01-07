using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Route
{
    /// <summary>
    /// DTO for a single stop to be created as part of a new route.
    /// </summary>
    public class RegisterStopDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// The geographic latitude.
        /// We use Latitude/Longitude in DTOs as it's a standard
        /// way for clients (like web/mobile) to send location data.
        /// Your application layer will convert this to a NetTopologySuite 'Point'.
        /// </summary>
        [Range(-90, 90)]
        public double Latitude { get; set; }

        /// <summary>
        /// The geographic longitude.
        /// </summary>
        [Range(-180, 180)]
        public double Longitude { get; set; }

        /// <summary>
        /// The ID of the stop type (e.g., 1 = 'Bus Stop', 2 = 'Terminal').
        /// This corresponds to the 'stop_types' table.
        /// </summary>
        [Required]
        public int StopTypeId { get; set; }
    }
}