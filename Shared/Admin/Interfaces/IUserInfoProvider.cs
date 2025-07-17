
using Shared.Admin.Models;

namespace Shared.Admin.Interfaces
{
	public interface IUserInfoProvider
	{
        /// <summary>The current authenticated user + tenant.</summary>
        UserInfo GetUserInfo();
        int InstallationId { get; }

    }
}
