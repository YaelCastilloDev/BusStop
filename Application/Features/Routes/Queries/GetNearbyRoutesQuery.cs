// --- Application/Features/Routes/Queries/GetNearbyRoutesQuery.cs ---
using Application.DTOs.Route;
using MediatR;
using System.Collections.Generic;

namespace Application.Features.Routes.Queries
{
    // ✨ El secreto está en el ": IRequest<List<RouteDto>>" del final
    public record GetNearbyRoutesQuery(double Longitude, double Latitude) : IRequest<List<RouteDto>>;
}