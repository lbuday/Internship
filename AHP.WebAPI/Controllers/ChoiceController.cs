using AHP.Model.Common;
using AHP.Service.Common;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AHP.WebAPI.Controllers
{
    public class ChoiceController : ApiController
    {
        ICalculateAHPScores _AHPcalculate;
        IChoiceService _choiceService;
        IMapper _mapper;

        public ChoiceController(
            IMapper mapper,
            ICalculateAHPScores AHPcalculate,
            IChoiceService choiceService)
        {
            _mapper = mapper;
            _AHPcalculate = AHPcalculate;
            _choiceService = choiceService;
        }


        public async Task<IHttpActionResult> Post(ChoiceControllerModel choice)
        {
            if (choice == null)
            {
                return BadRequest();
            }
            var _choice = _mapper.Map<ChoiceControllerModel, IChoiceModel>(choice);
            var status = await _choiceService.CreateAsync(_choice);

            if(status != null)
            {
                return Ok(_mapper.Map<IChoiceModel, ChoiceControllerModel>(status));
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        
        public async Task<IHttpActionResult> GetChoice(ChoiceRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }
            var status = await _choiceService.GetAsync(request.UserID, request.Page);

            if (status.Any())
            {
                return Ok(_mapper.Map<List<IChoiceModel>, List<ChoiceControllerModel>>(status));
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        [Route("api/choice/calculate/{choiceid}")]
        public async Task<IHttpActionResult> GetCalculate(Guid choiceid)
        {
            
             var status = await _AHPcalculate.CalculateCriteriaWeights(choiceid);

            return Ok(status);
        }
        public async Task<IHttpActionResult> Put(ChoiceControllerModel choice)
        {
            if (choice == null)
            {
                return BadRequest();
            }
            var _choice = _mapper.Map<ChoiceControllerModel, IChoiceModel>(choice);
            var status = await _choiceService.UpdateAsync(_choice);

            if (status != null)
            {
                return Ok(_mapper.Map<IChoiceModel, ChoiceControllerModel>(status));
            }
            else
            {
                return NotFound();
            }
        }
        public async Task<IHttpActionResult> Delete(ChoiceControllerModel choice)
        {
            if (choice == null)
            {
                return BadRequest();
            }
            var _choice = _mapper.Map<ChoiceControllerModel, IChoiceModel>(choice);
            var status = await _choiceService.DeleteAsync(_choice);

            if (status)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }


    }
    public class ChoiceRequest
    {
        public Guid UserID;
        public int Page;
    }
    public class ChoiceControllerModel
    {
        public System.Guid ChoiceID { get; set; }
        public string ChoiceName { get; set; }
        public System.Guid UserID { get; set; }
        public virtual ICollection<ICriterionModel> Criteria { get; set; }
        public virtual ICollection<IAlternativeModel> Alternatives { get; set; }
    }
}
