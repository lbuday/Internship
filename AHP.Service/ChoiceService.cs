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
    class ChoiceService : IChoiceService
    {
        IUnitOfWorkFactory _unitOfWorkFactory;
        IChoiceRepository _choiceRepository;

        public ChoiceService(IUnitOfWorkFactory unitOfWorkFactory, IChoiceRepository choiceRepository)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _choiceRepository = choiceRepository;
        }
        public async Task<IChoiceModel> CreateAsync(IChoiceModel choice)
        {
            choice.ChoiceID = Guid.NewGuid();
            choice.DateCreated = DateTime.Now;
            choice.DateUpdated = DateTime.Now;
            
                _choiceRepository.Add(choice);
                await _choiceRepository.SaveAsync();
                
            
            return choice;
        }

        public async Task<bool> DeleteAsync(IChoiceModel choice)
        {
            bool b = true;
            
                b = await _choiceRepository.DeleteAsync(choice);
                await _choiceRepository.SaveAsync();
             
            return b;
        }
        public async Task<List<IChoiceModel>> GetAsync(Guid userId, int page)
        {
            var choices = await _choiceRepository.GetChoicesByUserIDAsync(userId, page);
            return choices;
        }
        public async Task<IChoiceModel> UpdateAsync(IChoiceModel choice)
        {
            IChoiceModel updated;
            var _baseChoice = await _choiceRepository.GetByIDAsync(choice.ChoiceID);
            if (choice.ChoiceName != null) _baseChoice.ChoiceName = choice.ChoiceName;
            _baseChoice.DateUpdated = DateTime.Now;

            updated = await _choiceRepository.UpdateAsync(_baseChoice);
            await _choiceRepository.SaveAsync();
          
            return updated;
        }

        public async Task<IChoiceModel> GetByIdAsync(Guid choiceID)
        {
            var _choice = await _choiceRepository.GetByIDAsync(choiceID);
            return _choice;
        }

    }
}
