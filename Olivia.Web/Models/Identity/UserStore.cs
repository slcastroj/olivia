using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Olivia.Web.Models.Data;

namespace Olivia.Web.Models.Identity
{
    public class UserStore : IUserStore<User>, IUserPasswordStore<User>, IUserEmailStore<User>
    {
        private OliviaContext Context { get; }

        public UserStore(OliviaContext context)
        {
            Context = context;
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            var u = await Context.User.AddAsync(user, cancellationToken);

            if (u is null) { return IdentityResult.Failed(); }

            await Context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            var e = Context.User.Remove(user);

            if (e is null) { return IdentityResult.Failed(); }

            await Context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                Context?.Dispose();
            }
        }

        public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return Context.User.FindAsync(userId);
        }

        public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return FindByIdAsync(normalizedUserName, cancellationToken);
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Username.ToLowerInvariant());
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Password);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Username);
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Username);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!String.IsNullOrWhiteSpace(user.Password));
        }

        public async Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.Username = normalizedName;
            await Context.SaveChangesAsync();
        }

        public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.Password = passwordHash;
            await Context.SaveChangesAsync();
        }

        public async Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.Username = userName;
            await Context.SaveChangesAsync();
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            Context.User.Update(user);
            return Task.FromResult(IdentityResult.Success);
        }

        public async Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            await Context.SaveChangesAsync();
        }

        public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.IsEmailConfirmed == 1);
        }

        public async Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            user.IsEmailConfirmed = (byte)(confirmed ? 1 : 0);
            await Context.SaveChangesAsync();
        }

        public Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.FromResult(Context.User.Where(x => x.Email == normalizedEmail).FirstOrDefault());
        }

        public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email.ToLowerInvariant());
        }

        public async Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.Email = normalizedEmail;
            await Context.SaveChangesAsync();
        }
    }
}