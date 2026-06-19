namespace Dsw2026Ej15.Domain.Entities;

public class Doctor : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string LicenseNumber { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public Speciality Speciality { get; set; } = null!;
}