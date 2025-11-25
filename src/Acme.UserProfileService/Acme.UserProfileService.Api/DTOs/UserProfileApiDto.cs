namespace Acme.UserProfileService.Api.DTOs
{
    public class UserProfileApiDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int Age { get; set; }
    }
}
