using TechMoveGLMS.Interfaces;

namespace TechMoveGLMS.States
{
    public class DraftState : IContractState
    {
        public bool CanCreateRequest()
        {
            return false;
        }
    }
}
