using XelerationTask.Core.Exceptions;
using XelerationTask.Core.Interfaces;
using XelerationTask.Core.Models;

namespace XelerationTask.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IPasswordHasher _passwordHasher;

        private readonly ITokenGenerator _tokenGenerator;
        public AuthService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
        }


        public async Task<User> RegisterAsync(User user, String rawPassword)
        {
           var existingUsers = await _unitOfWork.UserRepository.FindAsync(u => u.Email == user.Email);

           if (existingUsers.Any()) throw new ResourceAlreadyExistsException("The Provided Email is already in use .");

           var hashedResult = _passwordHasher.HashPassword(rawPassword);

           user.PasswordHash = hashedResult.passwordHash;
           user.PasswordSalt = hashedResult.passwordSalt;

           user.CreatedAt = DateTime.Now;
           user.UpdatedAt = DateTime.Now;

           await _unitOfWork.UserRepository.AddAsync(user);

           await _unitOfWork.CompleteAsync();

           user.CreatedBy = user.Id;
           user.UpdatedBy = user.Id;

           _unitOfWork.UserRepository.Update(user);

           await _unitOfWork.CompleteAsync();

           return user;
        }

        public async Task<(string accessToken , string refreshToken)> LoginAsync(string email , string password)
        {
            var existingUsers = await _unitOfWork.UserRepository.FindAsync(u => u.Email == email);

            if (!existingUsers.Any()) throw new ResourceNotFoundException("This email isn't registered.");

            var user = existingUsers.FirstOrDefault(u => !u.IsDeleted);

            if(user==null) throw new ResourceNotFoundException("Please contact omar.b@Xeleration.net to reactivate your account.");

            if (!_passwordHasher.VerifyHashedPassword(user.PasswordHash, user.PasswordSalt, password)) throw new NotAuthorized("Somthing is wrong with your credentials.");

            var accessToken = _tokenGenerator.CreateAccessToken(user);

            var refreshToken = _tokenGenerator.CreateRefreshToken();

            user.RefreshToken = refreshToken;

            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            _unitOfWork.UserRepository.Update(user);

            await _unitOfWork.CompleteAsync();

            return (accessToken, refreshToken);

        }

        public async Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string refreshToken)
        {
            var user = await _unitOfWork.UserRepository.FindAsync(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiryTime >= DateTime.Now && !u.IsDeleted);

            if(!user.Any()) throw new NotAuthorized("Invalid Refresh Token.");

            var userToUpdate = user.FirstOrDefault();

            var newAccessToken = _tokenGenerator.CreateAccessToken(userToUpdate);

            var newRefreshToken = _tokenGenerator.CreateRefreshToken();

            userToUpdate.RefreshToken = newRefreshToken;

            userToUpdate.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            _unitOfWork.UserRepository.Update(userToUpdate);

            await _unitOfWork.CompleteAsync();

            return (newAccessToken, newRefreshToken);
        }

        public async Task LogoutAsync(int userId)
        {
            var user = await _unitOfWork.UserRepository.FindAsync(u => u.Id == userId && !u.IsDeleted);
            var userToLogout = user.FirstOrDefault();

            if (userToLogout == null)
                throw new ResourceNotFoundException("User not found.");

            if (userToLogout.RefreshToken == null || userToLogout.RefreshTokenExpiryTime < DateTime.Now)
                throw new InvalidOperationError("You need to sign in in order to sign out.");

            userToLogout.RefreshToken = null;
            userToLogout.RefreshTokenExpiryTime = null;

            _unitOfWork.UserRepository.Update(userToLogout);
            await _unitOfWork.CompleteAsync();
            return;
        }

    }

}
