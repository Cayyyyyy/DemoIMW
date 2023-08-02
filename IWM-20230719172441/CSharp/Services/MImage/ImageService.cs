using IWM.Common;
using IWM.Repositories;
using RestSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Threading.Tasks;
using TrueSight.Common;
using Image = IWM.Entities.Image;
using File = IWM.Entities.File;
using System;

namespace IWM.Services.MImage
{
    public interface IImageService : IServiceScoped
    {
        Task<Image> LongTermCreate(Image Image, string path, string thumbnailPath, int width, int height);
        Task<Image> ShortTermCreate(Image Image, string path, string thumbnailPath, int width, int height);
        Task<Image> TemporaryCreate(Image Image, string path, string thumbnailPath, int width, int height);
    }

    public class ImageService : BaseService, IImageService
    {
        private readonly IUOW UOW;
        private readonly ICurrentContext CurrentContext;

        private const string LongTermRoute = "/rpc/utils/file/long-term-upload";
        private const string ShortTermRoute = "/rpc/utils/file/short-term-upload";
        private const string TemporaryRoute = "/rpc/utils/file/temporary-upload";

        public ImageService(
            IUOW UOW,
            ICurrentContext CurrentContext
        )
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }
        
        public async Task<Image> LongTermCreate(Image Image, string path, string thumbnailPath, int width, int height)
        {
            Image = await CallRequest(Image, path, LongTermRoute);
            if(Image != null)
            {
                if (!string.IsNullOrWhiteSpace(thumbnailPath))
                    Image.ThumbnailUrl = await CreateThumbnail(Image, thumbnailPath, width, height, LongTermRoute);

                try
                {
                    await UOW.ImageRepository.Create(Image);
                }
                catch (Exception ex)
                {
                    throw new MessageException(ex, nameof(ImageService));
                }
            }

            return Image;
        }

        public async Task<Image> ShortTermCreate(Image Image, string path, string thumbnailPath, int width, int height)
        {
            Image = await CallRequest(Image, path, ShortTermRoute);
            if (Image != null)
            {
                if (!string.IsNullOrWhiteSpace(thumbnailPath))
                    Image.ThumbnailUrl = await CreateThumbnail(Image, thumbnailPath, width, height, ShortTermRoute);

                try
                {
                    await UOW.ImageRepository.Create(Image);
                }
                catch (Exception ex)
                {
                    throw new MessageException(ex, nameof(ImageService));
                }
            }

            return Image;
        }

        public async Task<Image> TemporaryCreate(Image Image, string path, string thumbnailPath, int width, int height)
        {
            Image = await CallRequest(Image, path, TemporaryRoute);
            if (Image != null)
            {
                if (!string.IsNullOrWhiteSpace(thumbnailPath))
                    Image.ThumbnailUrl = await CreateThumbnail(Image, thumbnailPath, width, height, TemporaryRoute);

                try
                {
                    await UOW.ImageRepository.Create(Image);
                }
                catch (Exception ex)
                {
                    throw new MessageException(ex, nameof(ImageService));
                }
            }

            return Image;
        }

        private async Task<string> CreateThumbnail(Image Image, string thumbnailPath, int width, int height, string route)
        {
            // save thumbnail image
            MemoryStream output = new MemoryStream();
            MemoryStream input = new MemoryStream(Image.Content);
            using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(input, out SixLabors.ImageSharp.Formats.IImageFormat format))
            {
                image.Mutate(x => x
                     .Resize(width, height));
                image.Save(output, format); // Automatic encoder selected based on extension.
            }

            RestClient restClient = new RestClient(InternalServices.UTILS);
            RestRequest restRequest = new RestRequest(route);
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.Method = Method.POST;
            restRequest.AddCookie("Token", CurrentContext.Token);
            restRequest.AddCookie("X-Language", CurrentContext.Language);
            restRequest.AddHeader("Content-Type", "multipart/form-data");
            restRequest.AddFile("file", output.ToArray(), $"thumbs_{Image.Name}");
            restRequest.AddParameter("path", thumbnailPath);
            try
            {
                var response = restClient.Execute<File>(restRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return "/rpc/utils/file/download" + response.Data.Path;
                }
                else
                {
                    Exception ex = response.ErrorException ?? new Exception(response.ErrorMessage);
                    throw new MessageException(ex, nameof(ImageService));
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(ImageService));
            }
            return null;
        }

        private async Task<Image> CallRequest(Image Image, string path, string url)
        {
            RestClient restClient = new RestClient(InternalServices.UTILS);
            RestRequest restRequest = new RestRequest(url);
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.Method = Method.POST;
            restRequest.AddCookie("Token", CurrentContext.Token);
            restRequest.AddCookie("X-Language", CurrentContext.Language);
            restRequest.AddHeader("Content-Type", "multipart/form-data");
            restRequest.AddFile("file", Image.Content, Image.Name);
            restRequest.AddParameter("path", path);
            try
            {
                var response = restClient.Execute<File>(restRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Image.Id = response.Data.Id;
                    Image.Url = "/rpc/utils/file/download" + response.Data.Path;
                    Image.RowId = response.Data.RowId;
                }
                else
                {
                    Exception ex = response.ErrorException ?? new Exception(response.ErrorMessage);
                    throw new MessageException(ex, nameof(ImageService));
                }
                
                return Image;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(ImageService));
            }
            return null;
        }
    }
}
