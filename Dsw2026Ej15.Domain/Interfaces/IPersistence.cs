using Dsw2026Ej15.Domain.Entities;

namespace Dsw2026Ej15.Domain.Interfaces;

public interface IPersistence
{
    IReadOnlyCollection<Doctor> GetActiveDoctors();

    Doctor? GetActiveDoctorById(Guid id);

    void AddDoctor(Doctor doctor);

    void DeactivateDoctor(Guid id);

    Speciality? GetSpecialityById(Guid id);

    IReadOnlyCollection<Speciality> GetSpecialities();
}