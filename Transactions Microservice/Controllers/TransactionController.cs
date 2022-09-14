using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transactions_Microservice.Model;
using Transactions_Microservice.Service;
using Microsoft.AspNetCore.Http;

namespace Transactions_Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private ITransactionService _service;
        public TransactionController(ITransactionService service)
        {
            _service = service;
        }

       
        [HttpGet]
        [Route("getTransactions/{CustomerId}")]
        public IActionResult getTransactions(int CustomerId)
        {

            if (CustomerId == 0)
            {
                
                return NotFound();
            }


            try
            {
                List<TransactionHistory> Ts = _service.GetTransactionHistory(CustomerId);
                
                return Ok(Ts);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

       
        [HttpPost]
        [Route("deposit")]
        public IActionResult deposit([FromBody] dynamic model)
        {
        
            if (Convert.ToInt32(model.AccountId)==0 || Convert.ToInt32(model.amount)==0)
            {
                
                return NotFound(new TransactionStatus() { message = "Withdraw Not Allowed" });

            }
            


            try
            {
                Account account = _service.GetAccount(Convert.ToInt32(model.AccountId));

                

                TransactionStatus status = _service.Deposit(Convert.ToInt32(model.AccountId), Convert.ToInt32(model.amount));
                _service.AddToTransactionHistory(status, account);
                
                return Ok(status);

            }
            catch (Exception e)
            {
                
                throw e;
            }

        }

        
        [HttpPost]
        [Route("withdraw")]
        public IActionResult withdraw([FromBody] dynamic model)
        {

            if (Convert.ToInt32(model.AccountId) == 0 || Convert.ToInt32(model.amount) == 0)
            {
                
                return NotFound(new TransactionStatus() { message = "Withdraw Not Allowed" });
            }

            try
            {
                Account account = _service.GetAccount(Convert.ToInt32(model.AccountId));

                RuleStatus rulestatus = _service.KnowRuleStatus(Convert.ToInt32(model.AccountId), Convert.ToInt32(model.amount), account);


                if (rulestatus.Status == "allowed")
                {
                    


                    TransactionStatus status = _service.Withdraw(Convert.ToInt32(model.AccountId), Convert.ToInt32(model.amount));

                    if (status.message == null)
                    {
                        return NotFound(new TransactionStatus() { message = "Record Not Found" });
                    }

                    _service.AddToTransactionHistory(status, account);
                    
                    return Ok(status);
                }
                return NotFound(new TransactionStatus() { message = "Withdraw Not Allowed" });
            }

            catch (Exception e)
            {
                
                throw e;
            }

        }


        [HttpPost]
        [Route("transfer")]
        public IActionResult transfer([FromBody] dynamic model)
        {
            if (Convert.ToInt32(model.Source_AccountId) == 0 || Convert.ToInt32(model.Target_AccountId) == 0 || Convert.ToInt32(model.amount) == 0)
            {
                
                return NotFound(new TransactionStatus() { message = "Transfer Not Allowed" });
            }

            try
            {
                TransactionStatus statusfinal = new TransactionStatus();
                statusfinal.message = "Transaction Not Allowed";
                Account account = _service.GetAccount(Convert.ToInt32(model.Source_AccountId));
                RuleStatus rulestatus = _service.KnowRuleStatus(Convert.ToInt32(model.Source_AccountId), Convert.ToInt32(model.amount), account);
                if (rulestatus.Status == "allowed")
                {

                    statusfinal.message = "Transfered from Account no. " + model.Source_AccountId + " To Account no. " + model.Target_AccountId;

                    TransactionStatus status = _service.Withdraw(Convert.ToInt32(model.Source_AccountId), Convert.ToInt32(model.amount));

                    if (status.message == null)
                    {
                        return NotFound(new TransactionStatus() { message = "Record Not Found" });
                    }

                    _service.AddToTransactionHistory(status, account);
                    statusfinal.source_balance = status.destination_balance;


                    account = _service.GetAccount(Convert.ToInt32(model.Target_AccountId));
                    status = _service.Deposit(Convert.ToInt32(model.Target_AccountId), Convert.ToInt32(model.amount));

                    if (status.message == null)
                    {
                        return NotFound(new TransactionStatus() { message = "Record Not Found" });
                    }

                    _service.AddToTransactionHistory(status, account);
                    statusfinal.destination_balance = status.destination_balance;
                    
                    return Ok(statusfinal);
                }
                return NotFound();
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }
    }
}
