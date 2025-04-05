using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIAirline.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class DestinationController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DestinationController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Destination
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Destination>>> GetDestinations()
    {
        return await _context.Destinations.ToListAsync();
    }

    // GET: api/Destination/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Destination>> GetDestination(int id)
    {
        var destination = await _context.Destinations.FindAsync(id);

        if (destination == null)
        {
            return NotFound();
        }

        return destination;
    }

    // GET: api/Destination/byCity/{cityCode}
    [HttpGet("byCity/{cityCode}")]
    public async Task<ActionResult<IEnumerable<Destination>>> GetDestinationsByCity(string cityCode)
    {
        var destinations = await _context.Destinations
            .Where(d => d.CityCode == cityCode)
            .ToListAsync();

        if (destinations == null || !destinations.Any())
        {
            return NotFound($"No destinations found for city code: {cityCode}");
        }

        return destinations;
    }

    // GET: api/Destination/byAirport/{airportCode}
    [HttpGet("byAirport/{airportCode}")]
    public async Task<ActionResult<Destination>> GetDestinationByAirportCode(string airportCode)
    {
        var destination = await _context.Destinations
            .FirstOrDefaultAsync(d => d.AirportCode == airportCode);

        if (destination == null)
        {
            return NotFound($"No destination found with airport code: {airportCode}");
        }

        return destination;
    }

    // POST: api/Destination
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Destination>> CreateDestination(Destination destination)
    {
        // Check if airport code already exists
        if (await _context.Destinations.AnyAsync(d => d.AirportCode == destination.AirportCode))
        {
            return BadRequest($"Airport code {destination.AirportCode} already exists");
        }

        _context.Destinations.Add(destination);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDestination), new { id = destination.Id }, destination);
    }

    // PUT: api/Destination/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateDestination(int id, Destination destination)
    {
        if (id != destination.Id)
        {
            return BadRequest();
        }

        // Check if airport code already exists for a different destination
        if (await _context.Destinations.AnyAsync(d => d.AirportCode == destination.AirportCode && d.Id != id))
        {
            return BadRequest($"Airport code {destination.AirportCode} already exists for a different destination");
        }

        _context.Entry(destination).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DestinationExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Destination/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteDestination(int id)
    {
        var destination = await _context.Destinations.FindAsync(id);
        if (destination == null)
        {
            return NotFound();
        }

        // Check if the destination is used in any flights
        bool isUsedInFlights = await _context.Flights
            .AnyAsync(f => f.DepartureDestinationId == id || f.ArrivalDestinationId == id);

        if (isUsedInFlights)
        {
            return BadRequest("Cannot delete destination as it is referenced by one or more flights");
        }

        _context.Destinations.Remove(destination);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool DestinationExists(int id)
    {
        return _context.Destinations.Any(e => e.Id == id);
    }
}