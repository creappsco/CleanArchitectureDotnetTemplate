using Clean.Core.Application.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFC.Core.Application.Contracts.Identity
{
    public interface IAuthenticationService
    { 
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request); 
        Task<RegistrationResponse> RegisterAsync(RegistrationRequest request, string rol); 
    }
}
