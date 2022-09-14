using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transactions_Microservice.Model;

namespace Transactions_Microservice.Repo
{
    public class TransactionRepository : ITransactionRepository
    {
        

        static List<TransactionHistory> historyList = new List<TransactionHistory>() {

            new TransactionHistory(){TransactionId=1,AccountId=1,CustomerId=1,
                message="Account has been credited",source_balance=1000,destination_balance=1500,DateOfTransaction=DateTime.Now},

            new TransactionHistory(){TransactionId=2,AccountId=3,CustomerId=2,
                message="Account has been Debited",source_balance=2000,destination_balance=1500,DateOfTransaction=DateTime.Now},

                        new TransactionHistory(){TransactionId=1,AccountId=2,CustomerId=1,
                message="Account has been credited",source_balance=4000,destination_balance=1500,DateOfTransaction=DateTime.Now},

            new TransactionHistory(){TransactionId=2,AccountId=4,CustomerId=2,
                message="Account has been Debited",source_balance=22000,destination_balance=1500,DateOfTransaction=DateTime.Now}

      };

        List<TransactionHistory> historyList2 = new List<TransactionHistory>();

        static int cnt = 24;

        
        public bool AddToTransactionHistory(TransactionStatus status, Account account)
        {
            if (status == null && account == null)
            {
                return false;
            }
            cnt = cnt + 256;
            TransactionHistory history = new TransactionHistory()
            {
                TransactionId = cnt,
                AccountId = account.AccountId,
                message = status.message,
                source_balance = status.source_balance,
                destination_balance = status.destination_balance,
                DateOfTransaction = DateTime.Now,
                CustomerId = account.CustomerId
            };
            historyList.Add(history);
            return true;
        }



        public List<TransactionHistory> GetTransactionHistory(int CustomerId)
        {

            try
            {
                foreach (var list in historyList)
                {
                    if (list.AccountId == CustomerId)
                    {
                        historyList2.Add(list);
                    }
                }

                if (historyList2.Count == 0)
                {
                    throw new System.ArgumentException("No Record Found for this Customer Id: " + CustomerId);
                }

            }
            catch (Exception e)
            {
               
                throw e;
            }
            return historyList2;
        }
    }
}
