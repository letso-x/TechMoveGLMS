using TechMoveGLMS.Interfaces;
using TechMoveGLMS.Models;
using TechMoveGLMS.States;

namespace TechMoveGLMS.Services
{
    public class ContractStateFactory
    {
        public static IContractState Create(ContractStatus status)
        {
            return status switch
            {
                ContractStatus.Active => new ActiveState(),
                ContractStatus.Expired => new ExpiredState(),
                ContractStatus.OnHold => new OnHoldState(),
                _ => new DraftState()
            };
        }
    }
}
