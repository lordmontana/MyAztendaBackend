using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Persistence
{
	public class ApplicationUser : IdentityUser
	{

		// Add custom properties here if needed
		public int IId { get; set; }

		[MaxLength(50)]
		public string? Role { get; set; } 

	}
}