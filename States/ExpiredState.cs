using TechMoveGLMS.Interfaces;

namespace TechMoveGLMS.States
{
    public class ExpiredState : IContractState
    {
        public bool CanCreateRequest()
        {
            return false;
        }
    }
}
