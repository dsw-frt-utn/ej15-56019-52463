namespace Dsw2026Ej15.Api.Dtos;

public class DoctorResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string LicenseNumber { get; set; } = string.Empty;

    public string SpecialityName { get; set; } = string.Empty;
}