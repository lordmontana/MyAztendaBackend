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
        public string UserName { get; }

        public UserInfo(string userId, int installationId,string userName)
		{
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            InstallationId = installationId;
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));

        }
	}
}
