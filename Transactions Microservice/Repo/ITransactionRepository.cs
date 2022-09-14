using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transactions_Microservice.Model;

namespace Transactions_Microservice.Repo
{
    public interface ITransactionRepository
    {
        bool AddToTransactionHistory(TransactionStatus status, Account account);
        List<TransactionHistory> GetTransactionHistory(int CustomerId);
    }
}
