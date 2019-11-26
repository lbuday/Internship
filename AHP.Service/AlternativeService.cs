using AHP.Model.Common;
using AHP.Repository.Common;
using AHP.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Service
{
    class AlternativeService : IAlternativeService
    {
        IAlternativeRepository _altRepo;
        IUnitOfWorkFactory _unitOfWorkFactory;
        public AlternativeService(
            IAlternativeRepository altRepo,
            IUnitOfWorkFactory unitOfWorkFactory
            )
        {
            _altRepo = altRepo;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task<IAlternativeModel> AddAsync(IAlternativeModel alternative)
        {
            alternative.AlternativeID = Guid.NewGuid();
            alternative.DateCreated = DateTime.Now;
            alternative.DateUpdated = DateTime.Now;
            alternative.AlternativeScore = null;

          
            alternative = _altRepo.Add(alternative);
            await _altRepo.SaveAsync();
               
            return alternative;
        }
        public async Task<bool> DeleteAsync(IAlternativeModel alternative)
        {
            
                var deleted = await _altRepo.DeleteAsync(alternative);
                await _altRepo.SaveAsync();
               
                return deleted;
            
        }
        public async Task<List<IAlternativeModel>> GetAsync(Guid id, int page = 1)
        {
            var alternatives = await _altRepo.GetPageByChoiceIDAsync(id, page);

            return alternatives;
        }
        public async Task<IAlternativeModel> UpdateAsync(IAlternativeModel alternative)
        {
            var _alternative = await _altRepo.GetByIDAsync(alternative.AlternativeID);
         
                _alternative.AlternativeName = alternative.AlternativeName;
                _alternative.DateUpdated = DateTime.Now;
                var updatedAlternative = await _altRepo.UpdateAsync(_alternative);
                await _altRepo.SaveAsync();
               
                return updatedAlternative;
        }

        public async Task<IAlternativeModel> GetByIdAsync(Guid alternativeID)
        {
            var _alternative = await _altRepo.GetByIDAsync(alternativeID);
            return _alternative;
        }
    }
}
