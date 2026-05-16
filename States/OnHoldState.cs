using TechMoveGLMS.Interfaces;
namespace TechMoveGLMS.States
{
    public class OnHoldState : IContractState
    {
        public bool CanCreateRequest()
        {
            return false;
        }
    }
}
