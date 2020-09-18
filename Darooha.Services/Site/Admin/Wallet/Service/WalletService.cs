using Darooha.Common.ErrorsAndMessages;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Models;
using Darooha.Repo.Infrastructure;
using Darooha.Services.Site.Admin.Wallet.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darooha.Services.Site.Admin.Wallet.Service
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork<DaroohaDbContext> _db;

        public WalletService(IUnitOfWork<DaroohaDbContext> dbContext)
        {
            _db = dbContext;
        }

        public async Task<bool> CheckInventoryAsync(int cost, string userID)
        {
            var wallet = (await _db.WalletRepository
                .GetManyAsync(p => p.UserId == userID, null, "")).SingleOrDefault();
            if (wallet != null)
            {
                if (wallet.Inventory >= cost)
                    return true;
                else
                    return false;
            }
            else
            {
                var walletForCreate = new Tbl_Wallet()
                {
                    UserId = userID,
                    Inventory = 0
                };
                await _db.WalletRepository.InsertAsync(walletForCreate);

                if (await _db.SaveAsync())
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }
        public async Task<ReturnErrorMessage> IncreaseInventoryAsync(int cost, string userID)
        {
            var wallet = (await _db.WalletRepository
                .GetManyAsync(p => p.UserId == userID, null, "")).SingleOrDefault();
            if (wallet != null)
            {
                wallet.Inventory += cost;
                _db.WalletRepository.Update(wallet);
                if (await _db.SaveAsync())
                {
                    return new ReturnErrorMessage()
                    {
                        Status = true
                    };
                }
                else
                {
                    return new ReturnErrorMessage()
                    {
                        Status = false,
                        Message = "خطا در افزایش موجودی"
                    };
                }
            }
            else
            {
                var walletForCreate = new Tbl_Wallet()
                {
                    UserId = userID,
                    Inventory = cost
                };
                await _db.WalletRepository.InsertAsync(walletForCreate);

                if (await _db.SaveAsync())
                {
                    return new ReturnErrorMessage()
                    {
                        Status = true
                    };
                }
                else
                {
                    return new ReturnErrorMessage()
                    {
                        Status = false,
                        Message = "خطا در ایجاد کیف پول"
                    };
                }
            }
        }

        public async Task<ReturnErrorMessage> DecreaseInventoryAsync(int cost, string userID)
        {
            var wallet = (await _db.WalletRepository
                .GetManyAsync(p => p.UserId == userID, null, "")).SingleOrDefault();
            if (wallet != null)
            {
                if (wallet.Inventory >= cost)
                {
                    wallet.Inventory -= cost;
                    _db.WalletRepository.Update(wallet);
                    if (await _db.SaveAsync())
                    {
                        return new ReturnErrorMessage()
                        {
                            Status = true
                        };
                    }
                    else
                    {
                        return new ReturnErrorMessage()
                        {
                            Status = false,
                            Message = "خطا در کاهش موجودی"
                        };
                    }
                }
                else
                {
                    return new ReturnErrorMessage()
                    {
                        Status = false,
                        Message = "موجودی کیف پول شما کافی نمیباشد"
                    };
                }
            }
            else
            {
                var walletForCreate = new Tbl_Wallet()
                {
                    UserId = userID,
                    Inventory = 0
                };
                await _db.WalletRepository.InsertAsync(walletForCreate);

                if (await _db.SaveAsync())
                {
                    return new ReturnErrorMessage()
                    {
                        Status = false,
                        Message = "موجودی کیف پول شما کافی نمیباشد"
                    };
                }
                else
                {
                    return new ReturnErrorMessage()
                    {
                        Status = false,
                        Message = "خطا در ایجاد کیف پول"
                    };
                }
            }
        }


    }
}
