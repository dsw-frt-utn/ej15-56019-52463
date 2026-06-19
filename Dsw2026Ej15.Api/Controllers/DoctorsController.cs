using Dsw2026Ej15.Api.Dtos;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Exceptions;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2026Ej15.Api.Controllers;

[ApiController]
[Route("api/doctors")]
public class DoctorsController : ControllerBase
{
    private readonly IPersistence _persistence;

    public DoctorsController(IPersistence persistence)
    {
        _persistence = persistence;
    }

    [HttpPost]
    public IActionResult CreateDoctor(CreateDoctorRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ValidationException("El nombre del médico es requerido.");
        }

        if (string.IsNullOrWhiteSpace(request.LicenseNumber))
        {
            throw new ValidationException("La matrícula del médico es requerida.");
        }

        var speciality = _persistence.GetSpecialityById(request.SpecialityId);

        if (speciality is null)
        {
            throw new ValidationException("La especialidad indicada no existe.");
        }

        var doctor = new Doctor
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            LicenseNumber = request.LicenseNumber.Trim(),
            Speciality = speciality,
            IsActive = true
        };

        _persistence.AddDoctor(doctor);

        var response = MapToResponse(doctor);

        return CreatedAtAction(
            nameof(GetDoctorById),
            new { id = doctor.Id },
            response);
    }

    [HttpGet]
    public IActionResult GetActiveDoctors()
    {
        var doctors = _persistence
            .GetActiveDoctors()
            .Select(MapToResponse)
            .ToList();

        return Ok(doctors);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetDoctorById(Guid id)
    {
        var doctor = _persistence.GetActiveDoctorById(id);

        if (doctor is null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(doctor));
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteDoctor(Guid id)
    {
        var doctor = _persistence.GetActiveDoctorById(id);

        if (doctor is null)
        {
            return NotFound();
        }

        _persistence.DeactivateDoctor(id);

        return NoContent();
    }

    private static DoctorResponse MapToResponse(Doctor doctor)
    {
        return new DoctorResponse
        {
            Id = doctor.Id,
            Name = doctor.Name,
            LicenseNumber = doctor.LicenseNumber,
            SpecialityName = doctor.Speciality.Name
        };
    }
}