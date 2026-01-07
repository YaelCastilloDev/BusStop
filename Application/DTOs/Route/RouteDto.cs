//using Application.DTOs.Comment;
using Application.DTOs.Stop;
using Application.DTOs.User;

namespace Application.DTOs.Route
{
    /// <summary>
    /// Represents a route with all its details, including stops and comments.
    /// This is typically returned from a 'GetRouteById' endpoint.
    /// </summary>
    public class RouteDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// A simplified summary of the user who created the route.
        /// </summary>
        public UserDto CreatedBy { get; set; } = default!;

        /// <summary>
        /// The list of stops for this route, in order.
        /// Your service layer is responsible for sorting this list.
        /// </summary>
        public List<StopDto> Stops { get; set; } = new List<StopDto>();

        /// <summary>
        /// A list of top-level comments for the route.
        /// </summary>
       // public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
    }
}