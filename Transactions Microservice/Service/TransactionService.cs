using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Transactions_Microservice.APICon;
using Transactions_Microservice.Model;
using Transactions_Microservice.Repo;

namespace Transactions_Microservice.Service
{
    public class TransactionService : ITransactionService
    {
        
        private ITransactionRepository _repo;
        public TransactionService(ITransactionRepository repo)
        {
            _repo = repo;
        }
        
        public bool AddToTransactionHistory(TransactionStatus status, Account account)
        {

            try
            {
                bool output = _repo.AddToTransactionHistory(status, account);

                if (output == false)
                {
                    throw new System.ArgumentNullException("Not able to add data to transaction history for: " + account.AccountId);
                }
            }
            catch (Exception e)
            {
                
                throw e;
            }

            return true;
        }

        
        public TransactionStatus Deposit(int AccountId, int amount)
        {
            TransactionStatus status = new TransactionStatus();
            try
            {
                Client obj = new Client();
                HttpClient client = obj.AccountDetails();
                HttpResponseMessage response = client.PostAsJsonAsync("api/Account/deposit", new { accountId = AccountId, amount = amount }).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                status = JsonConvert.DeserializeObject<TransactionStatus>(result);
            }
            catch (Exception e)
            {
                
                throw e;
            }

            return status;
        }

        
        public Account GetAccount(int AccountId)
        {
            Account account = new Account();
            try
            {
                Client obj = new Client();
                HttpClient client = obj.AccountDetails();

                HttpResponseMessage response = client.GetAsync("api/Account/getAccount/" + AccountId).Result;

                var result = response.Content.ReadAsStringAsync().Result;
                account = JsonConvert.DeserializeObject<Account>(result);
            }

            catch (Exception e)
            {

                throw e;
            }

            return account;
        }

        
        public List<TransactionHistory> GetTransactionHistory(int CustomerId)
        {
            try
            {
                List<TransactionHistory> list = _repo.GetTransactionHistory(CustomerId);
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public RuleStatus KnowRuleStatus(int AccountId, int amount, Account account)
        {
            RuleStatus rulestatus = new RuleStatus();
            try
            {
                Client obj = new Client();
                HttpClient client = obj.RuleApi();
                int balance = account.Balance - amount;
                HttpResponseMessage response = client.GetAsync("api/Rules/EvaluateMinBal/" + AccountId + "/" + balance).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                rulestatus = JsonConvert.DeserializeObject<RuleStatus>(result);
            }
            catch (Exception e)
            {
               
                throw e;
            }

            return rulestatus;
        }


        public TransactionStatus Withdraw(int AccountId, int amount)
        {
            TransactionStatus status = new TransactionStatus();
            try
            {
                Client obj = new Client();
                HttpClient client = obj.AccountDetails();
                HttpResponseMessage response = client.PostAsJsonAsync("api/Account/withdraw", new { AccountId = AccountId, amount = amount }).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                status = JsonConvert.DeserializeObject<TransactionStatus>(result);
            }
            catch (Exception e)
            {

                throw e;
            }
            return status;
        }
    }
}
