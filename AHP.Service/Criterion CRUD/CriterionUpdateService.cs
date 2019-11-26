using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AHP.Model.Common;
using AHP.Repository.Common;
using AHP.Service.Common;

namespace AHP.Service.Choices_CRUD
{
    class CriterionUpdateService : ICriterionUpdateService
    {
        IUnitOfWork _unitOfWork;

        public UserUpdateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ICriterionModel> Update(ICriterionModel criterion)
        {
            return await _unitOfWork.CriterionRepository.UpdateAsync(criterion);
        }

    }
}
