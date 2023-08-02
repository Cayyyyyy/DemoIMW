using IWM.Entities;
using IWM.Common;
using IWM.Models;
using System.Threading.Tasks;

namespace IWM.Repositories
{
    public interface IImageRepository
    {
        Task<bool> Create(Image Image);
    }
    public class ImageRepository : IImageRepository
    {
        private readonly DataContext DataContext;
        public ImageRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        public async Task<bool> Create(Image Image)
        {
            ImageDAO ImageDAO = new ImageDAO();
            ImageDAO.Id = Image.Id;
            ImageDAO.Name = Image.Name;
            ImageDAO.Url = Image.Url;
            ImageDAO.ThumbnailUrl = Image.ThumbnailUrl;
            ImageDAO.RowId = Image.RowId;
            ImageDAO.CreatedAt = StaticParams.DateTimeNow;
            ImageDAO.UpdatedAt = StaticParams.DateTimeNow;
            DataContext.Add(ImageDAO);
            await DataContext.SaveChangesAsync();
            return true;
        }
    }
}
