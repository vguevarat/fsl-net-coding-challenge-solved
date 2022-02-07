using HeyUrl.UnitOfWork.Abstraction;

namespace HeyUrl.Application.Base
{
    public class BaseApplication
    {
        protected readonly IUnitOfWork _unitOfWork;

        public BaseApplication(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
