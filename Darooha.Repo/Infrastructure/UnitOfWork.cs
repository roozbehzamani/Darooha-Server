using Darooha.Repo.Repositories.Interface;
using Darooha.Repo.Repositories.Repo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Darooha.Repo.Infrastructure
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext , new()
    {
        #region ctor
        protected readonly DbContext _db;

        public UnitOfWork()
        {
            _db = new TContext();
        }

        #endregion

        #region save
        public bool Save()
        {
            if (_db.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        public async Task<bool> SaveAsync()
        {
            if (await _db.SaveChangesAsync() > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region despose
        private bool dispose = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!dispose)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            dispose = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
        #endregion

        #region private repository
        //
        private IUserRepository userRepository { get; set; }
        public IUserRepository UserRepository
        {
            get
            {
                if(userRepository == null)
                {
                    userRepository = new UserRepository(_db);
                }
                return userRepository;
            }
        }
        //
        private ISettingRepository settingRepository;
        public ISettingRepository SettingRepository
        {
            get
            {
                if (settingRepository == null)
                {
                    settingRepository = new SettingRepository(_db);
                }
                return settingRepository;
            }
        }
        //
        private IRoleRepository roleRepository;
        public IRoleRepository RoleRepository
        {
            get
            {
                if (roleRepository == null)
                {
                    roleRepository = new RoleRepository(_db);
                }
                return roleRepository;
            }
        }
        //
        private ITokenRepository tokenRepository;
        public ITokenRepository TokenRepository
        {
            get
            {
                if (tokenRepository == null)
                {
                    tokenRepository = new TokenRepository(_db);
                }
                return tokenRepository;
            }
        }
        //
        private INotificationRepository notificationRepository;
        public INotificationRepository NotificationRepository
        {
            get
            {
                if (notificationRepository == null)
                {
                    notificationRepository = new NotificationRepository(_db);
                }
                return notificationRepository;
            }
        }
        //
        private IUserAddressRepository userAddressRepository;
        public IUserAddressRepository UserAddressRepository
        {
            get
            {
                if (userAddressRepository == null)
                {
                    userAddressRepository = new UserAddressRepository(_db);
                }
                return userAddressRepository;
            }
        }
        //
        private IWalletRepository walletRepository;
        public IWalletRepository WalletRepository
        {
            get
            {
                if (walletRepository == null)
                {
                    walletRepository = new WalletRepository(_db);
                }
                return walletRepository;
            }
        }
        //
        private ITicketRepository ticketRepository;
        public ITicketRepository TicketRepository
        {
            get
            {
                if (ticketRepository == null)
                {
                    ticketRepository = new TicketRepository(_db);
                }
                return ticketRepository;
            }
        }
        //
        private ITicketContentRepository ticketContentRepository;
        public ITicketContentRepository TicketContentRepository
        {
            get
            {
                if (ticketContentRepository == null)
                {
                    ticketContentRepository = new TicketContentRepository(_db);
                }
                return ticketContentRepository;
            }
        }
        //
        private IProductRepository productRepository;
        public IProductRepository ProductRepository
        {
            get
            {
                if (productRepository == null)
                {
                    productRepository = new ProductRepository(_db);
                }
                return productRepository;
            }
        }
        //
        private IProductImageRepository productImageRepository;
        public IProductImageRepository ProductImageRepository
        {
            get
            {
                if (productImageRepository == null)
                {
                    productImageRepository = new ProductImageRepository(_db);
                }
                return productImageRepository;
            }
        }
        //
        private IOrderRepository orderRepository;
        public IOrderRepository OrderRepository
        {
            get
            {
                if (orderRepository == null)
                {
                    orderRepository = new OrderRepository(_db);
                }
                return orderRepository;
            }
        }
        //
        private IOrderItemRepository orderItemRepository;
        public IOrderItemRepository OrderItemRepository
        {
            get
            {
                if (orderItemRepository == null)
                {
                    orderItemRepository = new OrderItemRepository(_db);
                }
                return orderItemRepository;
            }
        }
        //
        private IMenuRepository menuRepository;
        public IMenuRepository MenuRepository
        {
            get
            {
                if (menuRepository == null)
                {
                    menuRepository = new MenuRepository(_db);
                }
                return menuRepository;
            }
        }
        //
        private ISubMenuRepository subMenuRepository;
        public ISubMenuRepository SubMenuRepository
        {
            get
            {
                if (subMenuRepository == null)
                {
                    subMenuRepository = new SubMenuRepository(_db);
                }
                return subMenuRepository;
            }
        }
        //
        private IHomeFirstSliderRepository homeFirstSliderRepository;
        public IHomeFirstSliderRepository HomeFirstSliderRepository
        {
            get
            {
                if (homeFirstSliderRepository == null)
                {
                    homeFirstSliderRepository = new HomeFirstSliderRepository(_db);
                }
                return homeFirstSliderRepository;
            }
        }
        //
        private ICommentRepository commentRepository;
        public ICommentRepository CommentRepository
        {
            get
            {
                if (commentRepository == null)
                {
                    commentRepository = new CommentRepository(_db);
                }
                return commentRepository;
            }
        }

        #endregion
    }
}
