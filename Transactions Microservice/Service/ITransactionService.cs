using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transactions_Microservice.Model;

namespace Transactions_Microservice.Service
{
     public interface ITransactionService
    {
        bool AddToTransactionHistory(TransactionStatus status, Account account);
        List<TransactionHistory> GetTransactionHistory(int CustomerId);
        Account GetAccount(int AccountId);
        TransactionStatus Deposit(int AccountId, int amount);
        TransactionStatus Withdraw(int AccountId, int amount);
        RuleStatus KnowRuleStatus(int AccountId, int amount, Account account);
    }
}
