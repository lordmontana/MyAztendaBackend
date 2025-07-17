using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Shared.Admin.Interfaces;
using Shared.Admin.Models;

namespace Shared.Admin.Services
{
    public sealed class UserInfoProvider : IUserInfoProvider
    {
        private readonly IHttpContextAccessor _ctx;
        private UserInfo? _cached;                      // per-request cache

        public UserInfoProvider(IHttpContextAccessor ctx) => _ctx = ctx;

        public UserInfo GetUserInfo()
        {
            if (_cached is not null) return _cached;

            var http = _ctx.HttpContext;
            var principal = http?.User;

            // ── 1. design-time / non-HTTP code path ───────────────
            if (principal == null || principal.Identity?.IsAuthenticated != true)
            {
                _cached = new UserInfo("design-time", 0);   // neutral tenant
                return _cached;
            }

            // ── 2. runtime: claim *must* be present ───────────────
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var instStr = principal.FindFirst("installationId")?.Value;

            if (string.IsNullOrWhiteSpace(userId) ||
                !int.TryParse(instStr, out var installationId))
            {
                throw new UnauthorizedAccessException("Missing installationId claim.");
            }

            _cached = new UserInfo(userId, installationId);
            return _cached;
        }

        public int InstallationId => GetUserInfo().InstallationId;
    }
}
