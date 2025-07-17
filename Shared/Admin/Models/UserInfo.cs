using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Admin.Models
{
	public class UserInfo
	{
        public string UserId { get; }
        public int InstallationId { get; }

        public UserInfo(string userId, int installationId)
		{
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            InstallationId = installationId;

        }
	}
}
