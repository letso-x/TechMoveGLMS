using TechMoveGLMS.Interfaces;
namespace TechMoveGLMS.States
{
    public class ActiveState : IContractState

    {
        public bool CanCreateRequest()
        {
            return true;
        }
    }
}
