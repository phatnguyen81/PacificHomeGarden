using System;
using pCMS.Core;

namespace pCMS.Data
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
    }

    public interface IDalContext : IUnitOfWork
    {
        IAlbumRepository Albums { get; }
        IAlbumPictureRepository AlbumPictures { get; }
        IPictureRepository Pictures { get; }
        IChannelRepository Channels { get; }
        IArticleRepository Articles { get; }
        IChannelArticleRepository ChannelArticles { get; }
        ICategoryRepository Categories { get; }
        IProductTypeRepository ProductTypes { get; }
        IEventRepository Events { get; }
        IConfigLanguageRepository ConfigLanguages { get; }
        IConfigResourceRepository ConfigResources { get; }
        IConfigSettingRepository ConfigSettings { get; }
        IManufacturerRepository Manufacturers { get; }
        IPollRepository Polls { get; }
        IPollAnswerRepository PollAnswers { get; }
        IProductAttributeRepository ProductAttributes { get; }
        IProductRepository Products { get; }
        ICollectionRepository Collections { get; }
        IOrderRepository Orders { get; }
        IUserRepository Users { get; }
        ILogRepository Logs { get; }
        IPageRepository Pages { get; }
        IFileDownloadRepository FileDownloads { get; }

        IVideoRepository Videos { get; }
    }

    public class DalContext : IDalContext
    {
        private readonly pCMSEntities _dbContext;
        private IAlbumRepository _albums;
        private IAlbumPictureRepository _albumPictures;
        private IPictureRepository _pictures;
        private IChannelRepository _channels;
        private IArticleRepository _articles;
        private IChannelArticleRepository _channelArticles;
        private ICategoryRepository _categories;
        private IProductTypeRepository _productTypes;
        private IEventRepository _events;
        private IConfigLanguageRepository _configLanguages;
        private IConfigResourceRepository _configResources;
        private IConfigSettingRepository _configSettings;
        private IManufacturerRepository _manufacturers;
        private IPollRepository _polls;
        private IPollAnswerRepository _pollAnswers;
        private IProductAttributeRepository _productAttributes;
        private IProductRepository _products;
        private IOrderRepository _orders;
        private IUserRepository _users;
        private ILogRepository _logs;
        private IPageRepository _pages;
        private ICollectionRepository _collections;
        private IFileDownloadRepository _filedowloads;
        private IVideoRepository _videos;

        public DalContext()
        {
            _dbContext = new pCMSEntities();
        }


        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            if (_albums != null)
                _albums.Dispose();
            if (_albumPictures != null)
                _albumPictures.Dispose();
            if (_pictures != null)
                _pictures.Dispose();
            if (_channels != null)
                _channels.Dispose();
            if (_articles != null)
                _articles.Dispose();
            if (_channelArticles != null)
                _channelArticles.Dispose();
            if (_categories != null)
                _categories.Dispose();
            if (_productTypes != null)
                _productTypes.Dispose();
            if (_events != null)
                _events.Dispose();
            if (_configLanguages != null)
                _configLanguages.Dispose();
            if (_configResources != null)
                _configResources.Dispose();
            if (_configSettings != null)
                _configSettings.Dispose();
            if (_manufacturers != null)
                _manufacturers.Dispose();
            if (_polls != null)
                _polls.Dispose();
            if (_pollAnswers != null)
                _pollAnswers.Dispose();
            if (_productAttributes != null)
                _productAttributes.Dispose();
            if (_products != null)
                _products.Dispose();
            if (_orders != null)
                _orders.Dispose();
            if (_users != null)
                _users.Dispose();
            if (_logs != null)
                _logs.Dispose();
            if(_pages != null)
                _pages.Dispose();
            if(_collections != null)
                _collections.Dispose();
            if(_filedowloads != null)
                _filedowloads.Dispose();
            if (_videos != null)
                _videos.Dispose();
            if (_dbContext != null)
                _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public IAlbumRepository Albums
        {
            get { return _albums ?? (_albums = new AlbumRepository(_dbContext)); }
        }

        public IAlbumPictureRepository AlbumPictures
        {
            get { return _albumPictures ?? (_albumPictures = new AlbumPictureRepository(_dbContext)); }
        }

        public IPictureRepository Pictures
        {
            get { return _pictures ?? (_pictures = new PictureRepository(_dbContext)); }
        }

        public IChannelRepository Channels
        {
            get { return _channels ?? (_channels = new ChannelRepository(_dbContext)); }
        }

        public IArticleRepository Articles
        {
            get { return _articles ?? (_articles = new ArticleRepository(_dbContext)); }
        }

        public IChannelArticleRepository ChannelArticles
        {
            get { return _channelArticles ?? (_channelArticles = new ChannelArticleRepository(_dbContext)); }
        }

        public ICategoryRepository Categories
        {
            get { return _categories ?? (_categories = new CategoryRepository(_dbContext)); }
        }

        public IProductTypeRepository ProductTypes
        {
            get { return _productTypes ?? (_productTypes = new ProductTypeRepository(_dbContext)); }
        }

        public IEventRepository Events
        {
            get { return _events ?? (_events = new EventRepository(_dbContext)); }
        }

        public IConfigLanguageRepository ConfigLanguages
        {
            get { return _configLanguages ?? (_configLanguages = new ConfigLanguageRepository(_dbContext)); }
        }

        public IConfigResourceRepository ConfigResources
        {
            get { return _configResources ?? (_configResources = new ConfigResourceRepository(_dbContext)); }
        }

        public IConfigSettingRepository ConfigSettings
        {
            get { return _configSettings ?? (_configSettings = new ConfigSettingRepository(_dbContext)); }
        }

        public IManufacturerRepository Manufacturers
        {
            get { return _manufacturers ?? (_manufacturers = new ManufacturerRepository(_dbContext)); }
        }

        public IPollRepository Polls
        {
            get { return _polls ?? (_polls = new PollRepository(_dbContext)); }
        }

        public IPollAnswerRepository PollAnswers
        {
            get { return _pollAnswers ?? (_pollAnswers = new PollAnswerRepository(_dbContext)); }
        }

        public IProductAttributeRepository ProductAttributes
        {
            get { return _productAttributes ?? (_productAttributes = new ProductAttributeRepository(_dbContext)); }
        }

        public IProductRepository Products
        {
            get { return _products ?? (_products = new ProductRepository(_dbContext)); }
        }

        public IOrderRepository Orders
        {
            get { return _orders ?? (_orders = new OrderRepository(_dbContext)); }
        }

        public IUserRepository Users
        {
            get { return _users ?? (_users = new UserRepository()); }
        }

        public ILogRepository Logs
        {
            get { return _logs ?? (_logs = new LogRepository(_dbContext)); }
        }

        public IPageRepository Pages
        {
            get { return _pages ?? (_pages = new PageRepository(_dbContext)); }
        }

        public IFileDownloadRepository FileDownloads
        {
            get { return _filedowloads ?? (_filedowloads = new FileDownloadRepository(_dbContext)); }
        }

        public IVideoRepository Videos
        {
            get { return _videos ?? (_videos = new VideoRepository(_dbContext)); }
        }

        public ICollectionRepository Collections
        {
            get { return _collections ?? (_collections = new CollectionRepository(_dbContext)); }
        }
        
    }
}