using Microsoft.AspNetCore.Identity;
using NutritionalRecipeBook.Application.Common.Models.User;
using NutritionalRecipeBook.Domain.Entities;
using NutritionalRecipeBook.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutritionalRecipeBook.Application.Contracts
{
    public interface IIdentityService
    {
        public Task<IdentityResult> Register(RegisterUserRequest request);

        public Task<Result<LoginResponse>> Login(LoginRequest request);

        public Task<Result<IdentityResult>> ConfirmEmail(string userId, string token);

        public Task<User?> GetUserByIdWithRelationsAsync(string id);

        public Task<User?> FindUserByIdAsync(string id);
    }
}
