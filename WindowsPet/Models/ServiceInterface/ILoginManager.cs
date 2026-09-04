using System.Threading.Tasks;

namespace WindowsPet.Models.ServiceInterface
{
    public interface ILoginManager
    {
        Task NormalLogin(LoginCommand login);
        void GoogleLogin();
        Task RegisterationRequest(RegisterCommand command);
        void UserLoggedInSuccess(PersonalData tempPersonalData);
    }
}
