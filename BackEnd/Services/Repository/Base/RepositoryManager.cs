//namespace Services.Repository
//{
//    public interface IRepositoryManager
//    {
//        IAccountRepository AccountRepository { get; }
//        IRefreshTokenRepository RefreshTokenRepository { get; }
//        ICategoryRepository CategoryRepository { get; }
//        IChipRepository ChipRepository { get; }
//        IDisplayRepository DisplayRepository { get; }
//        IGiftRepository GiftRepository { get; }
//        IGiftByProductRepository GiftByProductRepository { get; }
//        IImportProductRepository ImportProductRepository { get; }
//        IMemoryRepository MemoryRepository { get; }
//        INewsRepository NewsRepository { get; }
//        IOrderDetailRepository OrdDetailRepository { get; }
//        IOrderProductRepository OrdProductRepository { get; }
//        IProductRepository ProductRepository { get; }
//        IPromotionRepository PromotionRepository { get; }
//        IRateRepository RateRepository { get; }
//        IStorageRepository StorageRepository { get; }
//        ISubProductRepository SubProductRepository { get; }
//        ISupplierRepository SupplierRepository { get; }

//        Task SaveAsync();
//    }

//    public class RepositoryManager : IRepositoryManager
//    {
//        private readonly IAccountRepository _accountRepository;
//        private readonly IRefreshTokenRepository _refreshTokenRepository;
//        private readonly ICategoryRepository _categoryRepository;
//        private readonly IChipRepository _chipRepository;
//        private readonly IDisplayRepository _displayRepository;
//        private readonly IGiftRepository _giftRepository;
//        private readonly IGiftByProductRepository _giftByProductRepository;
//        private readonly IImportProductRepository _importProductRepository;
//        private readonly IMemoryRepository _memoryRepository;
//        private readonly INewsRepository _newsRepository;
//        private readonly IOrderDetailRepository _ordDetailRepository;
//        private readonly IOrderProductRepository _ordProductRepository;
//        private readonly IProductRepository _productRepository;
//        private readonly IPromotionRepository _promotionRepository;
//        private readonly IRateRepository _rateRepository;
//        private readonly IStorageRepository _storageRepository;
//        private readonly ISubProductRepository _subProductRepository;
//        private readonly ISupplierRepository _supplierRepository;

//        private readonly RepositoryContext _repositoryContext;

//        public RepositoryManager(RepositoryContext repositoryContext)
//        {
//            _repositoryContext = repositoryContext;
//        }

//        public async Task SaveAsync()
//        {
//            await _repositoryContext.SaveChangesAsync();
//        }

//        public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository ?? new RefreshTokenRepository(_repositoryContext);
//        public IAccountRepository AccountRepository => _accountRepository ?? new AccountRepository(_repositoryContext);
//        public ICategoryRepository CategoryRepository => _categoryRepository ?? new CategoryRepository(_repositoryContext);
//        public IChipRepository ChipRepository => _chipRepository ?? new ChipRepository(_repositoryContext);
//        public IDisplayRepository DisplayRepository => _displayRepository ?? new DisplayRepository(_repositoryContext);
//        public IGiftRepository GiftRepository => _giftRepository ?? new GiftRepository(_repositoryContext);
//        public IGiftByProductRepository GiftByProductRepository => _giftByProductRepository ?? new GiftByProductRepository(_repositoryContext);
//        public IImportProductRepository ImportProductRepository => _importProductRepository ?? new ImportProductRepository(_repositoryContext);
//        public IMemoryRepository MemoryRepository => _memoryRepository ?? new MemoryRepository(_repositoryContext);
//        public INewsRepository NewsRepository => _newsRepository ?? new NewsRepository(_repositoryContext);
//        public IOrderDetailRepository OrdDetailRepository => _ordDetailRepository ?? new OrderDetailRepository(_repositoryContext);
//        public IOrderProductRepository OrdProductRepository => _ordProductRepository ?? new OrderProductRepository(_repositoryContext);
//        public IProductRepository ProductRepository => _productRepository ?? new ProductRepository(_repositoryContext);
//        public IPromotionRepository PromotionRepository => _promotionRepository ?? new PromotionRepository(_repositoryContext);
//        public IRateRepository RateRepository => _rateRepository ?? new RateRepository(_repositoryContext);
//        public IStorageRepository StorageRepository => _storageRepository ?? new StorageRepository(_repositoryContext);
//        public ISubProductRepository SubProductRepository => _subProductRepository ?? new SubProductRepository(_repositoryContext);
//        public ISupplierRepository SupplierRepository => _supplierRepository ?? new SupplierRepository(_repositoryContext);
//    }
//}
