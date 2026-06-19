using System.Text.Json;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Exceptions;
using Dsw2026Ej15.Domain.Interfaces;

namespace Dsw2026Ej15.Data;

public class PersistenceInMemory : IPersistence
{
    private readonly List<Doctor> _doctors = [];
    private readonly List<Speciality> _specialities;

    public PersistenceInMemory()
    {
        _specialities = LoadSpecialities();
    }

    public IReadOnlyCollection<Doctor> GetActiveDoctors()
    {
        return _doctors
            .Where(doctor => doctor.IsActive)
            .ToList();
    }

    public Doctor? GetActiveDoctorById(Guid id)
    {
        return _doctors
            .FirstOrDefault(doctor => doctor.Id == id && doctor.IsActive);
    }

    public void AddDoctor(Doctor doctor)
    {
        _doctors.Add(doctor);
    }

    public void DeactivateDoctor(Guid id)
    {
        var doctor = GetActiveDoctorById(id);

        if (doctor is null)
        {
            return;
        }

        doctor.IsActive = false;
    }

    public Speciality? GetSpecialityById(Guid id)
    {
        return _specialities
            .FirstOrDefault(speciality => speciality.Id == id);
    }

    public IReadOnlyCollection<Speciality> GetSpecialities()
    {
        return _specialities.ToList();
    }

    private static List<Speciality> LoadSpecialities()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "specialities.json");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                "No se encontró el archivo specialities.json.",
                filePath);
        }

        var json = File.ReadAllText(filePath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var specialities = JsonSerializer.Deserialize<List<Speciality>>(json, options);

        return specialities ?? [];
    }
}